using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nav.AStart
{
    /// <summary> 单元格向上下左右移动的导航的计算器 </summary>
    public abstract class FourWayAStarNavorMgrBase : MonoBehaviour
    {
        private AStartNavor navor;
        protected List<Vector2Int> rounds;
        protected virtual void Awake()
        {
            navor = new AStartNavor(Rounds, Exist, GetF);
            rounds = new List<Vector2Int>() { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, -1) };
        }
        /// <summary> 导航 </summary>
        /// <param name="startIndex"> 起点 </param>
        /// <param name="targetIndex"> 终点 </param>
        /// <returns> 路径 </returns>
        public Vector2Int[] Nav(Vector2Int startIndex, Vector2Int targetIndex, out Vector2Int[] routes)
        {
            return navor.Nav(startIndex, targetIndex, out routes);
        }
        /// <summary> 周围寻路规则 </summary>
        protected List<Vector2Int> Rounds() { return rounds; }
        /// <summary> 检测点存在/合法规则 </summary>
        protected abstract bool Exist(Vector2Int index);
        /// <summary> 计算寻路消耗规则(AStar的简单规则认为是:点到起点位置+点到终点位置) </summary>
        protected abstract float GetF(Vector2Int self, Vector2Int start, Vector2Int end);
    }
}
