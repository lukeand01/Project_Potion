using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientUnit : ButtonBase
{
    ProductionUI uiHandler;

    //can be interracted to retriev it to the hand. 
    public ItemClass item { get; private set; }
    ItemDataIngredient ingredientData; //these two are the same thing but one is already the pure version is that required.

    [SerializeField] GameObject empty;

    [Separator("Availability")]
    [SerializeField] Image availabilityImage;
    [SerializeField] Color greenColor;
    [SerializeField] Color yellowColor;
    [SerializeField] Color redColor;
    [SerializeField]AvailabityType currentAvailabityType;

    [SerializeField]Image portrait;
    [SerializeField] TextMeshProUGUI nameText;

    [SerializeField]bool isHand;
    bool isClickable;
    public void SetUp(ItemClass item, bool isHand)
    {

        uiHandler = UIHolder.instance.production;

        this.isHand = isHand;

        if (item == null) return;
       
        this.item = item;

        if (item.data == null) return;      

        ItemDataIngredient foundIngredient = item.data.GetIngredient();
        this.ingredientData = foundIngredient;

        if(foundIngredient == null)
        {
            currentAvailabityType = AvailabityType.NotType;
            availabilityImage.color = redColor;
        }

       
    }

    public void UpdateUI()
    {
        empty.SetActive(item.data == null); //we make it dark if it does not have data.

        if (item.data == null)
        {

            return;
        }

        

        portrait.sprite = item.data.itemSprite;
        nameText.text = item.data.name;

    }

    public override void OnPointerClick(PointerEventData eventData)
    {

        if(item.data == null) return;
        

        if(isHand) if (currentAvailabityType == AvailabityType.NotAllowedBecauseOfRecipe || currentAvailabityType == AvailabityType.NotType) return;


        bool isSuccess = false;

        if (isHand)
        {
            //if its hand chck if can send.
            isSuccess = uiHandler.SendFromHandToProduction(item.listIndex);
        }
        else
        {
            //if its production th
           isSuccess = uiHandler.SendFromProductionToHand(item.listIndex, transform);
        }

        if (isSuccess)
        {
        }

    }


    //there are three types of ui signs: can use something, cannot us ingreidnet, andd its not ingredient.

    public void UpdateAvaialabity(bool shouldBeAvaialble)
    {
        if (!isHand) return;

        if (shouldBeAvaialble)
        {
            currentAvailabityType = AvailabityType.Allowed;
            availabilityImage.color = greenColor;
            
        }
        else
        {
            currentAvailabityType = AvailabityType.NotAllowedBecauseOfRecipe;
            availabilityImage.color = yellowColor;
        }

        

    }

    public bool ShouldSkipThisHand()
    {
        return currentAvailabityType == AvailabityType.NotType;
    }

    public enum AvailabityType
    {
        Allowed,
        NotAllowedBecauseOfRecipe,
        NotType
    }

}

