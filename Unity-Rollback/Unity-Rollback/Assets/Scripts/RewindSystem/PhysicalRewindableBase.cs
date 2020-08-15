using System.Collections.Generic;
using UnityEngine;

namespace RewindSystem
{
    
    public abstract class PhysicalRewindableTimeStampBase : RewindableTimeStampBase 
    {}
    
    public abstract class PhysicalRewindableBase<T> : RewindableBase
    where T : PhysicalRewindableTimeStampBase
    {
        [SerializeField] protected Rigidbody _rigidbody;


        protected override void OnRewindActivated()
        {
            _rigidbody.isKinematic = true;

            _canSaveTimeStamps = false;
            
            RewindActivatedCustomActions();
        }

        protected override void OnRewindDeactivated()
        {
            _timeStamps = new List<RewindableTimeStampBase>();
            
            _backUpTimeStamps = new List<RewindableTimeStampBase>();
            
            _rigidbody.isKinematic = false;
            
            _canSaveTimeStamps = true;

            RewindDectivatedCustomActions();
        }

        protected override void OnRewindRequested(ERewindDirection eRewindDirection)
        {
            if (eRewindDirection == ERewindDirection.Backward && _timeStamps.Count > 0)
            {
                T timeStamp = (T) _timeStamps[_timeStamps.Count - 1];
                
                _timeStamps.RemoveAt(_timeStamps.Count - 1);
                
                _backUpTimeStamps.Add(timeStamp);

                ExecuteTimeStamp(timeStamp);
            }
            
            else if (eRewindDirection == ERewindDirection.Forward && _backUpTimeStamps.Count > 0)
            {
                T timeStamp = (T) _backUpTimeStamps[_backUpTimeStamps.Count - 1];
                
                _backUpTimeStamps.RemoveAt(_backUpTimeStamps.Count - 1);
                
                _timeStamps.Add(timeStamp);

                ExecuteTimeStamp(timeStamp);
            }
        }
    }
}