using System;
using UnityEngine;

namespace RewindSystem
{
    public enum ERewindDirection
    {
        None,
        Backward,
        Forward
    }
    
    public class RewindManager : MonoBehaviour
    {
        private static RewindManager _instance;

        public static RewindManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<RewindManager>();

                return _instance;
            }
        }

        private bool _isActive;

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
        }

        public Action OnRewindModeActivated { get; set; }
        public Action OnRewindModeDeactivated { get; set; }
        public Action<ERewindDirection> OnRewindRequested { get; set; }
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.Z))
            {
                TryActivateRewindMode();
                
                OnRewindRequested?.Invoke(ERewindDirection.Backward);
            }
            else if (Input.GetKey(KeyCode.C))
            {
                TryActivateRewindMode();
                
                OnRewindRequested?.Invoke(ERewindDirection.Forward);
            }
            if (Input.GetKeyDown(KeyCode.Space))
                SwitchRewindMode();
        }

        private void TryActivateRewindMode()
        {
            if (!_isActive)
            {
                _isActive = true;
                
                OnRewindModeActivated?.Invoke();
            }
        }
        
        
        private void SwitchRewindMode()
        {
            if (!_isActive)
                OnRewindModeActivated?.Invoke();
            else
                OnRewindModeDeactivated?.Invoke();

            _isActive = !_isActive;
        }
    }
}