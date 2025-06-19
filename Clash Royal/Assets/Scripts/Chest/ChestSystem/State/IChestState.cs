namespace ChestSystem.State
{
    using ChestSystem.Model;

    public interface IChestState
    {
        void Enter(ChestModel model);
        void Update(ChestModel model);
        void OnChestTap(ChestModel model);
    }
}