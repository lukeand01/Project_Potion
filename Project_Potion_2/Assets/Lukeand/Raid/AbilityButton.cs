using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButton : InputButton
{
    AbilityClass ability;

    [SerializeField] Image portrait;

    public void SetUpAbility(AbilityClass ability)
    {
        this.ability = ability;
        UpdateUI();
    }

    void UpdateUI()
    {

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        //ability.Act();
    }

}
//