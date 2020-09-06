using UnityEngine;
using System.Collections.Generic;

public class TileModuleUtility
{
    public const float MaxSize = 800;
    public const float Spacing = 20;
    public const float Offset = (MaxSize - Spacing * 5) / 4;

    private static Dictionary<int, string> _cachedNumberString =
        new Dictionary<int, string>();

    public static Vector2 GetPosition(int index)
    {
        int x = index % 4;
        int y = index / 4;
        return new Vector2()
        {
            x = Spacing * (x + 1) + Offset * x,
            y = Spacing * (y + 1) + Offset * y
        };
    }

    public static string GetCachedNumberString(int value)
    {
        string result;
        if(!_cachedNumberString.TryGetValue(value, out result))
        {
            result = value.ToString();
            _cachedNumberString[value] = result;
        }
        return result;
    }
}