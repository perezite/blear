using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

// helper of UnityEngine.GameObject
public static class GameObjectHelper
{
    // set a game object and all its children active. It's bascially a replacement for the obsolete SetActiveRecursively
    public static void SetActiveRecursive(this GameObject gameObject, bool setActive)
    {
        var children = gameObject.transform.GetAllChildrenRecursively().ToList();
        gameObject.SetActive(setActive);
        children.ForEach(x => x.gameObject.SetActive(setActive));
    }

    // reset a game object's transform
    public static void ResetLocalTransform(this GameObject gameObject)
    {
        gameObject.transform.ResetLocal();
    }

    // insert sibling gameObject as sibling before other gameObject
    public static GameObject InsertAsSiblingBefore(this GameObject sibling, GameObject other)
    {
        var siblingIndex = sibling.transform.GetSiblingIndex();
        var otherIndex = other.transform.GetSiblingIndex();

        if (siblingIndex != otherIndex - 1)
        {
            sibling.transform.SetSiblingIndex(otherIndex);
        }

        return sibling;
    }

    // reference: http://answers.unity3d.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
    public static T GetCopyOf<T>(this Component component, T other) where T : Component
    {
        Type type = component.GetType();
        if (type != other.GetType())
        {
            return null; // type mis-match
        }

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;

        // set properties
        PropertyInfo[] propertyInfos = type.GetProperties(flags);
        foreach (var propertyInfo in propertyInfos)
        {
            if (propertyInfo.CanWrite)
            {
                try
                {
                    propertyInfo.SetValue(component, propertyInfo.GetValue(other, null), null);
                }
                catch
                {
                } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }

        // set fields
        FieldInfo[] fieldInfos = type.GetFields(flags);
        foreach (var fieldInfo in fieldInfos)
        {
            fieldInfo.SetValue(component, fieldInfo.GetValue(other));
        }

        return component as T;
    }

    // reference: http://answers.unity3d.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
    public static T AddComponent<T>(this GameObject gameObject, T componentToAdd) where T : Component
    {
        return gameObject.AddComponent<T>().GetCopyOf(componentToAdd) as T;
    }

    // safely destroy a game object in edit mode and playmode
    public static GameObject SafeDestroy(GameObject gameObject)
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                GameObject.DestroyImmediate(gameObject);
            };
        }
#else
        GameObject.Destroy(gameObject);
#endif

        return null;
    }
}