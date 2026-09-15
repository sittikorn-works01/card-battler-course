using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestPanel : BasePanel
{
    [SerializeField] private Button healButton;
    [SerializeField] private Button manageDeckButton;
    [SerializeField] private Button exitButton;

    [SerializeField, Range(0f, 1f)] private float healPercentage = 0.3f;
    [SerializeField] private GameObject deckManager;

    // A rest site grants one action: heal or manage deck, not both.
    private bool hasChosenOption;

    private void OnEnable()
    {
        hasChosenOption = false;

        healButton.onClick.AddListener(HealButton);
        manageDeckButton.onClick.AddListener(ManageDeckButton);
        exitButton.onClick.AddListener(ExitRest);
    }

    private void OnDisable()
    {
        healButton.onClick.RemoveAllListeners();
        manageDeckButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }

    private void HealButton()
    {
        if (!TryChooseOption())
        {
            return;
        }

        PlayerData.Instance.Heal(PlayerData.Instance.MaxHealth * healPercentage);
        ExitRest();
    }

    private void ManageDeckButton()
    {
        if (!TryChooseOption())
        {
            return;
        }

        deckManager.SetActive(true);
        Close();
    }

    // Returns false (and does nothing) once an option has already been chosen.
    private bool TryChooseOption()
    {
        if (hasChosenOption)
        {
            return false;
        }

        hasChosenOption = true;
        return true;
    }

    private void ExitRest()
    {
        Close();
        GameManager.Instance.EnterState(GameState.Map);
    }
}
