using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class BattleManager : MonoBehaviour
{
    private GameManager GameManager => GameManager.Instance;
    private bool isBattleActive = false;
    public bool IsBattleActive() => isBattleActive;

    [SerializeField] private PlayerHand playerHand;
    [SerializeField] private GameObject battleCanvas;
    [SerializeField] private PlayZoneTrigger playZone;

    //Units for instantiating
    [SerializeField] private GameObject playerCharacter;
    [SerializeField] private GameObject bossCharacter;
    [SerializeField] private GameObject eliteCharacter;

    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform bossPosition;

    private GameObject currentPlayerCharacter;
    private GameObject currentEnemyCharacter;

    private void OnEnable()
    {
        PlayerEvents.OnPlayerDeath += OnBattleEnd;
        BossEvents.OnBossDeath += OnBattleEnd;

        
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerDeath -= OnBattleEnd;
        BossEvents.OnBossDeath -= OnBattleEnd;
        OnExitBattleState();
    }

    public void SetupBattle(GameState enemyType)
    {
        isBattleActive = true;

        playerHand.Initialize();
        battleCanvas.SetActive(true);
        playZone.EnablePlayZone();
        TurnSystem.Instance.Initialize();

        InstantiateCharacters(enemyType);

        print(enemyType);
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
        battleCanvas.SetActive(false);

        Destroy(currentPlayerCharacter.gameObject);
        Destroy(currentEnemyCharacter.gameObject);

        playerHand.Dispose();
    }

    private void OnBattleEnd()
    {
        isBattleActive = false;
    }



}
