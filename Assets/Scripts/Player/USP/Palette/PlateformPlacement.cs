using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static ColorPowerController;

public class PlateformPlacement : MonoBehaviour
{
    // Singleton
    private static PlateformPlacement _instance;
    public static PlateformPlacement Instance => _instance;

    [Header("Pouvoir")]
    private PlateformesData _currentData;

    [Header("PLacement")]
    [HideInInspector] public bool _canPlace = false; // Peut-on poser la plateforme ?
    [HideInInspector] public bool _hasInvoke = false; // A-t-on invoqué la plateforme fantôme ?
    [HideInInspector] public GameObject _currentPlatform = null; // La plateforme actuelle

    private float _maxDistance = 5f; // distance de pose maximal par rapport au joueur
    private Vector3 _startingPosition; // position de départ de la plateforme
    
    private float _powerDelay = 20f; // Temps de pose de pouvoir
    private bool _placed = false; // Est-ce que la plateforme est posée ?
    private float lastOrientation; // derniere orientation du joueur


    //[Header("Red power")]
    //private float FireCoolDownTimer = 1f;
    //private bool _canAttack = true;
    //[SerializeField] GameObject _projectilePrefab;


    [Header("Pouvoir Rouge")]
    [HideInInspector] public bool HasBounce = false;


    private void Awake()
    {
        // Initialisation du Singleton
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Appelé quand on sélectionne un pouvoir (depuis la palette de couleur -> ColorPowerController)
    /// </summary>
    public void SetAbility(PlateformesData ability)
    {
        StartCoroutine(WaitForParticule());
        
        // On stocke la data de la plateforme sélectionnée
        _currentData = ability;

        // On réinitialise les variables
        _currentPlatform = null;
        _hasInvoke = false;
        _placed = false;
        _powerDelay = 20f;

        lastOrientation = Mathf.Sign(GameManager.Instance.Player.localScale.x);

        // Afficher la couleur de placement 
        // ColorPowerController.Instance.ShowCurrentColor();
    }

    private IEnumerator WaitForParticule()
    {
        yield return new WaitForSeconds(0.2f);
        // On désactive le panel de la palette 
        HUDManager.Instance.PalettePanel.SetActive(false);
    }



    public void Update()
    {
        
        if (_currentData != null)
        {
            UpdatePlacementTimer();
            InvokePlatform(); 
        }            

    }

    /// <summary>
    /// Gère le timer de pose de bloc
    /// </summary>
    private void UpdatePlacementTimer()
    {
        // Si le délai de pose est dépacé, on eneleve le pouvoir actuel
        if (_powerDelay <= 0 && !_placed)
        {
            ResetPower();
            if (_currentPlatform != null)
                Destroy(_currentPlatform);

            HUDManager.Instance.PowerTimer.SetActive(false);
            return;
        }

        // Si le délai est encore en cours et qu'on a placé la platefome, on arrête le délai
        else if (_powerDelay >= 0 && _placed)
        {
            HUDManager.Instance.PowerTimer.SetActive(false);
            ResetPower();
        }
        // Sinon le délai continue
        else
        {
            _powerDelay -= Time.deltaTime;
            HUDManager.Instance.PowerTimer.SetActive(true);
            HUDManager.Instance.PowerTimer.GetComponent<Text>().text = ((int)_powerDelay).ToString();
        }
    }



    /// <summary>
    /// Appelé quand on sélectionne un pouvoir (depuis la palette de couleur -> ColorPowerController)
    /// </summary>
    public void InvokePlatform()
    {
        if (_currentData == null)
            return;


        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Si on a pas déjà invoqué la plateforme
        if (!_hasInvoke)
        {

            _currentPlatform = Instantiate(_currentData.Prefab, mousePos, transform.rotation);

            // Mettre sa couleur en bleu et opacité 50%
            _currentPlatform.GetComponent<SpriteRenderer>().color = new Color32(173, 216, 230, 128);

            _currentPlatform.GetComponent<PlateformBehavior>().Init(_currentData);

            // La plateforme est invoqué
            _hasInvoke = true;
        }

        // Si notre plateforme est invoqué et bien en mémoire
        if (_currentPlatform != null && !_placed)
        {

            _currentPlatform.transform.position = mousePos;

            // Si je peux la placer, elle est en bleue
            if (_canPlace)
            {
                _currentPlatform.GetComponent<SpriteRenderer>().color = new Color32(173, 216, 230, 128);
            }
            // Sinon elle est en rouge
            else
            {
                _currentPlatform.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 128);
            }

            if (Input.GetMouseButtonDown(0) && _canPlace)
                ActivePlateforme();
            else if (Input.GetMouseButtonDown(0) && !_canPlace)
                HUDManager.Instance.DisplayError("Pas assez d'espace pour poser le bloc ...");

        }

        if (HasBounce)
        {
            ResetPower();
        }
    }



    private void ResetPower()
    {
        ColorPowerController.Instance._state = STATE_POWER.NONE;

        _currentData = null;
        //HUDManager.Instance.PowerTimer.SetActive(false);

        HasBounce = false;

    }


    private void ActivePlateforme()
    {
        _currentData.number -= 1;

        if (!_currentData.Istrigger)
            _currentPlatform.GetComponent<Collider2D>().isTrigger = false;

        _currentPlatform.GetComponent<SpriteRenderer>().color = Color.white;

        _currentPlatform.transform.SetParent(null);
        _placed = true;

        switch (_currentData.color)
        {
            case ColorAbilities.Red:
                break;

            case ColorAbilities.Blue:
                break;

            case ColorAbilities.Yellow:
                _currentPlatform.transform.GetChild(0).gameObject.SetActive(true);
                break;

            default:
                break;
        }

        // On display la current color
        //HUDManager.Instance.ColorsListActive.Add(_currentData);
        //HUDManager.Instance.ShowCurrentPower(_currentPlatform);

        // On récupère le script behavior de la plateforme
        PlateformBehavior plateformBehavior = _currentPlatform.GetComponent<PlateformBehavior>();

        // La plateforme est placé, on appelle son auto destroy
        plateformBehavior.StartDelai = true;
        _currentPlatform = null;

        

    }




}
