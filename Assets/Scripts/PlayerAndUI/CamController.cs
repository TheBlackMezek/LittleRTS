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

    [Header("Links")]

    [SerializeField] private Team team;
    [SerializeField] private Camera cam;



    private List<Unit> selectedUnits = new List<Unit>();



    private void Start()
    {
        transform.position = new Vector3(0f, startZoomLevel, 0f);
    }

    private void Update()
    {
        //Get variables for later use
        float dt = Time.deltaTime;

        float iScroll = Input.GetAxis("Mouse ScrollWheel");
        float iVert = Input.GetAxis("Vertical");
        float iHorz = Input.GetAxis("Horizontal");



        //Move camera along zoom & orthagonal axes
        Vector3 pos = transform.position;
        pos.y += pos.y * iScroll * deltaZoomMultiplier;
        pos.y = Mathf.Clamp(pos.y, minZoom, maxZoom);

        pos.x += iHorz * dt * (moveSpeed + pos.y * extraMoveZoomMultiplier);
        pos.z += iVert * dt * (moveSpeed + pos.y * extraMoveZoomMultiplier);

        transform.position = pos;



        //Mouse commands
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, maxZoom))
            {
                Unit unit = hit.transform.GetComponent<Unit>();
                if (unit && unit.TeamID == team.ID)
                {
                    unit.Select();
                    selectedUnits.Add(unit);
                }
                else
                {
                    for(int i = 0; i < selectedUnits.Count; ++i)
                    {
                        selectedUnits[i].Deselect();
                    }
                    selectedUnits.Clear();
                }
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, maxZoom))
            {
                for (int i = 0; i < selectedUnits.Count; ++i)
                {
                    selectedUnits[i].SetDestination(hit.point);
                }
            }
        }
    }

}
