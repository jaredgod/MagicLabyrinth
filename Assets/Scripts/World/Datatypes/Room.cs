using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {
    public static ushort DefaultIndSize = 16;
    public static ushort DefaultRoomSize = 4;


    public ushort RoomSize { get; private set; }
    public short X { get; private set; } //northmost index
    public short Y { get; private set; } //westmost index
    public ushort Rotation { get; private set; } //+1 = +90 degrees clockwise


    public short[] Gates { get; private set; } // -1 if no gate, otherwise (short)CharacterClass.X
    public ushort[][] Tiles { get; private set; }
    public short[][] Items { get; private set; }


    public Room(ushort size, short x, short y, ushort rotation) {
        RoomSize = size;
        X = x;
        Y = y;
        Rotation = rotation;

        Tiles = new ushort[size][];
        for (int i = 0; i < size; i++) {
            Tiles[i] = new ushort[size];
        }
    }

    public void addTile(ushort[] tile) {
        if (!inPosBounds(tile[0]) || !inPosBounds(tile[1]) || !inIndBounds(tile[2])) return;

        Tiles[tile[0]][tile[1]] = tile[2];
    }

    public void addTile(ushort x, ushort y, ushort tileIndex) {
        if (!inPosBounds(x) || !inPosBounds(y) || !inIndBounds(tileIndex)) {
            return;
        }
        Tiles[x][y] = tileIndex;
    }

    private bool inPosBounds(ushort i) {
        return i < RoomSize && i >= 0;
    }

    private bool inIndBounds(ushort i) {
        return i < DefaultIndSize && i >= 0;
    }

    public void addGates(short[] gates) {
        Gates = gates;
    }
    public void addItem(short[][] portals) {
        Items = portals;
    }
}
