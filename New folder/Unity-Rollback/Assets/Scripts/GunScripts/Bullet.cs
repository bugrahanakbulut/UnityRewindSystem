using UnityEngine;

namespace GunSys
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        
        [SerializeField] private Vector3 _bulletForce = new Vector3(0, 0, 100f);
        
        public void Throw()
        {
            _rigidbody.AddForce(_bulletForce, ForceMode.Force);
        }
    }
}