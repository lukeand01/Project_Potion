using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Skill / BigArrow")]
public class AbilityActiveBigArrow : AbilityActiveData
{
    [SerializeField] GameObject bigArrowTemplate;
    public override void Act(AbilityClass ability)
    {
        base.Act(ability);
        //just shoot a big arrow at the current target.

        EntityDamageable currentDamageableTarget = ability.entityHandler.currentDamageableTarget;

        if (currentDamageableTarget == null)
        {
            return;
        }

        Debug.Log("got here in big arrow");

        GameObject newObject = Instantiate(bigArrowTemplate, ability.entityHandler.transform.position, Quaternion.identity);
        newObject.transform.localScale *= 2f;
        newObject.transform.position += new Vector3(0, 0, 10);

        ProjectilBase projectil = newObject.AddComponent<ProjectilBase>();
        EntityDamageDealer damageDealer = newObject.AddComponent<EntityDamageDealer>();

        Vector3 dir = (currentDamageableTarget.transform.position - ability.entityHandler.transform.position).normalized;
        projectil.SetUpDir(dir, 40);


        float damageValue = GetTotalScaleValue(ability.entityHandler.ttStat);
        damageDealer.SetUp( ability.entityHandler, new DamageClass(damageValue));
        damageDealer.SetID(currentDamageableTarget.id);
        damageDealer.SetAmountOfCollisionAllowed(10);
    }


    public override bool CanAct(AbilityClass ability)
    {
        return ability.entityHandler.currentDamageableTarget != null;
    }
    //its the same thing as auto but its bigger and deals more damage.
    //and doesnt stop in the first target.
    //it does not follow target. it goes to the dir of the last char.

}
