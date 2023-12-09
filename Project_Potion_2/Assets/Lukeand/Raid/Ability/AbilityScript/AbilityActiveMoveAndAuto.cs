using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Skill / MoveAndAuto")]
public class AbilityActiveMoveAndAuto : AbilityActiveData
{


    //there are certain bd that are boolean bds.

    public override void Act(AbilityClass ability)
    {
        base.Act(ability);


        //this give a buff that increases speed and allows the player to attack while moving.


        EntityStat stat = ability.entityHandler.ttStat;

        if (stat == null)
        {
            Debug.Log("dont care");
            return;
        }

        stat.AddBD(GetMoveBD());
        stat.AddBD(GetBooleanMoveAndAttack());

        Debug.Log("this was called");
    }



    BDClass GetMoveBD()
    {

        BDClass bd = new BDClass("MoveAndAttack", StatType.MoveSpeed);
        bd.MakeTemp(5);
        bd.MakeValuePercent(20);
        return bd;
    }
    BDClass GetBooleanMoveAndAttack()
    {
        BDClass bd = new BDClass("MoveAndAttack", BDBooleanType.ShootAndMove);
        bd.MakeTemp(5);
        return bd;
    }
}
