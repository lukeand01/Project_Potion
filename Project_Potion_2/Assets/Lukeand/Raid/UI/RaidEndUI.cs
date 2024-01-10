using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class RaidEndUI : MonoBehaviour
{
    //show everyone that i got or show the player's failure
    //show all itens that the player got. show the limit of the chest.
    //show another container for seeling those itens.
    //get all the itens received and separaeted a list of stacckable. any excess of such iten will be separated into its own category for deccisioning.
    //show experineced received by champs. support receive half the xp.



    GameObject holder;

    [Separator("SCORE")]
    [SerializeField] GameObject scoreHolder;
    [SerializeField] Image scoreBackground;
    [SerializeField] TextMeshProUGUI scoreText;
    Vector3 scoreTextOriginalPos;
    float scoreModifier;


    [Separator("CHAMP")]
    [SerializeField] Transform champContainer;
    [SerializeField] RaidChampEndUnit champEndTemplate;
    [SerializeField] Transform[] champPos;
    List<RaidChampEndUnit> raidChampUnitList = new();

    [Separator("INVENTORY")]
    [SerializeField] RaidEndInventoryUnit raidEndInventoryTemplate;
    [SerializeField] Transform inventoryContainer;
    List<RaidEndInventoryUnit> raidInventoryList = new();



    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
        scoreTextOriginalPos = scoreText.transform.position;
    }

    public void Open()
    {
        holder.SetActive(true);
    }

    public void Close()
    {
        holder?.SetActive(false);   
    }

    public void StartDefeat(List<ChampClass> copyChampList, float totalExperienceGained, RaidScoreType raidScore)
    {
        Debug.Log("start defeat");
        ReadyScore(raidScore);
        ReadyEmptyInventoryList();
        ReadyChamps(totalExperienceGained);
        StartCoroutine(DisplayProcess());
    }


    public void StartVictory(List<ChampClass> copyChampList, float totalExperienceGained, RaidScoreType raidScore)
    {


        ReadyScore(raidScore);
        ReadyChamps(totalExperienceGained);
        ReadyRaidInventoryList();

        StartCoroutine(DisplayProcess());   
    }




    void ReadyScore(RaidScoreType raidScore)
    {    
        scoreModifier = (float)raidScore * 0.01f;
        scoreText.text = raidScore.ToString();

        scoreHolder.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }


    //we add all the itens in the inventopry of pchandler.
    //we check who can be added and who cannot

    void ReadyRaidInventoryList()
    {
        //we need to get the inventroy of the items we found.

        List<ItemClass> itemList = PCHandler.instance.inventory.raidList;
        raidInventoryList.Clear();

        foreach (var item in itemList)
        {
            if (item.data == null) continue;
            RaidEndInventoryUnit newObject = Instantiate(raidEndInventoryTemplate, Vector2.zero, Quaternion.identity);
            newObject.transform.parent = inventoryContainer;
            newObject.SetUp(item, item.raidinventoryType);
            newObject.Hide();
            raidInventoryList.Add(newObject);
        }
       
        //
    }

    void ReadyEmptyInventoryList()
    {

    }

    void ReadyChamps(float totalExpGained)
    {
        //get the 


        PCHandler handler = PCHandler.instance;
        raidChampUnitList.Clear();
        RaidChampEndUnit newObject = Instantiate(champEndTemplate, Vector2.zero, Quaternion.identity);
        newObject.transform.parent = champContainer;
        newObject.transform.position = champPos[0].transform.position;
        newObject.SetUp(new ChampClass(handler.champ),  totalExpGained, scoreModifier ,  true);
        newObject.Hide();
        List<ChampClass> allyList = handler.GetAllies();

        
        raidChampUnitList.Add(newObject);



        for (int i = 0; i < allyList.Count; i++)
        {
            RaidChampEndUnit secondObject = Instantiate(champEndTemplate, Vector2.zero, Quaternion.identity);
            secondObject.SetUp(allyList[i], totalExpGained / 3, scoreModifier,  false);
            newObject.transform.parent = champContainer;
            secondObject.transform.position = champPos[i + 1].transform.position;
            secondObject.Hide();
            raidChampUnitList.Add(secondObject);
        }



    }



    IEnumerator DisplayProcess()
    {
        holder.SetActive(true); 
       yield return StartCoroutine(ResultProcess());
        yield return StartCoroutine(ChampProcess());
        yield return StartCoroutine(ItemsProcess());


    }
    
    IEnumerator ResultProcess()
    {
        //bring in the background of the holder.
        //then 

        scoreHolder.SetActive(true);
        var backgroundColor = scoreBackground.color;
        backgroundColor.a = 0;
        scoreBackground.color = backgroundColor;

        scoreText.gameObject.SetActive(true);
        var textColor = scoreText.color;
        textColor.a = 0;
        scoreText.color = textColor;
        scoreText.transform.position = scoreTextOriginalPos;
        scoreText.transform.localScale = Vector3.one;

        while(scoreBackground.color.a < 1)
        {
            scoreBackground.color += new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.005f);
        }

        //then we make the text fade in and fall into place.


        while (scoreText.color.a < 1)
        {
            scoreText.color += new Color(0, 0, 0, 0.1f);
            scoreText.transform.position -= new Vector3(0, 12f, 0);
            yield return new WaitForSeconds(0.01f);
        }



    }
    IEnumerator ChampProcess()
    {
        //

        for (int i = 0; i < raidChampUnitList.Count; i++)
        {
            raidChampUnitList[i].gameObject.SetActive(true);
            raidChampUnitList[i].Show();
            if (i == 0)
            {
                yield return new WaitForSeconds(0.2f);
            }
        }

        yield return new WaitForSeconds(0.5f);

        foreach (var item in raidChampUnitList)
        {
            item.ShowExperience();
        }

    }
    IEnumerator ItemsProcess()
    {
        foreach (var item in raidInventoryList)
        {
            item.gameObject.SetActive(true);            
            item.Show();
            yield return new WaitForSeconds(0.1f);
        }


    }

    public void ReturnBase()
    {

        //we some itens and give others.

        RaidHandler raid = GameHandler.instance.raid;
        raid.EndRaid();

    }
    
    public void ReplayCurrentStage()
    {
        //we receive everything but we start the raid.
        RaidHandler raid = GameHandler.instance.raid;
        raid.EndRaid();
    }
    public void PlayNextStage()
    {
        //we go to thee next if there is next.
        RaidHandler raid = GameHandler.instance.raid;
        raid.EndRaid();

    }


    //i just need to hold the itens somewhere.
    //hold thee money gained.
    //also hold thee champions?


}



//first i will just show the grade.