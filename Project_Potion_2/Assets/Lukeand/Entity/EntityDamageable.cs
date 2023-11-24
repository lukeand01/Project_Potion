using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EntityDamageable : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public float initalHealth;
    public bool isDead;
    public bool isImmortal;
    public bool shouldShowDamageUI;
    public string id { get; private set; }

    EntityHandler handler;

    //anytime this is attackced it creates a text ui.
    //

    private void Awake()
    {
        id = Guid.NewGuid().ToString();
        handler = GetComponent<EntityHandler>();
    }


    public void TakeDamage(EntityHandler attacker, DamageClass damage)
    {
        float damageValue = damage.GetDamage();
        currentHealth -= damageValue;

        //apply bd if there are any here.
        if(handler.ttStat != null) damage.ApplyBDToStat(handler.ttStat);

        CreateDamagePopUp(damageValue);
        
        if (currentHealth <= 0 && !damage.cannotFinishEntity && !isImmortal)
        {           
            Die(attacker);
        }

    }

    void CreateDamagePopUp(float damage)
    {
        if (handler.ttCanvas == null) return;
        handler.ttCanvas.CreateDamageFadeUI(damage.ToString(), Color.red);
        
    }
    //create the ui effecct.



    void Die(EntityHandler attacker)
    {
        if (attacker.ttEvents != null) attacker.ttEvents.OnKillEnemy(handler);
        Destroy(gameObject);
    }

}

public enum DamageType
{
    Normal
}