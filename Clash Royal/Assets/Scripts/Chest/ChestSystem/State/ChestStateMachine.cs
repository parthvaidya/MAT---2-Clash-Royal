namespace ChestSystem.State
{
    using ChestSystem.Model;

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