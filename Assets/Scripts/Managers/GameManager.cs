using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

/// <summary>
/// Gère l'état du jeu : Mort / ...
/// </summary>
/// 
public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager _instance;
    public static GameManager Instance => _instance ;

    [SerializeField] private GameObject _ennemiWin;
    private bool _victory = false;
    public Camera Camera;

    private bool _canWin = true;
    public VideoPlayer videoPlayer;
    public GameObject MenuBtn;

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
    }

    private void Start()
    {
        if (_ennemiWin == null)
            _canWin = false;

        Time.timeScale = 1;
    }

    private void Update()
    {
        if (_ennemiWin == null && !_victory && _canWin)
            Win();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool OpenMenu = Time.timeScale == 1 ? true : false;
            Pause(OpenMenu);
            HUDManager.Instance.DislayMenuInGame(OpenMenu);
        }
    }

    public void Win()
    {
        //HUDManager.Instance.VictoryPanel.GetComponent<Animator>().SetTrigger("FadeIn");
        Pause(true);
        Player.gameObject.SetActive(false);

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();

        _victory = true;
    }

    public void OnVideoFinished(VideoPlayer vp)
    {
        MenuBtn.SetActive(true);
    }

    // Mort du joueur
    public void Death(DeathCauses cause)
    {
        // gerer les différentes animations de mort
        // ...
        HUDManager.Instance.DeathPanel.SetActive(true);
        // Affichage du panel de mort
        HUDManager.Instance.DeathPanel.GetComponent<Animator>().SetTrigger("FadeIn");
        Pause(true);
        
    }

    // recommencer le niveau / la scene
    public void ReStart()
    {
       
        HUDManager.Instance.DeathPanel.GetComponent<Animator>().SetTrigger("FadeOut");
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        Pause(false);
        //Player.GetComponent<PlayerHealth>().ResetPlayer();
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void Pause(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
    }

}
