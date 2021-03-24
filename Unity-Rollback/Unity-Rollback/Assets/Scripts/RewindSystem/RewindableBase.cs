using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RewindSystem
{
    public abstract class RewindableTimeStampBase { }
    
    public abstract class RewindableBase<T> : MonoBehaviour
    where T : RewindableTimeStampBase
    {
        [SerializeField] protected int _maxTimeStampCount = 180;

        private List<T> _timeStamps = new List<T>();
        
        private List<T> _backUpTimeStamps = new List<T>();
        
        protected abstract bool ExecuteTimeStamp(T timeStamp);
        
        protected abstract T GetTimeStamp();

        protected bool _canSaveTimeStamps = false;
        
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
        
        protected void Update()
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
            
            RewindActivatedCustomActions();
        }

        private void OnRewindModeDeactivated()
        {
            _timeStamps = new List<T>();
            
            _backUpTimeStamps = new List<T>();

            SetCanSaveTimeStamp(true);

            RewindDectivatedCustomActions();
        }

        private void OnRewindRequested(ERewindDirection eRewindDirection)
        {
            if (eRewindDirection == ERewindDirection.Backward && _timeStamps.Count > 0)
            {
                T timeStamp =_timeStamps[_timeStamps.Count - 1];
                
                _timeStamps.RemoveAt(_timeStamps.Count - 1);
                
                _backUpTimeStamps.Add(timeStamp);

                ExecuteTimeStamp(timeStamp);
            }
            
            else if (eRewindDirection == ERewindDirection.Forward && _backUpTimeStamps.Count > 0)
            {
                T timeStamp = _backUpTimeStamps[_backUpTimeStamps.Count - 1];
                
                _backUpTimeStamps.RemoveAt(_backUpTimeStamps.Count - 1);
                
                _timeStamps.Add(timeStamp);

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

        protected virtual void RewindDectivatedCustomActions() { }

        protected virtual void OnRewindRequestedCustomActions(ERewindDirection eRewindDirection) { }
    }
}
