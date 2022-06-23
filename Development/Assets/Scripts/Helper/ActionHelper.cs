using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class ActionHelper
{
    public static void TryInvoke(this Action action)
    {
        if (action != null)
        {
            action();
        }
    }

    public static void TryInvoke<T>(this Action<T> action, T t)
    {
        if (action != null)
        {
            action(t);
        }
    }
}
