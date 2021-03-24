using System;
using GunSystem;
using PoolingSystem;
using UnityEngine;

[Serializable]
public class BulletFactory : IFactory<BulletActivationInfo, Bullet>
{
    [SerializeField] private Bullet _referenceBullet = null;

    [SerializeField] private Transform _bulletHolderTransform = null;
    
    public Action OnCreatedPoolObject { get; set; }
    
    public Bullet Create()
    {
        Bullet createdBullet = GameObject.Instantiate(
            _referenceBullet.gameObject, 
            _bulletHolderTransform)
                .GetComponent<Bullet>();

        return createdBullet;
    }
}
