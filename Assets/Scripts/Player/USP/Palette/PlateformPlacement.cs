using System.Collections;
using TMPro;
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
    private bool _canPlace = false; // Peut-on poser la plateforme ?
    public bool CanPlace { get => _canPlace; set { _canPlace = value; } } 


    [HideInInspector] public bool _hasInvoke = false; // A-t-on invoqué la plateforme fantôme ?
    [HideInInspector] public GameObject _currentPlatform = null; // La plateforme actuelle

    private float _maxPoseDistance = 8f; // distance de pose maximal par rapport au joueur
    
    private float _powerDelay = 20f; // Temps de pose de pouvoir
    private bool _placed = false; // Est-ce que la plateforme est posée ?


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
            HUDManager.Instance.PowerTimer.GetComponent<TextMeshProUGUI>().text = ((int)_powerDelay).ToString();
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
            
            _currentPlatform.GetComponentInChildren<TextMeshProUGUI>().text = _currentData.AutoDestroyTimer.ToString();

            if (Vector3.Distance(transform.position, mousePos) <= _maxPoseDistance)
                _currentPlatform.transform.position = mousePos;
            else
            {
                // on calcule la position max du bord : transform.position : on part de la position du joueur / (mousePos - transform.position).normalized : on recupere la direction que le vecteur doit prendre / * max dist = pour qu'il soit a la max dist
                Vector3 limitedPos = transform.position + (mousePos - transform.position).normalized * _maxPoseDistance;
                _currentPlatform.transform.position = limitedPos;

            }


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

        HasBounce = false;

    }


    private void ActivePlateforme()
    {
        HUDManager.Instance.PaletteInfos.SetActive(false);
        _currentData.number -= 1;

        if (!_currentData.Istrigger)
            _currentPlatform.GetComponent<Collider2D>().isTrigger = false;

        _currentPlatform.GetComponent<SpriteRenderer>().color = _currentData.PowerColor;

        _currentPlatform.transform.SetParent(null);
        _placed = true;

        // On récupère le script behavior de la plateforme
        PlateformBehavior plateformBehavior = _currentPlatform.GetComponent<PlateformBehavior>();

        // On active son pouvoir
        plateformBehavior.ActivePower();

        // La plateforme est placé, on appelle son auto destroy
        plateformBehavior.StartDelai = true;
        _currentPlatform = null;

        

    }




}
