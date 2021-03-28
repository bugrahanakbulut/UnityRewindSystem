using UnityEngine;

namespace RewindSystem
{
    public class RigidbodyTimeStamp : RewindableTimeStampBase
    {
        public Vector3 Position { get; }
    
        public Vector3 Velocity { get; }
    
        public Quaternion Rotation { get; }

        public RigidbodyTimeStamp(Vector3 position, Vector3 velocity, Quaternion rotation)
        {
            Position = position;
            Velocity = velocity;
            Rotation = rotation;
        }
    }

    [RequireComponent(typeof(Rigidbody))]
    public class SimpleRigidbodyRewindable : RewindableBase<RigidbodyTimeStamp>
    {
        [SerializeField] private Rigidbody _rigidbody = null;
    
        protected override bool ExecuteTimeStamp(RigidbodyTimeStamp timeStamp)
        {
            _rigidbody.position = timeStamp.Position;

            _rigidbody.rotation = timeStamp.Rotation;

            _rigidbody.velocity = timeStamp.Velocity;

            return true;
        }

        protected override RigidbodyTimeStamp GetTimeStamp()
        {
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
    }    
}

