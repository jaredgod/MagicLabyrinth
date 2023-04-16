using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GateData
{
    public int rotation;
    public CharacterClass characterClass;
    public GameObject gameObject;
}

public struct CharacterPortalData {
    public int x;
    public int y;
    public CharacterClass characterClass;
    public GameObject gameObject;
}
public struct CharacterItemData {
    public int x;
    public int y;
    public CharacterClass characterClass;
    public GameObject gameObject;
}
public struct CharacterExitData {
    public int x;
    public int y;
    public CharacterClass characterClass;
    public GameObject gameObject;
}
public struct LinkedPortalData {
    public int x;
    public int y;
    public int lx;
    public int ly;
    public GameObject gameObject;
}

public struct TimerData {
    public int x;
    public int y;
    public bool active;
    public GameObject gameObject;
}

public class ItemRenderer : MonoBehaviour
{
    public GameObject alchemistGate;
    public GameObject archerGate;
    public GameObject swordsmanGate;
    public GameObject warriorGate;

    public GameObject[] ventObject; 
    public GameObject[] portalObject;
    public GameObject[] itemObject;
    public GameObject[] exitObject;

    public GameObject timerObject;

    public Dictionary<Vector2Int, GateData> gate;
    public Dictionary<Vector2Int, CharacterPortalData> alchemistPortal;
    public Dictionary<Vector2Int, CharacterPortalData> archerPortal;
    public Dictionary<Vector2Int, CharacterPortalData> swordsmanPortal;
    public Dictionary<Vector2Int, CharacterPortalData> warriorPortal;
    public Dictionary<Vector2Int, CharacterItemData> alchemistItem;
    public Dictionary<Vector2Int, CharacterItemData> archerItem;
    public Dictionary<Vector2Int, CharacterItemData> swordsmanItem;
    public Dictionary<Vector2Int, CharacterItemData> warriorItem;
    public Dictionary<Vector2Int, CharacterExitData> alchemistExit;
    public Dictionary<Vector2Int, CharacterExitData> archerExit;
    public Dictionary<Vector2Int, CharacterExitData> swordsmanExit;
    public Dictionary<Vector2Int, CharacterExitData> warriorExit;
    public Dictionary<Vector2Int, LinkedPortalData> lportal;
    public Dictionary<Vector2Int, TimerData> timer;

    void Start() {
        gate = new Dictionary<Vector2Int, GateData>();
        alchemistPortal = new Dictionary<Vector2Int, CharacterPortalData>();
        archerPortal = new Dictionary<Vector2Int, CharacterPortalData>();
        swordsmanPortal = new Dictionary<Vector2Int, CharacterPortalData>();
        warriorPortal = new Dictionary<Vector2Int, CharacterPortalData>();
        alchemistItem = new Dictionary<Vector2Int, CharacterItemData>();
        archerItem = new Dictionary<Vector2Int, CharacterItemData>();
        swordsmanItem = new Dictionary<Vector2Int, CharacterItemData>();
        warriorItem = new Dictionary<Vector2Int, CharacterItemData>();
        alchemistExit = new Dictionary<Vector2Int, CharacterExitData>();
        archerExit = new Dictionary<Vector2Int, CharacterExitData>();
        swordsmanExit = new Dictionary<Vector2Int, CharacterExitData>();
        warriorExit = new Dictionary<Vector2Int, CharacterExitData>();
        lportal = new Dictionary<Vector2Int, LinkedPortalData>();
        timer = new Dictionary<Vector2Int, TimerData>();
    }

    public void placeRoom(Room r) {

        placeGates(r);
        placePortals(r.Items, r.X, r.Y, r.Rotation);
    }

    public bool canMove(Direction direction, int x, int y) {
        bool ret = true;
        Vector2Int pos = new Vector2Int(x, y);

        if (ret && gate.ContainsKey(pos) && gate[pos].rotation == (int)direction) ret = false;

        return ret;
    }


    private void placeGates(Room r) {
        int i = (4 - r.Rotation) % 4;

        if (r.Gates.Length > i && r.Gates[i] >= 0) {
            placeGate(r.X, r.Y + 2, 3, (CharacterClass)r.Gates[i]);
        }

        i = (i + 1) % 4;
        if (r.Gates.Length > i && r.Gates[i] >= 0) {
            placeGate(r.X + 2, r.Y + 3, 0, (CharacterClass)r.Gates[i]);
        }

        i = (i + 1) % 4;
        if (r.Gates.Length > i && r.Gates[i] >= 0) {
            placeGate(r.X + 3, r.Y + 1, 1, (CharacterClass)r.Gates[i]);
        }

        i = (i + 1) % 4;
        if (r.Gates.Length > i && r.Gates[i] >= 0) {
            placeGate(r.X + 1, r.Y, 2, (CharacterClass)r.Gates[i]);
        }
    }

