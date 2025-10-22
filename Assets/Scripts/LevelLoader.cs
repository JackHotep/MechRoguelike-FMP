using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;
    public float transitionDuration = 1f;

    public void LoadMainMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void LoadTutorial()
    {
        StartCoroutine(LoadLevel(1));
    }

    public void LoadEndless()
    {
        StartCoroutine(LoadLevel(2));
    }
    
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionDuration);
        
        SceneManager.LoadScene(levelIndex);
    }

}
