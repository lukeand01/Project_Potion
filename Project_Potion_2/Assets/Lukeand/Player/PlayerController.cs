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
    PlayerMove move;

    [SerializeField] FloatingJoystick joystick;
    InputButton interactButton;
    //we set up them.

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
        move = GetComponent<PlayerMove>();
        //joystick = handler.joystick;
        //interactButton = handler.interactButton;

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

        if (joystick == null) Debug.Log("no joystick");

        move.Move(joystick.Direction);
    }

    public bool IsMoving()
    {
        if (joystick.Direction.x == 0 && joystick.Direction.y == 0) return false;
        return true;
    }

}
