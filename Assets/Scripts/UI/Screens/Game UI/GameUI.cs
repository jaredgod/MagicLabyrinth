using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject gameUI;
    [SerializeField] private TMP_Text timerDisplay;
    [SerializeField] private GameObject playerList;
    [SerializeField] private GameObject playerCard;
    [SerializeField] private GameObject pingTint;
    [SerializeField] private Color playerBackground;
    [SerializeField] private Color selfBackground;
    [SerializeField] private Color pingBackground;

    Dictionary<int, PlayerCard> players = new Dictionary<int, PlayerCard>();


    public void Start() {
        UIController.gameUI = this;
        hide();
        setTimer((int)timerTick);
        pingTint.SetActive(false);
    }

    public void FixedUpdate() {
        timerUpdate();
        pingUpdate();
    }

    public void show() {
        gameUI.SetActive(true);
    }

    public void hide() {
        gameUI.SetActive(false);
    }

    #region Timer

    public float timerTick;
    private bool timePaused = true;
    private float timeFlowScalar = 1;
    const int tps = 60;

    private void timerUpdate() {
        if (timePaused) return;
        incrementTimer();
        setTimer((int)timerTick);
    }

    public void incrementTimer() {
        timerTick -= timeFlowScalar;
    }

    public void pauseTimer() {
        timePaused = true;
    }

    public void startTimer(int maxTicks) {
        timerTick = maxTicks;

        timePaused = false;
    }

    public void setTimer(int timerTick) {
        int timerSecond = (timerTick / tps) % 60;
        int timerMinute = (timerTick / tps) / 60;

        string secondString = (timerSecond < 10 ? "0" : "") + timerSecond;
        string minuteString = (timerMinute < 10 ? "0" : "") + timerMinute;

        timerDisplay.text = $"{minuteString}:{secondString}"; //TODO: pad with 0's better?
    }

    public void syncClock(float tick) {
        timerTick = tick;
    }
    #endregion

    #region Player List

    public void addPlayers(Dictionary<ushort, PlayerInfo>.ValueCollection players) {
        foreach (PlayerInfo player in players) {
            GameObject playerCard = Instantiate(this.playerCard, playerList.transform);
            PlayerCard pc = playerCard.GetComponent<PlayerCard>();

            if (player.isLocal) pc.init(player.username, selfBackground, pingBackground, player.id);
            else pc.init(player.username, playerBackground, pingBackground, player.id);

            this.players.Add(player.id, pc);
        }
    }

    public void removePlayerCard(ushort id) {
        if (players.ContainsKey(id)) {
            PlayerCard p = players[id];
            players.Remove(id);
            Destroy(p.gameObject);
        }
    }

    #endregion

    #region Ping
    private static float maxPingTimer = 1f;
    private float pingTimer = 0;
    public void ping() {
        pingTint.SetActive(true);
        pingTimer = maxPingTimer;
    }

    private void pingUpdate() {
        if (pingTimer > 0) {
            pingTimer -= Time.deltaTime;
            if (pingTimer <= 0) {
                pingTint.SetActive(false);
                pingTimer = 0;
            }
        }
    }
    #endregion
}
