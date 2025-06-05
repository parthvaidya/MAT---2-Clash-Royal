using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectChestCommand : ICommand
{
    private ChestModel chest;
    private ChestController controller;
    private PlayerUI playerUI;

    public CollectChestCommand(ChestModel chest, ChestController controller, PlayerUI playerUI)
    {
        this.chest = chest;
        this.controller = controller;
        this.playerUI = playerUI;
    }

    public void Execute()
    {
        PlayerData.Instance.Coins += chest.generatedCoins;
        PlayerData.Instance.Gems += chest.generatedGems;
        chest.chestState = ChestState.Collected;

        controller.RemoveChest(chest);
        // Make sure this exists in ChestController
        playerUI.UpdateUI();
    }

    public void Undo()
    {
        PlayerData.Instance.Coins -= chest.generatedCoins;
        PlayerData.Instance.Gems -= chest.generatedGems;
        chest.chestState = ChestState.Unlocked;

        controller.SpawnChestAgain(chest); 
        playerUI.UpdateUI();
    }
}