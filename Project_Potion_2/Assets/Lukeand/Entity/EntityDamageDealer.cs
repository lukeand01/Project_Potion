using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDamageDealer : MonoBehaviour
{
    EntityHandler attacker;
    int amountOfCollisionsAllowed = 1;
    int currentAmountOfCollisionAllowed = 0;

    DamageClass damage;
    string id = "";
    int[] layerMaskIndex;


    //we set up special effect.
    //apply stats.
    //deal damage.
    public void SetUp(EntityHandler attacker, DamageClass damage)
    {
        this.damage = new DamageClass(damage.baseDamage);
        this.attacker = attacker;
        


    }

    public void SetLayer(int[] layerMaskIndex)
    {
        this.layerMaskIndex = layerMaskIndex;
    }

    public void SetID(string id)
    {
        this.id = id;
    }

    public void SetAmountOfCollisionAllowed(int newAmount)
    {
        amountOfCollisionsAllowed = newAmount;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        EntityDamageable damageable = collision.collider.GetComponent<EntityDamageable>();
        bool isSucccess = HandleCollision(damageable);

        
        if (isSucccess)
        {
            currentAmountOfCollisionAllowed += 1;
           

            if (currentAmountOfCollisionAllowed >= amountOfCollisionsAllowed)
            {
                Destroy(gameObject);
                Debug.Log(amountOfCollisionsAllowed + " " + currentAmountOfCollisionAllowed);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        EntityDamageable damageable = collision.GetComponent<EntityDamageable>();
        bool isSucccess = HandleCollision(damageable);

       
        if (isSucccess)
        {
            currentAmountOfCollisionAllowed += 1;
            
            if (currentAmountOfCollisionAllowed >= amountOfCollisionsAllowed)
            {
                Destroy(gameObject);
            }
        }

    }


    bool HandleCollision(EntityDamageable damageable)
    {
        if (damageable == null) return false;

        if(layerMaskIndex != null)
        {
            if (layerMaskIndex.Length > 0)
            {
                if (EntityContainsLayerMask(damageable.gameObject.layer))
                {
                    DealDamage(damageable);
                    return true;
                }
                else return false;
            }
        }
            

        if (id != "")
        {

            if (damageable.id == id)
            {
                DealDamage(damageable);
                return true;
            }
            else
            {
                return false;
            }
        }


        DealDamage(damageable);
        return true;
    }

    bool EntityContainsLayerMask(int layerMask)
    {
        foreach (var item in layerMaskIndex)
        {
            if (item == layerMask) return true;
        }

        return false;
    }
    void DealDamage(EntityDamageable damageable)
    {
        if(damageable == null)
        {
            return;
        }
        if(damage == null)
        {
            return;
        }

        damageable.TakeDamage(attacker, damage);

        
    }

}
