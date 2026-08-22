using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip playCardSFX;
    [SerializeField] private AudioClip drawSFX;
    [SerializeField] private AudioClip playerDeathSFX;
    [SerializeField] private AudioClip bossDeathSFX;
    [SerializeField] private AudioClip healSFX;
    [SerializeField] private AudioClip shuffleSFX;
    [SerializeField] private AudioClip playerHitSFX;
    [SerializeField] private AudioClip bossHitSFX;

    private void OnEnable()
    {
        PlayerEvents.OnCardPlayed += PlayerEvents_OnCardPlayed;
        PlayerEvents.OnDrawCardRequested += PlayerEvents_DrawCardRequested;
        PlayerEvents.OnPlayerDeath += PlayerEvents_PlayerDeath;        
        PlayerEvents.OnPlayerHit += PlayerEvents_PlayerHit;        
        PlayerEvents.OnReshuffleRequested += PlayerEvents_ReshuffleRequested;
        PlayerEvents.OnPlayerHealed += PlayerEvents_PlayerHealed;

        BossEvents.OnBossDeath += BossEvents_BossDeath;
        BossEvents.OnBossHit += BossEvents_BossHit;
    }

    private void OnDisable()
    {
        PlayerEvents.OnCardPlayed -= PlayerEvents_OnCardPlayed;
        PlayerEvents.OnDrawCardRequested -= PlayerEvents_DrawCardRequested;
        PlayerEvents.OnPlayerDeath -= PlayerEvents_PlayerDeath;
        PlayerEvents.OnPlayerHit -= PlayerEvents_PlayerHit;
        PlayerEvents.OnReshuffleRequested -= PlayerEvents_ReshuffleRequested;
        PlayerEvents.OnPlayerHealed -= PlayerEvents_PlayerHealed;

        BossEvents.OnBossDeath -= BossEvents_BossDeath;
        BossEvents.OnBossHit -= BossEvents_BossHit;
    }

    private void PlayerEvents_PlayerHealed()
    {
        PlaySFX(healSFX);
    }

    private void PlayerEvents_OnCardPlayed(CardData _)
    {
        PlaySFX(playCardSFX);
    }

    private void PlayerEvents_DrawCardRequested()
    {
        PlaySFX(drawSFX);
    }

    private void PlayerEvents_PlayerDeath()
    {
        PlaySFX(playerDeathSFX);
    }

    private void PlayerEvents_PlayerHit(int _)
    {
        PlaySFX(playerHitSFX);
    }

    private void PlayerEvents_ReshuffleRequested(List<CardData> _)
    {
        PlaySFX(shuffleSFX);
    }

    private void BossEvents_BossDeath()
    {
        PlaySFX(bossDeathSFX);
    }

    private void BossEvents_BossHit(int _)
    {
        PlaySFX(bossHitSFX);
    }

    private void PlaySFX(AudioClip audioClip)
    {
        if (audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}
