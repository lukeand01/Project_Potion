using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    GameHandler handler;
    //we unload the scene.
    [SerializeField] int currentScene;

    private void Awake()
    {
        handler = gameObject.GetComponent<GameHandler>();
    }


    public void LoadScene(int newScene)
    {
        StartCoroutine(LoadSceneProcess(newScene));
    }

    int brake = 0;

    IEnumerator LoadSceneProcess(int newScene)
    {

        //bring the black screen.
        //i can load an empty scene

        AsyncOperation emptyAsync = SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);

        while (!emptyAsync.isDone)
        {
            yield return new WaitForSeconds(0.01f);
        }

        AsyncOperation unloadAsync = SceneManager.UnloadSceneAsync(currentScene, UnloadSceneOptions.None);

        while (!unloadAsync.isDone)
        {
            yield return new WaitForSeconds(0.01f);
        }

        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

        while (!loadAsync.isDone)
        {
            yield return new WaitForSeconds(0.01f);
        }

        AsyncOperation emptyRemoveAsync = SceneManager.UnloadSceneAsync(0);

        while (!emptyRemoveAsync.isDone)
        {
            yield return new WaitForSeconds(0.01f);
        }



        while (UIHolder.instance == null)
        {
            brake ++;   
            if(brake > 1000)
            {
                Debug.Log("brake this");
                yield break;
            }
            yield return new WaitForSeconds(0.01f);
        }



        if(newScene != 0)
        {
            //this means that we are in the in a raid scene.
            //we tell teh raid handler to start doing its thing.            
            RaidLocalHandler raidLocal = RaidLocalHandler.instance;

            //we pass the chestlist around and the new champs.
            GameHandler.instance.raid.CallLocalHandler();

            if(raidLocal != null) 
            {
                handler.playerHandler.gameObject.SetActive(false);
                while (!raidLocal.isReady)
                {
                    yield return new WaitForSeconds(0.01f);
                }
            
            }


        }
        else
        {
            //then wee give everything stored in the raid
            GameHandler.instance.raid.GiveDataFromRaidToPlayer();
            handler.playerHandler.gameObject.SetActive(true);
        }
        
    }

}
