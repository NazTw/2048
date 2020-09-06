using System;
using System.Collections.Generic;

public enum Direction
{
    Left,
    Right,
    Up,
    Down,
}

public class TileModule
{
    public TileModule()
    {
        Items = new int[4 * 4];
        _random = new Random(0);
        _emptySpaces = new List<int>();
    }

    private readonly static int[] HorizontalStart =
        new int[] { 0, 1, 2, 3 };
    private readonly static int[] VerticalStart =
        new int[] { 0, 4, 8, 12 };
    private readonly static int[] LeftOffset =
        new int[] { 0, 1, 2, 3 };
    private readonly static int[] RightOffset =
        new int[] { 3, 2, 1, 0 };
    private readonly static int[] UpOffset =
        new int[] { 12, 8, 4, 0 };
    private readonly static int[] DownOffset =
        new int[] { 0, 4, 8, 12 };

    // 左到右
    // 下到上 
    // 12 13 14 15
    // 08 09 10 11
    // 04 05 06 07 
    // 00 01 02 03
    public int[] Items { get; private set; }

    private Random _random;
    private List<int> _emptySpaces;

    public Action<int, int> OnTileMoved { get; set; }

    public Action OnNoSpace { get; set; }

    public Action<int> OnTileAdded { get; set; }

    public Action<int> OnTileRemoved { get; set; }

    public void AddTile()
    {
        _emptySpaces.Clear();
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] != 0)
                continue;
            _emptySpaces.Add(i);
        }

        if(_emptySpaces.Count == 0)
        {
            OnNoSpace?.Invoke();
            return;
        }

        var random = _random.Next(0, _emptySpaces.Count);
        var index = _emptySpaces[random];
        Items[index] = 1;

        OnTileAdded?.Invoke(index);
    }

    public void RemoveTile(int index)
    {
        Items[index] = 0;
        OnTileRemoved?.Invoke(index);
    }

    public void Move(Direction direction)
    {
        int[] start;
        int[] offset;

        switch (direction)
        {
            case Direction.Left:
                start = VerticalStart;
                offset = LeftOffset;
                break;
            case Direction.Right:
                start = VerticalStart;
                offset = RightOffset;
                break;
            case Direction.Up:
                start = HorizontalStart;
                offset = UpOffset;
                break;
            case Direction.Down:
                start = HorizontalStart;
                offset = DownOffset;
                break;
            default:
                return;
        }

        for (int row = 0; row < start.Length; row++)
        {
            var startIndex = start[row];
            for (int from = 0; from < offset.Length - 1; from++)
            {
                var fromIndex = startIndex + offset[from];
                for (int to = from + 1; to < offset.Length; to++)
                {
                    var toIndex = startIndex + offset[to];
                    if (Items[fromIndex] == 0 ||
                        Items[toIndex] == 0 ||
                        Items[fromIndex] == Items[toIndex])
                    {
                        if (Items[toIndex] == 0)
                            continue;

                        Items[fromIndex] += Items[toIndex];
                        Items[toIndex] = 0;

                        OnTileMoved?.Invoke(toIndex, fromIndex);
                    }
                    else
                        break;
                }
            }
        }

        AddTile();
    }
}
