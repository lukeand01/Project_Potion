using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class ChestUI : MonoBehaviour
{
    GameObject holder;
    [SerializeField] ChestUIUnit chestUnitTemplate;
    [SerializeField] Transform chestContainer;
    [SerializeField] Transform handContainer;


    [SerializeField] UIDescriptor descriptor;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    private void FixedUpdate()
    {
        CheckSelectedUnit();
    }

    public void SetUpChestUnits(List<ItemClass> itemList)
    {
        foreach (var item in itemList)
        {
            ChestUIUnit newObject = Instantiate(chestUnitTemplate, new Vector3(0, 0, 0), Quaternion.identity);
            newObject.transform.parent = chestContainer;
            newObject.SetUp(item, true);
            item.UpdateChestUnit(newObject);
        }
        
    }

    public void CreateHandUnits(List<ItemClass> itemList)
    {
        ClearContainer(handContainer);
        foreach (var item in itemList)
        {
            ChestUIUnit newObject = Instantiate(chestUnitTemplate, new Vector3(0, 0, 0), Quaternion.identity);
            newObject.transform.parent = handContainer;
            newObject.SetUp(item, false);
            item.UpdateChestUnit(newObject);
        }
    }
    public void FTEForHand(ItemClass item)
    {
        GameHandler.instance.CreateFTEImage(item, item.GetTransfromOfChestUnit(), handContainer.transform, transform, 100f);
    }

    ChestUIUnit selectedUnit;
    public void SelectUnit(ChestUIUnit selectedUnit, ItemClass item)
    {
        this.selectedUnit = selectedUnit;
        descriptor.ControlHolder(true);
        descriptor.UpdateBasic(item.data.itemSprite, item.data.itemName, item.data.itemDescription);
    }

    void CheckSelectedUnit()
    {
        if (selectedUnit == null) return;

        if (!selectedUnit.HasItem())
        {
            descriptor.ControlHolder(false);
            selectedUnit = null;
        }
    }


    public void OpenUI() => holder.SetActive(true);
    public void CloseUI() => holder.SetActive(false);

    void ClearContainer(Transform container)
    {
        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }
}


//things i want for the chest ui.
//dragging