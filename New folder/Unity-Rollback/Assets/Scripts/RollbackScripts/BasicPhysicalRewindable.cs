using UnityEngine;

namespace RewindSystem
{
    public class BasicPhysicTimeStamp : RewindableTimeStampBase
    {
        public Vector3 Position { get; }
        
        public Quaternion Rotation { get; }
        
        public Vector3 Velocity { get; }

        public BasicPhysicTimeStamp(Vector3 position, Quaternion rotation, Vector3 velocity)
        {
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
        }
    }
    
    public class BasicPhysicalRewindable : PhysicalRewindableBase<BasicPhysicTimeStamp>
    {
        protected Vector3 _lastExecutedVelocity;
        
        protected override bool ExecuteTimeStamp(BasicPhysicTimeStamp timeStamp)
        {
            transform.position = timeStamp.Position;
            transform.rotation = timeStamp.Rotation;

            _lastExecutedVelocity = timeStamp.Velocity;
            
            return true;
        }

        protected override BasicPhysicTimeStamp GetTimeStamp()
        {
            return new BasicPhysicTimeStamp(transform.position, transform.rotation, _rigidbody.velocity);
        }

        protected override void RewindDectivatedCustomActions()
        {
            base.RewindDectivatedCustomActions();
            
            _rigidbody.AddForce(_lastExecutedVelocity, ForceMode.VelocityChange);
        }
    }
}