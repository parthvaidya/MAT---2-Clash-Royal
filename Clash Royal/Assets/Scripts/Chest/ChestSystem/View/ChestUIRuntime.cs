using ChestSystem.Controller;
using ChestSystem.Model;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.View
{
    public class ChestUIRuntime : MonoBehaviour
    {
        [Header("UI Bindings")]
        [SerializeField] private Image chestImage;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI gemText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI chestNameText;


        private ChestModel model;
        public ChestModel Model => model; //model data required 
        private bool isInitialized;

        //Initialize constructor
        public void Init(ChestModel model)
        {
            this.model = model;
            chestImage.sprite = model.chestData.chestSprite;
            coinText.text = $"Coins: {model.generatedCoins}";
            gemText.text = $"Gems: {model.generatedGems}";
            timerText.text = "Locked";
            chestNameText.text = model.chestData.chestName;
            isInitialized = true;
            UpdateTimerText();

            //Used Getcomponent since ChestUI prefab cannot attach button from scene
            GetComponent<Button>().onClick.AddListener(OnTap);
        }

        private void Update()
        {
            //Skips logic if chest not ready or not assigned
            if (!isInitialized || model == null || timerText == null)
                return;

            model.StateMachine.Update();
            UpdateTimerText();
        }

        
        private void OnTap()
        {
            
            model.StateMachine.OnTap();

            
            if (model.chestState == ChestState.Collected)
            {
                StartCoroutine(HideAndDestroyChest(0f));
                return;
            }

            
            if (model.chestState == ChestState.Locked)
                
                ServiceLocator.Get<ChestPopupView>()?.Show(model);
        }


        private void UpdateTimerText()
        {
            switch (model.chestState)
            {
                case ChestState.Locked:
                    timerText.text = "Locked";
                    break;

                case ChestState.Unlocking:
                    timerText.text =
                        $"{Mathf.CeilToInt((float)model.RemainingTime.TotalSeconds)}s";
                    break;

                case ChestState.Unlocked:
                    timerText.text = "Unlocked! Tap to collect";
                    break;

                case ChestState.Collected:
                    timerText.text = "Collected";
                    break;
            }
        }


        private System.Collections.IEnumerator HideAndDestroyChest(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }

        
        

        //Refresh Timer text
        public void RefreshtheUI()
        {
            UpdateTimerText();
        }
    }
}
    //Runtime UI Logic
   