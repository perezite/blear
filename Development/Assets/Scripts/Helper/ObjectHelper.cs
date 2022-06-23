using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public static class ObjectHelper
{
    public static U Try<Object, U>(this Object x, System.Func<Object, U> func)
    {
        if (!IsDefault(x))
        {
            return func(x);
        }

        return default(U);
    }

    public static void Try<Object>(this Object x, System.Action<Object> action)
    {
        if (!IsDefault(x))
        {
            action(x);
        }
    }

    public static Coroutine TryStartCoroutine<T>(this T x, MonoBehaviour caller, System.Func<T, IEnumerator> func)
    {
        if (!IsDefault(x))
        {
            return caller.StartCoroutine(func(x));
        }

        return null;   
    }

    // reference: http://forum.unity3d.com/threads/null-check-inconsistency-c.220649/
    private static bool IsDefault<T>(T t)
    {
        return EqualityComparer<T>.Default.Equals(t, default(T));
    }
}