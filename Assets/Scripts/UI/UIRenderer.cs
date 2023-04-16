using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRenderer : MonoBehaviour {
    public GameObject oConnectUI;
    public GameObject oLoadingUI;
    public GameObject oWaitingUI;
    public GameObject oGameUI;
    public GameObject oEndUI;

    // Start is called before the first frame update
    void Start() {
        Instantiate(oConnectUI, this.transform);
        Instantiate(oLoadingUI, this.transform);
        Instantiate(oWaitingUI, this.transform);
        Instantiate(oGameUI, this.transform);
        Instantiate(oEndUI, this.transform);
    }
}
