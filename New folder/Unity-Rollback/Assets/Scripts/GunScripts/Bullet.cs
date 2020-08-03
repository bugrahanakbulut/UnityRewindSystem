using UnityEngine;

namespace GunSys
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        
        [SerializeField] private float _bulletForce = 2500;
        
        public void Throw()
        {
            _rigidbody.AddForce(_bulletForce * transform.forward.normalized, ForceMode.Force);
        }
    }
}