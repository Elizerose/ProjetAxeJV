using UnityEngine;

public class DatabaseManager : MonoBehaviour
{

    private static DatabaseManager _instance;
    public static DatabaseManager Instance => _instance;

    [SerializeField] private EnemyDatabase enemyDatabase;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public EnemyData GetData(int id)  => enemyDatabase.GetData(id);


}
