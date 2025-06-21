namespace ChestSystem.State
{
    using System;
    using ChestSystem.Model;

    public class UnlockingState : IChestState
    {
        public void Enter(ChestModel model)
        {
            model.chestState = ChestState.Unlocking;
            model.unlockStartTime = DateTime.Now;
        }
    
        public void Update(ChestModel model)
        {
            if (System.DateTime.Now >= model.unlockStartTime + model.unlockDuration)
            {
                model.StateMachine.SetState(new UnlockedState());
            }
        }

        public void OnChestTap(ChestModel model)
        {
            
        }
    }
}