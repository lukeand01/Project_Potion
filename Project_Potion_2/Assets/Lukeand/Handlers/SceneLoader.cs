using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    //we unload the scene.
    [SerializeField] int currentScene;

    public void LoadScene(int newScene)
    {
        StartCoroutine(LoadSceneProcess(newScene));
    }

    IEnumerator LoadSceneProcess(int newScene)
    {

        //bring the black screen.


        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

        while (!loadAsync.isDone)
        {
            yield return new WaitForSeconds(0.01f);
        }

        AsyncOperation unloadAsync = SceneManager.UnloadSceneAsync(currentScene, UnloadSceneOptions.None);

        while (!unloadAsync.isDone)
        {
            yield return new WaitForSeconds(0.01f);
        }


        if(RaidHandler.instance != null)
        {
            //this means that we are in th
        }
    }

}
