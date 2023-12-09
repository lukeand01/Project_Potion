using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Passive / EveryKillIncreaseAS")]
public class AbilityPassiveEveryKillIncreaseAS : AbilityPassiveData
{
    public override void Add(AbilityClass ability)
    {
        //i should be able to call the abilityclass in the passive as well.
        ability.entityHandler.ttEvents.EventKillEnemy += (handler) => CallPassive(ability, handler);

    }

    void CallPassive(AbilityClass ability, EntityHandler handler)
    {


        EntityHandler entity = ability.entityHandler;

        string id = "EveryKillIncreaseAS";
        BDClass bd = new BDClass(id , StatType.AutoAttackCooldown);
        bd.MakeTemp(5);
        bd.MakeStackable(100, true);
        bd.MakeFlatValue(3.5f) ; //it increases the attack speed based on the current value.
        

        //apply bd but one that wants to stack to a limit.
        entity.ttStat.AddBDWithID(bd);

    }
    


    public override void Remove(AbilityClass ability)
    {
        ability.entityHandler.ttEvents.EventKillEnemy -=(handler) =>  CallPassive(ability,handler);
    }

}
