using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterClass
{
    Alchemist,
    Archer,
    Swordsman,
    Warrior
}

public class CharacterRenderer : MonoBehaviour
{
    public GameObject alchemist;
    public GameObject archer;
    public GameObject swordsman;
    public GameObject warrior;

    public List<Character> characterList = new List<Character>();
    public Dictionary<Vector2Int, Character> characterPos = new Dictionary<Vector2Int, Character>();


    public GameObject oActionRenderer;
    private ActionRenderer actionRenderer;


    // Start is called before the first frame update
    void Start()
    {
        actionRenderer = oActionRenderer.GetComponent<ActionRenderer>();
        actionRenderer.setCharacterController(this);
    }

    public void spawnCharacter(int ind, int x, int y, CharacterClass c)
    {
        if (characterList.Count != ind) return;

        GameObject instObj = null;
        if (c == CharacterClass.Alchemist) instObj = alchemist;
        else if (c == CharacterClass.Archer) instObj = archer;
        else if (c == CharacterClass.Swordsman) instObj = swordsman;
        else if (c == CharacterClass.Warrior) instObj = warrior;
        else return;

        GameObject temp = Instantiate(instObj, transform);
        Character character = temp.AddComponent<Character>();
        character.setInd(ind);
        character.setClass(c);
        character.setPos(x, y);
        characterList.Add(character);
        characterPos.Add(new Vector2Int(x, y), character);
    }

    public void moveCharacter(int ind, int x, int y)
    {
        Character c = characterList[ind];
        int oldX = c.X;
        int oldY = c.Y;

        characterPos.Remove(new Vector2Int(oldX, oldY));
        c.setPos(x, y);
        characterPos.Add(new Vector2Int(x, y), c);
    }

    public bool canMove(Direction dir, int x, int y)
    {
        if (dir == Direction.U) return !characterPos.ContainsKey(new Vector2Int(x, y + 1));
        if (dir == Direction.R) return !characterPos.ContainsKey(new Vector2Int(x + 1, y));
        if (dir == Direction.D) return !characterPos.ContainsKey(new Vector2Int(x, y - 1));
        if (dir == Direction.L) return !characterPos.ContainsKey(new Vector2Int(x - 1, y));

        return false;
    }
    
    public void clear() {
        foreach(Character c in characterList) {
            Destroy(c.gameObject);
        }
        characterList.Clear();
        characterPos.Clear();
        actionRenderer.clearSelection();
    }    
}
