using RiptideNetworking;
using UnityEngine;

public static class MessageExtensions {
    #region Vector2
    /// <inheritdoc cref="Add(Message, Vector2)"/>
    /// <remarks>Relying on the correct Add overload being chosen based on the parameter type can increase the odds of accidental type mismatches when retrieving data from a message. This method calls <see cref="Add(Message, Vector2)"/> and simply provides an alternative type-explicit way to add a <see cref="Vector2"/> to the message.</remarks>
    public static Message AddVector2(this Message message, Vector2 value) => Add(message, value);

    /// <summary>Adds a <see cref="Vector2"/> to the message.</summary>
    /// <param name="value">The <see cref="Vector2"/> to add.</param>
    /// <returns>The message that the <see cref="Vector2"/> was added to.</returns>
    public static Message Add(this Message message, Vector2 value) {
        message.AddFloat(value.x);
        message.AddFloat(value.y);
        return message;
    }

    /// <summary>Retrieves a <see cref="Vector2"/> from the message.</summary>
    /// <returns>The <see cref="Vector2"/> that was retrieved.</returns>
    public static Vector2 GetVector2(this Message message) {
        return new Vector2(message.GetFloat(), message.GetFloat());
    }
    #endregion

    #region Vector3
    /// <inheritdoc cref="Add(Message, Vector3)"/>
    /// <remarks>Relying on the correct Add overload being chosen based on the parameter type can increase the odds of accidental type mismatches when retrieving data from a message. This method calls <see cref="Add(Message, Vector3)"/> and simply provides an alternative type-explicit way to add a <see cref="Vector3"/> to the message.</remarks>
    public static Message AddVector3(this Message message, Vector3 value) => Add(message, value);

    /// <summary>Adds a <see cref="Vector3"/> to the message.</summary>
    /// <param name="value">The <see cref="Vector3"/> to add.</param>
    /// <returns>The message that the <see cref="Vector3"/> was added to.</returns>
    public static Message Add(this Message message, Vector3 value) {
        message.AddFloat(value.x);
        message.AddFloat(value.y);
        message.AddFloat(value.z);
        return message;
    }

    /// <summary>Retrieves a <see cref="Vector3"/> from the message.</summary>
    /// <returns>The <see cref="Vector3"/> that was retrieved.</returns>
    public static Vector3 GetVector3(this Message message) {
        return new Vector3(message.GetFloat(), message.GetFloat(), message.GetFloat());
    }
    #endregion

    #region Quaternion
    /// <inheritdoc cref="Add(Message, Quaternion)"/>
    /// <remarks>Relying on the correct Add overload being chosen based on the parameter type can increase the odds of accidental type mismatches when retrieving data from a message. This method calls <see cref="Add(Message, Quaternion)"/> and simply provides an alternative type-explicit way to add a <see cref="Quaternion"/> to the message.</remarks>
    public static Message AddQuaternion(this Message message, Quaternion value) => Add(message, value);

    /// <summary>Adds a <see cref="Quaternion"/> to the message.</summary>
    /// <param name="value">The <see cref="Quaternion"/> to add.</param>
    /// <returns>The message that the <see cref="Quaternion"/> was added to.</returns>
    public static Message Add(this Message message, Quaternion value) {
        message.AddFloat(value.x);
        message.AddFloat(value.y);
        message.AddFloat(value.z);
        message.AddFloat(value.w);
        return message;
    }

    /// <summary>Retrieves a <see cref="Quaternion"/> from the message.</summary>
    /// <returns>The <see cref="Quaternion"/> that was retrieved.</returns>
    public static Quaternion GetQuaternion(this Message message) {
        return new Quaternion(message.GetFloat(), message.GetFloat(), message.GetFloat(), message.GetFloat());
    }
    #endregion

    #region Room
    /// <inheritdoc cref="Add(Message, Room)"/>
    /// <remarks>Relying on the correct Add overload being chosen based on the parameter type can increase the odds of accidental type mismatches when retrieving data from a message. This method calls <see cref="Add(Message, Vector2)"/> and simply provides an alternative type-explicit way to add a <see cref="Vector2"/> to the message.</remarks>
    public static Message AddRoom(this Message message, Room value) => Add(message, value);

    /// <summary>Adds a <see cref="Room"/> to the message.</summary>
    /// <param name="value">The <see cref="Room"/> to add.</param>
    /// <returns>The message that the <see cref="Room"/> was added to.</returns>
    public static Message Add(this Message message, Room value) {
        message.AddUShort(value.RoomSize);
        message.AddShort(value.X);
        message.AddShort(value.Y);
        message.AddUShort(value.Rotation);
        message.AddShorts(value.Gates);
        message.AddUShort((ushort)value.Items.Length);
        for (int i = 0; i < value.Items.Length; i++) message.AddShorts(value.Items[i]);
        for (ushort i = 0; i < value.RoomSize; i++) {
            for (ushort j = 0; j < value.RoomSize; j++) {
                message.AddUShorts(new ushort[] { i, j, value.Tiles[i][j] });
            }
        }
        return message;
    }

    /// <summary>Retrieves a <see cref="Room"/> from the message.</summary>
    /// <returns>The <see cref="Room"/> that was retrieved.</returns>
    public static Room GetRoom(this Message message) {
        Room ret = new Room(message.GetUShort(), message.GetShort(), message.GetShort(), message.GetUShort());
        ret.addGates(message.GetShorts());
        int portalCount = message.GetUShort();
        short[][] portals = new short[portalCount][];
        for (int i = 0; i < portalCount; i++) portals[i] = message.GetShorts();
        ret.addItem(portals);
        for (int i = 0; i < ret.RoomSize * ret.RoomSize; i++) {
            ushort[] tile = message.GetUShorts();
            ret.addTile(tile);
        }
        return ret;
    }
    #endregion
}