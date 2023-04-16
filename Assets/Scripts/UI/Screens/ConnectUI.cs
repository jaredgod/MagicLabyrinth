
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConnectUI : MonoBehaviour
{
    [SerializeField] private GameObject connectUI;
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField IPField;
    [SerializeField] private TMP_InputField PortField;

    public void Start() {
        UIController.connectUI = this;
    }

    public void ConnectClicked() {
        NetworkManager.Singleton.setIp(IPField.text, PortField.text);
        UIController.connect();
    }

    public void show() {
        usernameField.interactable = true;
        connectUI.SetActive(true);
    }

    public void hide() {
        usernameField.interactable = false;
        connectUI.SetActive(false);
    }

    public string getName() {
        return usernameField.text;
    }
}
