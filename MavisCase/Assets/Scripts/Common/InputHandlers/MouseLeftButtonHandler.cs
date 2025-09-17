using MavisCase.Common.InputKinds;
using MavisCase.Common.InputSystem;
using UnityEngine;
using Zenject;

namespace MavisCase.Common.InputHandlers
{
    public class MouseLeftButtonHandler : ITickable
    {
        private readonly Camera _camera;

        private readonly InputBlocker _inputBlocker;
        
        private readonly IInputConfig _inputConfig;

        private bool _down = false;

        private float _downTime;

        private Vector2 _downPosition;

        private IListener _downElement = null;

        public MouseLeftButtonHandler(
            Camera camera,
            InputBlocker inputBlocker,
            IInputConfig inputConfig
        ){
            _camera = camera;
            _inputBlocker = inputBlocker;
            _inputConfig = inputConfig;
        }

        private void Clear()
        {
            _down = false;
            _downTime = 0;
            _downPosition = default;
            _downElement = null;
        }

        public void Tick()
        {
            if(Input.GetMouseButtonDown(0))
            {
                _down = true;
                _downTime = Time.unscaledTime;
                _downPosition = Input.mousePosition;

                RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(_downPosition), Vector2.zero);

                if(hit.collider != null)
                {
                    if(hit.collider.TryGetComponent(out IListener component))
                    {
                        _downElement = component;
                    }
                }
            }
            else
            {
                if(!_down)
                    return;

                if(_inputBlocker.IsInputDisabled)
                {
                    Clear();
                    return;
                }

                var pos = Input.mousePosition;
                var duration = Time.unscaledTime - _downTime;

                if(pos.y - _inputConfig.SwipeThreshold > _downPosition.y)
                {
                    (_downElement as IListener<MouseLeftSwipeUp>)?.OnInteraction();
                    Clear();
                }
                else if(pos.y < _downPosition.y - _inputConfig.SwipeThreshold)
                {
                    (_downElement as IListener<MouseLeftSwipeDown>)?.OnInteraction();
                    Clear();
                }
                else if(Input.GetMouseButtonUp(0) && duration <= _inputConfig.HoldDuration)
                {
                    (_downElement as IListener<MouseLeftClick>)?.OnInteraction();
                    Clear();
                }
                else if(Input.GetMouseButtonUp(0) && duration > _inputConfig.HoldDuration)
                {
                    (_downElement as IListener<MouseLeftHoldClick>)?.OnInteraction();
                    Clear();
                }
                
            }
        }
    }
}
