using System;
using UnityEngine;

namespace RollbackSys
{
    public enum ERollbackDirection
    {
        None,
        Backward,
        Forward
    }
    
    public class RollbackManager : MonoBehaviour
    {
        private static RollbackManager _instance;

        public static RollbackManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<RollbackManager>();

                return _instance;
            }
        }

        private bool _isActive;

        public Action OnRollbackActivated { get; set; }
        public Action OnRollbackDeactivated { get; set; }
        public Action<ERollbackDirection> OnRollbackRequested { get; set; }
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.Z))
            {
                TryActivateRollback();
                
                OnRollbackRequested?.Invoke(ERollbackDirection.Backward);
            }
            else if (Input.GetKey(KeyCode.C))
            {
                TryActivateRollback();
                
                OnRollbackRequested?.Invoke(ERollbackDirection.Forward);
            }
            if (Input.GetKeyDown(KeyCode.Space))
                SwitchRollback();
        }

        private void TryActivateRollback()
        {
            if (!_isActive)
                OnRollbackActivated?.Invoke();

            _isActive = !_isActive;
        }
        
        
        private void SwitchRollback()
        {
            if (!_isActive)
                OnRollbackActivated?.Invoke();
            else
                OnRollbackDeactivated?.Invoke();

            _isActive = !_isActive;
        }
    }
}