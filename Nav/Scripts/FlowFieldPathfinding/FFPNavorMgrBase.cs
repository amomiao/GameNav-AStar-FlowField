using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Nav.FlowFieldPathfinding
{
    public abstract class FFPNavorMgrBase : MonoBehaviour
    {
        private FFPNavor navor;

        private void Awake()
        {
            navor = new FFPNavor(Exist);
        }

        public Vector2[,] Nav(Vector2Int size, Vector2Int startIndex, out FFPNavor.Point[,] points)
        {
            return navor.Nav(size, startIndex, out points);
        }

        /// <summary> 检测点存在/合法规则 </summary>
        protected abstract bool Exist(Vector2Int index);
    }
}