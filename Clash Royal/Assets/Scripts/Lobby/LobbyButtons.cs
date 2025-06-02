using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyButtons : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        startButton.onClick.AddListener(startGame);
        quitButton.onClick.AddListener(Quit);
    }


    private void startGame()
    {
        
        SceneManager.LoadScene(1);
    }


    private void Quit()
    {
        
        SceneManager.LoadScene(2);
    }
}
