using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;


public enum GameScene {
    notStarted,
    waiting,
    explore,
    escape,
    end,
}

public class GameController
{
    public static WorldController worldController;

    public static GameScene currentScene = GameScene.notStarted;

    public static Dictionary<Vector2Int, ActionType> getPossibleActions(int x, int y, CharacterClass c)
    {
        return worldController.getPossibleActions(x, y, c);
    }

    public static void setScene(GameScene scene) {
        currentScene = scene;
    }
    public static void disableTimer(int x, int y) {
        worldController.disableTimer(x, y);
    }

    public static void clearGameBoard() {
        worldController.clear();
        currentScene = GameScene.notStarted;
    }

    #region Messages

    public static void sendCharacterMoved(ushort ind, short startX, short startY, short endX, short endY)
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.moveCharacter);

        message.AddUShort(ind);
        message.AddShort(startX);
        message.AddShort(startY);
        message.AddShort(endX);
        message.AddShort(endY);

        NetworkManager.Singleton.Client.Send(message);
    }
    public static void sendExplore(short x, short y)
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.explore);

        message.AddShort(x);
        message.AddShort(y);

        NetworkManager.Singleton.Client.Send(message);
    }
    public static void sendTeleport(ushort ind, short startX, short startY, short endX, short endY) {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.teleport);

        message.AddUShort(ind);
        message.AddShort(startX);
        message.AddShort(startY);
        message.AddShort(endX);
        message.AddShort(endY);

        NetworkManager.Singleton.Client.Send(message);
    }
    public static void sendVent(ushort ind, short startX, short startY, short endX, short endY) {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.vent);

        message.AddUShort(ind);
        message.AddShort(startX);
        message.AddShort(startY);
        message.AddShort(endX);
        message.AddShort(endY);

        NetworkManager.Singleton.Client.Send(message);
    }

    public static void sendTimerFlipped(ushort ind, short x, short y) {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.flipTimer);

        message.AddUShort(ind);
        message.AddShort(x);
        message.AddShort(y);

        NetworkManager.Singleton.Client.Send(message);
    }

    [MessageHandler((ushort)ServerToClientId.roomPlaced)]
    private static void getRoomPlaced(Message message)
    {
        worldController.placeRoom(message.GetRoom());
    }

    [MessageHandler((ushort)ServerToClientId.characterSpawned)]
    private static void getCharacterPlaced(Message message)
    {
        ushort ind = message.GetUShort();
        short x = message.GetShort();
        short y = message.GetShort();
        ushort c = message.GetUShort();

        worldController.spawnCharacter(ind, x, y, (CharacterClass)c);
    }

    [MessageHandler((ushort)ServerToClientId.characterMoved)]
    private static void getCharacterMoved(Message message)
    {
        ushort ind = message.GetUShort();
        short x = message.GetShort();
        short y = message.GetShort();

        worldController.moveCharacter(ind, x, y);
    }

    [MessageHandler((ushort)ServerToClientId.gateRemoved)]
    private static void getGateRemoved(Message message)
    {
        short x = message.GetShort();
        short y = message.GetShort();

        worldController.removeGate(x, y);
    }

    [MessageHandler((ushort)ServerToClientId.tileChanged)]
    private static void getTileChanged(Message message) {
        short x = message.GetShort();
        short y = message.GetShort();
        short t = message.GetShort();

        worldController.changeTile(x, y, t);
    }

    [MessageHandler((ushort)ServerToClientId.playerActions)]
    private static void getPlayerActions(Message message) {
        ushort id = message.GetUShort();
        ushort[] actions = message.GetUShorts();

        if(id == NetworkManager.Singleton.Client.Id) worldController.setPlayerActions(actions);
    }

    #endregion
}
