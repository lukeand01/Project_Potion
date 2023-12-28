using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] RaidChampEndUnit champEndTemplate;
    [SerializeField] Transform[] champPos;
    List<RaidChampEndUnit> raidChampUnitList = new();

    [Separator("INVENTORY")]
    [SerializeField] RaidEndInventoryUnit raidEndInventoryTemplate;
    [SerializeField] Transform inventoryContainer;




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
        ReadyScore(raidScore);
        ReadyEmptyInventoryList();
        ReadyChamps(totalExperienceGained);
        StartCoroutine(DisplayProcess());
    }


    public void StartVictory(List<ChampClass> copyChampList, float totalExperienceGained, RaidScoreType raidScore)
    {
        Debug.Log("start victory");

        //we set all those things but we make them invisible to show one by one.


        ReadyScore(raidScore);
        ReadyChamps(totalExperienceGained);
        //GetRaidInventoryList();

        StartCoroutine(DisplayProcess());   
    }

    List<RaidEndInventoryUnit> raidInventoryList = new();


    void ReadyScore(RaidScoreType raidScore)
    {    
        scoreModifier = (float)raidScore * 0.01f;
        Debug.Log("the score modifier " + scoreModifier);
        scoreText.text = raidScore.ToString();

        scoreHolder.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }


    //we add all the itens in the inventopry of pchandler.
    //we check who can be added and who cannot

    void ReadyRaidInventoryList()
    {
        //here we get

        //we check this fella here to tell 
        //this is jsut for show.







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
        newObject.SetUp(new ChampClass(handler.champ),  totalExpGained, scoreModifier ,  true);
        newObject.Hide();
        List<ChampClass> allyList = handler.GetAllies();

        
        raidChampUnitList.Add(newObject);

        for (int i = 0; i < allyList.Count; i++)
        {
            RaidChampEndUnit secondObject = Instantiate(champEndTemplate, Vector2.zero, Quaternion.identity);
            secondObject.SetUp(allyList[i], totalExpGained / 3, scoreModifier,  false);
            secondObject.transform.parent = transform;
            secondObject.transform.position = champPos[i + 1].transform.position;
            secondObject.Hide();
            raidChampUnitList.Add(secondObject);
        }



    }



    IEnumerator DisplayProcess()
    {
        holder.SetActive(true); 
       yield return StartCoroutine(ChampProcess());
       yield return StartCoroutine(ResultProcess());
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
        Debug.Log("start champ");

        //show each

        foreach (var item in raidChampUnitList)
        {
            item.gameObject.SetActive(true);
            item.Show();            
            yield return new WaitForSeconds(0.2f);
            item.ShowExperience();
            yield return new WaitForSeconds(0.5f);
        }


        yield return null;
    }
    IEnumerator ItemsProcess()
    {
        yield return null;
    }

    public void Confirm()
    {

        //we some itens and give others.

        RaidHandler raid = GameHandler.instance.raid;
        //raid.ReceiveNewItens();
        raid.EndRaid();

    }
    

}



//first i will just show the grade.