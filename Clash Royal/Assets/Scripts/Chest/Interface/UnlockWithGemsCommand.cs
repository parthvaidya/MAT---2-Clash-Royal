using UnityEngine;

public class UnlockWithGemsCommand : ICommand
{
    private ChestModel chest;
    private int usedGems;
    private ChestController controller;
    private PlayerUI playerUI;

    private int prevCoins;
    private int prevGems;
  

    public UnlockWithGemsCommand(ChestModel chest, ChestController controller, PlayerUI playerUI)
    {
        this.chest = chest;
        this.controller = controller;
        this.playerUI = playerUI;
    }

    
    public void Execute()
    {
        // Save original player data before changing
        prevCoins = PlayerData.Instance.Coins;
        prevGems = PlayerData.Instance.Gems;

        usedGems = chest.chestData.gemCost; // or how many gems required to unlock

        if (PlayerData.Instance.Gems < usedGems)
        {
            Debug.Log("Not enough gems!");
            SoundManager.Instance.Play(Sounds.Warning);
            controller.ShowNotEnoughGemsPopup();
            return;
        }

        PlayerData.Instance.Gems -= usedGems;
        chest.chestState = ChestState.Unlocked;

        Debug.Log($"Execute(): Coins after {PlayerData.Instance.Coins}, Gems after {PlayerData.Instance.Gems}");

        playerUI.UpdateUI();
    }

    public void Undo()
    {
        Debug.Log($"Undo(): Coins before restore {PlayerData.Instance.Coins}, Gems before restore {PlayerData.Instance.Gems}");

        // Restore exact previous values
        PlayerData.Instance.Coins = prevCoins;
        PlayerData.Instance.Gems = prevGems;

        chest.chestState = ChestState.Locked;

        Debug.Log($"Undo(): Coins after restore {PlayerData.Instance.Coins}, Gems after restore {PlayerData.Instance.Gems}");

        playerUI.UpdateUI();
    }
}