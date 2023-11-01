
using UnityEngine;

public class MoveForwardCommand : Command
{
    public override void Execute(Rigidbody rb)
    {
        rb.transform.position += rb.transform.forward;
        //rb.AddForce(_speed * rb.transform.forward);
    }

    public override void Undo(Rigidbody rb)
    {
        rb.transform.position -= rb.transform.forward;
    }
}
