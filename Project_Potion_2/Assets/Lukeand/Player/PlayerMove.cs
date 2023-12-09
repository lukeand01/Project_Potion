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

        if (dir.x == 0 && dir.y == 0)
        {

        }
        else
        {
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

    //might channge but it works for now.

}
