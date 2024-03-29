using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    enum PlayerState
    {
        JUMPING,
        STANDING,
        CROUCHING
    }

    enum ColorState
    {
        NORMAL,
        GREEDY,
        KILLER
    }

    [SerializeField] private bool grounded = false;

    [SerializeField] private float colorTimer;
    
    [SerializeField] private PlayerState my_state = PlayerState.JUMPING;
    [SerializeField] private ColorState my_color = ColorState.NORMAL;
    [SerializeField] private Renderer _renderer;
    
    public float jumpForce = 10;

    [SerializeField] private Rigidbody rb;

    // Commands:
    Command cmd_W = new MoveForwardCommand();
    Command cmd_S = new MoveBackCommand();
    Command cmd_D = new MoveRightCommand();
    Command cmd_A = new MoveLeftCommand();

    private Stack<Command> _undoStack = new Stack<Command>();
    private Stack<Command> _redoStack = new Stack<Command>();

    private bool replaying = false;
    private int replayCounter = 0;
    private float replayInterval = .1f;
    private float timer = 0;

    private void OnEnable()
    {
        Coin.OnCoinCollected += GotGreedy;;
        Enemy.OnEnemyDestroyed += GotBloody;
    }

    private void OnDisable()
    {
        Coin.OnCoinCollected -= GotGreedy;;
        Enemy.OnEnemyDestroyed -= GotBloody;
    }

    private void Jump()
    {
        rb.AddForce(new Vector3(0,1,0) * jumpForce, ForceMode.Impulse);
    }

    private void GotGreedy()
    {
        colorTimer = 3;
        _renderer.material.color = Color.yellow;
        my_color = ColorState.GREEDY;
    }

    private void GotBloody()
    {
        colorTimer = 3;
        _renderer.material.color = Color.red;
        my_color = ColorState.KILLER;
    }

    void Update()
    {
        if (!replaying)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, .9f, 0),  Vector3.down, out hit, 1f))
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
            
            switch (my_state)
            {
                case PlayerState.JUMPING:
                    if (grounded)
                    {
                        my_state = PlayerState.STANDING;
                    }
                    break;
                case PlayerState.STANDING:
                    if (!grounded)
                    {
                        my_state = PlayerState.JUMPING;
                    }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Jump();
                        my_state = PlayerState.JUMPING;
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftControl))
                    {
                        this.transform.localScale = new Vector3(1, .7f, 1);
                        my_state = PlayerState.CROUCHING;
                    }
                    break;
                case PlayerState.CROUCHING:
                    if (!grounded)
                    {
                        my_state = PlayerState.JUMPING;
                    }
                    else if (Input.GetKeyUp(KeyCode.LeftControl))
                    {
                        this.transform.localScale = new Vector3(1, 1, 1);
                        my_state = PlayerState.STANDING;
                    }
                    break;
            }

            switch (my_color)
            {
                case ColorState.NORMAL:
                    _renderer.material.color = Color.blue;
                    break;
                case ColorState.GREEDY:
                    colorTimer -= Time.deltaTime;
                    if (colorTimer < 0)
                    {
                        colorTimer = 0;
                        my_color = ColorState.NORMAL;
                    }
                    break;
                case ColorState.KILLER:
                    colorTimer -= Time.deltaTime;
                    if (colorTimer < 0)
                    {
                        colorTimer = 0;
                        my_color = ColorState.NORMAL;
                    }
                    break;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                cmd_W.Execute(rb);
                _undoStack.Push(cmd_W);
                _redoStack.Clear();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                cmd_A.Execute(rb);
                _undoStack.Push(cmd_A);
                _redoStack.Clear();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                cmd_D.Execute(rb);
                _undoStack.Push(cmd_D);
                _redoStack.Clear();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                cmd_S.Execute(rb);
                _undoStack.Push(cmd_S);
                _redoStack.Clear();
            }

            // Undo
            if (Input.GetKeyDown(KeyCode.Z))
            {
                UndoCommand();
            }

            // Redo
            if (Input.GetKeyDown(KeyCode.X))
            {
                RedoCommand();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                replaying = true;
                while (_undoStack.Count != 0)
                {
                    Command cmd = _undoStack.Pop();
                    cmd.Undo(rb);
                    _redoStack.Push(cmd);
                    replayCounter++;
                }
                // optional
                //StartCoroutine(Replay());

            }
            
            if (Input.GetKeyDown((KeyCode.Escape)))
            {
                SwapCommands(ref cmd_A, ref cmd_D);
                //rb.AddForce(-transform.up * jumpForce, ForceMode.Impulse);
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= replayInterval)
            {
                timer = 0;
                if (replayCounter > 0)
                {
                    replayCounter--;
                    Command cmd = _redoStack.Pop();
                    cmd.Execute(rb);
                    _undoStack.Push(cmd);
                }
                else
                {
                    replaying = false;
                }
            }
        }
    }

    void UndoCommand()
    {
        if (_undoStack.Count != 0)
        {
            Command cmd = _undoStack.Pop();
            cmd.Undo(rb);
            _redoStack.Push(cmd);
        }
    }

    void RedoCommand()
    {
        if (_redoStack.Count != 0)
        {
            Command cmd = _redoStack.Pop();
            cmd.Execute(rb);
            _undoStack.Push(cmd);
        }
    }
    
    void SwapCommands(ref Command a, ref Command b)
    {
        Command tmp = a;
        a = b;
        b = tmp;
    }

    // optional
    IEnumerator Replay()
    {
        int r = 0;
        while (_undoStack.Count > 0)
        {
            UndoCommand();
            r++;
        }

        while (r > 0)
        {
            r--;
            RedoCommand();
            yield return new WaitForSeconds(.5f);
        }
        replaying = false;
    }

}
