using System;

public class ChestModel
{
    public ChestDataSO chestData;
    public int generatedCoins;
    public int generatedGems;
    public System.TimeSpan unlockDuration;
    public ChestState chestState = ChestState.Locked;

    public System.DateTime unlockStartTime;


    public void GenerateRewards()
    {
        generatedCoins = UnityEngine.Random.Range(chestData.minCoins, chestData.maxCoins);
        generatedGems = UnityEngine.Random.Range(chestData.minGems, chestData.maxGems);
    }

    public System.TimeSpan RemainingTime =>
        unlockStartTime == default ? unlockDuration : unlockStartTime.Add(unlockDuration) - System.DateTime.Now;
}
