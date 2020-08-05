using System.Collections;
using RewindSystem;
using UnityEngine;

namespace GunSystem
{
    public class Bullet : BasicPhysicalRewindable
    {
        [SerializeField] private float _bulletForce = 2500;
        
        public void Throw()
        {
            _rigidbody.AddForce(_bulletForce * transform.forward.normalized, ForceMode.Force);
        }

        protected override void Awake()
        {
            base.Awake();

            if (_canSaveTimeStamps)
            {
                _canSaveTimeStamps = false;

                StartCoroutine(DelayProgress());
            }
        }

        protected override void RewindDectivatedCustomActions()
        {
            base.RewindDectivatedCustomActions();
            
            if (_canSaveTimeStamps)
            {
                _canSaveTimeStamps = false;

                StartCoroutine(DelayProgress());
            }
        }

        protected override void SpawnedAtActiveRewindCustomActions()
        {
            RewindManager.Instance.OnRewindModeDeactivated += OnRollbackDeactivatedCA;
            
            OnRewindActivated();
        }

        private void OnRollbackDeactivatedCA()
        {
            RewindManager.Instance.OnRewindModeDeactivated -= OnRollbackDeactivatedCA;
            
            _rigidbody.AddForce(_bulletForce * transform.forward.normalized, ForceMode.Force);
        }

        private IEnumerator DelayProgress()
        {
            yield return new WaitForFixedUpdate();

            _canSaveTimeStamps = !RewindManager.Instance.IsActive;
        }
    }
}