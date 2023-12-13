using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaidEndUI : MonoBehaviour
{
    //show everyone that i got or show the player's failure
    //show all itens that the player got. show the limit of the chest.
    //show another container for seeling those itens.
    //get all the itens received and separaeted a list of stacckable. any excess of such iten will be separated into its own category for deccisioning.
    //show experineced received by champs. support receive half the xp.



    GameObject holder;

    [Separator("SCORE")]
    [SerializeField] TextMeshProUGUI scoreText;
    float scoreModifier;


    [Separator("CHAMP")]
    [SerializeField] RaidChampEndUnit champEndTemplate;
    [SerializeField] Transform[] champPos;

    [Separator("INVENTORY")]
    [SerializeField] RaidEndInventoryUnit raidEndInventoryTemplate;
    [SerializeField] Transform inventoryContainer;




    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    public void Open()
    {
        holder.SetActive(true);
    }

    public void Close()
    {
        holder?.SetActive(false);   
    }

    public void StartDefeat()
    {
        //no itens gathered just a bit of experiene
        

    }


    //in reality the experience has already worked in pchandler.


    //we will send a copy of the champ and the xp gained.
    //but also the ui will be the thing to decide what item will be added by the player.


    public void StartVictory(List<ChampClass> copyChampList, float totalExperienceGained, RaidScoreType raidScore)
    {
        //we show all the iten gathered.

        //first we show the results.
        //then we show the champ 
        //then we show the itens
        //one after another very quicckly.





        GetScore(raidScore);
        GetRaidInventoryList();
        GetChamps();
        StartCoroutine(VictoryProcess());   
    }

    List<RaidEndInventoryUnit> raidInventoryList = new();

    void GetScore(RaidScoreType raidScore)
    {
      
        scoreModifier = (float)raidScore * 0.01f;
        Debug.Log("the scccore modifier " + scoreModifier);
        scoreText.text = raidScore.ToString();
    }

    void GetRaidInventoryList()
    {
       List<ItemClass> newList = PlayerHandler.instance.inventory.GetListForRaidInventoryBasedInChestInventory(PCHandler.instance.inventory.raidList);

        raidInventoryList.Clear();

        foreach (var item in newList)
        {
            RaidEndInventoryUnit newObject = Instantiate(raidEndInventoryTemplate, Vector2.zero, Quaternion.identity);
            newObject.SetUp(item, item.raidinventoryType);
            newObject.transform.parent = inventoryContainer;
            newObject.Hide();
            raidInventoryList.Add(newObject);
        }

    }

    void GetChamps()
    {
        //get the 
        PCHandler handler = PCHandler.instance;

        RaidChampEndUnit newObject = Instantiate(champEndTemplate, Vector2.zero, Quaternion.identity);
        newObject.SetUp(new ChampClass(handler.champ),  handler.raidGainedExperiene, scoreModifier ,  true);
        newObject.Hide();
        List<ChampClass> allyList = handler.GetAllies();

        for (int i = 0; i < allyList.Count; i++)
        {
            RaidChampEndUnit secondObject = Instantiate(champEndTemplate, Vector2.zero, Quaternion.identity);
            secondObject.SetUp(allyList[i], handler.raidGainedExperiene / 3, scoreModifier,  false);
            secondObject.transform.parent = transform;
            secondObject.transform.position = champPos[i + 1].transform.position;
            secondObject.Hide();

        }



    }



    IEnumerator VictoryProcess()
    {
       yield return StartCoroutine(ChampProcess());
       yield return StartCoroutine(ResultProcess());
       yield return StartCoroutine(ItemsProcess());

        Debug.Log("end of process");
    }

    IEnumerator ResultProcess()
    {
        yield return null;
    }
    IEnumerator ChampProcess()
    {
        yield return null;
    }
    IEnumerator ItemsProcess()
    {
        yield return null;
    }

    public void Confirm()
    {
        //now we give the list 
        //give the newlist
        //and give the gold and diamonds we found

        
        //we get the values.
        //



    }
    

}
