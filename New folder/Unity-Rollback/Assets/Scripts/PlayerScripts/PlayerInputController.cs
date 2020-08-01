using System;
using UnityEngine;

namespace PlayerSys
{
    public class PlayerInputController : MonoBehaviour
    {
        public Action OnFireTriggered { get; set; }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                OnFireTriggered?.Invoke();
        }
    }
}
