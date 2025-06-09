using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Create the Criptable for the Chest
[CreateAssetMenu(fileName = "ChestData", menuName = "Chest System/Chest Data")]
public class ChestDataSO : ScriptableObject
{
    //Initialize chest attributes
    public string chestName;
    public Sprite chestSprite;
    public int minCoins;
    public int maxCoins;
    public int minGems;
    public int maxGems;
    public float unlockTimeMinutes;
    public int gemCost;
}
