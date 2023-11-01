using UnityEngine;

public class JumpCommand : Command
{
    public override void Execute(Rigidbody rb)
    {
        rb.transform.position += rb.transform.up;
        //rb.AddForce(_jumpForce * rb.transform.up, ForceMode.Impulse);
    }

    public override void Undo(Rigidbody rb)
    {
        rb.transform.position -= rb.transform.up;
    }
}
