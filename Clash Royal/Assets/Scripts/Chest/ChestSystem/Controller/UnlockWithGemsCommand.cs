using UnityEngine;
using ChestSystem.Model;
using ChestSystem.View;
using ChestSystem.Utility;

namespace ChestSystem.Controller
{
    public class UnlockWithGemsCommand : ICommand
    {
        //Initialize 
        private ChestModel chest;
        private ChestController controller;
        private PlayerUI playerUI;
        private UIPopupHandler popupHandler;

        private int usedGems;
        private int prevCoins;
        private int prevGems;

        //Initiaze the constructor
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
            usedGems = chest.chestData.gemCost; // how many gems required to unlock the chest

            //Check if there are insufficient gems
            if (PlayerData.Instance.Gems < usedGems)
            {
                SoundManager.Instance.Play(Sounds.Warning);
                popupHandler.ShowNotEnoughGemsPopup();
                return;
            }

            PlayerData.Instance.Gems -= usedGems; //If there are enough gems deduct them from the data 
            chest.chestState = ChestState.Unlocked; // Update the chest state to unlocked
            playerUI.UpdateUI(); //Update the UI
        }

        public void Undo()
        {
            // Restore exact previous values
            PlayerData.Instance.Coins = prevCoins;
            PlayerData.Instance.Gems = prevGems;

            chest.chestState = ChestState.Locked; //Revert back to unlocked
            playerUI.UpdateUI(); //Update UI
        }
    }
}
    