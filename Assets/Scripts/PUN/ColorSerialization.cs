using System.IO;
using UnityEngine;

public static class ColorSerialization
{
    public static object DeserializeColor(byte[] data)
    {
        Color color = new Color();
        using (MemoryStream stream = new MemoryStream(data))
        using (BinaryReader reader = new BinaryReader(stream))
        {
            color.r = reader.ReadSingle();
            color.g = reader.ReadSingle();
            color.b = reader.ReadSingle();
            color.a = reader.ReadSingle();
        }
        return color;
    }

    public static byte[] SerializeColor(object customType)
    {
        Color color = (Color)customType;
        using (MemoryStream stream = new MemoryStream())
        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            writer.Write(color.r);
            writer.Write(color.g);
            writer.Write(color.b);
            writer.Write(color.a);
            return stream.ToArray();
        }
    }
}
