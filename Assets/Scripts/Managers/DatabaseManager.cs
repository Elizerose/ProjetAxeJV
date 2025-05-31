using UnityEngine;
using static ColorPowerController;
using static PlateformesDataBase;

/// <summary>
/// Gère les différentes databases
/// </summary>
public class DatabaseManager : MonoBehaviour
{

    private static DatabaseManager _instance;
    public static DatabaseManager Instance => _instance;

    [SerializeField] private EnemyDatabase _enemyDatabase;
    [SerializeField] private PlateformesDataBase _plateformesDataBase;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public EnemyData GetData(Type type)  => _enemyDatabase.GetData(type);

    public PlateformesData GetPlateformesData(ColorAbilities color) => _plateformesDataBase.GetData(color);




}
