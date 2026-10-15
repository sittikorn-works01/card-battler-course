using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneNavButton : MonoBehaviour
{
    public string sceneName;
    [SerializeField] private Button button; 

    void OnEnable() => button.onClick.AddListener(OnButtonClick);
    void OnDisable() => button.onClick.RemoveListener(OnButtonClick);

    void OnButtonClick() => SceneManager.LoadScene(sceneName);
}