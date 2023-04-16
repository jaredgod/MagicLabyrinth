using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this instantiates and keeps track of action gameobjects
public class ActionRenderer : MonoBehaviour {
    public GameObject selector;

    public GameObject[] actionObjects;

    public List<GameObject> actionList = new List<GameObject>();

    private CharacterRenderer cr;
    private int selectedCharacter = -1;

    private Vector2Int[] displacement = new Vector2Int[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
    };


    // Start is called before the first frame update
    void Start() {
        selector.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        checkIfClicked();
    }

    public void setCharacterController(CharacterRenderer characterController) {
        cr = characterController;
    }

    public void selectCharacter(int ind) {
        selectedCharacter = ind;
        selector.transform.position = new Vector3(cr.characterList[ind].X, 0, cr.characterList[ind].Y);
        selector.SetActive(true);
        placeActions();
    }
    public void clearSelection() {
        selectedCharacter = -1;
        selector.SetActive(false);
        foreach (GameObject o in actionList.ToArray()) {
            Destroy(o);
        }
    }
    private void placeActions() {
        int x = cr.characterList[selectedCharacter].X;
        int y = cr.characterList[selectedCharacter].Y;

        Dictionary<Vector2Int, ActionType> actions = GameController.getPossibleActions(x, y, cr.characterList[selectedCharacter].C);

        foreach (Vector2Int actionPos in actions.Keys) {

            ActionType actionType = actions[actionPos];
            GameObject actionObject = Instantiate(actionObjects[(int)actionType], transform);

            Action action = actionObject.AddComponent<Action>();
            action.setPos(actionPos.x, actionPos.y);
            action.setAction(actionType);

            actionList.Add(actionObject);
        }
    }

    private void checkIfClicked() {
        if (Input.GetButtonDown("Fire1")) {
            bool clear = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (hit.transform != null) {
                Character character = hit.transform.GetComponent<Character>();
                if (character != null && character.Ind != selectedCharacter) {
                    clearSelection();
                    selectCharacter(character.Ind);
                    clear = false;
                }

                Action action = hit.transform.GetComponent<Action>();
                if (action != null) {
                    if (action.A <= ActionType.moveL) {
                        GameController.sendCharacterMoved(
                            (ushort)selectedCharacter,
                            (short)cr.characterList[selectedCharacter].X,
                            (short)cr.characterList[selectedCharacter].Y,
                            (short)action.X,
                            (short)action.Y);
                    }
                    else if (action.A == ActionType.explore) {
                        GameController.sendExplore((short)cr.characterList[selectedCharacter].X, (short)cr.characterList[selectedCharacter].Y);
                    }
                    else if (action.A == ActionType.teleport) {
                        GameController.sendTeleport((ushort)selectedCharacter, (short)cr.characterList[selectedCharacter].X, (short)cr.characterList[selectedCharacter].Y, (short)action.X, (short)action.Y);
                    }
                    else if (action.A == ActionType.vent) {
                        GameController.sendVent((ushort)selectedCharacter, (short)cr.characterList[selectedCharacter].X, (short)cr.characterList[selectedCharacter].Y, (short)action.X, (short)action.Y);
                    }
                    else if (action.A == ActionType.flipTimer) {
                        GameController.sendTimerFlipped((ushort)selectedCharacter, (short)action.X, (short)action.Y);
                    }
                }
            }
            if (clear) clearSelection();
        }
    }
}
