using System;

namespace PoolingSystem
{
    public interface IFactory<T1, T2>
        where T1 : PoolObjectActivationInfo
        where T2 : IPoolObject<T1>
    {
        Action OnCreatedPoolObject { get; set; }
        
        T2 Create();
    }
}
