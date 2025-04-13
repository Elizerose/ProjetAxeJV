using UnityEngine;

public class HUDManager : MonoBehaviour
{
    // Singleton
    private static HUDManager _instance;
    public static HUDManager Instance => _instance;

    [Header("ENNEMIES HUD")]

    public Sprite exclamation;
    public Sprite interrogation;


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


}
