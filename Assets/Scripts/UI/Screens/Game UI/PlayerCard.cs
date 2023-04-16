using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerCard : MonoBehaviour, IPointerClickHandler {
    [SerializeField] private GameObject cardBackground;
    [SerializeField] private TMP_Text usernameText;
    
    private Color Background;
    private Color Ping;

    private ushort Id;
    private static float maxPingTimer = 1f;
    private float pingTimer = 0;

    public void init(string username, Color background, Color ping, ushort id) {
        Background = background;
        Ping = ping;

        cardBackground.GetComponent<Image>().color = Background;

        usernameText.text = username;

        Id = id;
    }

    public void ping() {
        cardBackground.GetComponent<Image>().color = Ping;
        pingTimer = maxPingTimer;
    }

    private void Update() {
        if (pingTimer > 0) {
            pingTimer -= Time.deltaTime;
            if(pingTimer <= 0) {
                cardBackground.GetComponent<Image>().color = Background;
                pingTimer = 0;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ping();
        UIController.sendPing(Id);
    }
}
