
using UnityEngine;

public class MoveLeftCommand : Command
{
    public override void Execute(Rigidbody rb)
    {
        rb.transform.position -= rb.transform.right;
        //rb.AddForce(_speed * -rb.transform.right);
    }

    public override void Undo(Rigidbody rb)
    {
        rb.transform.position += rb.transform.right;
    }
}
