using UnityEngine;

public class ItemHandUnit : MonoBehaviour
{
    //this is the thing being held by the player.
    //should it be monobehavior?
    ItemClass item;
    SpriteRenderer rend;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    public void SetUp(ItemClass item, int sortingLayer)
    {
        SetItem(item);
        SetSortingOrder(sortingLayer);
       
    }

    public void SetItem(ItemClass item)
    {
        this.item = item;
        UpdateRend();
    }

    public void SetSortingOrder(int sortingLayer)
    {
        rend.sortingOrder = sortingLayer;
    }

    
    public void UpdateRend()
    {
        if (item.data == null)
        {
            return;
        }
        if (item.data.itemSprite == null)
        {
            Debug.LogError("not sprite for item");
            return;
        }
        rend.sprite = item.data.itemSprite;
    }
    

}
