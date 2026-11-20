using UnityEngine;
using CardBattlerCourse.Cards;
using CardBattlerCourse.Systems;

namespace CardBattlerCourse.Systems
{
    public class BattleManager : MonoBehaviour
    {
        private GameManager GameManager => GameManager.Instance;
        private bool isBattleActive = false;
        public bool IsBattleActive() => isBattleActive;

        [SerializeField] private PlayerHand playerHand;
        [SerializeField] private GameObject battleUI;
        [SerializeField] private PlayZoneTrigger playZone;

        //Units for instantiating
        [SerializeField] private GameObject playerCharacter;
        [SerializeField] private GameObject bossCharacter;
        [SerializeField] private GameObject eliteCharacter;

        [SerializeField] private Transform playerPosition;
        [SerializeField] private Transform bossPosition;

        private GameObject currentPlayerCharacter;
        private GameObject currentEnemyCharacter;

        [SerializeField] private GameObject gameBoard;

        private int rewardGold = 10;

        private void OnEnable()
        {
            PlayerEvents.OnPlayerDeath += OnPlayerLose;
            EnemyEvents.OnEnemyDeath += OnPlayerWin;
        }

        private void OnDisable()
        {
            PlayerEvents.OnPlayerDeath -= OnPlayerLose;
            EnemyEvents.OnEnemyDeath -= OnPlayerWin;
            OnExitBattleState();
        }

        private void OnPlayerWin()
        {
            PlayerData.Instance.AddGold(rewardGold);
            PlayerData.Instance.Save();
            OnBattleEnd();
        }

        private void OnPlayerLose()
        {
            OnBattleEnd();
        }

        private void OnBattleEnd()
        {
            isBattleActive = false;
            gameBoard.SetActive(false);
        }

        public void SetupBattle(GameState enemyType)
        {
            isBattleActive = true;

            gameBoard.SetActive(true);
            playerHand.Initialize();
            battleUI.SetActive(true);
            playZone.EnablePlayZone();
            TurnSystem.Instance.Initialize();

            InstantiateCharacters(enemyType);
        }

        private void InstantiateCharacters(GameState enemyType)
        {
            //Instantiate player & boss character on their position
            currentPlayerCharacter = Instantiate(playerCharacter, playerPosition.position, Quaternion.identity);

            switch (enemyType)
            {
                case GameState.EliteBattle:
                    currentEnemyCharacter = Instantiate(eliteCharacter, bossPosition.position, Quaternion.identity);
                    break;
                case GameState.BossBattle:
                    currentEnemyCharacter = Instantiate(bossCharacter, bossPosition.position, Quaternion.identity);
                    break;
            }

        }

        private void OnExitBattleState()
        {
            battleUI.SetActive(false);

            Destroy(currentPlayerCharacter.gameObject);
            Destroy(currentEnemyCharacter.gameObject);

            playerHand.Dispose();
        }



    }

}
