using System;
using UnityEngine;
using UnityEngine.UI;

public class SceneNavButton : MonoBehaviour
{
    public string sceneName;
    public Button button; //The underlying Unity UI button to handle the click.

    public event Action<string> OnClick;

    void OnEnable() => button.onClick.AddListener(OnButtonClick);
    void OnDisable() => button.onClick.RemoveListener(OnButtonClick);

    void OnButtonClick() => OnClick?.Invoke(sceneName);
}