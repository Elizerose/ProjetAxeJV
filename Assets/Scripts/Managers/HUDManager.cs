using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    // Singleton
    private static HUDManager _instance;
    public static HUDManager Instance => _instance;

    [Header("ENNEMIES HUD")]

    public Sprite Exclamation;
    public Sprite Interrogation;

    [Header("USP COLOR PALETTE")]
    public GameObject PowerTimer;

    [Header("OXYGENE")]

    public GameObject OxygeneTimerGO;


    [Header("DEATH")]
    public GameObject DeathPanel;

    [Header("INFOS")]
    public GameObject TextInfos;
    private bool _isDisplaying;


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        OxygeneTimerGO.SetActive(false);
    }

    // Fonction qui affiche des petits messages d'erreur (ex : vous ne pouvez pas placer le bloc ici ... )
    public void DisplayError(string error)
    {
        TextInfos.GetComponent<Text>().text = error;
        if (!_isDisplaying)
            StartCoroutine(DelayToDisplay());
    }

    // Delai d'affichage
    IEnumerator DelayToDisplay()
    {
        _isDisplaying = true;
        TextInfos.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(2.5f);
        TextInfos.GetComponent<Animator>().SetTrigger("FadeOut");
        _isDisplaying = false;
    }
}
