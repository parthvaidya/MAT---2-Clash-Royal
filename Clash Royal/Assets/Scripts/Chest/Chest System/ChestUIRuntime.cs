using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestUIRuntime : MonoBehaviour
{
    private ChestModel model;
    [SerializeField]private TextMeshProUGUI timerText;

    public ChestModel Model => model;
    private bool isInitialized;

   

    public void Init(ChestModel model, TextMeshProUGUI timerText)
    {
        this.model = model;
        this.timerText = timerText;
        isInitialized = true;
        UpdateTimerText();

        GetComponent<Button>().onClick.AddListener(ShowPopup);
    }

    private void Update()
    {
        if (!isInitialized || model == null || timerText == null)
            return;

        if (model.chestState == ChestState.Unlocking)
        {
            var timeRemaining = model.unlockStartTime + model.unlockDuration - DateTime.Now;
            if (timeRemaining.TotalSeconds <= 0)
            {
                model.chestState = ChestState.Unlocked;
                timerText.text = "Unlocked! Tap to collect";
            }
            else
            {
                timerText.text = $"{Mathf.CeilToInt((float)timeRemaining.TotalSeconds)}s";
            }
        }
        else
        {
            UpdateTimerText();
        }
    }

     private void UpdateTimerText()
    {
        switch (model.chestState)
        {
            case ChestState.Locked:
                timerText.text = "Locked";
                break;
            case ChestState.Unlocking:
                var remaining = model.unlockStartTime + model.unlockDuration - DateTime.Now;
                timerText.text = $"{Mathf.CeilToInt((float)remaining.TotalSeconds)}s";
                break;
            case ChestState.Unlocked:
                timerText.text = "Unlocked! Tap to collect";
                break;
            case ChestState.Collected:
                timerText.text = ""; // or “Collected”
                break;
        }
    }



    private void ShowPopup()
    {
        if (model.chestState == ChestState.Unlocked)
        {
            // Give rewards
            PlayerData.Instance.Coins += model.generatedCoins;
            PlayerData.Instance.Gems += model.generatedGems;
            model.chestState = ChestState.Collected;

            SoundManager.Instance.Play(Sounds.SoldItem);

            // Update global UI
            FindObjectOfType<PlayerUI>().UpdateUI();

            // Remove chest model from controller list
            var chestController = FindObjectOfType<ChestController>();
            if (chestController != null)
            {
                chestController.RemoveChest(model); // <== You need to create this method
            }

            // Show chest text
            timerText.text = "Collected!";

            // Show reward message if available
            var rewardMessageText = transform.Find("RewardMessageText")?.GetComponent<TextMeshProUGUI>();
            if (rewardMessageText != null)
            {
                rewardMessageText.text = $"+{model.generatedCoins} Coins\n+{model.generatedGems} Gems";
                rewardMessageText.gameObject.SetActive(true);
                StartCoroutine(HideAndDestroyChest(rewardMessageText.gameObject, 2.5f));
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else if (model.chestState == ChestState.Locked)
        {
            FindObjectOfType<ChestPopupView>().Show(model);
        }
    }

    private IEnumerator HideAndDestroyChest(GameObject messageGO, float delay)
    {
        yield return new WaitForSeconds(delay);
        messageGO.SetActive(false);
        Destroy(this.gameObject);
    }

    public void ForceRefresh()
    {
        Debug.Log($"ForceRefresh called. ChestState: {model.chestState}");
        UpdateTimerText();
    }
}