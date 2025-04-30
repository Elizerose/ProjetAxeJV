using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEditor.U2D.Aseprite;
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
    private int _rotationStep = 90;

    [Header("Gestion de la couleur choisie")]
    [HideInInspector] public int _currentIndexColor = 0;
    private PlateformesData currentPlateformData;
    private int _oldIndexColor;

    [Header("Gestion de la palette")]

    public bool CanInvokePaletteUnderWater = false;
    


    public enum ColorAbilities
    {
        None = 0,
        Blue,
        Yellow,   
        Red       
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
            // Si on était en choix, c'est qu'on a validé la couleur : on passe en mode placement
            if (_state == STATE_POWER.INCHOICE)
            {
                // si on est dans l'eau on ne peut pas invoquer la palette
                if (GameManager.Instance.Player.GetComponent<Water>().InWater && !CanInvokePaletteUnderWater)
                {
                    HUDManager.Instance.DisplayError("Vous ne pouvez pas invoquer la palette de couleurs dans l'eau ...");
                    return;
                }
                else
                {
                    if (_state == STATE_POWER.INPLACEMENT)
                        Destroy(PlateformPlacement.Instance._currentPlatform);
                    _state = STATE_POWER.INPLACEMENT;
                }
            }
            // Sinon, on passe en mode choix de couleur
            else
                _state = STATE_POWER.INCHOICE;
        }

        switch (_state)
        {
            case STATE_POWER.NONE:
                break;
            case STATE_POWER.INCHOICE:
                InvokeColorPalette();
                break;
            case STATE_POWER.INPLACEMENT:
                // Si on est en mode placement (on a choisi notre couleur) on envoie notre couleur choisi au script qui va gerer son placement
                PlateformPlacement.Instance.SetAbility(DatabaseManager.Instance.GetPlateformesData((ColorAbilities)_currentIndexColor));
                break;


        }
    }


    private void InvokeColorPalette()
    {
        HUDManager.Instance.PalettePanel.SetActive(true);

        // couleur de gauche
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotatePalette(-1);
        }
        // couleur de droite
        else if (Input.GetKeyDown(KeyCode.D))
        {
            RotatePalette(1);
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
        Quaternion TextRotationStart = HUDManager.Instance.Compteurs[0].transform.localRotation;

        Quaternion endRotationPalette = PaletteRotationStart * Quaternion.Euler(0f, 0f, angle);
        Quaternion endRotationText = TextRotationStart * Quaternion.Euler(0f, 0f, -angle);

        Vector3 _startSize = HUDManager.Instance.ColorsList[_currentIndexColor].transform.localScale;
        Vector3 _endSize = _startSize + new Vector3(0.2f, 0.2f, 0.2f);

        // rotation progressive 
        for (float time = 0f; time < duration; time += Time.deltaTime)
        {
            // Rotation de la palette
            HUDManager.Instance.Palette.transform.rotation = Quaternion.Slerp(PaletteRotationStart, endRotationPalette, time / duration);

            
            // Rotation des compteurs pour que le texte reste droit
            foreach (GameObject go in HUDManager.Instance.Compteurs)
            {
                go.transform.localRotation = Quaternion.Slerp(PaletteRotationStart, endRotationText, time / duration);
            }

            // On affiche en plus gros la couleur choisi
            HUDManager.Instance.ColorsList[_currentIndexColor].transform.localScale = Vector3.Lerp(_startSize, _endSize, time / duration);

            // on resize la old color
            HUDManager.Instance.ColorsList[_oldIndexColor].transform.localScale = Vector3.Lerp(_endSize, _startSize, time / duration);

            
            yield return null;
        }





        // On remet bien les valeurs de fin au cas où

        HUDManager.Instance.ColorsList[_currentIndexColor].transform.localScale = _endSize;

        foreach (GameObject go in HUDManager.Instance.Compteurs)
        {
            go.transform.localRotation = endRotationText;
        }

        HUDManager.Instance.Palette.transform.rotation = endRotationPalette;

        
        HUDManager.Instance.ColorsList[_oldIndexColor].transform.localScale = _startSize;

        _isRotating = false;
    }

















    //public bool IsInColorChoice = false;

    //private bool _isRotating = false;

    //private Vector3 _startSize;
    //private Vector3 _endSize;

    //private Quaternion _startRotation;


    // Remettre la current color power a none
    //public void ResetPower()
    //{
    //    _currentIndexColor = 0;
    //    _palette.transform.rotation = Quaternion.Euler(0f, 0f, -45f);

    //    for (int i = 0; i < _colorsList.Count; i++)
    //    {
    //        _colorsList[i].transform.localScale = _startSize;
    //    }

    //    _colorsList[_currentIndexColor].transform.localScale = _endSize;
    //}



    //IEnumerator Rotation(float angle)
    //{
    //    _isRotating = true;
    //    float duration = 0.2f;

    //    _startRotation = _palette.transform.rotation;

    //    Quaternion _startRotationText = HUDManager.Instance.blueCtp.transform.localRotation;
    //    Quaternion endRotationText = _startRotationText * Quaternion.Euler(0f, 0f, -angle);

    //    Quaternion endRotation = _startRotation * Quaternion.Euler(0f, 0f, angle); // * entre deux rotation = +


    //    _startSize = _colorsList[_currentIndexColor].transform.localScale;
    //    _endSize = _startSize + new Vector3(0.2f, 0.2f, 0.2f);





    //    // rotation progressive 
    //    for (float time = 0f; time < duration; time += Time.deltaTime)
    //    {
    //        _palette.transform.rotation = Quaternion.Slerp(_startRotation, endRotation, time / duration);
    //        _colorsList[_currentIndexColor].transform.localScale = Vector3.Lerp(_startSize, _endSize, time / duration);


    //        HUDManager.Instance.blueCtp.transform.localRotation = Quaternion.Slerp(_startRotationText, endRotationText, time / duration); ;
    //        HUDManager.Instance.RedCtp.transform.localRotation = Quaternion.Slerp(_startRotationText, endRotationText, time / duration); ;
    //        HUDManager.Instance.YellowCtp.transform.localRotation = Quaternion.Slerp(_startRotationText, endRotationText, time / duration); ;


    //        // resize la old color
    //        _colorsList[_oldIndexColor].transform.localScale = Vector3.Lerp(_endSize, _startSize, time / duration);

    //        yield return null;
    //    }

    //    HUDManager.Instance.blueCtp.transform.localRotation = endRotationText;
    //    HUDManager.Instance.RedCtp.transform.localRotation = endRotationText;
    //    HUDManager.Instance.YellowCtp.transform.localRotation = endRotationText;


    //    _palette.transform.rotation = endRotation;

    //    _colorsList[_currentIndexColor].transform.localScale = _endSize;
    //    _colorsList[_oldIndexColor].transform.localScale = _startSize;

    //    _isRotating = false;
    //}













    //void Start()
    //{
    //    _palettePanel.SetActive(false);
    //}

    //void Update()
    //{ 

    //    // Si on appuie sur E on active la roue de couleurs
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        // si on est dans l'eau on ne peut pas invoquer la palette
    //        if (GameManager.Instance.Player.GetComponent<Water>().InWater && !CanInvokePaletteUnderWater)
    //        {
    //            HUDManager.Instance.DisplayError("Vous ne pouvez pas invoquer la palette de couleurs dans l'eau ...");
    //            return;
    //        }

    //        // On change : si on etait en choix : on selectionne la couleur et ferme la palette / si on etait pas en choix : on invoque la palette
    //        IsInColorChoice = !IsInColorChoice;
    //        _palettePanel.SetActive(IsInColorChoice);

    //        // Si on a choisit, on envoie la couleur a PlayerAbilities pour qu'il declenche le pouvoir correspondant
    //        if (!IsInColorChoice)
    //            GameManager.Instance.Player.GetComponent<PlayerAbilities>().SetAbility((ColorAbilities)_currentIndexColor);
    //    }

    //    // On vient d'etre en choix, on invoque notre palette
    //    if (IsInColorChoice && !_isRotating)
    //        InvokeColorPalette();
    //}



    // tourner la palette
    //private void RotatePalette(int direction)
    //{
    //    _oldIndexColor = _currentIndexColor;
    //    _currentIndexColor = (_currentIndexColor + direction + _colorsList.Count) % _colorsList.Count;
    //    StartCoroutine(Rotation(_rotationStep *  direction));
    //}

    //IEnumerator Rotation(float angle)
    //{
    //    _isRotating = true;
    //    float duration = 0.2f;

    //    _startRotation = _palette.transform.rotation;

    //    Quaternion _startRotationText = HUDManager.Instance.blueCtp.transform.localRotation;
    //    Quaternion endRotationText = _startRotationText * Quaternion.Euler(0f, 0f, -angle);

    //    Quaternion endRotation = _startRotation * Quaternion.Euler(0f, 0f, angle); // * entre deux rotation = +


    //    _startSize = _colorsList[_currentIndexColor].transform.localScale;
    //    _endSize = _startSize + new Vector3(0.2f, 0.2f, 0.2f);



    //    // rotation progressive 
    //    for (float time = 0f; time < duration; time += Time.deltaTime)
    //    {
    //        _palette.transform.rotation = Quaternion.Slerp(_startRotation, endRotation, time / duration);
    //        _colorsList[_currentIndexColor].transform.localScale = Vector3.Lerp(_startSize, _endSize, time / duration);


    //        HUDManager.Instance.blueCtp.transform.localRotation = Quaternion.Slerp(_startRotationText, endRotationText, time / duration); ;
    //        HUDManager.Instance.RedCtp.transform.localRotation = Quaternion.Slerp(_startRotationText, endRotationText, time / duration); ;
    //        HUDManager.Instance.YellowCtp.transform.localRotation = Quaternion.Slerp(_startRotationText, endRotationText, time / duration); ;


    //        // resize la old color
    //        _colorsList[_oldIndexColor].transform.localScale = Vector3.Lerp(_endSize, _startSize, time / duration);

    //        yield return null;
    //    }

    //    HUDManager.Instance.blueCtp.transform.localRotation = endRotationText;
    //    HUDManager.Instance.RedCtp.transform.localRotation = endRotationText;
    //    HUDManager.Instance.YellowCtp.transform.localRotation = endRotationText;


    //_palette.transform.rotation = endRotation;

    //    _colorsList[_currentIndexColor].transform.localScale = _endSize;
    //    _colorsList[_oldIndexColor].transform.localScale = _startSize;

    //    _isRotating = false;
    //}

    //// Remettre la current color power a none
    //public void ResetPower()
    //{
    //    _currentIndexColor = 0;
    //    _palette.transform.rotation = Quaternion.Euler(0f, 0f, -45f);

    //    for (int i = 0; i < _colorsList.Count; i++)
    //    {
    //        _colorsList[i].transform.localScale = _startSize;
    //    }

    //    _colorsList[_currentIndexColor].transform.localScale = _endSize;
    //}


    //// afficher visuellement la couleur active
    //public void ShowCurrentColor()
    //{



    //    // Si notre liste de couleurs actives est pas vide
    //    if (ColorsListActive != null)
    //    {
    //        HUDManager.Instance._currentColorParent.gameObject.SetActive(true);

    //        // On parcours toutes les couleurs actives pour les afficher 
    //        foreach (PlateformesData data in ColorsListActive)
    //        {
    //            GameObject _currentColor = Instantiate(HUDManager.Instance.currentColorFeedback,HUDManager.Instance._currentColorParent);

    //            _currentColor.GetComponent<Image>().color = data.PowerColor;
    //        }
    //    }
    //    else
    //    {
    //        HUDManager.Instance._currentColorParent.gameObject.SetActive(false);
    //    }


    //}


}
