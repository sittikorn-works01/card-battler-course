using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    public SceneNavButton[] navButtons;

    void OnEnable()
    {
        for (int i = 0; i < navButtons.Length; i++)
        {
            navButtons[i].OnClick += NavigateScene;
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < navButtons.Length; i++)
        {
            navButtons[i].OnClick -= NavigateScene;
        }
    }

    void NavigateScene(string sceneName) => SceneManager.LoadScene(sceneName);
}