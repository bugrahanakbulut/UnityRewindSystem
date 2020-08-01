using RollbackSys;
using UnityEngine;

namespace GunSys
{
    public class BulletTimeStamp : RetrievableTimeStampBase
    {
        public Vector3 Position { get; }
        
        public Quaternion Rotation { get; }

        public BulletTimeStamp(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
    
    public class BulletRetrievable : RetrievableBase<BulletTimeStamp>
    {
        protected override bool ExecuteTimeStamp(BulletTimeStamp timeStamp)
        {
            transform.position = timeStamp.Position;
            transform.rotation = timeStamp.Rotation;
            
            return true;
        }

        protected override BulletTimeStamp GetTimeStamp()
        {
            return new BulletTimeStamp(transform.position, transform.rotation);
        }
    }
}