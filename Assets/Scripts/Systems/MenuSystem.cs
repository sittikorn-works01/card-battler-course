using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    //[SerializeField] private Button startButton;
    //[SerializeField] private Button deckBuilderButton;
    //[SerializeField] private Button quitButton;

    //private void OnEnable()
    //{
    //    startButton.onClick.AddListener(StartButton);
    //    deckBuilderButton.onClick.AddListener(DeckBuilderButton);
    //    quitButton.onClick.AddListener(QuitButton);
    //}

    //private void OnDisable()
    //{
    //    startButton.onClick.RemoveAllListeners();
    //    deckBuilderButton.onClick.RemoveAllListeners();
    //    quitButton.onClick.RemoveAllListeners();
    //}

    public void StartButton()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void DeckBuilderButton()
    {
        SceneManager.LoadScene("DeckBuilder");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else 
            Application.Quit();
        #endif

     
    }
}
