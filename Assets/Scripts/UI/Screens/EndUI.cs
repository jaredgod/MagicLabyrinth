using TMPro;
using UnityEngine;

public class EndUI : MonoBehaviour {

    [SerializeField] private TMP_Text endMessage;
    [SerializeField] private GameObject endUI;

    public void Start() {
        UIController.endUI = this;
        hide();
    }

    public void show() {
        endUI.SetActive(true);
    }

    public void hide() {
        endUI.SetActive(false);
    }

    public void setTextWon() {
        endMessage.text = "You Won!";
    }

    public void setTextLost() {
        endMessage.text = "Time's Up!";
    }
}
