using UnityEngine;
using System;

public static class Extensions
{
    public static Vector3 WhereX(this Vector3 vector, float x)
    {
        vector.x = x;
        return vector;
    }
    public static Vector3 WhereY(this Vector3 vector, float y)
    {
        vector.y = y;
        return vector;
    }
    public static Vector3 WhereZ(this Vector3 vector, float z)
    {
        vector.z = z;
        return vector;
    }
    public static Vector3 AddTo(this Vector3 vector, float x = 0, float y = 0, float z = 0)
    {
        vector.x += x;
        vector.y += y;
        vector.z += z;
        return vector;
    }
    public static Vector3 Multiply(this Vector3 vector, float x = 1, float y = 1, float z = 1)
    {
        vector.x *= x;
        vector.y *= y;
        vector.z *= z;
        return vector;
    }

    public static Vector2 WhereX(this Vector2 vector, float x)
    {
        vector.x = x;
        return vector;
    }
    public static Vector2 WhereY(this Vector2 vector, float y)
    {
        vector.y = y;
        return vector;
    }

    public static Vector2 AddTo(this Vector2 vector, float x = 0, float y = 0)
    {
        vector.x += x;
        vector.y += y;
        return vector;
    }
    public static Vector2 Multiply(this Vector2 vector, float x = 1, float y = 1)
    {
        vector.x *= x;
        vector.y *= y;
        return vector;
    }

    public static Vector2 Direction(this Transform t, Vector2 pos)
    {
        var heading = pos - (Vector2)t.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        return direction;
    }

    #region Coroutine
    /// <summary> Calls function with delay (seconds). Coroutine </summary>
    public static Coroutine Co_DelayedExecute(this MonoBehaviour caller, Action action, float delay, bool scaledTime = true) => caller.StartCoroutine(Utils.Co_DelayedExecute(action, delay, scaledTime));

    /// <summary> Calls function with delay (Update frames). Coroutine </summary>
    public static Coroutine Co_DelayedExecute(this MonoBehaviour caller, Action action, int framesDelay) => caller.StartCoroutine(Utils.Co_DelayedExecute(action, framesDelay));

    /// <summary> Calls function when "predicate" function evaluates to true. Coroutine </summary>
    public static Coroutine Co_DelayedExecute(this MonoBehaviour caller, Action action, Func<bool> predicate, float minTime = 0) => caller.StartCoroutine(Utils.Co_DelayedExecute(action, predicate, minTime));
    #endregion

    /// <summary>
    /// Perform action on every child including root parent
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="action"></param>
    public static void ActOnEveryChild(this Transform parent, System.Action<Transform> action)
    {
        action?.Invoke(parent);

        foreach (Transform child in parent)
        {
            ActOnEveryChild(child, action);
        }
    }
}
