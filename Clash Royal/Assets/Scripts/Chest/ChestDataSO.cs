using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestData", menuName = "Chest System/Chest Data")]
public class ChestDataSO : ScriptableObject
{
    public string chestName;
    public Sprite chestSprite;
    public int minCoins;
    public int maxCoins;
    public int minGems;
    public int maxGems;
    public float unlockTimeMinutes;
    public int gemCost;
}
