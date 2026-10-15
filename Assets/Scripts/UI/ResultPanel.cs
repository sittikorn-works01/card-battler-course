using CardBattlerCourse.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardBattlerCourse.UI
{
    public class ResultPanel : BasePanel
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private TextMeshProUGUI resultText;
        private bool doesPlayerWin = false;

        private void OnEnable()
        {
            PlayerEvents.OnPlayerDeath += OnPlayerDeath;
            EnemyEvents.OnEnemyDeath += OnEnemyDeath;
        }

        private void OnDisable()
        {
            PlayerEvents.OnPlayerDeath -= OnPlayerDeath;
            EnemyEvents.OnEnemyDeath -= OnEnemyDeath;
        }

        public override void Open()
        {
            base.Open();

            continueButton.onClick.AddListener(BattleExitButton);
            SetupButtons();

            resultText.gameObject.SetActive(true);    
        }

        public override void Close()
        {
            base.Close();

            continueButton.onClick.RemoveAllListeners();
            nextButton.onClick.RemoveAllListeners();
        }

        private void SetupButtons()
        {
            // Show the continue/reward button after a win, and the restart button after a loss.
            if (doesPlayerWin)
            {
                continueButton.gameObject.SetActive(false);

                if (GameManager.Instance.CurrentNode == NodeType.Boss)
                {
                    nextButton.onClick.AddListener(() => GameManager.Instance.EnterState(GameState.Ending));
                }
                else
                {
                    nextButton.onClick.AddListener(RandomDropCards);
                }

                nextButton.gameObject.SetActive(true);
            }
            else
            {
                // A defeated player can restart, but should not continue to rewards or the ending.
                nextButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(true);
            }
        }

        private void OnPlayerDeath()
        {
            resultText.text = "Defeat";
            doesPlayerWin = false;
            Open();
        }

        private void OnEnemyDeath()
        {
            resultText.text = "Victory!";
            doesPlayerWin = true;
            Open();
        }


        private void RandomDropCards()
        {
            nextButton.gameObject.SetActive(false);        
            RewardManager.Instance.SpawnRewardCards();
        }

        public void OnSelectRewardCard()
        {
            continueButton.gameObject.SetActive(true);
            RewardManager.Instance.HideDropCards();
        }

        private void BattleExitButton()
        {
            Close();
            GameManager.Instance.EnterState(GameState.Map);
        }
    }
}
