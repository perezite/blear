using UnityEngine;

public static class RectTransformHelper
{
    public static float GetHeight(this RectTransform trans)
    {
        return trans.rect.height;
    }

    public static Vector2 GetSize(this RectTransform trans)
    {
        return trans.rect.size;
    }

    public static float GetWidth(this RectTransform trans)
    {
        return trans.rect.width;
    }

    public static WorldSpaceRect GetWorldSpaceRect(this RectTransform rectTransform)
    {
        Vector3[] worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners);

        Vector3 bottomLeftWorldCorner = worldCorners[0];
        Vector3 topRightWorldCorner = worldCorners[2];
        float width = topRightWorldCorner.x - bottomLeftWorldCorner.x;
        float height = topRightWorldCorner.y - bottomLeftWorldCorner.y;

        return new WorldSpaceRect(bottomLeftWorldCorner.x, bottomLeftWorldCorner.y, width, height);
    }

    // Reference for the following rect transform extensions: http://orbcreation.com/orbcreation/page.orb?1099
    public static void SetDefaultScale(this RectTransform trans)
    {
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static void SetHeight(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(trans.rect.size.x, newSize));
    }

    public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static void SetPivotAndAnchors(this RectTransform trans, Vector2 vec)
    {
        trans.pivot = vec;
        trans.anchorMin = vec;
        trans.anchorMax = vec;
    }

    public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
    }

    public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static void SetSize(this RectTransform trans, Vector2 newSize)
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }

    public static void SetWidth(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(newSize, trans.rect.size.y));
    }
}
