using System;
using Cysharp.Threading.Tasks;
using MavisCase.Common;
using MavisCase.Common.Assets;
using MavisCase.Common.Communication;
using MavisCase.Common.GridSystem;
using MavisCase.Common.Helpers;
using MavisCase.Common.InputSystem;
using MavisCase.Common.Panels;
using MavisCase.Common.Persistence;
using MavisCase.Common.Pooling;
using MavisCase.Common.Popups;
using MavisCase.Common.Signals;

namespace MavisCase.Games.MemoryGame
{
    public class GameManager : IDisposable
    {
        private int _openTotal = 0;
        private int _openCardKind = -1;
        private int _openCardIndex = -1;
        private int _moves = 0;
        
        private IGridManagerInitializer _gridManager;
        private ItemLookup _itemLookup;
        private GraphicManager _graphicManager;
        private InputBlocker _inputBlocker;
        private PopupService _popupService;
        private SignalBus _signalBus;
        private IGridConfig _gridConfig;
        private ProgressStorage<Progress> _progressStorage;
        private PoolService _poolService;
        
        public GameManager(
            IGridManagerInitializer gridManager, 
            ItemLookup itemLookup, 
            GraphicManager graphicManager, 
            InputBlocker inputBlocker, 
            PopupService popupService, 
            SignalBus signalBus, 
            IGridConfig gridConfig, 
            ProgressStorage<Progress> progressStorage,
            PoolService poolService
        ){
            _gridManager = gridManager;
            _itemLookup = itemLookup;
            _graphicManager = graphicManager;
            _inputBlocker = inputBlocker;
            _popupService = popupService;
            _signalBus = signalBus;
            _gridConfig = gridConfig;
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
            _openCardKind = -1;
            _openCardIndex = -1;
            _openTotal = 0;
            
            _signalBus.Fire(new LevelUpdatedSignal(){ Level = _progressStorage.Progress.CurrentLevel });
            _moves = Constants.MoveCount;
            
            _gridManager.Initialize();
            var cells = _gridManager.GetCells();
            
            // Create Board
            _ = _poolService.GetGameAsset<BoardView>();
            
             // Create Cells
            for (int i = 0; i < cells.Length; i++)
            {
                var listener = _poolService.GetGameAsset<CellListener>();
                listener.transform.position = cells[i].WorldPosition;
                listener.SetCellIndex(cells[i].Index);
            }

            // Create Letter Cards
            var index = 0;
            var limit = _gridConfig.ColCount;
            var limiter = 0;
            var cards = new ItemKind[cells.Length];
            
            foreach (ItemKind type in Enum.GetValues(typeof(ItemKind)))
            {
                for (int k = 0; k < cells.Length / limit; k++)
                {
                    cards[index] = type;
                    index++;
                }

                limiter++;
                if (limiter == limit)
                {
                    break;
                }
            }
            
            ArrayHelper.Shuffle(cards);

            for (int c = 0; c < cards.Length; c++)
            {
                var generatedId = IdGenerator.GenerateId();
            
                var markItem = new Item()
                {
                    Kind = (int)cards[c],
                    Layer = CollisionLayer.Layer0,
                    CellIndex = c,
                    Id = generatedId,
                };

                var prefab = _itemLookup.GetPrefab(cards[c]);
                CardView cardInstance = _poolService.GetPrefabAsset<CardView>(prefab);
                cardInstance.gameObject.SetActive(true);
                cardInstance.SetLetter((byte)cards[c]);

                var graphic = new Graphic()
                {
                    GraphicReference = cardInstance,
                    ItemId = generatedId,
                };
                _graphicManager.SetGraphic(graphic);

                var cell = _gridManager.GetCell(c);
                cardInstance.transform.position = cell.WorldPosition;
                cell.AddItemOnTop(markItem);
            }

            _signalBus.Fire(new MoveChangedSignal(){Count = Constants.MoveCount});
            _inputBlocker.Unblock();
        }

        private void OnTappedCell(TappedCellSignal tappedCell)
        {
            var cell = _gridManager.GetCell(tappedCell.CellIndex);

            if (cell.Items.Count <= 0)
            {
                return;
            }
            
            var item = cell.Items[0];

            if (tappedCell.CellIndex == _openCardIndex)
            {
                return;
            }

            _moves--;
            _signalBus.Fire(new MoveChangedSignal(){ Count = _moves });
            
            if (_openCardKind == -1)
            {
                _openCardKind = cell.Items[0].Kind;
                _openCardIndex = tappedCell.CellIndex;
                var graphic = _graphicManager.GetGraphic(item.Id);
                var cardView = (CardView)graphic.GraphicReference;
                cardView.OnOpen();
                return;
            }

            if (item.Kind == _openCardKind)
            {
                var prevcell = _gridManager.GetCell(_openCardIndex);
                
                var prevgraphic = _graphicManager.GetGraphic(prevcell.Items[0].Id);
                var prevcardView = (CardView)prevgraphic.GraphicReference;
                prevcardView.OnDisappearDelayed();
                prevcell.RemoveItemFromTop();
                
                var graphic = _graphicManager.GetGraphic(cell.Items[0].Id);
                var cardView = (CardView)graphic.GraphicReference;
                cardView.OnOpen();
                cardView.OnDisappearDelayed();
                cell.RemoveItemFromTop();
                
                _openCardKind = -1;
                _openCardIndex = -1;
                _openTotal += 2;

                if (_openTotal == _gridManager.GetCells().Length)
                {
                    OnVictory();
                    return;
                }

                if (_moves == 0)
                {
                    OnDefeat(); 
                }
                return;
            }

            if (item.Kind != _openCardKind)
            {
                var graphic = _graphicManager.GetGraphic(item.Id);
                var cardView = (CardView)graphic.GraphicReference;
                cardView.OnOpen();
                cardView.OnCloseDelayed();
                
                var prevcell = _gridManager.GetCell(_openCardIndex);
                
                var prevgraphic = _graphicManager.GetGraphic(prevcell.Items[0].Id);
                var prevcardView = (CardView)prevgraphic.GraphicReference;
                prevcardView.OnCloseDelayed();
                
                _openCardKind = -1;
                _openCardIndex = -1;

                if (_moves == 0)
                {
                    OnDefeat();
                }
            }
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

        private async UniTaskVoid UpdateProgressForVictoryAsync()
        {
            var progress = _progressStorage.Progress;
            progress.CurrentLevel += 1;
            await _progressStorage.SaveProgressAsync(progress);
        }
        
        public void OnPostLevel(PostLevelSignal signal)
        {
            _poolService.ReturnAll();
            _graphicManager.Clear();
            OnPrepare();
        }
    }
}