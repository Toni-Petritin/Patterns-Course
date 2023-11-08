
using UnityEngine;

public abstract class Command
{
    //protected float _speed = 3;
    //protected float _jumpForce = 10;

    // Time stamp, but not like this
    //float _Time = 0.0f;

    // Execute must be implemented by each child-class.
    public abstract void Execute(Rigidbody rb);

    public abstract void Undo(Rigidbody rb);
}
