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
        
        private Stack<T> _timeStamps = new Stack<T>();
        private Stack<T> _backUpStamps = new Stack<T>();

        private bool _canSaveTimeStamps = true;

        protected abstract bool ExecuteTimeStamp(T timeStamp);
        
        protected abstract T GetTimeStamp();
        
        protected virtual void Awake()
        {
            RegisterToRollbackManager();
        }

        private void Update()
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
                _timeStamps.Pop();
                
                _timeStamps.Push(GetTimeStamp());
                
                return;
            }
            
            _timeStamps.Push(GetTimeStamp());
        }

        private void OnRollbackActivated()
        {
            _rigidbody.isKinematic = true;

            _canSaveTimeStamps = false;
            
            RollbackActivatedCustomActions();
        }

        private void OnRollbackDeactivated()
        {
            _rigidbody.isKinematic = false;
            
            _canSaveTimeStamps = true;

            RollbackDectivatedCustomActions();
        }

        private void OnRollbackRequested(ERollbackDirection eRollbackDirection)
        {
            if (eRollbackDirection == ERollbackDirection.Backward)
            {
                T timeStamp = _timeStamps.Pop();
                
                _backUpStamps.Push(timeStamp);

                ExecuteTimeStamp(timeStamp);
            }
            
            else if (eRollbackDirection == ERollbackDirection.Forward)
            {
                T timeStamp = _backUpStamps.Pop();
                
                _timeStamps.Push(timeStamp);

                ExecuteTimeStamp(timeStamp);
            }
        }

        protected virtual void RollbackActivatedCustomActions()
        {
        }

        protected virtual void RollbackDectivatedCustomActions()
        {
        }
    }
}