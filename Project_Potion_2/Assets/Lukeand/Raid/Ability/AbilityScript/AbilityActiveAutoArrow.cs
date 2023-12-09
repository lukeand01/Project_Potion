using MyBox;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Auto / ArrowAuto")]
public class AbilityActiveAutoArrow : AbilityActiveData
{
    [Separator("TEMPLATE")]
    [SerializeField] GameObject arrowTemplate;



    
    public override void Act(AbilityClass ability)
    {
        EntityDamageable currentDamageableTarget = ability.entityHandler.currentDamageableTarget;

        if (currentDamageableTarget == null)
        {
            return;
        }


        GameObject newObject = Instantiate(arrowTemplate, ability.entityHandler.transform.position , Quaternion.identity);
        
        ProjectilBase projectil = newObject.AddComponent<ProjectilBase>();        
        EntityDamageDealer damageDealer = newObject.AddComponent<EntityDamageDealer>();


        projectil.SetUpTarget(currentDamageableTarget.transform, 50);


        float damageValue = GetTotalScaleValue(ability.entityHandler.ttStat);
        damageDealer.SetUp(ability.entityHandler, new DamageClass(damageValue));
        damageDealer.SetID(currentDamageableTarget.id);
    }



}
