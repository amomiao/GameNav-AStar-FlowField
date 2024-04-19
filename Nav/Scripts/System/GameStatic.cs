using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatic : MonoBehaviour
{
    #region 单例
    public static GameStatic _instance;
    private void Awake() { _instance = this; }
    #endregion

    public Camera uiCamera;
}
