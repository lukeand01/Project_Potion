using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class RaidEndInventoryUnit : ButtonBase
{
    //it can be clcicked for organizing who will be sold and who will not.


    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] Image statusImage;

    [Separator("COLORS")]
    [SerializeField] Color colorStack;
    [SerializeField] Color colorCanTake;
    [SerializeField] Color colorSell;

    RaidInventoryType raidInventoryType;

    public void SetUp(ItemClass item, RaidInventoryType  raidInventoryType)
    {

        if(item == null)
        {
            Debug.Log("this was the problem ");
            return;
        }

        if (item.data == null)
        {
            Debug.Log("this was actually the problem ");
            return;
        }

        portrait.sprite = item.data.itemSprite;
        nameText.text = item.data.itemName;
        quantityText.text = item.quantity.ToString();
        this.raidInventoryType = raidInventoryType;
        ChangeColor();
    }

    void ChangeColor()
    {
        if(raidInventoryType == RaidInventoryType.Can)
        {
            statusImage.color = colorCanTake;
        }
        if (raidInventoryType == RaidInventoryType.Sell)
        {
            statusImage.color = colorSell;
        }
        if (raidInventoryType == RaidInventoryType.Stack)
        {
            statusImage.color = colorStack;
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        //stak - so basicaclly if its stacked then you cannot interact with it.
        //can - but if its using a free slot then you can choose.
        //cannot - then it only can do anything when 


        if (raidInventoryType == RaidInventoryType.Stack) return;

        if(raidInventoryType == RaidInventoryType.Can)
        {
            //then we remove this from the list and put it to seel
            Debug.Log("Can");
        }

        if (raidInventoryType == RaidInventoryType.Sell)
        {
            //we check if there is space and we put in the inventory tab.
            Debug.Log("sell");
        }
    }

    public void Hide()
    {
        transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        gameObject.SetActive(false);
    }
    public void Show()
    {
        //show just means it increases in size.
        transform.DOScale(1, 0.25f);
    }

}

public enum RaidInventoryType
{
    Stack,
    Can,
    Sell
}