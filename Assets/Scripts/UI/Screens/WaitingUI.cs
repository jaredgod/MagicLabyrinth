using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitingUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerCountText;
    [SerializeField] private GameObject waitingPlayerObj;
    [SerializeField] private GameObject listObj;

    private Dictionary<ushort, GameObject> playerObj = new Dictionary<ushort, GameObject>();
    private int maxNumPlayers;
    private int minNumPlayers;


    public void Start() {
        UIController.waitingUI = this;
        hide();
    }

    public void show() {
        gameObject.SetActive(true);
    }

    public void hide() {
        gameObject.SetActive(false);
    }

    public void setTextWaiting(int numPlayers, int maxPlayers, int minPlayers) {
        maxNumPlayers = maxPlayers;
        minNumPlayers = minPlayers;
        playerCountText.text = $"Waiting for Players to Join: {numPlayers} / {minPlayers} - {maxPlayers}";
    }

    public void setTextWaiting(int numPlayers) {
        playerCountText.text = $"Waiting for Players to Join: {numPlayers} / {minNumPlayers} - {maxNumPlayers}";
    }

    public void addPlayer(ushort id, string username, bool isLocal) {
        if (!playerObj.ContainsKey(id)) {
            GameObject player = Instantiate(waitingPlayerObj, listObj.transform);
            player.GetComponent<TMP_Text>().text = username;
            playerObj.Add(id, player);
        }
        setTextWaiting(UIController.players.Count);
    }

    public void removePlayer(ushort id) {
        if (playerObj.ContainsKey(id)) {
            GameObject temp = playerObj[id];
            playerObj.Remove(id);
            Destroy(temp);
        }
        setTextWaiting(UIController.players.Count);
    }

    public void startButtonClicked() {
        UIController.sendStartGame();
    }
}
