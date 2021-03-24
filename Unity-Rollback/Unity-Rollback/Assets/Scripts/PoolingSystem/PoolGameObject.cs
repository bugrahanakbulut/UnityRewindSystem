using System;
using UnityEngine;

namespace PoolingSystem
{
    public abstract class PoolGameObject<T> : MonoBehaviour, IPoolObject<T>
        where T : PoolObjectActivationInfo
    {
        public Action<IPoolObject<T>> OnObjectActivated { get; set; }
        public Action<IPoolObject<T>> OnObjectDeactivated { get; set; }

        protected abstract void ActivateCustomActions(T activationInfo);

        protected abstract void DeactivationCustomActions();
        
        public virtual void ActivatePoolObject(T activationInfo)
        {
            ActivateCustomActions(activationInfo);
            
            OnObjectActivated?.Invoke(this);
        }

        public virtual void DeactivatePoolObject()
        {
            DeactivationCustomActions();
            
            OnObjectDeactivated?.Invoke(this);
        }
    }
}