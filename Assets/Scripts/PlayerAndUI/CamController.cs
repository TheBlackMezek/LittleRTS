using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {

    [Header("Settings")]

    [SerializeField] private float moveSpeed;
    [SerializeField] private float startZoomLevel;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float deltaZoomMultiplier;
    [SerializeField] private float extraMoveZoomMultiplier;
    [SerializeField] private float maxDragSizeForClick;

    [Header("Links")]

    [SerializeField] private Team team;
    [SerializeField] private Camera cam;

    public bool DragSelecting { get; private set; }
    public Vector3 DragStart { get; private set; }
    public Vector3 DragEnd { get; private set; }
    private float dragTime = 0f;

    public VoidReturnV3 OnDragStart;
    public VoidReturnV3 OnDragEnd;



    private void Start()
    {
        transform.position = new Vector3(0f, startZoomLevel, 0f);
    }

    private void Update()
    {
        //Get variables for later use
        float dt = Time.deltaTime;

        float iScroll = -Input.GetAxis("Mouse ScrollWheel");
        float iVert = Input.GetAxis("Vertical");
        float iHorz = Input.GetAxis("Horizontal");

        Vector3 pos = transform.position;



        //Move camera along zoom & orthagonal axes
        pos.y += pos.y * iScroll * deltaZoomMultiplier;
        pos.y = Mathf.Clamp(pos.y, minZoom, maxZoom);

        pos.x += iHorz * dt * (moveSpeed + pos.y * extraMoveZoomMultiplier);
        pos.z += iVert * dt * (moveSpeed + pos.y * extraMoveZoomMultiplier);

        transform.position = pos;



        //Mouse commands
        if(Input.GetMouseButtonDown(0))
        {
            DragStart = Input.mousePosition;
            DragEnd = Input.mousePosition;
            DragSelecting = true;
            OnDragStart(DragStart);
        }
        else if(Input.GetMouseButton(0))
        {
            DragEnd = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            DragSelecting = false;
            OnDragEnd(DragEnd);

            team.DeselectAllUnits();

            bool clickNotDragX = Mathf.Abs(DragStart.x - DragEnd.x) < maxDragSizeForClick;
            bool clickNotDragY = Mathf.Abs(DragStart.y - DragEnd.y) < maxDragSizeForClick;

            if(clickNotDragX && clickNotDragY)
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, maxZoom))
                {
                    Unit unit = hit.transform.GetComponent<Unit>();
                    if (unit != null)
                        team.SelectUnit(unit);
                }
            }
            else
            {
                Bounds bounds = GetViewpointBounds(DragStart, DragEnd);

                team.SelectUnitsInBounds(bounds, cam);
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, maxZoom))
            {
                team.SetUnitsDestination(hit.point);
            }
        }
    }

    //Theis function taken from https://hyunkell.com/blog/rts-style-unit-selection-in-unity-5/
    private Bounds GetViewpointBounds(Vector3 screenPos1, Vector3 screenPos2)
    {
        Vector3 v1 = cam.ScreenToViewportPoint(screenPos1);
        Vector3 v2 = cam.ScreenToViewportPoint(screenPos2);
        Vector3 min = Vector3.Min(v1, v2);
        Vector3 max = Vector3.Max(v1, v2);

        min.z = cam.nearClipPlane;
        max.z = cam.farClipPlane;

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

}
