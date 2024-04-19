using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneGridCreater : MonoBehaviour
{
    private List<GameObject> gos = new List<GameObject>();
    /// <summary> 总的像素长宽数 </summary>
    private Vector2 pixedWH = new Vector2(1000, 1000);
    /// <summary> 总的单元格长宽数 </summary>
    private Vector2Int cellWH = new Vector2Int(20, 20);
    /// <summary> 边线宽 </summary>
    private float lineWidth = 10;
    /// <summary> 边线 </summary>
    public Image lineImg;
    /// <summary> 单元格 </summary>
    public Image cellImg;
    /// <summary> 管理器 </summary>
    public SceneGridManager manager;
    private GameObject MgrGrids { set => manager.grids.Add(new SceneGrid(value)); }
    private GameObject MgrXLine { set => manager.xLines.Add(new SceneGridLine(value)); }
    private GameObject MgrYLine { set => manager.yLines.Add(new SceneGridLine(value)); }

    private void Awake()
    {
        CreateGraph();
    }

    [ContextMenu("CreateGraph")]
    private void CreateGraph()
    {
        manager.Init(pixedWH, cellWH);
        //初始化
        lineImg.gameObject.SetActive(true);
        lineImg.GetComponent<RectTransform>().sizeDelta = new Vector2(lineWidth, pixedWH.y);
        cellImg.gameObject.SetActive(true);
        lineImg.enabled = true;
        cellImg.enabled = true;
        GameObject obj;
        //计算间距 并且=单元格大小
        Vector2 spanAndCellSize = manager.spanAndCellSize;
        ///创建单元格
        for (int i = 0; i < cellWH.y; i++)
        {
            for (int j = 0; j < cellWH.x; j++)
            {
                obj = GameObject.Instantiate(cellImg.gameObject, cellImg.transform.parent);
                obj.GetComponent<RectTransform>().anchoredPosition = manager.GetCellPos(i, j);
                gos.Add(obj);
                MgrGrids = obj;
            }
        }
        ///创建线
        for (float i = -(cellWH.x / 2); i <= (cellWH.x / 2); i++)
        {
            obj = GameObject.Instantiate(lineImg.gameObject, lineImg.transform.parent);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(spanAndCellSize.x, 0) * i;
            gos.Add(obj);
            MgrXLine = obj;
        }
        for (float i = -(cellWH.y / 2); i <= (cellWH.y / 2); i++)
        {
            obj = GameObject.Instantiate(lineImg.gameObject, lineImg.transform.parent);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, spanAndCellSize.y) * i;
            obj.GetComponent<RectTransform>().rotation *= Quaternion.Euler(0, 0, 90);
            gos.Add(obj);
            MgrYLine = obj;
        }
        ///记录值

        ///退出
        lineImg.enabled = false;
        cellImg.enabled = false;
    }

    [ContextMenu("Clear")]
    private void Clear()
    {
        foreach (GameObject go in gos)
        {
            DestroyImmediate(go);
        }
        gos.Clear();
    }
}
