using RollbackSys;
using UnityEngine;

namespace GunSys
{
    public class Bullet : PhysicsRetrievable
    {
        [SerializeField] private float _bulletForce = 2500;
        
        public void Throw()
        {
            _rigidbody.AddForce(_bulletForce * transform.forward.normalized, ForceMode.Force);
        }

        protected override void SpawnedAtActiveRollbackCustomActions()
        {
            RollbackManager.Instance.OnRollbackDeactivated += OnRollbackDeactivatedCA;
            
            OnRollbackActivated();
        }

        private void OnRollbackDeactivatedCA()
        {
            RollbackManager.Instance.OnRollbackDeactivated -= OnRollbackDeactivatedCA;
            
            _rigidbody.AddForce(_bulletForce * transform.forward.normalized, ForceMode.Force);
        }
    }
}