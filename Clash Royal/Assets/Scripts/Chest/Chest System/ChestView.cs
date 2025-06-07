using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestView : MonoBehaviour, IChestObserver
{
    //Initialize add UI and slot required to spawn the Chest
    [SerializeField] private GameObject chestUIPrefab;
    [SerializeField] private Transform chestSlotParent;

    private void Start()
    {
        var subject = ServiceLocator.Get<ChestSubject>(); //Get chest subject
        subject.Register(this); //Register the service
    }

    //Once the chest is spawned function
    public void OnChestSpawned(ChestModel model, int slotIndex)
    {
        var slot = chestSlotParent.GetChild(slotIndex); //Get slot for spawning chest
        var chestUI = Instantiate(chestUIPrefab, slot); //Instantiate chest UI at that slot

        //Ensures the prefab fills the slot properly initiazed to zero
        RectTransform rt = chestUI.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero; 
        rt.localScale = Vector3.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        // Get components inside the prefab 
        var image = chestUI.GetComponentInChildren<Image>();
        var coinText = chestUI.transform.Find("CoinText").GetComponent<TextMeshProUGUI>();
        var gemText = chestUI.transform.Find("GemText").GetComponent<TextMeshProUGUI>();
        var timerText = chestUI.transform.Find("TimerText").GetComponent<TextMeshProUGUI>();
        var chestNameText = chestUI.transform.Find("ChestNameText").GetComponent<TextMeshProUGUI>();


        // Bind data to Sets UI values from the ChestModel and ChestDataSO
        image.sprite = model.chestData.chestSprite;
        coinText.text = $"Coins: {model.generatedCoins}";
        gemText.text = $"Gems: {model.generatedGems}";
        timerText.text = "Locked"; // We update this only when unlock starts
        chestNameText.text = model.chestData.chestName;
        chestUI.GetComponent<ChestUIRuntime>().Init(model, timerText);
    }
}