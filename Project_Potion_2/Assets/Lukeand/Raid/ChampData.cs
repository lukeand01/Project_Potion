using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ChampData : ScriptableObject
{
    public string champName;
    public Sprite champSprite;
    public Sprite champModel;


    [SerializeField] AbilityActiveClass autoAttack;
    [SerializeField] AbilityActiveClass skill1;
    [SerializeField] AbilityActiveClass skill2;
    [SerializeField] AbilityPassiveClass passiveMain;
    [SerializeField] AbilityPassiveClass passiveSupport;


    public void GetCopyOfAttackClasses(ChampClass champ)
    {
        //we pass thesee fellas to the champ class so we can do stuff with the 
        champ.autoAttack = new AbilityActiveClass(autoAttack);
        champ.skill1 = new AbilityActiveClass(skill1);
        champ.skill2 = new AbilityActiveClass(skill2);

        champ.passiveMain = new AbilityPassiveClass(passiveMain);
        champ.passiveSupport = new AbilityPassiveClass(passiveSupport);
    }

    //active abilities as abilitiy that you use.
    //passive abilitiees are abilitis that wait for triggers.

}


//FIRST CHAMPIONS: boy(dps), girl(support), knight(tank)

//BOY : high damage.
//
//
//
//killing an enemy grants stacking attackspeed 3% for the main player till 30% after not killing an enemy for 5 seconds it fades away.


//GIRL : attack in area. magic.
//shoot ice at the curren target. it explodes slowing all.
//it increases the attack speed of partners.
//
//Heals after attacking a certain amount.


//KNIGHT : crowd control.
//Stun
//
//
//the main gains 10 % damage reduction.


//GOBLIN: Speed
//dash through enemies.
//
//
//gain move speed after every ability use.



//EQUIPS NEED TO BE FUN TOO
//they should mostly add stats or triggers.