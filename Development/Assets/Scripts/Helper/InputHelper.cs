using System.Collections.Generic;
using UnityEngine;

public static class InputHelper
{
    public static bool GetTapDown()
    {
        if (Input.touchSupported)
        {
            return CheckTouch(0, new List<TouchPhase> { TouchPhase.Began });
        }

        return Input.GetMouseButtonDown(0);
    }

    public static bool GetTap()
    {
        if (Input.touchSupported)
        {
            return CheckTouch(0, new List<TouchPhase> { TouchPhase.Moved, TouchPhase.Stationary });
        }

        return Input.GetMouseButton(0);
    }

    public static bool GetTapUp()
    {
        if (Input.touchSupported)
        {
            return CheckTouch(0, new List<TouchPhase> { TouchPhase.Ended });
        }

        return Input.GetMouseButtonUp(0);
    }

    public static Vector2 GetTapPosition()
    {
        if (Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {
                return Input.GetTouch(0).position;
            }
        }

        return Input.mousePosition;
    }

    private static bool CheckTouch(int finger, List<TouchPhase> touchPhases)
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(finger);
            if (touchPhases.Contains(touch.phase))
            {
                return true;
            }
        }

        return false;
    }
}
