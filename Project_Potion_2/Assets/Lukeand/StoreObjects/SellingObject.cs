using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingObject : StoreObject, IInteractable
{

    //how to organize the list of npcs waiting for the thing.
    //the problem it is if its a straight line then we might occupy too much space bcause we neede a fella per slot.
    //also it wouldnt look good if we put them all togehter.
    //the best possible scenario is creating two lines side by side.
    //if the line upddats while another npc is moving they should be updated.

    [SerializeField] Transform fadeContainer;


    List<NPCBase> npcMovingList = new(); //use this list to give updated information.
    List<NPCBase> npcArrived1List = new();
    List<NPCBase> npcArrived2List = new();

    bool arrived2Turn;
    Vector3 nextAvailablePos = Vector3.zero;

    #region INTERACTABLE
    public string GetInteractName(PlayerInventory inventory, bool isSecond =false)
    {
        return "Sell";
    }

    public void Interact(PlayerInventory inventory)
    {
        //we take the first npc and give the order.
        //make mony from the item it has

        float gainedValue = 0;
        if (arrived2Turn && npcArrived2List.Count > 0)
        {
            arrived2Turn = false;
            gainedValue = npcArrived2List[0].GetTotalItemCost();
            npcArrived2List[0].OrderLeave();
            npcArrived2List.RemoveAt(0);
            UpdateAllArrivedNpc(-1);
            
        }
        else
        {
            arrived2Turn = true;
            gainedValue = npcArrived1List[0].GetTotalItemCost();
            npcArrived1List[0].OrderLeave();
            npcArrived1List.RemoveAt(0);
            UpdateAllArrivedNpc(1);

        }
        GainMoney((int)gainedValue);
        UpdateAllMovingNpc();

    }

    public bool IsInteractable(PlayerInventory inventory)
    {
        return GetNpcTotalCount() > 0;
    }
    

    public bool IsSecondInteractable(PlayerInventory inventory)
    {
        return false;
    }

    public void SecondInteract(PlayerInventory inventory)
    {
        
    }

    public void UIInteract(PlayerInventory inventory, bool isClose = false)
    {

    }
    #endregion

    #region NPC LIST
    public void AddNpcToMovingList(NPCBase npc)
    {
        npcMovingList.Add(npc);
    }
    public void RemoveNpcFromMovingList(NPCBase npc)
    {
        for (int i = 0; i < npcMovingList.Count; i++)
        {
            if(npc.id == npcMovingList[i].id)
            {
                npcMovingList.RemoveAt(i);
                return;
            }
        }

        Debug.LogError("DIDNT FIND THE NPC IN THE MOVING LIST");
    }

    public void AddNpcToArrivedList(NPCBase npc)
    {

        //if it got then it must have something in the moving list.
        RemoveNpcFromMovingList(npc);

        //check if the bastard is in the
        if(npcArrived1List.Count > npcArrived2List.Count)
        {
            npcArrived2List.Add(npc);
        }
        else
        {
            npcArrived1List.Add(npc);
        }

        UpdateAllMovingNpc();
    }


    void UpdateAllMovingNpc()
    {
        //now we update then refering to the next available position.
        nextAvailablePos = GetLinePos();


        foreach (var item in npcMovingList)
        {
            item.MoveTo(nextAvailablePos);
        }
    }

    void UpdateAllArrivedNpc(int arriveList)
    {
        if(arriveList == 1)
        {
            for (int i = 0; i < npcArrived1List.Count; i++)
            {
                npcArrived1List[i].MoveTo(GetNextInLinePos(arriveList, i));
            }
        }

        if(arriveList == -1)
        {
            for (int i = 0; i < npcArrived2List.Count; i++)
            {
                npcArrived2List[i].MoveTo(GetNextInLinePos(arriveList, i));
            }
        }
    }
    #endregion

    #region GET POS

    public Vector3 GetCurrentFreePos()
    {
        if(nextAvailablePos == Vector3.zero)
        {
            nextAvailablePos = GetLinePos();
        }

        return nextAvailablePos;

    }

    Vector3 GetLinePos()
    {
        float listModifier = GetListModifier();
        Vector3 xOffset = new Vector2(listModifier * 0.5f, 0);

        Vector3 yOffset = listModifier == 1? new Vector2(0, -npcArrived1List.Count): new Vector2(0, -npcArrived2List.Count);

        Vector3 pos = transform.position + Vector3.down + xOffset + yOffset;
        return pos;
    }
    Vector3 GetNextInLinePos(int arriveList, int arriveIndex)
    {
        //next in line for this fella.

        Vector3 xOffset = new Vector2(arriveList * 0.5f, 0);
        Vector3 yOffset = new Vector2(0, -arriveIndex + 1);
        Vector3 pos = transform.position + Vector3.down + xOffset + yOffset;
        return pos;
    }

    #endregion

    #region MONEY&FAME
    //we spawn money here when the player buys something
    //fame as well.

    [ContextMenu("DebugGainMoney")]
    public void DEBUGGAINMONEY()
    {
        GainMoney(Random.Range(50,200));
    }
    [ContextMenu("DEBUGGAINMANYMONEY")]
    public void DEBUGGAINMANYMONEY()
    {
        StartCoroutine(TESTMONEYPROCESS());
    }

    IEnumerator TESTMONEYPROCESS()
    {
        yield return new WaitForSeconds(0.5f);
        GainMoney(Random.Range(50, 200));
        yield return new WaitForSeconds(0.2f);
        GainMoney(Random.Range(50, 200));
        yield return new WaitForSeconds(0.4f);
        GainMoney(Random.Range(50, 200));
    }

    void GainMoney(int value)
    {
        PlayerHandler.instance.GainMoney(value);
        FadeUI newObject = GameHandler.instance.CreateFadeUI();
        newObject.SetUp(value.ToString(), Color.green);
        newObject.transform.parent = fadeContainer;
        newObject.transform.position = fadeContainer.transform.position;


        float randomX = Random.Range(-0.55f, 0.55f);
        float randomY = Random.Range(-0.35f, 0.35f);


        newObject.transform.position += new Vector3(randomX, randomY, 0);
    }
    


    #endregion


    #region UTILS
    float GetListModifier()
    {
        if (npcArrived1List.Count > npcArrived2List.Count) return -1;
        else return 1;
    }
  
    public int GetNpcTotalCount() => npcArrived1List.Count + npcArrived2List.Count;

    public override SellingObject GetSellingObject() => this;
    #endregion

}
