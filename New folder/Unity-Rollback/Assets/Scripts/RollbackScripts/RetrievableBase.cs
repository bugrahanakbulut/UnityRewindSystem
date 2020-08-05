using System.Collections.Generic;
using UnityEngine;

namespace RollbackSys
{
    public abstract class RetrievableTimeStampBase {}
    
    public abstract class RetrievableBase<T> : MonoBehaviour
    where T : RetrievableTimeStampBase
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

            if (RollbackManager.Instance.IsActive)
            {
                _canSaveTimeStamps = false;
                
                SpawnedAtActiveRollbackCustomActions();
            }
        }

        private void FixedUpdate()
        {
            UpdateTimeStamps();
        }

        protected void OnDestroy()
        {
            UnregisterFromRollbackManager();
        }

        private void RegisterToRollbackManager()
        {
            RollbackManager.Instance.OnRollbackActivated += OnRollbackActivated;
            RollbackManager.Instance.OnRollbackDeactivated += OnRollbackDeactivated;
            RollbackManager.Instance.OnRollbackRequested += OnRollbackRequested;
        }

        private void UnregisterFromRollbackManager()
        {
            if (RollbackManager.Instance == null) return;
            
            RollbackManager.Instance.OnRollbackActivated -= OnRollbackActivated;
            RollbackManager.Instance.OnRollbackDeactivated -= OnRollbackDeactivated;
            RollbackManager.Instance.OnRollbackRequested -= OnRollbackRequested;
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

        protected virtual void OnRollbackActivated()
        {
            _rigidbody.isKinematic = true;

            _canSaveTimeStamps = false;
            
            RollbackActivatedCustomActions();
        }

        protected virtual void OnRollbackDeactivated()
        {
            _timeStamps = new List<T>();
            
            _backUpTimeStamps = new List<T>();
            
            _rigidbody.isKinematic = false;
            
            _canSaveTimeStamps = true;

            RollbackDectivatedCustomActions();
        }

        protected virtual void OnRollbackRequested(ERollbackDirection eRollbackDirection)
        {
            if (eRollbackDirection == ERollbackDirection.Backward && _timeStamps.Count > 0)
            {
                T timeStamp = _timeStamps[_timeStamps.Count - 1];
                
                _timeStamps.RemoveAt(_timeStamps.Count - 1);
                
                _backUpTimeStamps.Add(timeStamp);

                ExecuteTimeStamp(timeStamp);
            }
            
            else if (eRollbackDirection == ERollbackDirection.Forward && _backUpTimeStamps.Count > 0)
            {
                T timeStamp = _backUpTimeStamps[_backUpTimeStamps.Count - 1];
                
                _backUpTimeStamps.RemoveAt(_backUpTimeStamps.Count - 1);
                
                _timeStamps.Add(timeStamp);

                ExecuteTimeStamp(timeStamp);
            }
        }

        protected virtual void RollbackActivatedCustomActions()
        {
        }

        protected virtual void RollbackDectivatedCustomActions()
        {
        }

        protected virtual void SpawnedAtActiveRollbackCustomActions()
        {
        }
    }
}