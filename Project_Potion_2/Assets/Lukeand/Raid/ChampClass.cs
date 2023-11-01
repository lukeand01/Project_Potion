using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampClass 
{
    public ChampData data;

    public int champLevel;
    public int champCopies;

    public ChampClass(ChampData data)
    {
        this.data = data;
        champLevel = 1;
        champCopies = 1;
    }

    public ChampClass(ChampData data, int champLevel, int champCopies)
    {
        this.data = data;
        this.champLevel = champLevel;
        this.champCopies = champCopies;
    }



    #region UI UNIT
    public void SetUIUnit(ChampUIUnit champUnit)
    {
        this.champUnit = champUnit;
    }
    ChampUIUnit champUnit;
    #endregion
}