    private void placeGate(int x, int y, int r, CharacterClass c) {

        GameObject instObj = null;
        if (c == CharacterClass.Alchemist) instObj = Instantiate(alchemistGate, transform);
        else if (c == CharacterClass.Archer) instObj = Instantiate(archerGate, transform);
        else if (c == CharacterClass.Swordsman) instObj = Instantiate(swordsmanGate, transform);
        else if (c == CharacterClass.Warrior) instObj = Instantiate(warriorGate, transform);
        else return;

        instObj.transform.position = new Vector3(x, 0, y);
        instObj.transform.rotation = Quaternion.Euler(0, r * 90, 0);

        GateData data;
        data.rotation = r;
        data.characterClass = c;
        data.gameObject = instObj;

        gate.Add(new Vector2Int(x, y), data);
    }

    public void removeGate(int x, int y) {
        Vector2Int pos = new Vector2Int(x, y);
        GateData data = gate[pos];
        gate.Remove(pos);

        Destroy(data.gameObject);
    }

    private (int, int) rotate(int x, int y, int rotation) {
        if (rotation == 0) return (x, y);
        if (rotation == 1) return (y, 3 - x);
        if (rotation == 2) return (3 - x, 3 - y);
        if (rotation == 3) return (3 - y, x);
        return (0, 0);
    }

    private enum ItemType : short {
        SecondBridge = -2,
        FirstBridge = -1,
        AlchemistPortal,
        ArcherPortal,
        SwordsmanPortal,
        WarriorPortal,
        AlchemistItem,
        ArcherItem,
        SwordsmanItem,
        WarriorItem,
        AlchemistExit,
        ArcherExit,
        SwordsmanExit,
        WarriorExit,
        Timer,
    }

    private void placePortals(short[][] portals, int rx, int ry, int rr) {
        bool lpd1Set = false;
        bool lpd2Set = false;

        LinkedPortalData lpd1;
        lpd1.x = 0;
        lpd1.y = 0;
        LinkedPortalData lpd2;
        lpd2.x = 0;
        lpd2.y = 0;

        foreach (short[] itemData in portals) {
            (int x, int y) = rotate(itemData[0], itemData[1], rr);
            x += rx;
            y += ry;

            if ((ItemType)itemData[2] == ItemType.FirstBridge) {
                if(!lpd1Set) {
                    lpd1.x = x;
                    lpd1.y = y;
                    lpd1Set = true;
                }
                else {
                    lpd1.lx = x;
                    lpd1.ly = y;

                    LinkedPortalData temp;
                    temp.x = x;
                    temp.y = y;
                    temp.lx = lpd1.x;
                    temp.ly = lpd1.y;

                    lpd1.gameObject = Instantiate(ventObject[1], transform);
                    lpd1.gameObject.transform.position = new Vector3(lpd1.x, 0, lpd1.y);
                    temp.gameObject = Instantiate(ventObject[1], transform);
                    temp.gameObject.transform.position = new Vector3(temp.x, 0, temp.y);

                    lportal.Add(new Vector2Int(lpd1.x, lpd1.y), lpd1);
                    lportal.Add(new Vector2Int(temp.x, temp.y), temp);
                }
            }
            else if ((ItemType)itemData[2] == ItemType.SecondBridge) {
                if (!lpd2Set) {
                    lpd2.x = x;
                    lpd2.y = y;
                    lpd2Set = true;
                }
                else {
                    lpd2.lx = x;
                    lpd2.ly = y;

                    LinkedPortalData temp;
                    temp.x = x;
                    temp.y = y;
                    temp.lx = lpd2.x;
                    temp.ly = lpd2.y;

                    lpd2.gameObject = ventObject[0];
                    temp.gameObject = ventObject[0];


                    lpd2.gameObject = Instantiate(ventObject[0], transform);
                    lpd2.gameObject.transform.position = new Vector3(lpd2.x, 0, lpd2.y);
                    temp.gameObject = Instantiate(ventObject[0], transform);
                    temp.gameObject.transform.position = new Vector3(temp.x, 0, temp.y);

                    lportal.Add(new Vector2Int(lpd2.x, lpd2.y), lpd2);
                    lportal.Add(new Vector2Int(temp.x, temp.y), temp);
                }
            }
            else if ((ItemType)itemData[2] <= ItemType.WarriorPortal) {
                CharacterPortalData p;
                p.x = x;
                p.y = y;
                p.characterClass = (CharacterClass)(itemData[2] - ItemType.AlchemistPortal);

                p.gameObject = Instantiate(portalObject[(int)p.characterClass], transform);
                p.gameObject.transform.position = new Vector3(p.x, 0, p.y);

                if (p.characterClass == CharacterClass.Alchemist) alchemistPortal.Add(new Vector2Int(p.x, p.y), p);
                if (p.characterClass == CharacterClass.Archer) archerPortal.Add(new Vector2Int(p.x, p.y), p);
                if (p.characterClass == CharacterClass.Swordsman) swordsmanPortal.Add(new Vector2Int(p.x, p.y), p);
                if (p.characterClass == CharacterClass.Warrior) warriorPortal.Add(new Vector2Int(p.x, p.y), p);
            }
            else if ((ItemType)itemData[2] <= ItemType.WarriorItem) {
                CharacterItemData p;
                p.x = x;
                p.y = y;
                p.characterClass = (CharacterClass)(itemData[2] - ItemType.AlchemistItem);

                Debug.Log(p.characterClass);

                p.gameObject = Instantiate(itemObject[(int)p.characterClass], transform);
                p.gameObject.transform.position = new Vector3(p.x, 0, p.y);

                if (p.characterClass == CharacterClass.Alchemist) alchemistItem.Add(new Vector2Int(p.x, p.y), p);
                if (p.characterClass == CharacterClass.Archer) archerItem.Add(new Vector2Int(p.x, p.y), p);
                if (p.characterClass == CharacterClass.Swordsman) swordsmanItem.Add(new Vector2Int(p.x, p.y), p);
                if (p.characterClass == CharacterClass.Warrior) warriorItem.Add(new Vector2Int(p.x, p.y), p);
            }
            else if ((ItemType)itemData[2] <= ItemType.WarriorExit) {
                CharacterExitData p;
                p.x = x;
                p.y = y;
                p.characterClass = (CharacterClass)(itemData[2] - ItemType.AlchemistExit);

                p.gameObject = Instantiate(exitObject[(int)p.characterClass], transform);
                p.gameObject.transform.position = new Vector3(p.x, 0, p.y);

                if (p.characterClass == CharacterClass.Alchemist) alchemistExit.Add(new Vector2Int(p.x, p.y), p);
                if (p.characterClass == CharacterClass.Archer) archerExit.Add(new Vector2Int(p.x, p.y), p);
                if (p.characterClass == CharacterClass.Swordsman) swordsmanExit.Add(new Vector2Int(p.x, p.y), p);
                if (p.characterClass == CharacterClass.Warrior) warriorExit.Add(new Vector2Int(p.x, p.y), p);
            }
            else if ((ItemType)itemData[2] == ItemType.Timer) {
                TimerData p;
                p.x = x;
                p.y = y;
                p.active = true;

                p.gameObject = Instantiate(timerObject, transform);
                p.gameObject.transform.position = new Vector3(p.x, 0, p.y);

                timer.Add(new Vector2Int(p.x, p.y), p);
            }
        }
    }

