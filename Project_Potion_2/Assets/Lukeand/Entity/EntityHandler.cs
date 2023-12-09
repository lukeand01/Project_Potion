using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHandler : MonoBehaviour
{
    //this is used for grouping the refereces.
    //we goinna store the current target here.
    //we also give 





     public EntityDamageable ttDamageable { get; private set; }
     public EntityDamageDealer ttDamageDealer { get; private set; }
     public EntityEvents ttEvents { get; private set; }
     public EntityMove ttMove { get; private set; }
     public EntityStat ttStat { get; private set;}

    public EntityCanvas ttCanvas { get; private set; }

    public EntityDamageable currentDamageableTarget { get; private set; }

    public void SetTarget(EntityDamageable damageable)
    {
        currentDamageableTarget = damageable;
    }




    private void Awake()
    {
        ttDamageable = GetComponent<EntityDamageable>();
        ttDamageDealer = GetComponent<EntityDamageDealer>();
        ttEvents = GetComponent<EntityEvents>();
        ttMove = GetComponent<EntityMove>();
        ttStat = GetComponent<EntityStat>();


        if(transform.childCount > 0)
        {
            ttCanvas = transform.GetChild(0).gameObject.GetComponent<EntityCanvas>();
        }
       

        
    }


    


}
