using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nav.AStart
{
    public class NavPoint : IComparable<NavPoint>
    {
        /// <summary> 寻路消耗 </summary>
        public float f;
        /// <summary> 父点 </summary>
        public Vector2Int father;
        /// <summary> 本点 </summary>
        public Vector2Int self;

        /// <summary> 起点的构造方法 </summary>
        /// <param name="self">起点坐标</param>
        public NavPoint(Vector2Int self) : this(self, 0, new Vector2Int(-1, -1)) { }
        /// <summary> 非起点的路径点的构造方法 </summary>
        /// <param name="self">路径点坐标</param>
        /// <param name="f">寻路消耗</param>
        /// <param name="father">父节点</param>
        public NavPoint(Vector2Int self, float f, Vector2Int father)
        {
            this.self = self;
            this.father = father;
            this.f = f;
        }

        public int CompareTo(NavPoint other)
        {
            if (this.f > other.f)
            {
                return 1;
            }
            else if (this.f == other.f)
            {
                return 0;
            }
            else
            {
                return -1;
            }
            //return this.f <= other.f ? -1 : 1; 
        }
    }
}