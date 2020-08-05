using System;
using UnityEngine;

namespace PlayerSys
{
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;

        [SerializeField] private PlayerRotationController _playerRotationController;
        
        public Action OnFireTriggered { get; set; }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                OnFireTriggered?.Invoke();

            CheckCharacterControllerInput();

            CheckMouseInput();
        }

        private void CheckCharacterControllerInput()
        {
            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");

            Vector3 right = transform.right.normalized * horizontalInput;

            Vector3 forward = transform.forward.normalized * verticalInput;
            
            _characterController.SimpleMove((right + forward) * 2.5f);
        }

        private void CheckMouseInput()
        {
            float horizontalInput = Input.GetAxis("Mouse X");

            _playerRotationController.Rotate(horizontalInput);
        }
    }
}
