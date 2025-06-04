using UnityEngine;

public class UnlockWithGemsCommand : ICommand
{
    private ChestModel chest;
    private int usedGems;
    private ChestController controller;
    private PlayerUI playerUI;

    public UnlockWithGemsCommand(ChestModel chest, ChestController controller, PlayerUI playerUI)
    {
        this.chest = chest;
        this.controller = controller;
        this.playerUI = playerUI;
    }

    public void Execute()
    {
        usedGems = Mathf.CeilToInt((float)chest.RemainingTime.TotalMinutes / 10f);
        if (controller.playerGems < usedGems)
        {
            Debug.Log("Not enough gems!");
            return;
        }

        controller.playerGems -= usedGems;
        chest.chestState = ChestState.Unlocked;

        PlayerData.Instance.Gems -= usedGems;
        playerUI?.UpdateUI();
    }

    public void Undo()
    {
        controller.playerGems += usedGems;
        chest.chestState = ChestState.Locked;

        PlayerData.Instance.Gems += usedGems;
        playerUI?.UpdateUI();
    }
}