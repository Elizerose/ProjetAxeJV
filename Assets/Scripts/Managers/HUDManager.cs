using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ColorPowerController;

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

    public GameObject currentColorFeedbackPrefab;
    public Transform _currentColorParent;
    public GameObject PalettePanel;

    [Header("ORDRE IMPORTANT : Nocolor / blue / yellow / red")]
    public List<GameObject> ColorsList;

    public GameObject Palette;
    public List<PlateformesData> ColorsListActive;


    [Header("OXYGENE")]
    public GameObject OxygeneTimerGO;


    [Header("DEATH")]
    public GameObject DeathPanel;

    [Header("INFOS")]
    public GameObject TextInfos;
    private bool _isDisplaying;


    // Dictionnaire pour associer chaque ColorAbility à un GameObject color
    public Dictionary<ColorAbilities, GameObject> ColorAbilitiesPalette = new Dictionary<ColorAbilities, GameObject>();


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
        PalettePanel.SetActive(false);
        OxygeneTimerGO.SetActive(false);

        // Associer les couleurs à leurs GameObjects respectifs
        ColorAbilitiesPalette.Add(ColorAbilities.None, ColorsList[0]);
        ColorAbilitiesPalette.Add(ColorAbilities.Blue, ColorsList[1]);
        ColorAbilitiesPalette.Add(ColorAbilities.Yellow, ColorsList[2]);
        ColorAbilitiesPalette.Add(ColorAbilities.Red, ColorsList[3]);
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


    // afficher visuellement la couleur active
    public void ShowCurrentPower(GameObject currentPlatform = null)
    {

        // Si notre liste de couleurs actives est pas vide
        if (ColorsListActive.Count > 0)
        {
            _currentColorParent.gameObject.SetActive(true);
            // On parcours toutes les couleurs actives pour les afficher 

            foreach (Transform go in _currentColorParent)
            {
                Destroy(go.gameObject);
            }

            //foreach (PlateformesData data in ColorsListActive)
            //{
            //    GameObject _currentColor = Instantiate(currentColorFeedbackPrefab, _currentColorParent);
            //    _currentColor.GetComponent<Image>().color = data.PowerColor;

            //    if (currentPlatform != null)
            //    {
            //        Text timer = _currentColor.GetComponentInChildren<Text>();
            //        currentPlatform.GetComponent<PlateformBehavior>().timer = timer;
            //    }
                
            //}
            


            
        }
        else
        {
            _currentColorParent.gameObject.SetActive(false);
        }
    }

}
