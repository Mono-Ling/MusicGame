using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Track : MonoBehaviour
{
    public int id;
    public List<Unit> actionUnits = new List<Unit>();
    private BoxCollider2D boxCollider;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        Vector2 screenStep = new Vector2(Screen.width / 6f, Screen.height);
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(screenStep.x * (id + 0.5f),screenStep.y, 0));
        pos.y = 0;
        transform.position = pos;
        boxCollider.size = new Vector2(Camera.main.orthographicSize * 2 * Camera.main.aspect / 6, 10);
    }
    public Unit ComparInputUnit(float time, float window)
    {
        int length = actionUnits.Count;
        int left = 0;
        int right = length - 1;
        int targetIndex = -1;
        Unit targetUnit = null;
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            Unit midUnit = actionUnits[mid];
            if (midUnit == null)
            {
                right = mid - 1;
                continue;
            }
            if (Mathf.Abs(time - midUnit.unitEndTime) < window)
            {
                targetIndex = mid;
                targetUnit = midUnit;
                break;
            }
            else if (time - midUnit.unitEndTime > window)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }
        if (targetIndex != -1)
        {
            actionUnits.RemoveAt(targetIndex);
        }
        return targetUnit;
    }
}
