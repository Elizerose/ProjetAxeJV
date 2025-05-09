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
    [HideInInspector] public bool _hasInvoke = false; // A-t-on invoqu� la plateforme fant�me ?
    [HideInInspector] public GameObject _currentPlatform = null; // La plateforme actuelle

    private float _maxDistance = 5f; // distance de pose maximal par rapport au joueur
    private Vector3 _startingPosition; // position de d�part de la plateforme
    
    private float _powerDelay = 20f; // Temps de pose de pouvoir
    private bool _placed = false; // Est-ce que la plateforme est pos�e ?
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
    /// Appel� quand on s�lectionne un pouvoir (depuis la palette de couleur -> ColorPowerController)
    /// </summary>
    public void SetAbility(PlateformesData ability)
    {
        // On d�sactive le panel de la palette 
        HUDManager.Instance.PalettePanel.SetActive(false);

        // On stocke la data de la plateforme s�lectionn�e
        _currentData = ability;

        // On r�initialise les variables
        _currentPlatform = null;
        _hasInvoke = false;
        _placed = false;
        _powerDelay = 20f;

        lastOrientation = Mathf.Sign(GameManager.Instance.Player.localScale.x);

        // Afficher la couleur de placement 
        // ColorPowerController.Instance.ShowCurrentColor();
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
    /// G�re le timer de pose de bloc
    /// </summary>
    private void UpdatePlacementTimer()
    {
        // Si le d�lai de pose est d�pac�, on eneleve le pouvoir actuel
        if (_powerDelay <= 0 && !_placed)
        {
            ResetPower();
            if (_currentPlatform != null)
                Destroy(_currentPlatform);

            HUDManager.Instance.PowerTimer.SetActive(false);
            return;
        }

        // Si le d�lai est encore en cours et qu'on a plac� la platefome, on arr�te le d�lai
        else if (_powerDelay >= 0 && _placed)
        {
            HUDManager.Instance.PowerTimer.SetActive(false);
            ResetPower();
        }
        // Sinon le d�lai continue
        else
        {
            _powerDelay -= Time.deltaTime;
            HUDManager.Instance.PowerTimer.SetActive(true);
            HUDManager.Instance.PowerTimer.GetComponent<Text>().text = ((int)_powerDelay).ToString();
        }
    }



    /// <summary>
    /// Appel� quand on s�lectionne un pouvoir (depuis la palette de couleur -> ColorPowerController)
    /// </summary>
    public void InvokePlatform()
    {
        if (_currentData == null)
            return;

        // calcul de l'orientation du joueur
        float currentPlayerDirection = Mathf.Sign(GameManager.Instance.Player.localScale.x);

        // Taille du joueur
        float playerHeight = GetComponent<CapsuleCollider2D>().size.y * transform.localScale.y;

        // Taille de la plateforme (attention : la prefab est souvent en �chelle 1)
        float platformHeight;
        float offsetX = _currentData.startingPositionOffsetX;

        // Si la current color est pas �gale � bleu, c'est un box collider 
        if (_currentData.color != ColorAbilities.Blue)
        {
            platformHeight = _currentData.Prefab.GetComponent<BoxCollider2D>().size.y * _currentData.Prefab.transform.localScale.y;
        }
        // Si c'est le pouvoir bleu, c'est un circle collider
        else
        {
            platformHeight = _currentData.Prefab.GetComponent<CircleCollider2D>().radius * _currentData.Prefab.transform.localScale.y;
        }

        // Calcul du offset vertical pour que le bas de la plateforme soit align� au bas du joueur
        float offsetY = -(playerHeight / 2f) + (platformHeight / 2f);

        // Calcul de la starting Position (position du joueur + 2f de decalage)
        _startingPosition = transform.position + new Vector3(offsetX * currentPlayerDirection, offsetY - 0.1f, 0);

        // Si on a pas d�j� invoqu� la plateforme
        if (!_hasInvoke)
        {
            // step 1 : instancier un 'fantome' de la plateforme avec des reperes de placement
            _currentPlatform = Instantiate(_currentData.Prefab, _startingPosition, transform.rotation);

            // Mettre sa couleur en bleu et opacit� 50%
            _currentPlatform.GetComponent<SpriteRenderer>().color = new Color32(173, 216, 230, 128);

            _currentPlatform.GetComponent<PlateformBehavior>().Init(_currentData);
            Text timer = _currentPlatform.GetComponentInChildren<Text>();
            Vector3 newScale = timer.transform.localScale;
            newScale.x = Mathf.Abs(newScale.x);
            timer.transform.localScale = newScale;



            // Le mettre en enfant du joueur pou qu'elle le suive avec lui
            _currentPlatform.transform.SetParent(transform);

            // La plateforme est invoqu�
            _hasInvoke = true;
        }

        // Su notre plateforme est invoqu� et bien en m�moire
        if (_currentPlatform != null && !_placed)
        {
            if (currentPlayerDirection != lastOrientation)
            {
                Debug.Log("-----");
                Debug.Log("la joueur est tourn� � " + currentPlayerDirection);
                Debug.Log("Avant il �tait � " + lastOrientation);


                Text timer = _currentPlatform.GetComponentInChildren<Text>();
                Vector3 newScale = timer.transform.localScale;

                if (currentPlayerDirection == 1) 
                    newScale.x *=   -currentPlayerDirection ;
                else
                    newScale.x *=  currentPlayerDirection;


                Debug.Log("On met notre newScale � " + newScale.x);

                timer.transform.localScale = newScale;

                lastOrientation = currentPlayerDirection;
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



            // D�placer vers la gauche
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 position = _currentPlatform.transform.position;
                position.x -= 0.1f;

                float distance = 0f;

                if (currentPlayerDirection > 0)
                {
                    distance = position.x - _startingPosition.x;

                }
                else
                {
                    distance = _startingPosition.x - position.x;
                }

                if (distance >= 0 && distance <= _maxDistance)
                {
                    _currentPlatform.transform.position = position;
                }


            }
            // D�placer vers la droite
            if (Input.GetKey(KeyCode.X))
            {
                Vector3 position = _currentPlatform.transform.position;
                position.x += 0.1f;

                float distance = 0f;

                if (currentPlayerDirection > 0)
                {
                    distance = position.x - _startingPosition.x;

                }
                else
                {
                    distance = _startingPosition.x - position.x;
                }

                if (distance >= 0 && distance <= _maxDistance)
                {
                    _currentPlatform.transform.position = position;
                }

            }

            // Poser
            if (Input.GetKeyDown(KeyCode.Return) && _canPlace)
            {
                ActivePlateforme();

                //_currentCoroutine = StartCoroutine(TimeToDestroy(_data.AutoDestroyTimer));   
            }
            else if (Input.GetKeyDown(KeyCode.Return) && !_canPlace)
            {
                HUDManager.Instance.DisplayError("Pas assez d'espace pour poser le bloc ...");
            }
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

        // On r�cup�re le script behavior de la plateforme
        PlateformBehavior plateformBehavior = _currentPlatform.GetComponent<PlateformBehavior>();

        // La plateforme est plac�, on appelle son auto destroy
        plateformBehavior.StartDelai = true;
        _currentPlatform = null;

        

    }




}