    public void clear() {
        foreach (GateData d in gate.Values) {
            Destroy(d.gameObject);
        }
        gate.Clear();

        //portal
        foreach (CharacterPortalData d in alchemistPortal.Values) {
            Destroy(d.gameObject);
        }
        alchemistPortal.Clear();
        foreach (CharacterPortalData d in archerPortal.Values) {
            Destroy(d.gameObject);
        }
        archerPortal.Clear();
        foreach (CharacterPortalData d in swordsmanPortal.Values) {
            Destroy(d.gameObject);
        }
        swordsmanPortal.Clear();
        foreach (CharacterPortalData d in warriorPortal.Values) {
            Destroy(d.gameObject);
        }
        warriorPortal.Clear();

        //item
        foreach (CharacterItemData d in alchemistItem.Values) {
            Destroy(d.gameObject);
        }
        alchemistItem.Clear();
        foreach (CharacterItemData d in archerItem.Values) {
            Destroy(d.gameObject);
        }
        archerItem.Clear();
        foreach (CharacterItemData d in swordsmanItem.Values) {
            Destroy(d.gameObject);
        }
        swordsmanItem.Clear();
        foreach (CharacterItemData d in warriorItem.Values) {
            Destroy(d.gameObject);
        }
        warriorItem.Clear();

        //exit
        foreach (CharacterExitData d in alchemistExit.Values) {
            Destroy(d.gameObject);
        }
        alchemistExit.Clear();
        foreach (CharacterExitData d in archerExit.Values) {
            Destroy(d.gameObject);
        }
        archerExit.Clear();
        foreach (CharacterExitData d in swordsmanExit.Values) {
            Destroy(d.gameObject);
        }
        swordsmanExit.Clear();
        foreach (CharacterExitData d in warriorExit.Values) {
            Destroy(d.gameObject);
        }
        warriorExit.Clear();


        foreach (LinkedPortalData d in lportal.Values) {
            Destroy(d.gameObject);
        }
        lportal.Clear();
        foreach (TimerData d in timer.Values) {
            Destroy(d.gameObject);
        }
        timer.Clear();
    }
}