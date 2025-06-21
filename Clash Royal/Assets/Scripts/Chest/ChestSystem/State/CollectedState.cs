namespace ChestSystem.State
{
    using ChestSystem.Model;

    public class CollectedState : IChestState
    {
        public void Enter(ChestModel model)
        {
            model.chestState = ChestState.Collected;
        }

        public void Update(ChestModel model)
        {
            
        }

        public void OnChestTap(ChestModel model)
        {
            
        }
    }
}