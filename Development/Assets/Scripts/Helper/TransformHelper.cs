using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelper
{
    public static IEnumerator Scale(Transform trans, float scaleDuration, Vector3 endScale)
    {
        var startTime = Time.time;
        var timeElapsed = 0f;
        var startScale = trans.localScale;
        while (timeElapsed < scaleDuration)
        {
            timeElapsed = Time.time - startTime;
            float t = timeElapsed / scaleDuration;
            trans.localScale = VectorHelper.SmoothStep(startScale, endScale, t);
            yield return null;
        }
    }

    public static void ResetLocal(this Transform trans)
    {
        trans.localPosition = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = Vector3.one;
    }

    public static int GetNumActiveChildren(this Transform trans)
    {
        return trans.GetActiveChildren().Length;
    }

    public static void Apply(this Transform trans, TransformData transConfig)
    {
        trans.position = transConfig.Position;
        trans.rotation = transConfig.Rotation;
    }

    public static Transform[] GetActiveChildren(this Transform trans)
    {
        List<Transform> children = new List<Transform>();

        for (int i = 0; i < trans.childCount; i++)
        {
            Transform child = trans.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                children.Add(child);
            }
        }

        return children.ToArray();
    }

    // get all children
    public static Transform[] GetAllChildren(this Transform trans)
    {
        List<Transform> children = new List<Transform>();

        for (int i = 0; i < trans.childCount; i++)
        {
            Transform child = trans.GetChild(i);
            children.Add(child);
        }

        return children.ToArray();
    }

    // get all children recursively
    public static Transform[] GetAllChildrenRecursively(this Transform trans)
    {
        List<Transform> children = new List<Transform>();

        for (int i = 0; i < trans.childCount; i++)
        {
            Transform child = trans.GetChild(i);
            children.Add(child);

            if (child.childCount > 0)
            {
                var recursiveChildren = GetAllChildrenRecursively(child);
                children.AddRange(recursiveChildren);
            }
        }

        return children.ToArray();
    }
}
