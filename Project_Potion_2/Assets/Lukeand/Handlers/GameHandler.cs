using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    public static GameHandler instance;

    [HideInInspector] public StoreHandler store;
    [HideInInspector] public NPCHandler npc;
    [HideInInspector] public CraftHandler craft;

    [Separator("TEMPLATES")]
    [SerializeField] ItemHandUnit itemHandTemplate;
    [SerializeField] FollowTillEndItem fteItemTemplate;
    [SerializeField] FadeUI fadeUITemplate;
    [SerializeField] FollowTillEndImage fteImageTemplate;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        store = GetComponent<StoreHandler>();
        npc = GetComponent<NPCHandler>();
        craft = GetComponent<CraftHandler>();
    }

    public void CreateSFX(AudioClip clip)
    {

    }

    public void CreateFTEItem(ItemClass item,Transform original, Transform target, float speed)
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

    public ItemHandUnit CreateItemHandUnit()
    {
        return Instantiate(itemHandTemplate, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public FadeUI CreateFadeUI()
    {
        return Instantiate(fadeUITemplate, new Vector3(0, 0, 0), Quaternion.identity);
    }

}
