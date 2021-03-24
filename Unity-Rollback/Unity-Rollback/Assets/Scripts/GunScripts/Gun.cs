using PoolingSystem;
using UnityEngine;

namespace GunSystem
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Bullet _bullet;

        [SerializeField] private Transform _barrelTransform;
    
        [SerializeField] private BulletFactory _bulletFactory = new BulletFactory();

        [SerializeField] private int _initialBulletCount = 5;

        [SerializeField] private float _bulletVelocity = 5f;
        
        private Pool<
            BulletActivationInfo,
            Bullet,
            BulletFactory> _pool;

        private Pool<
            BulletActivationInfo,
            Bullet,
            BulletFactory> _Pool
        {
            get
            {
                if (_pool == null)
                    _pool = new Pool<
                        BulletActivationInfo,
                        Bullet, 
                        BulletFactory>(_bulletFactory, _initialBulletCount);

                return _pool;
            }
        }
        
        public void Shoot()
        {
            GameObject bulletObj = Instantiate(_bullet.gameObject, _barrelTransform);

            bulletObj.GetComponent<Bullet>().Throw(new BulletActivationInfo(_bulletVelocity, _barrelTransform.position, _barrelTransform.rotation));
        }
    }
}