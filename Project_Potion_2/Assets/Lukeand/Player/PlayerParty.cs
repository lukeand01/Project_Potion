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
        
      

    }

    private void Start()
    {
        //here we assign thee saved data too.
        ChampUI champUI = UIHolder.instance.champ;
        foreach (var item in referenceChampList)
        {
            ChampClass newChampClass = new(item);
            champUI.CreateUnitForChampClass(newChampClass);
            champList.Add(newChampClass);
        }

    }

    public List<ChampClass> GetAvailableChampList()
    {
        List<ChampClass> newList = new();

        foreach (var item in champList)
        {
            if(item.champCopies > 0)
            {
                newList.Add(item);
            }
        }

        return newList;
    }


}

//how should i deal with the loading?
//because i create new problems when i load the player. but how do i restore the champ? or do i leave turned off but dont destroy.