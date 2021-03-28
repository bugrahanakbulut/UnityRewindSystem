using System;
using PoolingSystem;
using UnityEngine;

namespace GunSystem
{
    public class BulletActivationInfo : PoolObjectActivationInfo
    {
        public float BulletVelocity { get; }
        
        public Vector3 BulletInitialPosition { get; }
        
        public Quaternion BulletRotation { get; }

        public BulletActivationInfo(float bulletVelocity, Vector3 bulletInitialPosition, Quaternion bulletRotation)
        {
            BulletVelocity = bulletVelocity;
            BulletInitialPosition = bulletInitialPosition;
            BulletRotation = bulletRotation;
        }
    }
    
    public class Bullet : PoolGameObject<BulletActivationInfo>
    {
        [SerializeField] private Rigidbody _rigidbody = null;

        // TODO : integrate with pool asap
        
        public Action<BulletActivationInfo> OnBulletActivated { get; set; }
        
        public void Throw(BulletActivationInfo activationInfo)
        {
            ActivateCustomActions(activationInfo);
            
            OnBulletActivated?.Invoke(activationInfo);
        }
        
        protected override void ActivateCustomActions(BulletActivationInfo activationInfo)
        {
            gameObject.SetActive(true);

            transform.parent = null;
            
            transform.position = activationInfo.BulletInitialPosition;

            transform.rotation = activationInfo.BulletRotation;

            _rigidbody.AddForce(activationInfo.BulletVelocity * transform.forward.normalized, ForceMode.VelocityChange);
        }

        protected override void DeactivationCustomActions()
        {
            gameObject.SetActive(false);
        }
    }
}