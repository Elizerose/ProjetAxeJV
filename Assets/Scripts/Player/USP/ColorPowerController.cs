using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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


    [SerializeField] private List<GameObject> _colorsList;
    [SerializeField] private GameObject _palette;
    [SerializeField] private GameObject _palettePanel;

    [SerializeField] private GameObject _currentColorFeedback;

    private int _rotationStep = 90; // dans le cas d'un triangle
    [HideInInspector] public int _currentIndexColor = 0;
    private int _oldIndexColor;
    private bool _isRotating = false;

    private Vector3 _startSize;
    private Vector3 _endSize;

    private Quaternion _startRotation;

    public bool IsInColorChoice = false;

    public bool CanInvokePaletteUnderWater = false;

    public enum ColorAbilities
    {
        None = 0,
        Blue,   // ex: Respirer sous l'eau
        Yellow,   // ex: plateformes lumineuses
        Red    // ex: Feu        
    }



    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _palettePanel.SetActive(false);
        _currentColorFeedback.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Si on appuie sur E on active la roue de couleurs
        if (Input.GetKeyDown(KeyCode.E))
        {
            // si on est dans l'eau on ne peut pas invoquer la palette
            if (GameManager.Instance.Player.GetComponent<Water>().InWater && !CanInvokePaletteUnderWater)
            {
                HUDManager.Instance.DisplayError("Vous ne pouvez pas invoquer la palette de couleurs dans l'eau ...");
                return;
            }


            IsInColorChoice = !IsInColorChoice;
            _palettePanel.SetActive(IsInColorChoice);

            // Si on a choisit, on envoie la couleur a PlayerAbilities pour qu'il declenche le pouvoir correspondant
            if (!IsInColorChoice)
                GameManager.Instance.Player.GetComponent<PlayerAbilities>().SetAbility((ColorAbilities)_currentIndexColor);
        }

        if (IsInColorChoice && !_isRotating)
            InvokeColorPalette();
    }

    private void InvokeColorPalette()
    {

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

    // tourner la palette
    private void RotatePalette(int direction)
    {
        _oldIndexColor = _currentIndexColor;
        _currentIndexColor = (_currentIndexColor + direction + _colorsList.Count) % _colorsList.Count;
        StartCoroutine(Rotation(_rotationStep *  direction));
    }

    IEnumerator Rotation(float angle)
    {
        _isRotating = true;
        float duration = 0.2f;

        _startRotation = _palette.transform.rotation;
        Quaternion endRotation = _startRotation * Quaternion.Euler(0f, 0f, angle); // * entre deux rotation = +

        _startSize = _colorsList[_currentIndexColor].transform.localScale;
        _endSize = _startSize + new Vector3(0.2f, 0.2f, 0.2f);

        // rotation progressive 
        for (float time = 0f; time < duration; time += Time.deltaTime)
        {
            _palette.transform.rotation = Quaternion.Slerp(_startRotation, endRotation, time / duration);
            _colorsList[_currentIndexColor].transform.localScale = Vector3.Lerp(_startSize, _endSize, time / duration);


            // resize la old color
            _colorsList[_oldIndexColor].transform.localScale = Vector3.Lerp(_endSize, _startSize, time / duration);

            yield return null;
        }

        _palette.transform.rotation = endRotation;
        _colorsList[_currentIndexColor].transform.localScale = _endSize;
        _colorsList[_oldIndexColor].transform.localScale = _startSize;

        _isRotating = false;
    }

    // Remettre la current color power a none
    public void ResetPower()
    {
        _currentIndexColor = 0;
        _palette.transform.rotation = Quaternion.Euler(0f, 0f, -45f);

        for (int i = 0; i < _colorsList.Count; i++)
        {
            _colorsList[i].transform.localScale = _startSize;
        }

        _colorsList[_currentIndexColor].transform.localScale = _endSize;
    }


    // achier visuellement la couleur active
    public void ShowCurrentColor()
    {
        _currentColorFeedback.GetComponent<Image>().color = _colorsList[_currentIndexColor].GetComponent<Image>().color;

        if (_currentIndexColor == 0)
        {
            _currentColorFeedback.SetActive(false);
        } else
        {
            _currentColorFeedback.SetActive(true);
        }
        
        
    }


}
