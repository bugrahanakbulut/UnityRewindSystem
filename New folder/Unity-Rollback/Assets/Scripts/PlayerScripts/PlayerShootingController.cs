using GunSys;
using UnityEngine;

namespace PlayerSys
{
    public class PlayerShootingController : MonoBehaviour
    {
        [SerializeField] private PlayerInputController _playerInputController;

        [SerializeField] private Gun _gun;
        
        private void Awake()
        {
            _playerInputController.OnFireTriggered += OnFireTriggered;
        }

        private void OnDestroy()
        {
            _playerInputController.OnFireTriggered -= OnFireTriggered;
        }

        private void OnFireTriggered()
        {
            _gun.Shoot();
        }
    }
}