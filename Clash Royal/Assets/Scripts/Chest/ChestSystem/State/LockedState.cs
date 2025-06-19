
namespace ChestSystem.State
{
    using ChestSystem.Model;
    using ChestSystem.View;
    using UnityEngine;

    public class LockedState : IChestState
    {
        public void Enter(ChestModel model)
        {
            model.chestState = ChestState.Locked;
            Debug.Log(" Enter LockedState");
        }

        public void Update(ChestModel model)
        {
            // No update logic needed for Locked state
        }

        public void OnChestTap(ChestModel model)
        {
            Object.FindObjectOfType<ChestPopupView>()?.Show(model);
        }
    }
}
