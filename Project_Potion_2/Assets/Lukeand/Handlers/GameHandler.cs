using MyBox;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    public static GameHandler instance;

    [HideInInspector] public StoreHandler store { get; private set; }
    [HideInInspector] public NPCHandler npc { get; private set; }
    [HideInInspector] public CraftHandler craft { get; private set; }
    [HideInInspector] public SceneLoader loader { get; private set; }
    [HideInInspector] public RaidHandler raid { get; private set; }


    [Separator("RAID REF")]
    public List<RaidWorldData> raidWorldList = new();

    [Separator("BD REF")]
    public List<BDData> bdRefList = new();

    [Separator("TEMPLATES")]
    [SerializeField] ItemHandUnit itemHandTemplate;
    [SerializeField] FollowTillEndItem fteItemTemplate;
    [SerializeField] FadeUI fadeUITemplate;
    [SerializeField] FollowTillEndImage fteImageTemplate;


    //this works for now but this is not good.
    [HideInInspector] public PlayerHandler playerHandler {  get; private set; }



    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        store = GetComponent<StoreHandler>();
        npc = GetComponent<NPCHandler>();
        craft = GetComponent<CraftHandler>();
        raid = GetComponent<RaidHandler>(); 
        loader = GetComponent<SceneLoader>();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {



        if(UIHolder.instance.raid) UIHolder.instance.raid.SetUp(raidWorldList);
        if(PlayerHandler.instance != null) playerHandler = PlayerHandler.instance;
    }

    [ContextMenu("SLOW GAME TO 40%")]
    public void SlowGameTo40()
    {
        Time.timeScale = 0.4f;
    }

    [ContextMenu("SLOW GAME TO 10%")]
    public void SlowGameTo10()
    {
        Time.timeScale = 0.1f;
    }


    public void CreateSFX(AudioClip clip)
    {

    }

    public void CreateFTEItem(ItemClass item, Transform original, Transform target, float speed)
    {
        FollowTillEndItem newObject = Instantiate(fteItemTemplate, original.transform.position, Quaternion.identity);
        newObject.SetUp(item, target.gameObject, speed);
    }


    public void CreateFTEImage(ItemClass item, Transform original, Transform target, Transform parent, float speed)
    {
        FollowTillEndImage newObject = Instantiate(fteImageTemplate, Vector3.zero, Quaternion.identity);
        newObject.transform.parent = parent;
        newObject.transform.position = original.position;
        newObject.SetUpBase(target, speed);
        newObject.ChangeSprite(item.data.itemSprite);
    }

    public void CreateChestItem(Vector3 dir, float timer, ItemClass item, Transform original, Transform target, float speed)
    {

        FollowTillEndItem newObject = Instantiate(fteItemTemplate, original.transform.position, Quaternion.identity);
        newObject.SetUp(null, target.gameObject, speed);
        newObject.MakeDelay(timer);
        newObject.MakeJustGraphical(item);
        newObject.MakePush(dir);
        newObject.transform.parent = original; 
    }


    public ItemHandUnit CreateItemHandUnit()
    {
        return Instantiate(itemHandTemplate, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public FadeUI CreateFadeUI()
    {
        return Instantiate(fadeUITemplate, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public BDData GetBDRef(string id)
    {
        foreach (var item in bdRefList)
        {
            if (item.idName == id) return item;
        }
        return null;
    }



    //the player is not destroyed.
    //therefore we can always reachc its thing.

}
//just wait a second. this is a mess.
//lets improve this
//1 - we call in the orioginal sene for the raid to start. now i need to store the chestlist and champlist
//the champlist should be just the fella that the player chose.
//2 - the raid handler should exist in all scenes. it will carry those variables.
//3 - playerhandler is not deleted but it will be turned off in any scene. just so i dont need to load it again. 
//4 - when we come back we will update the original list with this list.