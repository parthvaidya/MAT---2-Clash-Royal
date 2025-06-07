using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Runtime UI Logic
public class ChestUIRuntime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private ChestModel model;
    public ChestModel Model => model; //model data required 
    private bool isInitialized;

    //Initialize constructor
    public void Init(ChestModel model, TextMeshProUGUI timerText)
    {
        this.model = model;
        this.timerText = timerText;
        isInitialized = true;
        UpdateTimerText();
        GetComponent<Button>().onClick.AddListener(ShowPopup); //Used Getcomponent since ChestUI prefab cannot attach button from scene
    }

    private void Update()
    {
        //Skips logic if chest not ready or not assigned
        if (!isInitialized || model == null || timerText == null)
            return;

        //Check if Chest is unlocking
        if (model.chestState == ChestState.Unlocking)
        {
            //Calculate Remaining time
            var timeRemaining = model.unlockStartTime + model.unlockDuration - DateTime.Now;
            //Unlocked state
            if (timeRemaining.TotalSeconds <= 0)
            {
                model.chestState = ChestState.Unlocked;
                timerText.text = "Unlocked! Tap to collect";
            }
            else
            {
                //Show countdown in seconds
                timerText.text = $"{Mathf.CeilToInt((float)timeRemaining.TotalSeconds)}s";
            }
        }
        else
        {
            //Update timer text
            UpdateTimerText();
        }
    }

    //Update timerText based on Chest State
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
                timerText.text = "Collected"; 
                break;
        }
    }

    // When chest is tapped
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
                chestController.RemoveChest(model); 
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
                Destroy(this.gameObject); //Destory the object 
            }
        }
        else if (model.chestState == ChestState.Locked)
        {
            FindObjectOfType<ChestPopupView>().Show(model); //Find chest popup view 
        }
    }

    //Delay for the message
    private IEnumerator HideAndDestroyChest(GameObject messageGO, float delay)
    {
        yield return new WaitForSeconds(delay);
        messageGO.SetActive(false);
        Destroy(this.gameObject);
    }

    //Refresh Timer text
    public void RefreshtheUI()
    {
        UpdateTimerText();
    }
}