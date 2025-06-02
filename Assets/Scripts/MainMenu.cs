using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartSceneByIndex(int p_index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(p_index);
    }

    public void StartSceneByName(string p_name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(p_name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
