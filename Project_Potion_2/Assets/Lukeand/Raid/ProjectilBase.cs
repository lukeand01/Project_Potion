using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilBase : MonoBehaviour
{
    //we create something here that will follow the target.
    //this just dictates moving behavior.
    //what other behaviors? moving forward as well.

    bool isReady;
    Transform target;
    Vector3 dir;
    float speed;
    ProjectilType type;

    EntityHandler attacker;

    public void SetUpTarget(Transform target, float speed)
    {
        this.target = target;
        this.speed = speed;
        type = ProjectilType.Target;
        isReady = true;

        
    }
    public void SetUpDir(Vector2 dir, float speed)
    {
        this.dir = dir; 
        this.speed = speed; 
        type = ProjectilType.Forward;
        isReady = true;
    }

    

    enum ProjectilType
    {
        Target,
        Forward
    }

    private void FixedUpdate()
    {
        if (!isReady) return;


        if(type == ProjectilType.Forward)
        {
            //then it moves fowarad based on dir
            transform.position += dir * speed * 0.01f;
            Rotate(dir, 90);
            return;
        }

        if(type == ProjectilType.Target)
        {
            if(target == null)
            {
                Destroy(gameObject);
            }


            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * 0.01f);
            Rotate(target.position - transform.position, 90);
            return;
        }
    }


    private void Rotate(Vector2 rotateDir, float rotationModifier)
    {
        
        float angle = Mathf.Atan2(rotateDir.y, rotateDir.x) * Mathf.Rad2Deg - rotationModifier;

        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;
        //transform.rotation = Quaternion.Slerp(transform.rotation, q,  angle);
    }
}

