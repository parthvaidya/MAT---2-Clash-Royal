
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestView : MonoBehaviour, IChestObserver
{
    [SerializeField] private GameObject chestUIPrefab;
    [SerializeField] private Transform chestSlotParent;

    private void Start()
    {
        var subject = ServiceLocator.Get<ChestSubject>();
        subject.Register(this);
    }

    public void OnChestSpawned(ChestModel model, int slotIndex)
    {
        var slot = chestSlotParent.GetChild(slotIndex);
        var chestUI = Instantiate(chestUIPrefab, slot);

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

        var controller = FindObjectOfType<ChestController>();
        var playerUI = FindObjectOfType<PlayerUI>();

        // Bind data
        image.sprite = model.chestData.chestSprite;
        coinText.text = $"Coins: {model.generatedCoins}";
        gemText.text = $"Gems: {model.generatedGems}";
        timerText.text = "Locked"; // We update this only when unlock starts

        // Optionally attach model reference to the UI if needed later
        //chestUI.AddComponent<ChestUIRuntime>().Init(model, timerText);

        chestUI.GetComponent<ChestUIRuntime>().Init(model, timerText,  controller, playerUI);
    }

    private void OnEnable()
    {
        var subject = ServiceLocator.Get<ChestSubject>();
        subject.Register(this);
    }

    private void OnDisable()
    {
        var subject = ServiceLocator.Get<ChestSubject>();
        subject.Unregister(this);
    }
}