
namespace ChestSystem.State
{
    using ChestSystem.Model;
    using ChestSystem.View;
    using UnityEngine;

    public class LockedState : IChestState
    {
        private readonly ChestPopupView _popup;

        public LockedState()
        {
            _popup = ServiceLocator.Get<ChestPopupView>();
        }

        public void Enter(ChestModel model)
        {
            model.chestState = ChestState.Locked;
        }

        public void Update(ChestModel model)
        {
            // No behavior needed
        }

        public void OnChestTap(ChestModel model)
        {
            _popup?.Show(model);
        }
    }
}
