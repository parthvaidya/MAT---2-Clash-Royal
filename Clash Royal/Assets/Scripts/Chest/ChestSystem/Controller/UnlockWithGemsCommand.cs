using ChestSystem.Model;
using ChestSystem.View;
using ChestSystem.Utility;
using ChestSystem.State;

namespace ChestSystem.Controller
{
    public class UnlockWithGemsCommand : ICommand
    {
        
        private ChestModel chest;
        private ChestController controller;
        private PlayerUI playerUI;
        private UIPopupHandler popupHandler;

        private int usedGems;
        private int prevCoins;
        private int prevGems;

        public UnlockWithGemsCommand(ChestModel chest, ChestController controller, PlayerUI playerUI , UIPopupHandler popupHandler)
        {
            this.chest = chest;
            this.controller = controller;
            this.playerUI = playerUI;
            this.popupHandler = popupHandler;
        }

        public void Execute()
        {
            // Save original player data before changing
            prevCoins = PlayerData.Instance.Coins;
            prevGems = PlayerData.Instance.Gems;
            usedGems = chest.chestData.gemCost; 

            //Check if there are insufficient gems
            if (PlayerData.Instance.Gems < usedGems)
            {
                SoundManager.Instance.Play(Sounds.Warning);
                popupHandler.ShowNotEnoughGemsPopup();
                return;
            }

            PlayerData.Instance.Gems -= usedGems; //If there are enough gems deduct them from the data 
            chest.StateMachine.SetState(new UnlockedState());
            playerUI.UpdateUI(); //Update the UI
        }

        public void Undo()
        {
            // Restore exact previous values
            PlayerData.Instance.Coins = prevCoins;
            PlayerData.Instance.Gems = prevGems;
            chest.StateMachine.SetState(new LockedState());
            playerUI.UpdateUI(); //Update UI
        }
    }
}
    