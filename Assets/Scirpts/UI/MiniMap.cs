using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour 
{
    public Image map;
    public Vector3 MapSize;
    public Vector3 MiniMapSize;
    public Transform PlayerContainer;
    public Transform Positions;
    public GameObject PositionPrefab;

    private void Awake()
    {
        MapSize = new Vector3(50 , 0 , 50);
        MiniMapSize = new Vector3(map.rectTransform.rect.width / 2, map.rectTransform.rect.height / 2, 0);
    }
    // 1000 / x = map.x / pos -> pos = map.x * x / 1000

    // 人物的 z , x -> 二维地图上的 y , x

    private void Update()
    {
        foreach (Transform item in Positions)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in PlayerContainer)
        {
            GameObject tmp = Instantiate(PositionPrefab , Positions);
            var Pos = new Vector3(item.position.x * MiniMapSize.x / MapSize.x, item.position.z * MiniMapSize.y / MapSize.z, 0);
            tmp.GetComponent<Image>().rectTransform.localPosition = Pos;
        }
    }

}