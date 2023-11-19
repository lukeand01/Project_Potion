using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDamageable : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public float initalHealth;
    public bool isDead;
    public bool isImmortal;
    public string id { get; private set; }

    EntityHandler handler;


    private void Awake()
    {
        id = Guid.NewGuid().ToString();
        handler = GetComponent<EntityHandler>();
    }


    public void TakeDamage(EntityHandler attacker, DamageClass damage)
    {
        currentHealth -= damage.GetDamage();

        //apply bd if there are any here.
        damage.ApplyBDToStat(handler.ttStat);

        if(currentHealth <= 0 && !damage.cannotFinishEntity)
        {
            attacker.ttEvents.OnKillEnemy(handler);
            Die();
        }

    }

    void Die()
    {

    }

}

public enum DamageType
{
    Normal
}