using UnityEngine;
using static ColorPowerController;

public class PlayerAbilities : MonoBehaviour
{
    private ColorAbilities _currentAbility;

    [Header("Yellow power")]
    [SerializeField] private GameObject _platformPrefab;
    private bool _hasInvoke = false;
    private GameObject currentPlatform = null;
    private float MaxDistance = 5f;
    private float MinDistance = 2f;
    private Vector3 startingPosition;



    public void SetAbility(ColorAbilities ability)
    {
        Debug.Log("setting ok ?");
        _currentAbility = ability;
    }





    public void Update()
    {
        switch (_currentAbility)
        {
            case ColorAbilities.Red:
                // Fire
                break;

            case ColorAbilities.Blue:
                // respirer sous leau
                break;

            case ColorAbilities.Yellow:
                // platefomes lumineuses
                InvokePlatform();
                break;

            default:
                break;

        }
    }






    public void InvokePlatform()
    {
        float currentPlayerDirection = Mathf.Sign(GameManager.Instance.Player.localScale.x);

        startingPosition = transform.position + new Vector3(2 * currentPlayerDirection, 0, 0);
        if (!_hasInvoke)
        {
            
            // step 1 : instancier un 'fantome' de la plateforme avec des reperes de placement en enfant du player
            currentPlatform = Instantiate(_platformPrefab, startingPosition, transform.rotation); 

            currentPlatform.GetComponent<SpriteRenderer>().color = new Color32(173, 216, 230, 128);

            currentPlatform.transform.localScale = new Vector3(1, 2.6f, 1);

            currentPlatform.transform.SetParent(transform);
            _hasInvoke = true;
        }

        if (currentPlatform != null)
        {
            // Déplacer vers la gauche
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 position = currentPlatform.transform.position;
                position.x -= 0.1f;

                float distance = 0f;

                if (currentPlayerDirection > 0)
                {
                    distance = position.x - startingPosition.x;

                }
                else
                {
                    distance = startingPosition.x - position.x;
                }

                if (distance >= 0 && distance <= MaxDistance)
                {
                    currentPlatform.transform.position = position;
                }


            }
            // Déplacer vers la droite
            if (Input.GetKey(KeyCode.X))
            {
                Vector3 position = currentPlatform.transform.position;
                position.x += 0.1f;

                float distance = 0f;

                if (currentPlayerDirection > 0)
                {
                    distance = position.x - startingPosition.x;
                   
                } else
                {
                    distance = startingPosition.x - position.x;
                }

                if (distance >= 0 && distance <= MaxDistance)
                {
                    currentPlatform.transform.position = position;
                }

            }

            // Poser
            if (Input.GetKeyDown(KeyCode.Return))
            {
                currentPlatform.GetComponent<Collider2D>().isTrigger = false;
                currentPlatform.GetComponent<SpriteRenderer>().color = Color.white;
                currentPlatform.transform.SetParent(null);

                currentPlatform = null;
            }
        }
    }





}
