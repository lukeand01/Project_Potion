using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //
    public event Action EventMoveInput;
    public void OnMoveInput() => EventMoveInput?.Invoke();

    PlayerHandler handler;
    PCHandler combatHandler;
    PlayerMove move;

    FloatingJoystick joystick;
    InputButton interactButton;
    //we set up them.

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
        combatHandler = GetComponent<PCHandler>();
        move = GetComponent<PlayerMove>();


        if(handler != null)
        {
            joystick = handler.joystick;
            interactButton = handler.interactButton;
        }
        

    }

    private void Start()
    {
        joystick = UIHolder.instance.joystick;
    }



    private void Update()
    {
        if (handler == null && move == null) return;
        MoveInput();
    }

    

    void MoveInput()
    {
        if(IsMoving())
        {
            OnMoveInput();
        }

        if (joystick == null)
        {           
            return;
        }

        move.Move(joystick.Direction);
    }

    public bool IsMoving()
    {
        if(joystick == null)
        {

            return false;
        }
        if (joystick.Direction.x == 0 && joystick.Direction.y == 0) return false;
        return true;
    }

}
