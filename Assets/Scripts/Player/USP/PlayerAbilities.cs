using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;
using static ColorPowerController;

/// <summary>
///  Gere les differents pouvoirs d'invocation de plateformes selon les couleurs
/// </summary>
/// 
public class PlayerAbilities : MonoBehaviour
{
    public ColorAbilities CurrentAbility = ColorAbilities.None;

    private PlateformesData _data;


    [Header("Yellow power")]

    [HideInInspector] public bool _canPlace = true;

    private bool _hasInvoke = false;
    private GameObject _currentPlatform = null;
    private float _maxDistance = 5f;
    private Vector3 _startingPosition;

    //[Header("Red power")]
    //private float FireCoolDownTimer = 1f;
    //private bool _canAttack = true;
    //[SerializeField] GameObject _projectilePrefab;

    [Header("PREFABS")]
    [SerializeField] private GameObject _platformYellowPrefab;
    [SerializeField] private GameObject _platformBluePrefab;
    [SerializeField] private GameObject _platformRedPrefab;


    // Temps de pose de pouvoir
    private float _powerDelay = 20f;
    private bool _placed = false;

    [Header("Pouvoir Rouge")]
    [HideInInspector] public bool HasBounce = false;


    private Coroutine _currentCoroutine;


    public void SetAbility(ColorAbilities ability)
    {

        CurrentAbility = ability;
        ColorPowerController.Instance.ShowCurrentColor();
    }

    public void Update()
    {
        if (CurrentAbility != ColorAbilities.None)
        {
            _data = DatabaseManager.Instance.GetPlateformesData(CurrentAbility);
        }

        if (_data != null) 
            InvokePlatform();
        
    }



    // invoquer la plateforme correspondante
    public void InvokePlatform()
    {

        if (_powerDelay <= 0 && !_placed)
        {
            
            ResetPower();
            
            
            return;
        }

        else if (_powerDelay <= 0 || _placed) 
        {
            HUDManager.Instance.PowerTimer.SetActive(false);
        }

        else
        {
            _powerDelay -= Time.deltaTime;
            HUDManager.Instance.PowerTimer.SetActive(true);
            HUDManager.Instance.PowerTimer.GetComponent<Text>().text = ((int)_powerDelay).ToString();
        }


        

        // calcul de l'oriantation du joueure
        float currentPlayerDirection = Mathf.Sign(GameManager.Instance.Player.localScale.x);

        // Taille du joueur
        float playerHeight = GetComponent<CapsuleCollider2D>().size.y * transform.localScale.y;

        // Taille de la plateforme (attention : la prefab est souvent en échelle 1)
        float platformHeight;
        float offsetX = 2f;

        if (_data.color != ColorAbilities.Blue)
        {
            platformHeight = _data.Prefab.GetComponent<BoxCollider2D>().size.y * _data.Prefab.transform.localScale.y ;
        } else
        {
            offsetX = 4;
            platformHeight = _data.Prefab.GetComponent<CircleCollider2D>().radius * _data.Prefab.transform.localScale.y;
        }




        



        // Calcul du offset vertical pour que le bas de la plateforme soit aligné au bas du joueur
        float offsetY = -(playerHeight / 2f) + (platformHeight / 2f);

        // Calcul de la starting Position (position du joueur + 2f de decalage)
        _startingPosition = transform.position + new Vector3(offsetX * currentPlayerDirection, offsetY - 0.1f, 0);

        // Si on a pas déjà invoqué la plateforme
        if (!_hasInvoke)
        {
            // step 1 : instancier un 'fantome' de la plateforme avec des reperes de placement
            _currentPlatform = Instantiate(_data.Prefab, _startingPosition, transform.rotation); 

            // Mettre sa couleur en bleu et opacité 50%
            _currentPlatform.GetComponent<SpriteRenderer>().color = new Color32(173, 216, 230, 128);

            // Le mettre en enfant du joueur pou qu'elle le suive avec lui
            _currentPlatform.transform.SetParent(transform);

            // La plateforme est invoqué
            _hasInvoke = true;
        }

        // Su notre plateforme est invoqué et bien en mémoire
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



            // Déplacer vers la gauche
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
            // Déplacer vers la droite
            if (Input.GetKey(KeyCode.X))
            {
                Vector3 position = _currentPlatform.transform.position;
                position.x += 0.1f;

                float distance = 0f;

                if (currentPlayerDirection > 0)
                {
                    distance = position.x - _startingPosition.x;
                   
                } else
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

                _currentCoroutine = StartCoroutine(TimeToDestroy(_data.AutoDestroyTimer));   
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

    // --------------------------- Reference au current object et gestion apres placement a faire dans un script a part ---------------------
     
    // Délai de destruction
    private IEnumerator TimeToDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);

        ResetPower();
    }

