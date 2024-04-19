using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nav.FlowFieldPathfinding
{
    public class FFPNavor
    {
        // 等于使用int
        public struct Point
        {
            public int value;
            public Point(int v)
            {
                value = v;
            }
        }

        /// <summary> Heatmap阶段的周围询问，它是四向的 </summary>
        private Vector2Int[] rounds;
        /// <summary> 询问当前点(x,y)是否是存在的 </summary>
        private Func<Vector2Int, bool> Exist;

        public FFPNavor(Func<Vector2Int, bool> Exist)
        {
            // 右左 上下
            rounds = new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(-1, 0) };
            this.Exist = Exist;
        }

        public Vector2[,] Nav(Vector2Int size, Vector2Int startIndex, out Point[,] points)
        {
            // 创建一个Size.x * Size.y 尺寸的图
            points = new Point[size.x, size.y];
            // 权重(数字)计算
            GetHeatmap(startIndex, ref points);
            // 方向(箭头)计算
            return GetVector(points);
        }

        /// <summary> 获取网格热力图 </summary>
        private void GetHeatmap(Vector2Int startIndex, ref Point[,] points)
        {
            // 为热力图赋初值 = -1
            for (int i = 0; i < points.GetLength(0); i++)
                for (int j = 0; j < points.GetLength(1); j++)
                    points[i, j].value = -1;
            // 起点热力为1
            points[startIndex.x, startIndex.y].value = 0;
            // 访问循环
            Queue<Vector2Int> visit = new Queue<Vector2Int>();
            Queue<Vector2Int> waitVisit = new Queue<Vector2Int>();
            Vector2Int nowIndex, forIndex;
            // 起点入队列
            visit.Enqueue(startIndex);
            while (visit.Count != 0)
            {
                while (visit.TryDequeue(out nowIndex))
                {
                    // 访问出队单元格周围
                    for (int i = 0; i < rounds.Length; i++)
                    {
                        forIndex = nowIndex + rounds[i];
                        // 如果对应的位置存在一个合法单元格: 存在 并且 未被访问
                        if (Exist(forIndex) && points[forIndex.x, forIndex.y].value == -1)
                        {
                            // 加入等待访问的队列
                            waitVisit.Enqueue(forIndex);
                            // 赋权值: 访问者 +1
                            points[forIndex.x, forIndex.y].value = points[nowIndex.x, nowIndex.y].value + 1;
                        }
                    }
                }
                visit = waitVisit;
                waitVisit = new Queue<Vector2Int>();
            }
        }

        /// <summary> 获取流场向量图 </summary>
        private Vector2[,] GetVector(Point[,] heatmap)
        {
            Vector2[,] vectors = new Vector2[heatmap.GetLength(0), heatmap.GetLength(1)];
            Vector2Int visitIndex;
            float v1, v2;
            for (int i = 0; i < heatmap.GetLength(0); i++)
                for (int j = 0; j < heatmap.GetLength(1); j++)
                {
                    // 值为 0 为目标地点
                    // 值为-1 为流场未访问到,必然是不可到达的
                    if (heatmap[i, j].value < 1)    // 不可到达则无方向
                    {
                        vectors[i, j] = Vector2.zero;
                    }
                    else
                    {
                        // 这部分参考文字
                        // Vector.x = left_tile.distance - right_tile.distance, 向量x为左边的块-右边的块;
                        // Vector.y = up_tile.distance - down_tile.distance,向量y为上边的块 - 下面的块;
                        // 每个块的distance,就是上面Heatmap种计算出来的数值。
                        // 如果当前块的（左 / 右 / 上 / 下）不可行走(障碍物等)，则使用与当前块的距离来代替缺少的值。
                        // 一旦粗略计算了路径向量，就对其进行归一化，以避免以后出现不一致。
                        visitIndex = new Vector2Int(i, j) + rounds[1];
                        v1 = Exist(visitIndex) ? heatmap[visitIndex.x, visitIndex.y].value : heatmap[i, j].value;
                        visitIndex = new Vector2Int(i, j) + rounds[0];
                        v2 = Exist(visitIndex) ? heatmap[visitIndex.x, visitIndex.y].value : heatmap[i, j].value;
                        // left(or self) - right(or self) 左小右大值为负
                        vectors[i, j].x = v1 - v2;
                        visitIndex = new Vector2Int(i, j) + rounds[3];
                        v1 = Exist(visitIndex) ? heatmap[visitIndex.x, visitIndex.y].value : heatmap[i, j].value;
                        visitIndex = new Vector2Int(i, j) + rounds[2];
                        v2 = Exist(visitIndex) ? heatmap[visitIndex.x, visitIndex.y].value : heatmap[i, j].value;
                        // down(or self) - up(or self) 下小上大值为负
                        vectors[i, j].y = v1 - v2;
                        // 归一化
                        vectors[i, j].Normalize();
                    }
                }
            Debug.Log(vectors[0, 0]);
            return vectors;
        }
    }
}
