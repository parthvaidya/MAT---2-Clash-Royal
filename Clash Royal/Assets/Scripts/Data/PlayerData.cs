
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int Coins = 100;
    public int Gems = 100;

    public static PlayerData Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
