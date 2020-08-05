using UnityEngine;

namespace GunSystem
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Bullet _bullet;

        [SerializeField] private Transform _barrelTransform;
    
        public void Shoot()
        {
            Bullet bullet = Instantiate(_bullet);

            bullet.transform.position = _barrelTransform.position;

            bullet.transform.rotation = _barrelTransform.rotation;
            
            bullet.Throw();
        }
    }
}