using UnityEngine;

// Like a Unity Rect, but with the y-axis pointing upwards to match the orientation of the world space coordinate system
public struct WorldSpaceRect
{
    // bottom of rectangle
    public float Bottom;

    // height of rectangle
    public float Height;

    // left corner of rectangle
    public float Left;

    // width of rectangle
    public float Width;

    public WorldSpaceRect(float rectLeftPosition, float rectBottomPosition, float rectWidth, float rectHeight)
    {
        Left = rectLeftPosition;
        Bottom = rectBottomPosition;
        Width = rectWidth;
        Height = rectHeight;
    }

    public WorldSpaceRect(WorldSpaceRect other)
    {
        this = (WorldSpaceRect)other.MemberwiseClone();
    }

    // set WorldSpaceRect from Unity Rect
    public WorldSpaceRect(Rect rect)
    {
        Left = rect.xMin;
        Bottom = rect.yMax;
        Height = rect.height;
        Width = rect.width;
    }

    // center of rectangle
    public Vector2 Center
    {
        get
        {
            float centerX = Left + (0.5f * Width);
            float centerY = Bottom + (0.5f * Height);
            return new Vector2(centerX, centerY);
        }
    }

    // right corner of rectangle
    public float Right
    {
        get
        {
            return Left + Width;
        }
    }

    // size of rectangle
    public Vector2 Size
    {
        get
        {
            return new Vector2(Width, Height);
        }
    }

    // top corner of rectangle
    public float Top
    {
        get
        {
            return Bottom + Height;
        }
    }

    public bool Contains(Vector2 point)
    {
        bool containsNot = point.x < Left || point.x > Right || point.y < Bottom || point.y > Top;
        return !containsNot;
    }

    public override string ToString()
    {
        return string.Format("[WorldSpaceRect: left={0}, bottom={1}, height={2}, width={3}]", Left, Bottom, Height, Width);
    }
}