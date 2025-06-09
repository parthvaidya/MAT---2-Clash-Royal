using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyButtons : MonoBehaviour
{
    //Lobby buttons to be attached
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        //Add listener to the buttons
        startButton.onClick.AddListener(startGame);
        quitButton.onClick.AddListener(Quit);
    }

    private void startGame()
    {        
        SceneManager.LoadScene(1); //Load scene 1
        SoundManager.Instance.Play(Sounds.ButtonClick);
    }


    private void Quit()
    {
        
        SceneManager.LoadScene(2); //load scene 2
        SoundManager.Instance.Play(Sounds.ButtonClick);
    }
}
