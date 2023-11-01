using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //
    public event Action EventMoveInput;
    public void OnMoveInput() => EventMoveInput?.Invoke();

    PlayerHandler handler;

    FloatingJoystick joystick;
    InputButton interactButton;
    //we set up them.

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();

        joystick = handler.joystick;
        interactButton = handler.interactButton;

    }
   

    private void Update()
    {
        if (handler == null) return;
        MoveInput();
    }

    

    void MoveInput()
    {
        if(IsMoving())
        {
            OnMoveInput();
        }

        handler.move.Move(joystick.Direction);
    }

    public bool IsMoving()
    {
        if (joystick.Direction.x == 0 && joystick.Direction.y == 0) return false;
        return true;
    }

}
