using ChestSystem.State;

namespace ChestSystem.Model
{
    public class ChestModel
    {
        public ChestDataSO chestData; //Initialize scriptable object

        public int generatedCoins;
        public int generatedGems;
        public ChestStateMachine StateMachine { get; private set; }

        public System.TimeSpan unlockDuration; // chest takes to unlock
        public ChestState chestState = ChestState.Locked; //Current state of the chest
        public System.DateTime unlockStartTime; //Records the real-world time

        //Picks a random number of coins and gems within the defined range from the chest type
        public void GenerateRewards()
        {
            generatedCoins = UnityEngine.Random.Range(chestData.minCoins, chestData.maxCoins);
            generatedGems = UnityEngine.Random.Range(chestData.minGems, chestData.maxGems);
        }

        public ChestModel()
        {
            StateMachine = new ChestStateMachine(this);
            StateMachine.SetState(new LockedState());
        }

        //read-only property that tells how much time is left
        public System.TimeSpan RemainingTime =>
            unlockStartTime == default ? unlockDuration : unlockStartTime.Add(unlockDuration) - System.DateTime.Now;
    }

}


