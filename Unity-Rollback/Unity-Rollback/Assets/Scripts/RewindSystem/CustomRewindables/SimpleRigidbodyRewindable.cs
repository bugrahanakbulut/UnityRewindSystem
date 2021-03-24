using System.Collections;
using RewindSystem;
using UnityEngine;

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

    private IEnumerator _delayRoutine;

    private RigidbodyTimeStamp _tmp;
    
    protected override bool ExecuteTimeStamp(RigidbodyTimeStamp timeStamp)
    {
        if (RewindManager.Instance.IsRewindActive)
        {
            _tmp = timeStamp;
        }
        
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
        _tmp = GetTimeStamp();
        
        _rigidbody.isKinematic = true;

        _rigidbody.useGravity = false;
        
        base.RewindActivatedCustomActions();
    }

    protected override void RewindDectivatedCustomActions()
    {
        _rigidbody.isKinematic = false;

        _rigidbody.useGravity = true;
        
        if (_tmp != null)
        {
            ExecuteTimeStamp(_tmp);

            _tmp = null;
        }
        
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
}
