using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class ProductionUI : MonoBehaviour
{
    GameObject holder;

    ProductionObject currentProduction;

    //update the units.
    //
    [SerializeField] IngredientUnit ingredientUnitTemplate;
    List<IngredientUnit> ingredientUnitList = new();
    List<IngredientUnit> handList = new();
    [SerializeField] Transform ingredientContainer;
    [SerializeField] Image portrait;
    [SerializeField] BarUI bar;
    [SerializeField] Transform handContainer;


    [Separator("IINVETNORY POSITIONS")]
    [SerializeField] Transform handDeliveryPos;
    [SerializeField] Transform productionDeliveryPos;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        PlayerHandler.instance.controller.EventMoveInput += CloseUI;
    }

    void CloseUI() => ControlHolder(false);

    public void ControlHolder(bool choice) => holder.SetActive(choice);

    //but also need to know the hand at all times.
    public void StartUI(ProductionObject production)
    {
        currentProduction = production;
        ControlHolder(true);      
        UpdateUI();
    }

    void UpdateUI()
    {
        //we create the current list of ingredients 
        List<ItemClass> slotList = currentProduction.GetSlotList();
        ClearUI(ingredientContainer);
        ingredientUnitList.Clear();

        foreach (var item in slotList)
        {
            IngredientUnit newObject = Instantiate(ingredientUnitTemplate, ingredientContainer.position, Quaternion.identity);
            newObject.transform.parent = ingredientContainer;
            newObject.SetUp(item, false);
            newObject.UpdateUI();
            item.UpdateIngredientUnit(newObject);
            ingredientUnitList.Add(newObject);
        }

        foreach (var item in handList)
        {
            item.UpdateAvaialabity(currentProduction.CanAddIngredient(item.item.data.GetIngredient()));
        }
        //i need a function to tell if this can be addded to the current recipe.

    }

    public void UpdateHandList(List<ItemClass> itemList)
    {
        //we get everyone but give a dark color for non-ingreedients.
        ClearUI(handContainer);
        handList.Clear();

        foreach (var item in itemList)
        {
            IngredientUnit newObject = Instantiate(ingredientUnitTemplate, Vector2.zero, Quaternion.identity);
            newObject.SetUp(item, true);
            newObject.UpdateUI();
            newObject.transform.parent = handContainer;

            if(!newObject.ShouldSkipThisHand())
            {
                handList.Add(newObject);
            }

        }

        if (currentProduction == null) return;


        foreach (var item in handList)
        {
            item.UpdateAvaialabity(currentProduction.CanAddIngredient(item.item.data.GetIngredient()));
        }

    }
    void ClearUI(Transform container)
    {

        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }

    ItemClass GetItemFromProduction(int index)
    {
        return ingredientUnitList[index].item;

    }

    public bool SendFromProductionToHand(int index, Transform original)
    {
        //ask if there is space in playereinventory.
        //just that.


        PlayerInventory inventory = PlayerHandler.instance.inventory;

        if (inventory == null) return false;

        if (!inventory.HasSpaceInHand()) return false;

        //remove from production.
        ItemClass productionItem = GetItemFromProduction(index);

        


        ItemClass itemToGive = new ItemClass(productionItem.data, productionItem.quantity);

        GameHandler.instance.CreateFTEImage(productionItem, original, handDeliveryPos, transform, 150);

        currentProduction.RemoveIngredient(index);
        inventory.AddItemToHand(itemToGive);

        return true;
    }
    public bool SendFromHandToProduction(int index)
    {
        PlayerInventory inventory = PlayerHandler.instance.inventory;

        if (inventory == null) return false;

        int productionIndex = currentProduction.GetFreeSpace();

        if (productionIndex == -1) return false;


       

        ItemClass handItem = inventory.GetItemInHand(index);

        GameHandler.instance.CreateFTEImage(handItem, handDeliveryPos, productionDeliveryPos, transform, 150);

        currentProduction.AddIngredient(handItem, productionIndex);
        inventory.RemoveHandUnit(index);


        return true;
    }


}
