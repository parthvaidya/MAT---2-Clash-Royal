
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //initialize the coins
    public int Coins = 0;
    public int Gems = 0;
    public static PlayerData Instance; //make playerdata as singleton
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
