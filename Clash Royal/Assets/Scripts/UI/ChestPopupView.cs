using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestPopupView : MonoBehaviour
{
    //Add gameobjects to display
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Button startTimerButton;
    [SerializeField] private Button unlockWithGemsButton;
    [SerializeField] private TextMeshProUGUI gemCostText;
    [SerializeField] private Button closeButton;
    [SerializeField] private ChestController chestController;

    private ChestModel currentChest;
    

    private void Start()
    {
        popupPanel.SetActive(false); //set popup as inactive

        //start timer to unlock chest
        startTimerButton.onClick.AddListener(() =>
        {
            chestController.StartUnlockTimer(currentChest);
            SoundManager.Instance.Play(Sounds.ButtonClick);
            Close();
        });

        //unlock with the gems 
        unlockWithGemsButton.onClick.AddListener(() =>
        {
            chestController.TryUnlockWithGems(currentChest);
            SoundManager.Instance.Play(Sounds.ButtonClick);
            Close();
        });

        closeButton.onClick.AddListener(Close);
    }

    //Unlock chest 
    public void Show(ChestModel chest)
    {
        currentChest = chest;
        popupPanel.SetActive(true);

        int gemCost = Mathf.CeilToInt((float)chest.RemainingTime.TotalMinutes / 10f);
        gemCostText.text = $"Unlock with {gemCost} Gems";
    }

    //close button
    public void Close()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        popupPanel.SetActive(false);
    }
}
