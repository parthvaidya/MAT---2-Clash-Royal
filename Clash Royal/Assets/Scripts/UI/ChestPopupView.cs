using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestPopupView : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Button startTimerButton;
    [SerializeField] private Button unlockWithGemsButton;
    [SerializeField] private TextMeshProUGUI gemCostText;
    [SerializeField] private Button closeButton;

    private ChestModel currentChest;
    private ChestController chestController;

    private void Start()
    {
        chestController = FindObjectOfType<ChestController>();
        popupPanel.SetActive(false);

        startTimerButton.onClick.AddListener(() =>
        {
            chestController.StartUnlockTimer(currentChest);
            SoundManager.Instance.Play(Sounds.ButtonClick);
            Close();
        });

        unlockWithGemsButton.onClick.AddListener(() =>
        {
            chestController.TryUnlockWithGems(currentChest);
            SoundManager.Instance.Play(Sounds.ButtonClick);
            Close();
        });


        closeButton.onClick.AddListener(Close);
    }

    public void Show(ChestModel chest)
    {
        currentChest = chest;
        popupPanel.SetActive(true);

        int gemCost = Mathf.CeilToInt((float)chest.RemainingTime.TotalMinutes / 10f);
        gemCostText.text = $"Unlock with {gemCost} Gems";
    }

    public void Close()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        popupPanel.SetActive(false);
    }
}
