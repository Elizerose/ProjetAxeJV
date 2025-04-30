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



    [HideInInspector] public bool _canPlace = true;

    private bool _hasInvoke = false;

    [HideInInspector] public GameObject _currentPlatform = null;

    private float _maxDistance = 5f;
    private Vector3 _startingPosition;

    //[Header("Red power")]
    //private float FireCoolDownTimer = 1f;
    //private bool _canAttack = true;
    //[SerializeField] GameObject _projectilePrefab;


    // Temps de pose de pouvoir
    private float _powerDelay = 20f;
    private bool _placed = false;

    [Header("Pouvoir Rouge")]
    [HideInInspector] public bool HasBounce = false;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }







    public void SetAbility(PlateformesData ability)
    {
        ResetPower();
        _currentData = ability;
        //ColorPowerController.Instance.ShowCurrentColor();
        
    }

    public void Update()
    {
        if (_currentData != null)
        {
            InvokePlatform(); 
            //_data = DatabaseManager.Instance.GetPlateformesData(CurrentAbility);
        }            

    }



    // invoquer la plateforme correspondante
    public void InvokePlatform()
    {
        // Si le d�lai de pose est d�pac�, on eneleve le pouvoir actuel
        if (_powerDelay <= 0 && !_placed)
        {
            ResetPower();
            return;
        }

        // Si le d�lai est encore en cours et qu'on a plac� la platefome, on arr�te le d�lai
        else if (_powerDelay <= 0 || _placed)
        {
            HUDManager.Instance.PowerTimer.SetActive(false);
        }
        // Sinon le d�lai continue
        else
        {
            _powerDelay -= Time.deltaTime;
            HUDManager.Instance.PowerTimer.SetActive(true);
            HUDManager.Instance.PowerTimer.GetComponent<Text>().text = ((int)_powerDelay).ToString();
        }


        // calcul de l'orientation du joueur
        float currentPlayerDirection = Mathf.Sign(GameManager.Instance.Player.localScale.x);

        // Taille du joueur
        float playerHeight = GetComponent<CapsuleCollider2D>().size.y * transform.localScale.y;

        // Taille de la plateforme (attention : la prefab est souvent en �chelle 1)
        float platformHeight;
        float offsetX = 2f;

        // Si la current color est pas �gale � bleu, c'est un box collider 
        if (_currentData.color != ColorAbilities.Blue)
        {
            platformHeight = _currentData.Prefab.GetComponent<BoxCollider2D>().size.y * _currentData.Prefab.transform.localScale.y;
        }
        // Si c'est le pouvoir bleu, c'est un circle collider
        else
        {
            offsetX = 4;
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


            


            // Le mettre en enfant du joueur pou qu'elle le suive avec lui
            _currentPlatform.transform.SetParent(transform);

            // La plateforme est invoqu�
            _hasInvoke = true;
        }

        // Su notre plateforme est invoqu� et bien en m�moire
        if (_currentPlatform != null && !_placed)
        {
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
        HUDManager.Instance.PalettePanel.SetActive(false);

        //HUDManager.Instance.PowerTimer.SetActive(false);
        _currentPlatform = null;
        HasBounce = false;
        _powerDelay = 0f;
        // reset the color power
        //ColorPowerController.Instance.ResetPower();
        //SetAbility(ColorAbilities.None);
        _hasInvoke = false;
        _placed = false;
        _powerDelay = 20f;
        _currentData = null;
        //_currentCoroutine = null;
    }


    private void ActivePlateforme()
    {

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
        HUDManager.Instance.ColorsListActive.Add(_currentData);
        HUDManager.Instance.ShowCurrentPower(_currentPlatform);

        // On r�cup�re le script behavior de la plateforme
        PlateformBehavior plateformBehavior = _currentPlatform.GetComponent<PlateformBehavior>();

        // La plateforme est plac�, on appelle son auto destroy
        plateformBehavior.TimeToDestroy();

        
        

    }




}
