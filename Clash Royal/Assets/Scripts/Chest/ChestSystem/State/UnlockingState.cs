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
            var timeLeft = model.unlockStartTime + model.unlockDuration - DateTime.Now;
            if (timeLeft.TotalSeconds <= 0)
            {
                model.StateMachine.SetState(new UnlockedState());
            }
        }

        public void OnChestTap(ChestModel model)
        {
            // Maybe show a "wait" message or sound feedback here
        }
    }
}