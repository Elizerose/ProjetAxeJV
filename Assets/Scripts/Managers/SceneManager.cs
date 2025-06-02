using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    // Singleton
    private static SceneManager _instance;
    public static SceneManager Instance => _instance;


    private void Awake()
    {
            _instance = this;
    }


    public void ChangeScene(int SceneIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneIndex);
    }
}
