using MavisCase.Common.InputKinds;
using MavisCase.Common.InputSystem;
using UnityEngine;
using Zenject;

namespace MavisCase.Common.InputHandlers
{
    public class TouchHandler : ITickable
    {
        private readonly Camera _camera;

        private readonly InputBlocker _inputBlocker;
        
        private readonly IInputConfig _inputConfig;

        private bool _down = false;

        private float _downTime;

        private Vector2 _downPosition;

        private IListener _downElement = null;

        public TouchHandler(
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
            if(Input.touchCount == 0)
                return;

            var t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                _down = true;
                _downTime = Time.unscaledTime;
                _downPosition = t.position;

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

                var duration = Time.unscaledTime - _downTime;
                var pos = t.position;

                if(pos.y - _inputConfig.SwipeThreshold/2 > _downPosition.y)
                {
                    (_downElement as IListener<TouchSwipeUp>)?.OnInteraction();
                    Clear();
                }
                else if(pos.y < _downPosition.y - _inputConfig.SwipeThreshold/2)
                {
                    (_downElement as IListener<TouchSwipeDown>)?.OnInteraction();
                    Clear();
                }
                else if(t.phase == TouchPhase.Ended && duration <= _inputConfig.HoldDuration)
                {
                    (_downElement as IListener<TouchTap>)?.OnInteraction();
                    Clear();
                }
                else if(duration > _inputConfig.HoldDuration)
                {
                    (_downElement as IListener<TouchHold>)?.OnInteraction();
                    Clear();
                }
            }
        }
    }
}
