using UnityEngine;
using static ColorPowerController;

public class PlayerAbilities : MonoBehaviour
{
    private ColorAbilities _currentAbility;

    [Header("Yellow power")]
    [SerializeField] private GameObject _platformPrefab;
    private bool _hasInvoke = false;
    private GameObject currentPlatform = null;



    public void SetAbility(ColorAbilities ability)
    {
        Debug.Log("setting ok ?");
        _currentAbility = ability;
    }





    public void Update()
    {
        Debug.Log(_currentAbility);
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
        
        if (!_hasInvoke)
        {
            // step 1 : instancier un 'fantome' de la plateforme avec des reperes de placement en enfant du player
            currentPlatform = Instantiate(_platformPrefab, transform.position + new Vector3(2 * Mathf.Sign(GameManager.Instance.Player.localScale.x), 0,0) , transform.rotation);

            currentPlatform.GetComponent<SpriteRenderer>().color = new Color32(173, 216, 230, 128);

            currentPlatform.transform.localScale = new Vector3(1, 2.6f, 1);

            currentPlatform.transform.SetParent(transform);
            _hasInvoke = true;
        }

        if (currentPlatform != null)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                currentPlatform.GetComponent<Collider2D>().isTrigger = false;
                currentPlatform.GetComponent<SpriteRenderer>().color = Color.white;
                currentPlatform.transform.SetParent(null);

            }
        }

         


        // step 2 : gerer le deplacement de ce bloc de gauche a droite avec Q et D


    }





}
