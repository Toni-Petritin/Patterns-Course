using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public float speed = 2;
    public float jumpForce = 10;

    // Commands:
    Command cmd_W = new MoveForwardCommand();
    Command cmd_S = new MoveBackCommand();
    Command cmd_D = new MoveRightCommand();
    Command cmd_A = new MoveLeftCommand();
    Command cmd_up = new JumpCommand();
    Command cmd_down = new MoveCommand();

    Command _last_command = null;

    Stack<Command> _undo_commands = new Stack<Command>();

    // _undocommands.Push();
    // Command cmd = _undo_commands.Pop();

    void SwapCommands(ref Command A, ref Command B)
    {
        Command tmp = A;

        A = B;
        B = tmp;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            cmd_W.Execute(rb);
            _last_command = cmd_W;
            //rb.AddForce(transform.forward * speed);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            cmd_A.Execute(rb);
            _last_command = cmd_A;
            //rb.AddForce(-transform.right * speed);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            cmd_D.Execute(rb);
            _last_command = cmd_D;
            //rb.AddForce(transform.right * speed);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            cmd_S.Execute(rb);
            _last_command = cmd_S;
            //rb.AddForce(-transform.forward * speed);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            cmd_up.Execute(rb);
            _last_command = cmd_up;
            //rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            cmd_down.Execute(rb);
            _last_command = cmd_down;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (_last_command != null)
            {
                _last_command.Undo(rb);
                _last_command = new DoNothingCommand();
            }
        }

        if (Input.GetKeyDown((KeyCode.Escape)))
        {
            SwapCommands(ref cmd_A, ref cmd_D);
            //rb.AddForce(-transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}
