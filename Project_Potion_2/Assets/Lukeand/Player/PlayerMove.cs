using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    PlayerHandler handler;
    Rigidbody2D rb;
    [SerializeField] float speed;

    [HideInInspector] public Vector3 lastDir;
    [HideInInspector] public Vector3 currentDir;

    EntityStat stat;
    [SerializeField] float debugCurrentSpeed;
    [SerializeField] bool debugCanShootWhileMoving;
    [SerializeField] float debugAttackCooldown;
    //i want to influence this movement.
    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
        rb = GetComponent<Rigidbody2D>();
        stat = GetComponent<EntityStat>();
    }

    private void FixedUpdate()
    {
        if(stat != null)
        {
            debugCurrentSpeed = stat.GetStatValue(StatType.MoveSpeed);
            debugCanShootWhileMoving = stat.HasBDBoolean(BDBooleanType.ShootAndMove);
            debugAttackCooldown = stat.GetStatValue(StatType.AutoAttackCooldown);
        }

    }

    public void Move(Vector3 dir)
    {

       


        if(dir.x != 0)
        {
           if(handler != null) handler.graphics.RotateSprite(GetDirFromVector(dir));
        }

        if (dir.x == 0 && dir.y == 0)
        {
            //then we are not moving and we check if we have any item in hand 

            if(handler != null)
            {
                if (handler.inventory.HasItemInHand())
                {
                    handler.graphics.PlayAnimationIdleWithItem();
                }
                else
                {
                    handler.graphics.PlayAnimationIdle();
                }
            }
            

        }
        else
        {
           if(handler != null) handler.graphics.PlayAnimationMove();
            lastDir = dir;
            UIHolder.instance.OnMove();
        }

        currentDir = dir;
        float actualSpeed = speed;

        if(stat != null)
        {
            actualSpeed = stat.GetStatValue(StatType.MoveSpeed);
        }

        rb.velocity = new Vector2(dir.x, dir.y) * actualSpeed;
        
    }




    int GetDirFromVector(Vector2 dir)
    {

        if (dir.x > 0) return 1;
        if (dir.x < 0) return -1;

        Debug.LogError("WRONG");
        return 0;

        
    }

    //




    //might channge but it works for now.

}
