using System;

public class ChestModel
{
    public ChestDataSO chestData;
    public ChestState chestState = ChestState.Locked;
    public DateTime unlockStartTime;
    public TimeSpan unlockDuration;

    public int generatedCoins;
    public int generatedGems;

    public void GenerateRewards()
    {
        generatedCoins = UnityEngine.Random.Range(chestData.minCoins, chestData.maxCoins + 1);
        generatedGems = UnityEngine.Random.Range(chestData.minGems, chestData.maxGems + 1);
    }
}
