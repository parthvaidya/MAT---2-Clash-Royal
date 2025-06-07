using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    //Add text to update the UI of coins and gems
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI gemsText;
   
    private void Start()
    {
        UpdateUI();
    }
 
    //Updated UI
    public void UpdateUI()
    {
        coinsText.text = $"Coins: {PlayerData.Instance.Coins}";
        gemsText.text = $"Gems: {PlayerData.Instance.Gems}";
    }
}
