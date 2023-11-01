using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestUIUnit : ButtonBase
{
    ItemClass item;
    PlayerInventory inventoryHandler;
    ChestUI uiHandler;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] GameObject empty;

    bool isChest;

    float totalClick;
    float currentClick;
    int amountOfClicks;

    public void SetUp(ItemClass item, bool isChest)
    {
        this.item = item;
        this.isChest = isChest;

        inventoryHandler = PlayerHandler.instance.inventory;
        uiHandler = UIHolder.instance.chest;

        totalClick = 0.25f;

        UpdateUI();
    }

    public void UpdateUI()
    {

        empty.gameObject.SetActive(item.data == null);
        if(item.data == null)
        {
            return;
        }

        portrait.sprite = item.data.itemSprite;
        nameText.text = item.data.itemName;
        quantityText.gameObject.SetActive(isChest);
        quantityText.text = item.quantity.ToString();
    }


    private void FixedUpdate()
    {

        if(amountOfClicks > 0)
        {
            if(currentClick > 0)
            {
                currentClick -= 0.01f;
            }
            else
            {
                amountOfClicks = 0;
            }

        }
        
    }


    public void SelfDestroy() => Destroy(gameObject);

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (empty.activeInHierarchy) return;

        amountOfClicks++;
        currentClick = totalClick;

        if(amountOfClicks == 1)
        {
            uiHandler.SelectUnit(this, item);
            return;
        }

        if (isChest) ChestAction();
        else HandAction();
    }

    //theere is a little effect for the thing being teleported.
    //seend the info in a little animation.
    void ChestAction()
    {
        bool success = inventoryHandler.SendItemChestToHand(item, uiHandler.transform);

        if (success)
        {
            
        }
        else
        {

        }

    }
    void HandAction()
    {
        //check if it has space in chest.
        //
        bool success = inventoryHandler.SendItemHandToChest(item, uiHandler.transform);


        if (success)
        {
            
            //then we destroy this fella.
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("failure");
            //maybe do some warning
        }
    }

    public bool HasItem()
    {
        if (item == null) return false;
        if (item.data == null) return false;
        return true;
    }

}

//need to tell if its a 