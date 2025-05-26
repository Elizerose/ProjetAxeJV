using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
///  gère la palette de couleur et la selection de la couleur
/// 
/// </summary>



public class ColorPowerController : MonoBehaviour
{
    // Singleton
    private static ColorPowerController _instance;
    public static ColorPowerController Instance => _instance;


    public enum STATE_POWER
    {
        NONE = 0,
        INCHOICE,
        INPLACEMENT
    }

    [SerializeField] public STATE_POWER _state = STATE_POWER.NONE;

    [Header("Gestion de la rotation")]
    [SerializeField] private float duration = 0.2f;
    private bool _isRotating = false;

    [Header("Gestion de la rotation")]
    private int _rotationStep = 72;

    [Header("Gestion de la couleur choisie")]
    [HideInInspector] public int _currentIndexColor = 0;
    private int _oldIndexColor;

    [Header("Gestion de la palette")]
    public bool CanInvokePaletteUnderWater = false;
    private List<Text> Compteurs = new List<Text>();


    public enum ColorAbilities
    {
        None = 0,
        Blue,
        Red,
        Green,
        Yellow
    }



    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }



    void Update()
    {
        // Si on appuie sur E
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateDescription();

            // Si on était en choix, c'est qu'on a validé la couleur : on passe en mode placement
            if (_state == STATE_POWER.INCHOICE)
            {
                if ((ColorAbilities)_currentIndexColor != ColorAbilities.None)
                {
                    // on vérifie sil a assez de pot de peinture
                    if (DatabaseManager.Instance.GetPlateformesData((ColorAbilities)_currentIndexColor).number > 0)
                    {
                        if (PlateformPlacement.Instance._currentPlatform != null)
                        {
                            Destroy(PlateformPlacement.Instance._currentPlatform);
                        }
                        // Si on est en mode placement (on a choisi notre couleur) on envoie notre couleur choisi au script qui va gerer son placement
                        PlateformPlacement.Instance.SetAbility(DatabaseManager.Instance.GetPlateformesData((ColorAbilities)_currentIndexColor));
                        Compteurs[_currentIndexColor - 1].transform.parent.GetComponentInChildren<ParticleSystem>().Play();
                        _state = STATE_POWER.INPLACEMENT;
                        Time.timeScale = 1f;
                    }
                    // Sinon on ne peux pas invoquer le pouvoir
                    else
                    {
                        StartCoroutine(ShakePalette());
                    }
                }
                else
                {
                    if (PlateformPlacement.Instance._currentPlatform != null)
                    {
                        Destroy(PlateformPlacement.Instance._currentPlatform);
                    }
                    Time.timeScale = 1f;
                    HUDManager.Instance.PaletteInfos.SetActive(false);
                    HUDManager.Instance.PalettePanel.SetActive(false);
                    _state = STATE_POWER.NONE;
                }
            }
            // Sinon, on passe en mode choix de couleur
            else
            {
                // si on est dans l'eau on ne peut pas invoquer la palette
                if (GameManager.Instance.Player.GetComponent<Water>().InWater && !CanInvokePaletteUnderWater)
                {
                    HUDManager.Instance.DisplayError("Vous ne pouvez pas invoquer la palette de couleurs dans l'eau ...");
                    return;
                }
                else
                    _state = STATE_POWER.INCHOICE;
            }   

        }

        switch (_state)
        {
            case STATE_POWER.NONE:
                break;
            case STATE_POWER.INCHOICE:
                InvokeColorPalette();
                break;
            case STATE_POWER.INPLACEMENT:
                break;


        }
    }


    private void InvokeColorPalette()
    {
        // On ralenti le temps car sinon les ennemis peuvent nous attaquer trop facilement.
        Time.timeScale = 0.3f;

        HUDManager.Instance.PalettePanel.SetActive(true);
        HUDManager.Instance.PaletteInfos.SetActive(true);   

        foreach (ColorAbilities ability in HUDManager.Instance.ColorAbilitiesPalette.Keys)
        {
            if (ability != ColorAbilities.None)
            {
                Compteurs.Add(HUDManager.Instance.ColorAbilitiesPalette[ability].GetComponentInChildren<Text>());
                HUDManager.Instance.ColorAbilitiesPalette[ability].GetComponentInChildren<Text>().text = DatabaseManager.Instance.GetPlateformesData(ability).number.ToString();
            }
        }

        // couleur de gauche
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotatePalette(-1);
            UpdateDescription();
        }
        // couleur de droite
        else if (Input.GetKeyDown(KeyCode.D))
        {
            RotatePalette(1);
            UpdateDescription();
        }
    }

    //tourner la palette
    private void RotatePalette(int direction)
    {
        if (_isRotating)
            return;

        // on recupere l'ancien index pour le remettre a sa taille petite
        _oldIndexColor = _currentIndexColor;
        // on update le current index color
        _currentIndexColor = (_currentIndexColor + direction + HUDManager.Instance.ColorsList.Count) % HUDManager.Instance.ColorsList.Count; ;

        StartCoroutine(Rotation(_rotationStep * direction));
    }

    IEnumerator Rotation(float angle)
    {
        _isRotating = true;

        // Initialisation des Rotation et Size
        Quaternion PaletteRotationStart = HUDManager.Instance.Palette.transform.rotation;
        Quaternion TextRotationStart = Compteurs[0].transform.localRotation;

        Quaternion endRotationPalette = PaletteRotationStart * Quaternion.Euler(0f, 0f, angle);
        Quaternion endRotationText = TextRotationStart * Quaternion.Euler(0f, 0f, -angle);

        Vector3 _startSize = HUDManager.Instance.ColorsList[_currentIndexColor].transform.localScale;
        Vector3 _endSize = _startSize + new Vector3(0.2f, 0.2f, 0.2f);

        // rotation progressive 
        for (float time = 0f; time < duration; time += Time.unscaledDeltaTime)
        {
            // Rotation de la palette
            HUDManager.Instance.Palette.transform.rotation = Quaternion.Slerp(PaletteRotationStart, endRotationPalette, time / duration);


            // Rotation des compteurs pour que le texte reste droit
            foreach (Text go in Compteurs)
            {
                go.gameObject.transform.localRotation = Quaternion.Slerp(TextRotationStart, endRotationText, time / duration);
            }

            // On affiche en plus gros la couleur choisi
            HUDManager.Instance.ColorsList[_currentIndexColor].transform.localScale = Vector3.Lerp(_startSize, _endSize, time / duration);

            // on resize la old color
            HUDManager.Instance.ColorsList[_oldIndexColor].transform.localScale = Vector3.Lerp(_endSize, _startSize, time / duration);


            yield return null;
        }





        // On remet bien les valeurs de fin au cas où

        HUDManager.Instance.ColorsList[_currentIndexColor].transform.localScale = _endSize;

        foreach (Text go in Compteurs)
        {
            go.gameObject.transform.localRotation = endRotationText;
        }

        HUDManager.Instance.Palette.transform.rotation = endRotationPalette;


        HUDManager.Instance.ColorsList[_oldIndexColor].transform.localScale = _startSize;

        _isRotating = false;
    }


    IEnumerator ShakePalette()
    {
        GameObject palette = HUDManager.Instance.Palette;
        Vector3 originalPosition = palette.transform.localPosition;

        float duration = 0.2f;
        float magnitude = 20f; // distance max du shake en x
        float speed = 30f;

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            // Mathf.Sin donne une valeur entre -1 et +1 : parfait pour un tremblement => t * speed  
            float ShakeX = Mathf.Sin(t * speed) * magnitude * (1f - t / duration); // (1f - t / duration) pour diminuer le tremblement au fur et a mesure
            palette.transform.localPosition = originalPosition + new Vector3(ShakeX, 0f, 0f);
            yield return null;
        }

        palette.transform.localPosition = originalPosition;
    }


    private void UpdateDescription()
    {
        TextMeshProUGUI title = HUDManager.Instance.Title.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI desc = HUDManager.Instance.Description.GetComponent<TextMeshProUGUI>();

        PlateformesData data = DatabaseManager.Instance.GetPlateformesData((ColorAbilities)_currentIndexColor);


        if ((ColorAbilities)_currentIndexColor == ColorAbilities.None)
        {
            title.color = Color.white;
            desc.color = Color.white;

            title.text = "Désactiver la palette";
            desc.text = "Après avoir selectionné la couleur, vous avez 20s pour poser un bloc.";
        } else
        {
            title.text = data.Titre;
            desc.color = data.PowerColor;

            desc.text = data.description;
            title.color = data.PowerColor;
        }

    }

}


