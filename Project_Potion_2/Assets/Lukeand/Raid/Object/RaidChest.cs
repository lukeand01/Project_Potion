using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class RaidChest : MonoBehaviour, IRaidInteractable
{
    //
    bool isOpen;
    [SerializeField]List<ItemClass> itemList = new();
    [SerializeField] GameObject closedIndicator;
    Vector3 originalIndicatorPos;


    public void Interact(PCHandler handler)
    {
        //shoot the itens up and give to the player

        //we throw everyone in the list and update the inventory.

        foreach (var item in itemList)
        {
            PCHandler.instance.inventory.AddRaidItem(item);
        }

        



        gameObject.layer = (int)LayerMaskEnum.Default;
        Destroy(this); //destroy the chest script so it cannot be interacted anymore.

    }

    private void Start()
    {
        originalIndicatorPos = closedIndicator.transform.position; 

        StartCoroutine(AnimateClosedIndicatorProcess());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 3) return;
        if (isOpen) return;
        //and you spawn itens 



        foreach (var item in itemList)
        {
            PCHandler.instance.inventory.AddRaidItem(item);
        }

        //call this.

        StopAllCoroutines();
        closedIndicator.SetActive(false);


        StartCoroutine(ChestOpenProcess());
        isOpen = true;
    }

    [ContextMenu("DEBUG OPEN CHEST")]
    public void DEBUGOPECHEST()
    {

        StartCoroutine(ChestOpenProcess());
    }

    IEnumerator ChestOpenProcess()
    {
        //open

        yield return new WaitForSeconds(0.4f);

        //then we start throwing those fellas up.

        //this goes up and then meets with the player.
        bool isRight = false;
        foreach (var item in itemList)
        {
            Vector3 offset = Vector3.zero;
            if (isRight)
            {
                isRight = false;
                offset = new Vector3(1.2f, 0, 0);
            }
            else
            {
                isRight = true;
                offset = new Vector3(-1.2f, 0 ,0);
            }

            GameHandler.instance.CreateChestItem(Vector3.up + offset, 0.8f, item, transform, PCHandler.instance.transform, 2);
            yield return new WaitForSeconds(0.2f);
        }


    }

    IEnumerator AnimateClosedIndicatorProcess()
    {

        for (int i = 0; i < 30; i++)
        {
            closedIndicator.transform.position -= new Vector3(0, 0.01f, 0);

            yield return new WaitForSeconds(0.005f);
        }

        for (int i = 0; i < 30; i++)
        {
            closedIndicator.transform.position += new Vector3(0, 0.01f, 0);
            yield return new WaitForSeconds(0.005f);
        }

        StartCoroutine(AnimateClosedIndicatorProcess());
    }
    //i want the itens to go up and then to the player.


}

//dont need that. if the player colider it does it stuff.