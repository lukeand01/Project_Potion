using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    PlayerHandler handler;
    [SerializeField] float speed;

    [HideInInspector] public Vector3 lastDir;
    [HideInInspector] public Vector3 currentDir;


    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
    }

    public void Move(Vector3 dir)
    {
        if (dir.x == 0 && dir.y == 0)
        {

        }
        else
        {
            lastDir = dir;

            //
            UIHolder.instance.chest.CloseUI();

        }

        currentDir = dir;
        handler.rb.velocity = new Vector2(dir.x * speed, dir.y * speed);
    }



}
