using System.Collections.Generic;
using UnityEngine;

namespace RewindSystem
{
    public abstract class RewindableTimeStampBase {}
    
    public abstract class PhysicalRewindableBase<T> : MonoBehaviour
    where T : RewindableTimeStampBase
    {
        [SerializeField] protected Rigidbody _rigidbody;

        [SerializeField] protected int _maxTimeStampCount = 180;
        
        [SerializeField] private List<T> _timeStamps = new List<T>();
        [SerializeField] private List<T> _backUpTimeStamps = new List<T>();

        protected bool _canSaveTimeStamps = true;

        protected abstract bool ExecuteTimeStamp(T timeStamp);
        
        protected abstract T GetTimeStamp();
        
        protected virtual void Awake()
        {
            RegisterToRollbackManager();

            if (RewindManager.Instance.IsActive)
            {
                _canSaveTimeStamps = false;
                
                SpawnedAtActiveRewindCustomActions();
            }
        }

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

        protected void OnRewindActivated()
        {
            _rigidbody.isKinematic = true;

            _canSaveTimeStamps = false;
            
            RewindActivatedCustomActions();
        }

        protected virtual void OnRewindDeactivated()
        {
            _timeStamps = new List<T>();
            
            _backUpTimeStamps = new List<T>();
            
            _rigidbody.isKinematic = false;
            
            _canSaveTimeStamps = true;

            RewindDectivatedCustomActions();
        }

        protected virtual void OnRewindRequested(ERewindDirection eRewindDirection)
        {
            if (eRewindDirection == ERewindDirection.Backward && _timeStamps.Count > 0)
            {
                T timeStamp = _timeStamps[_timeStamps.Count - 1];
                
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