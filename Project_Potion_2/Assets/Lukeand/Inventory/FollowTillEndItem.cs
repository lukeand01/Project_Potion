using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTillEndItem : FollowTillEnd
{
    //this 

    ItemClass item;
    IInventory inventoryTarget;

    float currentTimer;
    float totalTimer;

    Vector3 delayDir;
    public void SetUp(ItemClass item, GameObject target, float speed)
    {
        SetUpBase(target.transform, speed);


        inventoryTarget = target.GetComponent<IInventory>();

        if(item != null)
        {
            this.item = new ItemClass(item.data, item.quantity, 0);
            rend.sprite = item.data.itemSprite;
        }
        

        

        
    }

    public void MakeJustGraphical(ItemClass item)
    {
           


        rend.sprite = item.data.itemSprite;
    }

    public void MakeDelay(float timer)
    {

        cantFollow = true;
        totalTimer = timer; 
    }
    Rigidbody2D rb;
    public void MakePush(Vector3 pushDir)
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.AddForce(new Vector3(pushDir.x , pushDir.y * 8,0), ForceMode2D.Impulse);

    }

    private void Update()
    {
        if(totalTimer > currentTimer)
        {
            currentTimer += Time.deltaTime;
        }
        else
        {
            Debug.Log("should be bale to follow");
            if(rb != null)
            {
                Destroy(rb);
                rb = null;
            }

            cantFollow = false;
        }
    }

    protected override void Act()
    {
        //we also give the information to the fella.

       if(inventoryTarget != null || item != null) inventoryTarget.IReceiveItem(item);
        base.Act();
    }

}
