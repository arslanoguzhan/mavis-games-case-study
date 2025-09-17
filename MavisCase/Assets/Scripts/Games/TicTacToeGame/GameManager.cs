using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MavisCase.Common.Assets;
using MavisCase.Common.Communication;
using MavisCase.Common.GridSystem;
using MavisCase.Common.InputSystem;
using MavisCase.Common.Panels;
using MavisCase.Common.Persistence;
using MavisCase.Common.Pooling;
using MavisCase.Common.Popups;
using MavisCase.Common.Signals;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MavisCase.Games.TicTacToeGame
{
    public class GameManager : IDisposable
    {
        private BoardView _boardView;
        
        private IGridManagerInitializer  _gridManager;
        private ItemLookup _itemLookup;
        private GraphicManager _graphicManager;
        private InputBlocker _inputBlocker;
        private PopupService _popupService;
        private SignalBus _signalBus;
        private ProgressStorage<Progress> _progressStorage;
        private PoolService _poolService;
        
        public GameManager(
            IGridManagerInitializer gridManager, 
            ItemLookup itemLookup, 
            GraphicManager graphicManager, 
            InputBlocker inputBlocker, 
            PopupService popupService, 
            SignalBus signalBus, 
            ProgressStorage<Progress> progressStorage, 
            PoolService poolService
        ){
            _gridManager = gridManager;
            _itemLookup = itemLookup;
            _graphicManager = graphicManager;
            _inputBlocker = inputBlocker;
            _popupService = popupService;
            _signalBus = signalBus;
            _progressStorage = progressStorage;
            _poolService = poolService;
        }

        private void SubscribeSignals()
        {
            _signalBus.Subscribe<TappedCellSignal>(OnTappedCell);
            _signalBus.Subscribe<PostLevelSignal>(OnPostLevel);
        }
        
        private void UnsubscribeSignals()
        {
            _signalBus.Unsubscribe<TappedCellSignal>(OnTappedCell);
            _signalBus.Unsubscribe<PostLevelSignal>(OnPostLevel);
        }
        
        public void OnInitialize()
        {
            SubscribeSignals();
            OnPrepare();
        }

        public void Dispose()
        {
            UnsubscribeSignals();
        }
        
        private void OnPrepare()
        {
            _signalBus.Fire(new LevelUpdatedSignal(){Level = _progressStorage.Progress.CurrentLevel});
            
            _gridManager.Initialize();
            var cells = _gridManager.GetCells();
            
            _ = _poolService.GetGameAsset<BoardView>();
            
            for (int i = 0; i < cells.Length; i++)
            {
                var listener = _poolService.GetGameAsset<CellListener>();
                listener.transform.position = cells[i].WorldPosition;
                listener.SetCellIndex(cells[i].Index);
            }

            _inputBlocker.Unblock();
        }

        private void OnTappedCell(TappedCellSignal obj)
        {
            var cell = _gridManager.GetCell(obj.CellIndex);

            if (cell.Items.Any(item => item.Layer == CollisionLayer.Layer0))
            {
                // cell is marked before
                return;
            }

            var generatedId = IdGenerator.GenerateId();
            
            var markItem = new Item()
            {
                Kind = (int)ItemKind.Cross,
                Layer = CollisionLayer.Layer0,
                CellIndex = obj.CellIndex,
                Id = generatedId,
            };

            var prefab = _itemLookup.GetPrefabByType(ItemKind.Cross);
            MarkView instance = _poolService.GetPrefabAsset<MarkView>(prefab);
            instance.gameObject.SetActive(true);
            instance.transform.position = cell.WorldPosition;
            instance.UpdateAsPlayer();

            var graphic = new Graphic()
            {
                GraphicReference = instance,
                ItemId = generatedId,
            };
            
            _graphicManager.SetGraphic(graphic);
            
            cell.AddItemOnTop(markItem);

            if (CheckStatus())
            {
                return;
            }
            
            ComputerMove();
            
            if (CheckStatus())
            {
                return;
            }
        }

        private async UniTaskVoid UpdateProgressForVictoryAsync()
        {
            var progress = _progressStorage.Progress;
            progress.CurrentLevel += 1;
            await _progressStorage.SaveProgressAsync(progress);
        }

        private int CalculateComputerMove()
        {
            var cells = _gridManager.GetCells();
            var emptyCellList = new List<int>();

            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].Items.Count == 0)
                {
                    emptyCellList.Add(cells[i].Index);
                }
            }
            
            var randomCellIndex = emptyCellList[Random.Range(0, emptyCellList.Count)];
            return randomCellIndex;
        }
        
        private void ComputerMove()
        {
            var computerMoveCellIndex = CalculateComputerMove();
            var generatedId = IdGenerator.GenerateId();
            var cell = _gridManager.GetCell(computerMoveCellIndex);
            
            var markItem = new Item()
            {
                Kind = (int)ItemKind.Circle,
                Layer = CollisionLayer.Layer0,
                CellIndex = computerMoveCellIndex,
                Id = generatedId,
            };

            GameObject prefab = _itemLookup.GetPrefabByType(ItemKind.Circle);
            MarkView instance = _poolService.GetPrefabAsset<MarkView>(prefab);
            instance.gameObject.SetActive(true);
            instance.transform.position = cell.WorldPosition;
            instance.UpdateAsComputer();
            
            var graphic = new Graphic()
            {
                GraphicReference = instance,
                ItemId = generatedId,
            };
            
            _graphicManager.SetGraphic(graphic);
            
            cell.AddItemOnTop(markItem);
        }
        
        public bool CheckStatus()
        {
            var cells = _gridManager.GetCells();

            bool emptyCellExists = false;
            bool playerWon = false;
            bool computerWon = false;
            
            foreach (var condition in Constants.WinConditions)
            {
                int a = condition[0];
                int b = condition[1];
                int c = condition[2];

                if (cells[a].Items.Count > 0 && cells[b].Items.Count > 0 && cells[c].Items.Count > 0)
                {
                    if (cells[a].Items[0].Kind == cells[b].Items[0].Kind && cells[b].Items[0].Kind == cells[c].Items[0].Kind)
                    {
                        if (cells[a].Items[0].Kind == (int)ItemKind.Circle)
                        {
                            OnDefeat();
                            return true;
                        }
                        
                        OnVictory();
                        return true;
                    }
                }
                else
                {
                    emptyCellExists = true;
                }
            }

            if (emptyCellExists == false)
            {
                OnDraw();
                return true;
            }

            return false;
        }

        private void OnDraw()
        {
            _inputBlocker.Block();
            var popupArgs = new GameEndPopupArguments()
            {
                Text = "Draw!",
                ButtonText = "Restart",
            };
            
            _popupService.ShowPopup<GameEndPopup>("Common", popupArgs);
        }

        private void OnDefeat()
        {
            _inputBlocker.Block();
            var popupArgs = new GameEndPopupArguments()
            {
                Text = "You Lose!",
                ButtonText = "Restart",
            };
            
            _popupService.ShowPopup<GameEndPopup>("Common", popupArgs);
        }

        private void OnVictory()
        {
            _inputBlocker.Block();
            UpdateProgressForVictoryAsync().Forget();
            var popupArgs = new GameEndPopupArguments()
            {
                Text = "You Win!",
                ButtonText = "Next",
            };
            
            _popupService.ShowPopup<GameEndPopup>("Common", popupArgs);
        }

        public void OnPostLevel(PostLevelSignal signal)
        {
            _poolService.ReturnAll();
            _graphicManager.Clear();
            OnPrepare();
        }
    }
}