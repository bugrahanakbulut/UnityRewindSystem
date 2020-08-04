using UnityEngine;

namespace PlayerSys
{
    public class PlayerRotationController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 5;
        
        public void Rotate(float x)
        {
            Vector3 euler = transform.rotation.eulerAngles;

            euler.y += (x * _rotationSpeed);
            
            transform.rotation = Quaternion.Euler(euler);
        }
    }
}