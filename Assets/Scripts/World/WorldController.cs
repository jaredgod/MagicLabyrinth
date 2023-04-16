using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject oWallRenderer;
    public GameObject oItemRenderer;
    public GameObject oCharacterRenderer;

    private WallRenderer wallRenderer;
    private ItemRenderer itemRenderer;
    private CharacterRenderer characterRenderer;

    List<ActionType> playerActions;

    // Start is called before the first frame update
    void Start()
    {
        GameController.worldController = this;
        wallRenderer = oWallRenderer.GetComponent<WallRenderer>();
        itemRenderer = oItemRenderer.GetComponent<ItemRenderer>();
        characterRenderer = oCharacterRenderer.GetComponent<CharacterRenderer>();
    }

    public void placeRoom(Room room)
    {
        wallRenderer.placeRoom(room);
        itemRenderer.placeRoom(room);
    }

    public void removeGate(int x, int y)
    {
        itemRenderer.removeGate(x, y);
    }
    public void spawnCharacter(int ind, int x, int y, CharacterClass c)
    {
        characterRenderer.spawnCharacter(ind, x, y, c);
    }

    public void moveCharacter(int ind, int x, int y)
    {
        characterRenderer.moveCharacter(ind, x, y);
    }

    public void changeTile(int x, int y, int t)
    {
        wallRenderer.changeTile(x, y, t);
    }

    public void disableTimer(int x, int y) {
        Vector2Int pos = new Vector2Int(x, y);
        if (itemRenderer.timer.ContainsKey(pos)) {
            TimerData t = itemRenderer.timer[pos];
            t.active = false;
            itemRenderer.timer[pos] = t;
        }
    }

    public void setPlayerActions(ushort[] actions) {
        List<ActionType> a = new List<ActionType>();

        foreach(ushort action in actions) {
            a.Add((ActionType)action);
        }
        playerActions = a;
    }

    public Dictionary<Vector2Int, ActionType> getPossibleActions(int x, int y, CharacterClass c)
    {
        Dictionary<Vector2Int, ActionType> action = new Dictionary<Vector2Int, ActionType>();
        if (GameController.currentScene != GameScene.explore && GameController.currentScene != GameScene.escape) return action;

        Vector2Int pos = new Vector2Int(x, y);
        Vector2Int[] d = new Vector2Int[] {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
        };


        // Add possible explore
        if(playerActions.Contains(ActionType.explore) && itemRenderer.gate.ContainsKey(pos) && itemRenderer.gate[pos].characterClass == c)
        {
            action.Add(pos + d[itemRenderer.gate[pos].rotation], ActionType.explore);
        }

        // Add possible movements
        int i = 0;
        while (playerActions.Contains(ActionType.moveU) && wallRenderer.canMove(Direction.U, x, y + i) && itemRenderer.canMove(Direction.U, x, y + i) && characterRenderer.canMove(Direction.U, x, y + i)) {
            i++;
            action.Add(pos + i * d[0], ActionType.moveU);
        }

        i = 0;
        while (playerActions.Contains(ActionType.moveR) && wallRenderer.canMove(Direction.R, x + i, y) && itemRenderer.canMove(Direction.R, x + i, y) && characterRenderer.canMove(Direction.R, x + i, y)) {
            i++;
            action.Add(pos + i * d[1], ActionType.moveR);
        }

        i = 0;
        while (playerActions.Contains(ActionType.moveD) && wallRenderer.canMove(Direction.D, x, y - i) && itemRenderer.canMove(Direction.D, x, y - i) && characterRenderer.canMove(Direction.D, x, y - i)) {
            i++;
            action.Add(pos + i * d[2], ActionType.moveD);
        }

        i = 0;
        while (playerActions.Contains(ActionType.moveL) && wallRenderer.canMove(Direction.L, x - i, y) && itemRenderer.canMove(Direction.L, x - i, y) && characterRenderer.canMove(Direction.L, x - i, y)) {
            i++;
            action.Add(pos + i * d[3], ActionType.moveL);
        }
        
        // Add possible teleports
        if(playerActions.Contains(ActionType.teleport) && GameController.currentScene == GameScene.explore) {
            if (c == CharacterClass.Alchemist && itemRenderer.alchemistPortal.ContainsKey(pos)) {
                foreach (Vector2Int portalPos in itemRenderer.alchemistPortal.Keys) {
                    if (portalPos != pos && !characterRenderer.characterPos.ContainsKey(portalPos)) action.Add(portalPos, ActionType.teleport);
                }
            }
            else if (c == CharacterClass.Archer && itemRenderer.archerPortal.ContainsKey(pos)) {
                foreach (Vector2Int portalPos in itemRenderer.archerPortal.Keys) {
                    if (portalPos != pos && !characterRenderer.characterPos.ContainsKey(portalPos)) action.Add(portalPos, ActionType.teleport);
                }
            }
            else if (c == CharacterClass.Swordsman && itemRenderer.swordsmanPortal.ContainsKey(pos)) {
                foreach (Vector2Int portalPos in itemRenderer.swordsmanPortal.Keys) {
                    if (portalPos != pos && !characterRenderer.characterPos.ContainsKey(portalPos)) action.Add(portalPos, ActionType.teleport);
                }
            }
            else if (c == CharacterClass.Warrior && itemRenderer.warriorPortal.ContainsKey(pos)) {
                foreach (Vector2Int portalPos in itemRenderer.warriorPortal.Keys) {
                    if (portalPos != pos && !characterRenderer.characterPos.ContainsKey(portalPos)) action.Add(portalPos, ActionType.teleport);
                }
            }
        }
        if (playerActions.Contains(ActionType.vent) && itemRenderer.lportal.ContainsKey(pos)) {
            Vector2Int linkedPos = new Vector2Int(itemRenderer.lportal[pos].lx, itemRenderer.lportal[pos].ly);
            if (!characterRenderer.characterPos.ContainsKey(linkedPos)) action.Add(linkedPos, ActionType.vent);
        }

        // Add possible timer
        if (itemRenderer.timer.ContainsKey(pos)) {
            TimerData t = itemRenderer.timer[pos];
            if (t.active) {
                action.Add(pos, ActionType.flipTimer);
            }
        }

        return action;
    }

    public void clear() {
        characterRenderer.clear();
        itemRenderer.clear();
        wallRenderer.clear();
    }
}
