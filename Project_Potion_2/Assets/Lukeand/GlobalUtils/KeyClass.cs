using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyClass 
{


    public KeyClass()
    {
        SetUpKeys();
    }

    KeyCode keyMoveLeft;
    KeyCode keyMoveRight;
    KeyCode keyMoveUp;
    KeyCode keyMoveDown;
    KeyCode keyInventory;
    KeyCode keyInteract;
    public KeyCode GetKey(string id)
    {
        switch (id)
        {
            case "MoveLeft":
                return keyMoveLeft;
            case "MoveRight":
                return keyMoveRight;
            case "MoveUp":
                return keyMoveUp;
            case "MoveDown":
                return keyMoveDown;
            case "Inventory":
                return keyInventory;
            case "Interact":
                return keyInteract;

        }

        return KeyCode.None;
    }

    void ChangeKey(string id, KeyCode key)
    {

    }

    void SetUpKeys()
    {
        keyMoveLeft = KeyCode.A;
        keyMoveRight = KeyCode.D;
        keyMoveDown = KeyCode.S;
        keyMoveUp = KeyCode.W;

        keyInventory = KeyCode.Tab;
        keyInteract = KeyCode.E;
    }



}
