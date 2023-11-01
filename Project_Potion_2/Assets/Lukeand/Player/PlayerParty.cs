using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{

    PlayerHandler handler;

    [SerializeField] List<ChampData> referenceChampList = new();
    List<ChampClass> champList = new();


    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
        ChampUI champUI = UIHolder.instance.champ;
        foreach (var item in referenceChampList)
        {
            ChampClass newChampClass = new(item);
            champUI.CreateUnitForChampClass(newChampClass);
            champList.Add(newChampClass);
        }
    }

    private void Start()
    {
        //here we assign thee saved data too.


    }

}