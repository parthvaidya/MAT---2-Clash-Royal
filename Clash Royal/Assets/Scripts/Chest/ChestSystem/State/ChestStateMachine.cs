namespace ChestSystem.State
{
    using ChestSystem.Model;
    using UnityEngine;
    public class ChestStateMachine
    {
        private IChestState currentState;
        private ChestModel model;

        public ChestStateMachine(ChestModel model)
        {
            this.model = model;
        }

        public void SetState(IChestState newState)
        {
            if (newState == null) return;
            currentState = newState;           
            currentState.Enter(model);
        }

        public void Update()
        {
            currentState?.Update(model);
        }

        public void OnTap()
        {
            currentState?.OnChestTap(model);
        }
    }
}