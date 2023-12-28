using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaidInventoryUI : MonoBehaviour
{
    //this will show the current itens the player has.
    GameObject holder;


    [Separator("BASE")]
    [SerializeField] Transform container;
    [SerializeField] RaidInventoryUnit raidInventoryTemplate;

    [Separator("Description")]
    [SerializeField] GameObject descriptionHolder;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject; 
    }

    private void Start()
    {
        
    }

    public void Open()
    {
        holder.SetActive(true);
    }
    public void Close()
    {
        holder.SetActive(false);
        descriptionHolder.SetActive(false);
    }


    //it updates as the inventory changes

    public void UpdateUI(List<ItemClass> inventoryList)
    {
        //wehn we update this we are going to group everyone.


        ClearUI(container);

        foreach (var item in inventoryList)
        {
            RaidInventoryUnit newObject = Instantiate(raidInventoryTemplate, Vector2.zero, Quaternion.identity);
            newObject.SetUp(item, this);
            newObject.transform.parent = container;
        }


    }


    public void SelectItemForDescription(ItemClass item)
    {
        //jsut describe this item. cannot delete the item thats for later.
        descriptionHolder.SetActive(true);
        portrait.sprite = item.data.itemSprite;
        nameText.text = item.data.itemName;
        descriptionText.text = item.data.itemDescription;
    }


    void ClearUI(Transform container)
    {
        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }
}
//i want the raid ui to show: map, inventory, a leave button, 