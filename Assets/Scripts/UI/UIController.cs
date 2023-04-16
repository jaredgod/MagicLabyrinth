using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public struct PlayerInfo {
    public ushort id;
    public string username;
    public bool isLocal;
}

public class UIController
{
    public static ConnectUI connectUI;
    public static LoadingUI loadingUI;
    public static WaitingUI waitingUI;
    public static GameUI gameUI;
    public static EndUI endUI;

    public static Dictionary<ushort, PlayerInfo> players = new Dictionary<ushort, PlayerInfo>();

    #region Connect UI

    private static void hideAll() {
        connectUI.hide();
        loadingUI.hide();
        waitingUI.hide();
        gameUI.hide();
        endUI.hide();
    }

    public static void showConnectUI() {
        hideAll();
        connectUI.show();
    }

    public static void showLoadingUI() {
        hideAll();
        loadingUI.show();
    }

    public static void showWaitingUI(int numPlayers, int minPlayers, int maxPlayers) {
        hideAll();
        waitingUI.show();
        waitingUI.setTextWaiting(numPlayers, maxPlayers, minPlayers);
    }

    public static void showGameUI(int maxTicks) {
        hideAll();
        gameUI.show();
        gameUI.startTimer(maxTicks);
        gameUI.addPlayers(players.Values);
    }

    public static void showEndUI(bool won) {
        hideAll();
        endUI.show();
        if (won) endUI.setTextWon();
        else endUI.setTextLost();
    }

    public static void connect() {
        showLoadingUI();
        sendConnect();
    }
    #endregion

    #region Game

    public static void disconnectPlayer(ushort id) {
        players.Remove(id);
        waitingUI.removePlayer(id);
        gameUI.removePlayerCard(id);
    }

    public static void disconnectAllPlayers() {
        foreach (PlayerInfo p in players.Values) {
            gameUI.removePlayerCard(p.id);
            waitingUI.removePlayer(p.id);
        }
        players.Clear();
    }

    #endregion

    #region Messages

    [MessageHandler((ushort)ServerToClientId.gamestateWaiting)]
    private static void getGamestateWaiting(Message message) {
        showWaitingUI(message.GetInt(), message.GetInt(), message.GetInt());
        GameController.setScene(GameScene.waiting);
        Debug.Log("Waiting");
    }

    [MessageHandler((ushort)ServerToClientId.gamestateExplore)]
    private static void getGamestateExplore(Message message) {
        showGameUI(message.GetInt());
        GameController.setScene(GameScene.explore);
        Debug.Log("Explore");
    }

    [MessageHandler((ushort)ServerToClientId.gamestateEscape)]
    private static void getGamestateEscape(Message message) {
        GameController.setScene(GameScene.escape);
        Debug.Log("Escape");
    }

    [MessageHandler((ushort)ServerToClientId.gamestateEnded)]
    private static void getGamestateEnded(Message message) {
        showEndUI(message.GetBool());
        GameController.setScene(GameScene.end);
        Debug.Log("Ended");
    }

    [MessageHandler((ushort)ServerToClientId.syncClock)]
    private static void getSyncClock(Message message) {
        gameUI.syncClock(message.GetFloat());
    }

    [MessageHandler((ushort)ServerToClientId.timerFlipped)]
    private static void getTimerFlipped(Message message) {
        short x = message.GetShort();
        short y = message.GetShort();
        short t = message.GetShort();

        GameController.disableTimer(x, y);

        gameUI.syncClock(t);
    }

    [MessageHandler((ushort)ServerToClientId.playerConnected)]
    private static void getPlayerConnected(Message message) {
        PlayerInfo p;

        p.id = message.GetUShort();
        p.username = message.GetString();
        p.isLocal = p.id == NetworkManager.Singleton.Client.Id;

        players.Add(p.id, p);
        waitingUI.addPlayer(p.id, p.username, p.isLocal);
    }

    [MessageHandler((ushort)ServerToClientId.playerDisconnected)]
    private static void getPlayerDisconnected(Message message) {
        ushort id = message.GetUShort();

        
    }

    [MessageHandler((ushort)ServerToClientId.pinged)]
    private static void getPinged(Message message) {
        ushort id = message.GetUShort();

        if (id == NetworkManager.Singleton.Client.Id) {
            gameUI.ping();
        }
    }

    public static void SendName() {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.name);
        message.AddString(connectUI.getName());
        NetworkManager.Singleton.Client.Send(message);
    }

    private static void sendConnect() {

        NetworkManager.Singleton.Connect();
    }

    public static void sendStartGame() {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.startGame);

        NetworkManager.Singleton.Client.Send(message);
    }

    public static void sendPing(ushort id) {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.ping);

        message.AddUShort(id);

        NetworkManager.Singleton.Client.Send(message);
    }
    #endregion
}
