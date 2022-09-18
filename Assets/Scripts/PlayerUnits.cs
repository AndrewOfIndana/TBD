using UnityEngine;

public class PlayerUnits : MonoBehaviour
{
    public static PlayerUnits playerUnitsInstance; 

    public Stats[] playerTowers;
    public int towerTypes = 0;
    private Stats unitToPlace;

    void Start()
    {
        unitToPlace = playerTowers[towerTypes];
    }
    void Awake()
    {
        if(playerUnitsInstance != null)
        {
            Debug.Log("Error more than one playerUnitsInstance");
            return;
        }
        playerUnitsInstance = this;
    }
    public Stats GetUnitToPlace()
    {
        return unitToPlace;
    }
}
