using UnityEngine;

public class MoveCommand : Command
{
    public override void Execute(Rigidbody rb)
    {
        rb.transform.position -= rb.transform.up;
    }

    public override void Undo(Rigidbody rb)
    {
        rb.transform.position += rb.transform.up;
    }


}
