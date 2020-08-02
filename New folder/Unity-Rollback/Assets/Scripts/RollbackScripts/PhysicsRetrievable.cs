using UnityEngine;

namespace RollbackSys
{
    public class PhysicRetrievableTimeStamp : RetrievableTimeStampBase
    {
        public Vector3 Position { get; }
        
        public Quaternion Rotation { get; }
        
        public Vector3 Velocity { get; }

        public PhysicRetrievableTimeStamp(Vector3 position, Quaternion rotation, Vector3 velocity)
        {
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
        }
    }
    
    public class PhysicsRetrievable : RetrievableBase<PhysicRetrievableTimeStamp>
    {
        protected override bool ExecuteTimeStamp(PhysicRetrievableTimeStamp timeStamp)
        {
            transform.position = timeStamp.Position;
            transform.rotation = timeStamp.Rotation;

            _rigidbody.velocity = timeStamp.Velocity;
            
            return true;
        }

        protected override PhysicRetrievableTimeStamp GetTimeStamp()
        {
            return new PhysicRetrievableTimeStamp(transform.position, transform.rotation, _rigidbody.velocity);
        }
    }
}