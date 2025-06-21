using ChestSystem.Model;


namespace ChestSystem.Utility
{
    //Observer Pattern
    public interface IChestObserver
    {
        void OnChestSpawned(ChestModel chestModel, int slotIndex);
    }
}
    