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
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform bossPosition;

    private GameObject currentPlayerCharacter;
    private GameObject currentEnemyCharacter;

    private void OnEnable()
    {
        PlayerEvents.OnPlayerDeath += OnBattleEnd;
        BossEvents.OnBossDeath += OnBattleEnd;

        SetupBattle();
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerDeath -= OnBattleEnd;
        BossEvents.OnBossDeath -= OnBattleEnd;
        OnExitBattleState();
    }

    private void SetupBattle()
    {
        isBattleActive = true;

        playerHand.Initialize();
        battleCanvas.SetActive(true);
        playZone.EnablePlayZone();
        TurnSystem.Instance.Initialize();

        //Instantiate player & boss character on their position
        currentPlayerCharacter = Instantiate(playerCharacter, playerPosition.position, Quaternion.identity);
        currentEnemyCharacter = Instantiate(bossCharacter, bossPosition.position, Quaternion.identity);
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
