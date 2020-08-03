using System;
using UnityEngine;

namespace PlayerSys
{
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        
        
        public Action OnFireTriggered { get; set; }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                OnFireTriggered?.Invoke();

            CheckCharacterControllerInput();
        }

        private void CheckCharacterControllerInput()
        {
            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");

            Vector3 right = transform.right.normalized * horizontalInput;

            Vector3 forward = transform.forward.normalized * verticalInput;
            
            _characterController.SimpleMove(right + forward);
        }
    }
}
