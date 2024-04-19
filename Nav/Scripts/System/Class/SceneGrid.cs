using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 网格对象 </summary>
[System.Serializable]
public class SceneGrid
{
    public enum E_GridState
    {
        /// <summary> 正常 </summary>
        Normal,
        /// <summary> 障碍物 </summary>
        Obstacle,
        /// <summary> 起点 </summary>
        StartPoint,
        /// <summary> 终点 </summary>
        EndPoint,
        /// <summary> 可能路径点 </summary>
        RoutePoint,
        /// <summary> 正确路径点 </summary>
        TrueRoutePoint,
    }

    public GameObject gameObject;
    public E_GridState nowState;
    public Image Image { get => gameObject.GetComponent<Image>(); }

    public SceneGrid(GameObject go)
    {
        gameObject = go;
        nowState = E_GridState.Normal;
        if (go.GetComponent<Image>() == null)
            Debug.LogError("No have Image");
    }
}
