using System;
using System.Collections.Generic;
using System.Linq;

namespace PoolingSystem
{
    public class Pool<T1, T2, T3>
        where T1 : PoolObjectActivationInfo
        where T2 : IPoolObject<T1>
        where T3 : IFactory<T1, T2>
    {
        public Pool(T3 factory, int initialPoolSampleCount)
        {
            _factory = factory;
        
            AddSamplesToPool(initialPoolSampleCount);
        }

        private T3 _factory;
    
        private Dictionary<T2, bool> _pool = new Dictionary<T2, bool>();

        #region Events

        public Action<T2> OnPoolObjectActivated { get; set; }
        
        public Action<T2> OnPoolObjectDeactivated { get; set; }

        #endregion Events
    
        public void ActivatePoolObject(T1 activationInfo)
        {
            T2 deactivePoolObject = _pool.FirstOrDefault(i => i.Value == false).Key;

            if (deactivePoolObject != null)
            {
                deactivePoolObject.ActivatePoolObject(activationInfo);
                
                return;
            }

            T2 poolObject = CreatePoolObject();
        
            _pool.Add(poolObject, false);
        
            poolObject.ActivatePoolObject(activationInfo);
        }

        public void DeactivatePoolObject(T2 poolObject)
        {
            if (_pool.ContainsKey(poolObject))
            {
                poolObject.DeactivatePoolObject();
            }
        }

        public void DeactivateAll()
        {
            List<T2> activeList = new List<T2>();
            
            foreach (KeyValuePair<T2,bool> kvp in _pool)
            {
                if (kvp.Value)
                {
                    activeList.Add(kvp.Key);
                }
            }
            
            activeList.ForEach(i => i.DeactivatePoolObject());
        }

        private void AddSamplesToPool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T2 poolObject = CreatePoolObject();
            
                poolObject.DeactivatePoolObject();
            
                _pool.Add(poolObject, false);
            } 
        }

        private T2 CreatePoolObject()
        {
            T2 newPoolObject = _factory.Create();

            newPoolObject.OnObjectActivated += OnObjectActivated;
            newPoolObject.OnObjectDeactivated += OnObjectDeactivated;

            return newPoolObject;
        }

        private void OnObjectActivated(IPoolObject<T1> poolObject)
        {
            if (!_pool.ContainsKey((T2)poolObject))
            {
                return;
            }

            _pool[(T2)poolObject] = true;
        }

        private void OnObjectDeactivated(IPoolObject<T1> poolObject)
        {
            if (!_pool.ContainsKey((T2)poolObject))
            {
                return;
            }

            _pool[(T2)poolObject] = false;
        }
    }
}
