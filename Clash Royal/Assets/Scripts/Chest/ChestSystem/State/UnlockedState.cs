namespace ChestSystem.State
{
    using ChestSystem.Controller;
    using ChestSystem.Model;
    using ChestSystem.View;
    using UnityEngine;

    public class UnlockedState : IChestState
    {
        public void Enter(ChestModel model)
        {
            model.chestState = ChestState.Unlocked;
            model.GenerateRewards();

            // Optional: clear unlock time to avoid future negative time math
            model.unlockStartTime = default;
        }

        public void Update(ChestModel model)
        {
            // Nothing needed, chest is idle waiting for player tap
        }

        public void OnChestTap(ChestModel model)
        {
            PlayerData.Instance.Coins += model.generatedCoins;
            PlayerData.Instance.Gems += model.generatedGems;

            model.StateMachine.SetState(new CollectedState());

            SoundManager.Instance.Play(Sounds.SoldItem);
            Object.FindObjectOfType<PlayerUI>()?.UpdateUI();
            Object.FindObjectOfType<ChestController>()?.RemoveChest(model);
        }
    }
}