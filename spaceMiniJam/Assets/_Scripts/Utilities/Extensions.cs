using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extensions
{
    public static void Fade(this SpriteRenderer renderer, float alpha)
    {
        var color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }

    public static T Rand<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t)
        {
            Object.Destroy(child.gameObject);
        }
    }

    public static Vector2 ToV2(this Vector3 input) => new Vector2(input.x, input.y);
    public static Vector3 Flat(this Vector3 input) => new Vector3(input.x, 0, input.z);
    public static Vector3 ToVector3Int(this Vector3 input) => new Vector3((int)input.x, (int)input.y, (int)input.z);


    /// <summary>
    /// Used instead of calling wait for seconds every time as it always allocating new memory instead of cacheing
    /// Usage is Extensions.GetWait(0.1f);
    /// </summary>

    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait))
        {
            return wait;
        }

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }

    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera.main, out var result);
        return result;
    }

    /// <summary>
    /// Returns if the current position of the mouse is over an UI element for blocking clicks
    /// </summary>
    private static PointerEventData s_eventDataCurrentPosition;
    private static List<RaycastResult> s_results;
    public static bool IsOverUI()
    {
        s_eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        s_results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(s_eventDataCurrentPosition, s_results);
        return s_results.Count > 0;
    }
}
