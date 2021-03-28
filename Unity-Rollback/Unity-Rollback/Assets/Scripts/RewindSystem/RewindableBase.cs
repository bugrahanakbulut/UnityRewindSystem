using System.Collections.Generic;
using UnityEngine;

namespace RewindSystem
{
    public abstract class RewindableTimeStampBase { }
    
    public abstract class RewindableBase<T> : MonoBehaviour
    where T : RewindableTimeStampBase
    {
        [SerializeField] protected int _maxTimeStampCount = 180;

        protected List<T> _timeStamps = new List<T>();
        
        private List<T> _backUpTimeStamps = new List<T>();

        protected bool _canSaveTimeStamps = false;

        protected T _lastExecutedTimeStamp = null;
        
        protected abstract bool ExecuteTimeStamp(T timeStamp);
        
        protected abstract T GetTimeStamp();

        protected void Awake()
        {
            RegisterToRollbackManager();

            SetCanSaveTimeStamp(!RewindManager.Instance.IsRewindActive);
            
            if (RewindManager.Instance.IsRewindActive)
            {
                OnRewindModeActivated();
            }

            AwakeCustomActions();
        }
        
        protected void FixedUpdate()
        {
            UpdateTimeStamps();
        }

        protected void OnDestroy()
        {
            UnregisterFromRollbackManager();
            
            OnDestroyCustomActions();
        }

        private void RegisterToRollbackManager()
        {
            RewindManager.Instance.OnRewindModeActivated += OnRewindModeActivated;
            RewindManager.Instance.OnRewindModeDeactivated += OnRewindModeDeactivated;
            RewindManager.Instance.OnRewindRequested += OnRewindRequested;
        }

        private void UnregisterFromRollbackManager()
        {
            if (RewindManager.Instance == null) return;
            
            RewindManager.Instance.OnRewindModeActivated -= OnRewindModeActivated;
            RewindManager.Instance.OnRewindModeDeactivated -= OnRewindModeDeactivated;
            RewindManager.Instance.OnRewindRequested -= OnRewindRequested;
        }
        
        private void UpdateTimeStamps()
        {
            if (!_canSaveTimeStamps) return; 
            
            if (_timeStamps.Count == _maxTimeStampCount)
            {
                _timeStamps.RemoveAt(0);
            }
            
            _timeStamps.Add(GetTimeStamp());
        }

        private void OnRewindModeActivated()
        {
            SetCanSaveTimeStamp(false);

            if (_timeStamps.Count > 0)
            {
                _lastExecutedTimeStamp = _timeStamps[_timeStamps.Count - 1];
            }
            
            RewindActivatedCustomActions();    
        }

        private void OnRewindModeDeactivated()
        {
            _timeStamps = new List<T>();
            
            _backUpTimeStamps = new List<T>();

            SetCanSaveTimeStamp(true);

            RewindDeactivatedCustomActions();
            
            if (_lastExecutedTimeStamp != null)
            {
                ExecuteTimeStamp(_lastExecutedTimeStamp);
                
                _timeStamps.Add(_lastExecutedTimeStamp);

                _lastExecutedTimeStamp = null;
            }
        }

        private void OnRewindRequested(ERewindDirection eRewindDirection)
        {
            if (eRewindDirection == ERewindDirection.Backward && _timeStamps.Count > 0)
            {
                T timeStamp =_timeStamps[_timeStamps.Count - 1];
                
                _timeStamps.RemoveAt(_timeStamps.Count - 1);
                
                _backUpTimeStamps.Add(timeStamp);

                _lastExecutedTimeStamp = timeStamp;
                
                ExecuteTimeStamp(timeStamp);
            }
            
            else if (eRewindDirection == ERewindDirection.Forward && _backUpTimeStamps.Count > 0)
            {
                T timeStamp = _backUpTimeStamps[_backUpTimeStamps.Count - 1];
                
                _backUpTimeStamps.RemoveAt(_backUpTimeStamps.Count - 1);
                
                _timeStamps.Add(timeStamp);
                
                _lastExecutedTimeStamp = timeStamp;

                ExecuteTimeStamp(timeStamp);
            }
            
            OnRewindRequestedCustomActions(eRewindDirection);
        }

        protected virtual void SetCanSaveTimeStamp(bool value)
        {
            _canSaveTimeStamps = value;
        }
        
        protected virtual void AwakeCustomActions() { }
        
        protected virtual void OnDestroyCustomActions() { }
        
        protected virtual void RewindActivatedCustomActions() { }

        protected virtual void RewindDeactivatedCustomActions() { }

        protected virtual void OnRewindRequestedCustomActions(ERewindDirection eRewindDirection) { }
    }
}
