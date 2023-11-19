using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHandler : MonoBehaviour
{
    //this is used for grouping the refereces.

     public EntityDamageable ttDamageable { get; private set; }
     public EntityDamageDealer ttDamageDealer { get; private set; }
     public EntityEvents ttEvents { get; private set; }
     public EntityMove ttMove { get; private set; }
     public EntityStat ttStat { get; private set;}

    private void Awake()
    {
        ttDamageable = GetComponent<EntityDamageable>();
        ttDamageDealer = GetComponent<EntityDamageDealer>();
        ttEvents = GetComponent<EntityEvents>();
        ttMove = GetComponent<EntityMove>();
        ttStat = GetComponent<EntityStat>();
    }





}
