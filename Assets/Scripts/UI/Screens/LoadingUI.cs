using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingUI : MonoBehaviour {

    public void Start() {
        UIController.loadingUI = this;
        hide();
    }

    public void show() {
        gameObject.SetActive(true);
    }

    public void hide() {
        gameObject.SetActive(false);
    }
}
