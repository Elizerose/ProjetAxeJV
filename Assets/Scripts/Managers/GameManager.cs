using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gère l'état du jeu : Mort / ...
/// </summary>
/// 
public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager _instance;
    public static GameManager Instance => _instance ;

    // causes de mort pour gerer les differentes animations
    public enum DeathCauses 
    {
        Water,
        Enemy,
        Fall
    }

    // reference de notre joueur
    [SerializeField] private Transform _player;
    public Transform Player 
    { 
        get 
        { 
            return _player; 
        }  
        private set 
        {  
            _player = value;
        } 
    }

    private void Awake() 
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Mort du joueur
    public void Death(DeathCauses cause)
    {
        // gerer les différentes animations de mort
        // ...

        // Affichage du panel de mort
        HUDManager.Instance.DeathPanel.GetComponent<Animator>().SetTrigger("FadeIn");
        
    }

    // recommencer le niveau / la scene
    public void ReStart()
    {
        HUDManager.Instance.DeathPanel.GetComponent<Animator>().SetTrigger("FadeOut");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
