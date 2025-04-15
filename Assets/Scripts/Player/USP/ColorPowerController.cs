using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;


/// <summary>
/// 
/// Class ColorPowerController :  gère l'USP 
/// 
/// Gestion de l'UI : 
///     
///     - InvokeColorPalette() -> Gestion de l'affichage des couleurs
///     - RotatePalette(int direction) -> Gestion de la rotation et des nouvelles variables
///     - IEnumerator Rotation(float angle) -> Gestion de l'animation de rotation
///     
/// Gestion des pouvoirs : 
/// 
///     
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

    private int _rotationStep = 90; // dans le cas d'un triangle
    private int _currentIndexColor = 0;
    private int _oldIndexColor;
    private bool _isRotating = false;

    public bool isInColorChoice = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        // Si on appuie sur E on active la roue de couleurs
        if (Input.GetKeyDown(KeyCode.E))
        {
            isInColorChoice = !isInColorChoice;
            _palettePanel.SetActive(isInColorChoice);

            if (!isInColorChoice)
                GameManager.Instance.Player.GetComponent<PlayerAbilities>().SetAbility((ColorAbilities)_currentIndexColor);
        }

        if (isInColorChoice &&!_isRotating)
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

        Quaternion startRotation = _palette.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, 0f, angle); // * entre deux rotation = +

        Vector3 startSize = _colorsList[_currentIndexColor].transform.localScale;
        Vector3 EndSize = startSize + new Vector3(0.2f, 0.2f, 0.2f);

        for (float time = 0f; time < duration; time += Time.deltaTime)
        {
            _palette.transform.rotation = Quaternion.Slerp(startRotation, endRotation, time / duration);
            _colorsList[_currentIndexColor].transform.localScale = Vector3.Lerp(startSize, EndSize, time / duration);


            // resize the old color
            _colorsList[_oldIndexColor].transform.localScale = Vector3.Lerp(EndSize, startSize, time / duration);

            yield return null;
        }

        _palette.transform.rotation = endRotation;
        _colorsList[_currentIndexColor].transform.localScale = EndSize;
        _colorsList[_oldIndexColor].transform.localScale = startSize;

        _isRotating = false;
    }

}
