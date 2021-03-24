using System;

namespace PoolingSystem
{
    public abstract class PoolObjectActivationInfo { }
    
    public interface IPoolObject<T>
        where T : PoolObjectActivationInfo
    {
        Action<IPoolObject<T>> OnObjectActivated { get; set; } 
        
        Action<IPoolObject<T>> OnObjectDeactivated { get; set; }
        
        void ActivatePoolObject(T activationInfo); 
        
        void DeactivatePoolObject();
    }
}