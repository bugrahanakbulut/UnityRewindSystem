using System.Collections.Generic;
using UnityEngine;

namespace RewindSystem
{
    public abstract class RewindableTimeStampBase {}
    
    public abstract class RewindableBase : MonoBehaviour
    {
        [SerializeField] protected int _maxTimeStampCount = 180;

        protected List<RewindableTimeStampBase> _timeStamps = new List<RewindableTimeStampBase>();
        protected List<RewindableTimeStampBase> _backUpTimeStamps = new List<RewindableTimeStampBase>();
        
        protected abstract bool ExecuteTimeStamp(RewindableTimeStampBase timeStamp);
        
        protected abstract RewindableTimeStampBase GetTimeStamp();

        protected abstract void OnRewindActivated();
        protected abstract void OnRewindDeactivated();
        protected abstract void OnRewindRequested(ERewindDirection eRewindDirection);
        
        
        protected virtual void Awake()
        {
            RegisterToRollbackManager();

            if (RewindManager.Instance.IsActive)
            {
                _canSaveTimeStamps = false;
                
                SpawnedAtActiveRewindCustomActions();
            }
        }

        protected bool _canSaveTimeStamps = true;
        
        protected void FixedUpdate()
        {
            UpdateTimeStamps();
        }

        protected void OnDestroy()
        {
            UnregisterFromRollbackManager();
        }

        private void RegisterToRollbackManager()
        {
            RewindManager.Instance.OnRewindModeActivated += OnRewindActivated;
            RewindManager.Instance.OnRewindModeDeactivated += OnRewindDeactivated;
            RewindManager.Instance.OnRewindRequested += OnRewindRequested;
        }

        private void UnregisterFromRollbackManager()
        {
            if (RewindManager.Instance == null) return;
            
            RewindManager.Instance.OnRewindModeActivated -= OnRewindActivated;
            RewindManager.Instance.OnRewindModeDeactivated -= OnRewindDeactivated;
            RewindManager.Instance.OnRewindRequested -= OnRewindRequested;
        }
        
        private void UpdateTimeStamps()
        {
            if (!_canSaveTimeStamps) return; 
            
            if (_timeStamps.Count == _maxTimeStampCount)
            {
                _timeStamps.RemoveAt(0);
                
                _timeStamps.Add(GetTimeStamp());
                
                return;
            }
            
            _timeStamps.Add(GetTimeStamp());
        }
        
        protected virtual void RewindActivatedCustomActions()
        {
        }

        protected virtual void RewindDectivatedCustomActions()
        {
        }
        
        protected virtual void SpawnedAtActiveRewindCustomActions()
        {
        }
    }
}
