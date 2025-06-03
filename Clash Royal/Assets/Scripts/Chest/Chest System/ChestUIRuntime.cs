using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestUIRuntime : MonoBehaviour
{
   [SerializeField] private ChestModel model;
    [SerializeField] private TextMeshProUGUI timerText;

    public void Init(ChestModel model, TextMeshProUGUI timerText)
    {
        this.model = model;
        this.timerText = timerText;
    }

    private void Update()
    {
        if (model.chestState == ChestState.Unlocking)
        {
            var timeRemaining = model.unlockStartTime + model.unlockDuration - System.DateTime.Now;
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
    }
}