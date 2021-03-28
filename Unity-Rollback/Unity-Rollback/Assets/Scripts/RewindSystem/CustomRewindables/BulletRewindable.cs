using GunSystem;
using UnityEngine;
using System.Collections;

namespace RewindSystem
{
    [RequireComponent(typeof(Rigidbody), typeof(Bullet))]
    public class BulletRewindable : RewindableBase<RigidbodyTimeStamp>
    {
        [SerializeField] private Bullet _bullet = null;
        
        [SerializeField] private Rigidbody _rigidbody = null;

        private RigidbodyTimeStamp _timeStampActivation = null;
        
        private IEnumerator _delayRoutine;
        
        protected override void AwakeCustomActions()
        {
            _bullet.OnBulletActivated += OnBulletActivated;
            
            base.AwakeCustomActions();
        }
        protected override void OnDestroyCustomActions()
        {
            _bullet.OnBulletActivated -= OnBulletActivated;
            
            base.OnDestroyCustomActions();
        }

        protected override bool ExecuteTimeStamp(RigidbodyTimeStamp timeStamp)
        {
            _rigidbody.position = timeStamp.Position;

            _rigidbody.rotation = timeStamp.Rotation;

            _rigidbody.velocity = timeStamp.Velocity;

            return true;
        }

        protected override RigidbodyTimeStamp GetTimeStamp()
        {
            if (_timeStampActivation != null)
            {
                RigidbodyTimeStamp tmp = _timeStampActivation;
                
                _timeStampActivation = null;
                
                Debug.Log(tmp.Velocity);
                
                return tmp;
            }

            return new RigidbodyTimeStamp(_rigidbody.position, _rigidbody.velocity, _rigidbody.rotation);
        }
        
        protected override void RewindActivatedCustomActions()
        {
            _rigidbody.isKinematic = true;

            _rigidbody.useGravity = false;
        
            base.RewindActivatedCustomActions();
        }

        protected override void RewindDectivatedCustomActions()
        {
            _rigidbody.isKinematic = false;

            _rigidbody.useGravity = true;

            base.RewindDectivatedCustomActions();
        }
        
        protected override void SetCanSaveTimeStamp(bool value)
        {
            _canSaveTimeStamps = false;

            if (gameObject.activeInHierarchy)
            {
                _delayRoutine = DelayProgress();

                StartCoroutine(_delayRoutine);
            }
            else
                base.SetCanSaveTimeStamp(value);
        }
        
        private IEnumerator DelayProgress()
        {
            yield return new WaitForFixedUpdate();

            _canSaveTimeStamps = !RewindManager.Instance.IsRewindActive;
        }
        
        private void OnBulletActivated(BulletActivationInfo activationInfo)
        {
            _timeStampActivation = 
                new RigidbodyTimeStamp(
                    _rigidbody.position, 
                    activationInfo.BulletVelocity * transform.forward.normalized,
                    _rigidbody.rotation);
        }
    }
}
