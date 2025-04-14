using Unity.VisualScripting;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    // Singleton
    private static HUDManager _instance;
    public static HUDManager Instance => _instance;

    [Header("ENNEMIES HUD")]

    public Sprite exclamation;
    public Sprite interrogation;

    [Header("USP COLOR PALETTE")]

    public GameObject ColorsContainer;


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // e activer la roue : pas de deplacement pendant la roue activée



}
