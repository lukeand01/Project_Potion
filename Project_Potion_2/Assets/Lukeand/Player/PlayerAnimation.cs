using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    PlayerHandler handler;
    Animator animator;

    const string BASE = "Animation_Player_";
    const string IDLE = "Idle";
    const string IDLEWITHITEM = "IdleWithItem";
    const string MOVE = "Move";

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
        animator = handler.anim;
    }

    public void RotateSprite(int dir)
    {
        if (dir == 0) return;
        //rotate to where you are facing.
        if (dir == 1)
        {
            //handler.graphicHolder.transform.localPosition = new Vector3(0, 0, 0);
            handler.body.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        if (dir == -1)
        {
            handler.body.transform.rotation = new Quaternion(0, 180, 0, 0);
            //handler.body.transform.localPosition = new Vector3(-0.4f, 0, 0);
        }
    }


    public void PlayAnimationIdle()
    {
        PlayAnimationByString(IDLE);
    }

    public void PlayAnimationIdleWithItem()
    {
        PlayAnimationByString(IDLEWITHITEM);
    }

    public void PlayAnimationMove()
    {
       PlayAnimationByString(MOVE);
    }


    void PlayAnimationByString(string nameID)
    {
        handler.anim.Play(BASE + nameID);
    }
    bool IsAnimationPlaying()
    {
        return false;
    }

}
