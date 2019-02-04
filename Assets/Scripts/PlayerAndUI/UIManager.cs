using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] private CamController camController;
    [SerializeField] private RectTransform dragSelect;



    private void Awake()
    {
        camController.OnDragStart = OnDragStart;
        camController.OnDragEnd = OnDragEnd;
    }

    private void Update()
    {
        if(camController.DragSelecting)
        {
            UpdateDragSelect();
        }
    }

    private void OnDragStart(Vector3 pos)
    {
        dragSelect.gameObject.SetActive(true);
        UpdateDragSelect();
    }

    private void OnDragEnd(Vector3 pos)
    {
        dragSelect.gameObject.SetActive(false);
    }

    private void UpdateDragSelect()
    {
        Vector2 start = new Vector2(camController.DragStart.x, camController.DragStart.y);
        Vector2 end = new Vector2(camController.DragEnd.x, camController.DragEnd.y);
        Vector2 dragCenter = (start + end) / 2f;
        Vector2 halfExtents = new Vector3(Mathf.Abs(start.x - end.x),
                                          Mathf.Abs(start.y - end.y));

        dragSelect.position = dragCenter;
        dragSelect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, halfExtents.x);
        dragSelect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, halfExtents.y);
    }

}