    private void ResetPower()
    {
        Destroy(_currentPlatform);
        HUDManager.Instance.PowerTimer.SetActive(false);
        _currentPlatform = null;
        HasBounce = false;
        _powerDelay = 0f;
        // reset the color power
        ColorPowerController.Instance.ResetPower();
        SetAbility(ColorAbilities.None);
        _hasInvoke = false;
        _placed = false;
        _powerDelay = 20f;
        _data = null;
        _currentCoroutine = null;
    }


    private void ActivePlateforme()
    {
        if (!_data.Istrigger)
            _currentPlatform.GetComponent<Collider2D>().isTrigger = false;

        _currentPlatform.GetComponent<SpriteRenderer>().color = Color.white;

        _currentPlatform.transform.SetParent(null);
        _placed = true;

        switch (CurrentAbility)
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


    }



    //// --------------------------- RED --------------------------

    //// Fonction pour l'attaquer / lancé de projectile peut s'adapter au pouvoir orange avec un parametre a mettre en place
    //private void Attack()
    //{
    //    abilityTimer -= Time.deltaTime;

    //    if (abilityTimer <= 0)
    //    {
    //        ColorPowerController.Instance.ResetPower();
    //        SetAbility(ColorAbilities.None);
    //        _canAttack = true;
    //        abilityTimer = 10f;
    //        return;
    //    }
    //    else
    //    {
    //        if (Input.GetKeyDown(KeyCode.Return))
    //        {
    //            if (_canAttack)
    //            {
    //                GameObject projectile = Instantiate(_projectilePrefab, transform.position, transform.rotation);
    //                ProjectileController controller = projectile.GetComponent<ProjectileController>();
    //                if (controller != null)
    //                {
    //                    controller.SetDirection(Mathf.Sign(GameManager.Instance.Player.localScale.x) * Vector2.right);
    //                    controller.SetTarget("Enemy");
    //                }
    //                else
    //                {
    //                    Debug.LogError("pas de script trouvé.");
    //                }

    //                _canAttack = false;

    //            }

                
    //        }

    //        _canAttack = FireCoolDownTimer <= 0;

    //        FireCoolDownTimer -= Time.deltaTime;

    //    }
    //}

    //private void BreatheUnderWater()
    //{
    //    OxygeneCooldwon -= Time.deltaTime;

    //    if (OxygeneCooldwon <= 0)
    //    {
    //        GetComponent<Water>().CanBreatheUnderWater = false;
    //        HUDManager.Instance.OxygeneTimerGO.SetActive(false);
    //        SetAbility(ColorAbilities.None);
    //        OxygeneCooldwon = 45f;
    //    } 
    //    else
    //    {
    //        GetComponent<Water>().CanBreatheUnderWater = true;
    //        HUDManager.Instance.OxygeneTimerGO.SetActive(true);
    //        HUDManager.Instance.OxygeneTimerGO.GetComponent<Text>().text = ((int)OxygeneCooldwon).ToString();
    //    }
    //}


}
