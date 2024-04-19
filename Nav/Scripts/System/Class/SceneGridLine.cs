using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 网格线对象 </summary>
[System.Serializable]
public class SceneGridLine
{
    public GameObject gameObject;

    public SceneGridLine(GameObject go)
    {
        gameObject = go;
    }
}
