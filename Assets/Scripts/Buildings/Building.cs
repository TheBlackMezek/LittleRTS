using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Damagable {

    [Header("Building Settings")]

    [SerializeField] private float unitSpawnTime;
    [SerializeField] private int unitSlots;

    [Header("Building Links")]

    [SerializeField] private GameObject slotImagePrefab;
    [SerializeField] private Transform slotImageHolder;
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform progressBar;
    [SerializeField] private UnityEngine.UI.Image progressBarColorImage;
    [SerializeField] private Transform unitSpawnPos;
    [SerializeField] private Transform unitRallyPos;

    private UnityEngine.UI.Image[] unitSlotImages;
    private int unitCount = 0;
    private float spawnProgress;



    protected override void Start()
    {
        base.Start();

        team.RegisterBuilding(this);

        progressBarColorImage.color = team.ProgressBarColor;

        unitSlotImages = new UnityEngine.UI.Image[unitSlots];

        RectTransform prefabT = slotImagePrefab.GetComponent<RectTransform>();
        float imgWidth = prefabT.sizeDelta.x;
        float totalWidth = imgWidth * unitSlots;
        float leftStart = -totalWidth / 2f + imgWidth / 2f;

        for (int i = 0; i < unitSlots; ++i)
        {
            GameObject obj = Instantiate(slotImagePrefab);
            RectTransform t = obj.GetComponent<RectTransform>();
            t.SetParent(slotImageHolder);
            t.localPosition = new Vector3(leftStart + imgWidth * i, 0f, 0f);
            t.localEulerAngles = Vector3.zero;
            UnityEngine.UI.Image[] imgs = obj.GetComponentsInChildren<UnityEngine.UI.Image>();
            unitSlotImages[i] = imgs[1];
        }

        UpdateSlotImages();

        Vector3 scale = progressBar.localScale;
        scale.x = spawnProgress / unitSpawnTime;
        progressBar.localScale = scale;
    }

    public override void CustomUpdate(float dt)
    {
        base.CustomUpdate(dt);

        if(unitCount < unitSlots)
        {
            spawnProgress += dt;

            if(spawnProgress >= unitSpawnTime)
            {
                spawnProgress = 0f;
                ++unitCount;
                UpdateSlotImages();

                GameObject obj = Instantiate(unitPrefab);
                obj.transform.position = unitSpawnPos.position;
                Unit unit = obj.GetComponent<Unit>();
                unit.OnKilled = OnSpawnedUnitKilled;
                unit.TeamID = TeamID;
                unit.SetDestination(unitRallyPos.position);
            }

            Vector3 scale = progressBar.localScale;
            scale.x = spawnProgress / unitSpawnTime;
            progressBar.localScale = scale;
        }
    }

    private void OnSpawnedUnitKilled(Unit u)
    {
        --unitCount;
        UpdateSlotImages();
    }

    private void UpdateSlotImages()
    {
        for(int i = 0; i < unitSlots; ++i)
        {
            if (unitCount > i)
                unitSlotImages[i].gameObject.SetActive(true);
            else
                unitSlotImages[i].gameObject.SetActive(false);
        }
    }

}
