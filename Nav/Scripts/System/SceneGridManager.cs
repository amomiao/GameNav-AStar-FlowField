using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneGridManager : MonoBehaviour
{
    public static SceneGridManager instance;
    private void Awake() { instance = this; }

    private SceneGrid navStartPos;
    private SceneGrid navEndPos;
    /// <summary> 设置起点的回调 </summary>
    public UnityAction<Vector2Int> setStartPoint;
    /// <summary> 设置终点的回调 </summary>
    public UnityAction<Vector2Int> setEndPoint;

    private Vector2 canvasSize = new Vector2(1920, 1080);
    private Vector2 halfSize;
    public List<SceneGrid> grids;
    public List<SceneGridLine> xLines;
    public List<SceneGridLine> yLines;
    /// <summary> 左上单元格的位置 </summary>
    public Vector2 startPos;
    /// <summary> 单元格的大小与点心之间的间距 </summary>
    public Vector2 spanAndCellSize;
    /// <summary> 网格大小 </summary>
    public Vector2Int gridSize;

    /// <summary> 通过索引号得到单元格 </summary>
    public SceneGrid GetCell(int index)
    {
        if (index >= 0 && index < grids.Count)
            return grids[index];
        else
            return null;
    }
    /// <summary> 通过行列号得到单元格 </summary>
    public SceneGrid GetCell(Vector2Int rac) { return GetCell(rac.x, rac.y); }
    /// <summary> 通过行列号得到单元格 </summary>
    public SceneGrid GetCell(int row, int col)
    {
        if (row > -1 && row < gridSize.x && col > -1 && col < gridSize.y)
            return GetCell(row * gridSize.x + col);
        else
            return null;
    }
    /// <summary> 通过V2Int行列号得到理论上的单元格位置 </summary>
    public Vector2 GetCellPos(Vector2Int rac) { return GetCellPos(rac.x, rac.y); }
    /// <summary> 通过行列号得到理论上的单元格位置 </summary>
    public Vector2 GetCellPos(int row, int col) { return new Vector2(-500 + spanAndCellSize.x * col, -500 + spanAndCellSize.y * row); }
    /// <summary> 通过行列号得到理论上的单元格中心位置 </summary>
    public Vector2 GetCellPosCenter(int row, int col) { return new Vector2(-500 + spanAndCellSize.x * col + spanAndCellSize.x / 2, -500 + spanAndCellSize.y * row + spanAndCellSize.y / 2); }
    /// <summary> 通过平面坐标得到单元格行列 </summary>
    public Vector2Int PositionCell(Vector2 pos)
    {
        return new Vector2Int(Mathf.FloorToInt((pos.y / spanAndCellSize.y) + (gridSize.y / 2)), Mathf.FloorToInt((pos.x / spanAndCellSize.x) + (gridSize.x / 2)));
    }

    #region 初始化 public void Init(Vector2 pixedWH,Vector2Int cellWH)
    /// <summary> 初始化 </summary>
    /// <param name="pixedWH"> 总的像素大小 </param>
    /// <param name="cellWH"> 网格大小 </param>
    public void Init(Vector2 pixedWH, Vector2Int cellWH)
    {
        halfSize = canvasSize / 2;
        //存储
        grids = new List<SceneGrid>();
        grids.Capacity = cellWH.x * cellWH.y;
        xLines = new List<SceneGridLine>();
        xLines.Capacity = cellWH.x;
        yLines = new List<SceneGridLine>();
        yLines.Capacity = cellWH.y;
        gridSize = cellWH;
        //每个单元格的大小，同时也是两个单元格之间的间距
        spanAndCellSize = new Vector2(pixedWH.x / cellWH.x, pixedWH.y / cellWH.y);
        //单元格的开始位置
        startPos = new Vector2(-spanAndCellSize.x * (cellWH.x / 2), -spanAndCellSize.y * (cellWH.y / 2));
    }
    #endregion 初始化

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SwitchObstacleState(GetCellInGrids());
        }
        else if (Input.GetMouseButtonDown(1))
        {
            SwitchNavPointState(GetCellInGrids());
        }
    }

    /// <summary> 重置整张图 </summary>
    public void Restart()
    {
        for (int i = 0; i < grids.Count; i++)
            ChangeState(grids[i], SceneGrid.E_GridState.Normal);
    }

    /// <summary> 得到鼠标当前位置单元格 </summary>
    private SceneGrid GetCellInGrids()
    {
        //更新nowPos
        Vector2 nowPos = GameStatic._instance.uiCamera.ScreenToViewportPoint(Input.mousePosition);
        nowPos.x *= canvasSize.x;
        nowPos.y *= canvasSize.y;
        //相对于中央偏移
        nowPos -= halfSize;
        //计算对应单元格
        return GetCell(PositionCell(nowPos));
    }
    /// <summary> 切换障碍物状态 </summary>
    /// <param name="cell"> 单元格 </param>
    private void SwitchObstacleState(SceneGrid cell)
    {
        if (cell == null) return;
        if (cell.nowState != SceneGrid.E_GridState.Normal)
            ChangeState(cell, SceneGrid.E_GridState.Normal);
        else
            ChangeState(cell, SceneGrid.E_GridState.Obstacle);
    }
    /// <summary> 切换起始点状态 </summary>
    /// <param name="cell"> 单元格 </param>
    private void SwitchNavPointState(SceneGrid cell)
    {
        if (cell == null) return;
        if (cell.nowState != SceneGrid.E_GridState.StartPoint)
            ChangeState(cell, SceneGrid.E_GridState.StartPoint);
        else
            ChangeState(cell, SceneGrid.E_GridState.EndPoint);
    }
    /// <summary> 切换单元格状态 </summary>
    /// <param name="cell"> 单元格行列号 </param>
    /// <param name="toState"> 到的状态 </param>
    public void ChangeState(Vector2Int cellIndex, SceneGrid.E_GridState toState) { ChangeState(GetCell(cellIndex), toState); }
    /// <summary> 切换单元格状态 </summary>
    /// <param name="cell"> 单元格 </param>
    /// <param name="toState"> 到的状态 </param>
    private void ChangeState(SceneGrid cell, SceneGrid.E_GridState toState)
    {
        if (cell == navStartPos)
        {
            navStartPos = null;
            setStartPoint?.Invoke(-Vector2Int.one);
        }
        if (cell == navEndPos)
        {
            navEndPos = null;
            setEndPoint?.Invoke(-Vector2Int.one);
        }
        switch (toState)
        {
            case SceneGrid.E_GridState.Normal:
                cell.Image.color = Color.gray;
                break;
            case SceneGrid.E_GridState.Obstacle:
                cell.Image.color = Color.red;
                break;
            case SceneGrid.E_GridState.StartPoint:
                if (navStartPos != null)
                    SwitchObstacleState(navStartPos);
                cell.Image.color = Color.green;
                navStartPos = cell;
                setStartPoint?.Invoke(PositionCell(cell.gameObject.GetComponent<RectTransform>().anchoredPosition));
                break;
            case SceneGrid.E_GridState.EndPoint:
                cell.Image.color = Color.cyan;
                navEndPos = cell;
                setEndPoint?.Invoke(PositionCell(cell.gameObject.GetComponent<RectTransform>().anchoredPosition));
                break;
            case SceneGrid.E_GridState.RoutePoint:
                cell.Image.color = Color.yellow;
                break;
            case SceneGrid.E_GridState.TrueRoutePoint:
                cell.Image.color = Color.blue;
                break;
        }
        cell.nowState = toState;
    }
}
