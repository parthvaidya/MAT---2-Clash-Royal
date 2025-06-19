namespace ChestSystem.State
{
    using ChestSystem.Controller;
    using ChestSystem.Model;
    using ChestSystem.View;
    using UnityEngine;

    public class UnlockedState : IChestState
    {
        //public void Enter(ChestModel model)
        //{
        //    model.chestState = ChestState.Unlocked;
        //    model.GenerateRewards();
        //    model.unlockStartTime = default;
        //}

        //public void Update(ChestModel model)
        //{

        //}

        //public void OnChestTap(ChestModel model)
        //{
        //    PlayerData.Instance.Coins += model.generatedCoins;
        //    PlayerData.Instance.Gems += model.generatedGems;

        //    model.StateMachine.SetState(new CollectedState());

        //    SoundManager.Instance.Play(Sounds.SoldItem);
        //    Object.FindObjectOfType<PlayerUI>()?.UpdateUI();
        //    Object.FindObjectOfType<ChestController>()?.RemoveChest(model);
        //}

        private readonly PlayerUI _playerUI;
        private readonly ChestController _controller;

        public UnlockedState()
        {
            _playerUI = ServiceLocator.Get<PlayerUI>();
            _controller = ServiceLocator.Get<ChestController>();
        }

        public void Enter(ChestModel model)
        {
            model.chestState = ChestState.Unlocked;
            model.GenerateRewards();
            model.unlockStartTime = default;
        }

        public void Update(ChestModel model)
        {
            // No behavior needed
        }

        public void OnChestTap(ChestModel model)
        {
            PlayerData.Instance.Coins += model.generatedCoins;
            PlayerData.Instance.Gems += model.generatedGems;

            model.StateMachine.SetState(new CollectedState());

            SoundManager.Instance.Play(Sounds.SoldItem);
            _playerUI?.UpdateUI();
            _controller?.RemoveChest(model);
        }
    }
}