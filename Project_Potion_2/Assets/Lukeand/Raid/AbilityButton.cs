using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButton : InputButton
{
    AbilityClass ability;
    [SerializeField] Image portrait;
    [SerializeField] Image cooldownImage;
    [SerializeField] GameObject cannotBeUsed;

    public void SetUpAbility(AbilityClass ability)
    {

        this.ability = ability;
        holder = transform.GetChild(0).gameObject;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (!HasAbilityAssigned())
        {
            holder.SetActive(false);
            Debug.Log("this class has abilities not assigned");
            return;
        }

        holder.SetActive(true);
        portrait.sprite = ability.abilityIcon;


    }

    bool HasAbilityAssigned()
    {
        if (ability == null)
        {
            Debug.Log("the ability was null");
            return false;
        }
        if (!ability.HasCompleteData())
        {
            Debug.Log("has no complete data");
            return false;
        }

        return true;
    }

    private void FixedUpdate()
    {
        if (ability.IsReadyToUse())
        {
            if (!ability.CanCall())
            {
                cannotBeUsed.SetActive(true);
                return;
            }
                    
        }

        cannotBeUsed.SetActive(false);

    }

    public void ControlCooldown(float current, float total)
    {
        

        cooldownImage.fillAmount = current / total;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        //ability.Act();

        if (ability.IsReadyToUse())
        {
            //then we can use the ability.
            Debug.Log("can use this fella");
            ability.TryToCall();
        }

    }

}
//