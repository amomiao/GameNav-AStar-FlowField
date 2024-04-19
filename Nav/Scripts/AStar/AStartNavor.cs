using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nav.AStart
{
    /// <summary> a星寻路导航器 </summary>
    public class AStartNavor
    {
        /// <summary> 环绕一个点的索引 </summary>
        private List<Vector2Int> rounds;
        /// <summary> 待选的列表 </summary>
        private List<NavPoint> open = new List<NavPoint>();
        /// <summary> 已选的列表 </summary>
        private List<NavPoint> close = new List<NavPoint>();
        /// <summary> 排除一些干扰后输出的结果 </summary>
        private LinkedList<Vector2Int> trueResult = new LinkedList<Vector2Int>();
        /// <summary> 最后输出的结果 </summary>
        private List<Vector2Int> result;

        /// <summary> 一个点外扩一圈到各个点的索引规则 </summary>
        private Func<List<Vector2Int>> Rounds;
        /// <summary> 询问当前点(x,y)是否是存在的 </summary>
        private Func<Vector2Int, bool> Exist;
        /// <summary> 计算寻路消耗的规则：所在点、起点、终点 </summary>
        private Func<Vector2Int, Vector2Int, Vector2Int, float> GetF;

        /// <summary> 实例化一个A星寻路器 </summary>
        /// <param name="Rounds"> 扩圈规则 </param>
        /// <param name="Exist"> 检测点存在/合法规则 </param>
        /// <param name="GetF"> 计算寻路消耗规则(AStar的简单规则认为是:点到起点位置+点到终点位置) </param>
        public AStartNavor(Func<List<Vector2Int>> Rounds, Func<Vector2Int, bool> Exist, Func<Vector2Int, Vector2Int, Vector2Int, float> GetF)
        {
            this.Rounds = Rounds;
            this.Exist = Exist;
            this.GetF = GetF;
        }

        /// <summary> 导航 </summary>
        /// <param name="startIndex">起点</param>
        /// <param name="targetIndex">终点</param>
        /// <returns>路径</returns>
        public Vector2Int[] Nav(Vector2Int startIndex, Vector2Int targetIndex, out Vector2Int[] routes)
        {
            //Init
            open.Clear();
            close.Clear();
            trueResult.Clear();
            //result.Clear();
            //Calculation
            // 起点加入open队列
            open.Add(new NavPoint(startIndex));
            while (open.Count > 0)
            {
                // open在逻辑中每次都进行Sort,open[0]放置着当前长度权重最小的点,也就是下一步的点
                // 当前open[0] == 目标点,寻路完成
                if (open[0].self == targetIndex)
                {
                    close.Add(open[0]);
                    break;
                }
                else
                {
                    if (!AddNew(open[0], startIndex, targetIndex))
                    {
                        // 进入这里时,说明AddNew没有添加任何一个新点到open队列,寻路基本上要失败
                        //Debug.Log("寻路失败");
                        //break;
                    }
                }
            }
            //true result
            NavPoint nav = close[close.Count - 1];
            int pos = close.Count - 1;
            routes = new Vector2Int[close.Count + open.Count];
            for (int i = 0; i < close.Count; i++)
            {
                routes[i] = close[i].self;
            }
            for (int j = 0; j < open.Count; j++)
            {
                routes[j + close.Count] = open[j].self;
            }
            while (nav.father != -Vector2Int.one)
            {
                trueResult.AddFirst(nav.self);
                pos = FindIndex(pos, nav.father);
                nav = close[pos];
            }
            trueResult.AddFirst(startIndex);
            //return
            result = new List<Vector2Int>(trueResult.Count);
            LinkedListNode<Vector2Int> n = trueResult.First;
            while (n != null)
            {
                result.Add(n.Value);
                n = n.Next;
            }
            return result.ToArray();
        }

        /// <summary> 增加一个点后，将他临近的点加入open队列，并将这个点移除open </summary>
        private bool AddNew(NavPoint point, Vector2Int startIndex, Vector2Int targetIndex)
        {
            bool rs = false;
            rounds = Rounds();
            for (int i = 0; i < rounds.Count; i++)
            {
                // 环绕一圈:
                // 点存在 并且 不在open队列 并且 不在close队列
                if (Exist(point.self + rounds[i]) && NotIn(point.self + rounds[i], open) && NotIn(point.self + rounds[i], close))
                {
                    // open队列接收新的点
                    open.Add(new NavPoint(point.self + rounds[i], GetF(point.self + rounds[i], startIndex, targetIndex), point.self));
                    rs = true;
                }
            }
            //如果进入这段代码，那么open中必然存在一个以上的对象。
            open.Sort();    // 权重排序
            close.Add(open[0]);
            open.RemoveAt(0);
            return rs;
        }

        /// <summary> 检测一个点是否不在列表 </summary>
        private bool NotIn(Vector2Int index, List<NavPoint> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].self == index)
                    return false;
            }
            return true;
        }

        /// <summary> 向上溯源正确的一条路径索引 </summary>
        private int FindIndex(int start, Vector2Int index)
        {
            for (int i = start; i >= 0; i--)
            {
                if (close[i].self == index)
                    return i;
            }
            return -1;
        }
    }
}
