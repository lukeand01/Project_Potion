using MyBox;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class RaidUI : MonoBehaviour
{
    //this is just for the main scene ui.
    //


    GameObject holder;
    [Separator("WORLD")]
    [SerializeField] RaidWorldUnit worldUnitTemplate;
    [SerializeField] Transform worldContainer;

    

    [Separator("STAGE")]
    [SerializeField] GameObject stageHolder;
    [SerializeField] Transform stageContainer;
    [SerializeField] RaidStageUnit stageUnitTemplate;
    [SerializeField] GameObject selectStageButton;
    Vector2 stageHolderInitialPos;

    [Separator("SELECTION")]
    [SerializeField] GameObject selectionHolder;
    [SerializeField] Transform selectAvailableChampContainer;
    [SerializeField] Transform selectAllyChampContainer;
    [SerializeField] RaidSelectChampUnit selectMainChampUnit;
    [SerializeField] RaidSelectChampUnit selectChampUnitTemplate;
    List<RaidSelectChampUnit> selectSlotUnitSupportList = new();
    Vector2 selectionHolderInitialPos;

    [HideInInspector] public DraggableSelectChampHandler draggableHandler;

    List<ChampClass> selectedChampList = new();

    [Separator("MISC")]
    [SerializeField] GameObject returnButton;

    bool isProcess;
    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
        draggableHandler = gameObject.GetComponent<DraggableSelectChampHandler>();
    }
    public void SetUp(List<RaidWorldData> worldDataList)
    {
        foreach (var item in worldDataList)
        {
            RaidWorldUnit newObject = Instantiate(worldUnitTemplate, new Vector3(0, 0, 0), Quaternion.identity);
            newObject.SetUp(this, item);
            newObject.transform.parent = worldContainer;
        }

        stageHolderInitialPos = stageHolder.transform.position;
        selectionHolderInitialPos = selectionHolder.transform.position;
    }


    public void OpenUI()
    {
        holder.SetActive(true);
        selectMainChampUnit.SetUp(this, null, true);
        UpdateAvailableChamps();
    }
    public void CloseUI()
    {
        StopAllCoroutines();
        ResetAllPieces();
        holder.SetActive(false);
    }

    private void Update()
    {
        if (!holder.activeInHierarchy) return;
        returnButton.SetActive(selectionHolder.activeInHierarchy || stageHolder.activeInHierarchy);
    }

    #region WORLD
    RaidWorldData currentWorld;

    public void SelectWorld(RaidWorldData worldData)
    {
        if (isProcess) return;
        currentWorld = worldData;        
        StartCoroutine(OpenStageProcess());
        //do the animation for the thing 
    }
    void CloseStage()
    {
        if (isProcess) return;
        StartCoroutine(CloseStageProcess());
    }

    IEnumerator OpenStageProcess()
    {
        //first we spawn the stages.
        //second we preparre the description.
        //third we bring it to the right.
        isProcess = true;

        ClearUI(stageContainer);
        SpawnStageUnits();

        stageHolder.SetActive(true);
        while (stageHolderInitialPos.x + Screen.width  > stageHolder.transform.position.x)
        {
            stageHolder.transform.position += new Vector3(20, 0);
            yield return new WaitForSeconds(Time.deltaTime * 0.01f);
        }

        isProcess = false;
    }

    IEnumerator CloseStageProcess()
    {
        isProcess = true;
        while (stageHolder.transform.position.x > stageHolderInitialPos.x)
        {
            stageHolder.transform.position -= new Vector3(20, 0);
            yield return new WaitForSeconds(Time.deltaTime * 0.01f);
        }
        stageHolder.SetActive(false);
        isProcess = false;

        
    }

    void SpawnStageUnits()
    {
        foreach (var item in currentWorld.stageList)
        {
            RaidStageUnit newObject = Instantiate(stageUnitTemplate, Vector3.zero, Quaternion.identity);
            newObject.SetUp(this, item);
            newObject.transform.parent = stageContainer;
        }
    }

    #endregion

    #region STAGE
    RaidStageData currentStage;
    RaidStageUnit currentStageUnit;

    public void SelectStage(RaidStageData stageData, RaidStageUnit newStageUnit)
    {
        if (isProcess) return;

        if(currentStageUnit != null)
        {
            currentStageUnit.Select(false);
        }

        currentStage = stageData;
        currentStageUnit = newStageUnit;
        currentStageUnit.Select(true);

        DescribeStage();
        selectStageButton.SetActive(true);
    }

    void DescribeStage()
    {
        
        
    }

    public void CloseSelection()
    {
        if (isProcess) return;
        StartCoroutine(CloseSelectionProcess());
    }

    public void ConfirmStage()
    {

        if (isProcess) return;
        StartCoroutine(OpenSelectionProcess());
    }

    IEnumerator OpenSelectionProcess()
    {
        isProcess = true;

        selectionHolder.SetActive(true);
        CreatePlayerSlots();

        while(selectionHolderInitialPos.y - Screen.height < selectionHolder.transform.position.y)
        {
            selectionHolder.transform.position -= new Vector3(0, 20);
            yield return new WaitForSeconds(Time.deltaTime * 0.01f);
        }


        isProcess = false;

        yield return null;
    }
    IEnumerator CloseSelectionProcess()
    {
        isProcess = true;

        while (selectionHolder.transform.position.y <  selectionHolderInitialPos.y)
        {
            selectionHolder.transform.position += new Vector3(0, 20);
            yield return new WaitForSeconds(Time.deltaTime * 0.01f);
        }
        selectionHolder.SetActive(false);
        isProcess = false;
        yield return null;
    }

    #endregion

    #region SELECTION    
  
    public void SelectChamp(ChampClass champ)
    {
        //we describe the bastard. in question.
    }
  
    public void ResetUnits(ChampClass champ)
    {
        selectMainChampUnit.TryRemoveChampHere(champ);

        foreach (var item in selectSlotUnitSupportList)
        {
            item.TryRemoveChampHere(champ);
        }

    }

    void UpdateAvailableChamps()
    {
        //here we get a list from the player
        List<ChampClass> champList = PlayerHandler.instance.party.GetAvailableChampList();

        ClearUI(selectAvailableChampContainer);

        foreach (var item in champList)
        {
            RaidSelectChampUnit newObject = Instantiate(selectChampUnitTemplate, Vector2.zero, Quaternion.identity);
            newObject.SetUp(this, item, false);
            newObject.transform.parent = selectAvailableChampContainer;
        }

    } //these are updates for the available.

    void CreatePlayerSlots()
    {
       

        ClearUI(selectAllyChampContainer);
        selectSlotUnitSupportList.Clear();

        for (int i = 0; i < 2; i++)
        {
            RaidSelectChampUnit newObject = Instantiate(selectChampUnitTemplate, Vector2.zero, Quaternion.identity);
            newObject.transform.parent = selectAllyChampContainer;
            newObject.SetUp(this, null, true);
            selectSlotUnitSupportList.Add(newObject);
        }


    } //these are for the champ slots.

    public void ConfirmRaid()
    {
        //start the loadidng process.
        if (!selectMainChampUnit.HasChamp()) return;
        //if at least main has a champ then we can start.
        List<ChampClass> champSelectedList = new();

        champSelectedList.Add(selectMainChampUnit.GetChamp());

        foreach (var item in selectSlotUnitSupportList)
        {
            champSelectedList.Add(item.GetChamp());
        }

        //then we give this list to a gamehandler that will now load everything to start the raid.
    }

    #endregion




    void ResetAllPieces()
    {
        //reset everything intot he right place. 
        StopAllCoroutines();
        isProcess = false;
        selectionHolder.transform.position = selectionHolderInitialPos;
        selectionHolder.SetActive(false);
        stageHolder.transform.position = stageHolderInitialPos;
        stageHolder.SetActive(false);

        selectStageButton.SetActive(false);
    }

    public void Return()
    {
        //we check the things that are working.


        if (selectionHolder.activeInHierarchy)
        {
            CloseSelection();
            return;
        }

        if (stageHolder.activeInHierarchy)
        {
            CloseStage();
            return;
        }

    }

    void ClearUI(Transform targetContainer)
    {
        for (int i = 0; i < targetContainer.childCount; i++)
        {
            Destroy(targetContainer.GetChild(i).gameObject);
        }
    }
}

//also when the player is using someone that is already being used we do something.
//so we ask everyone else that isnt the unit we interactd with if they have the same fella.


//now we will try also to add the drag feature into it.
//