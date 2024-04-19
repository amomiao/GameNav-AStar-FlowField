using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nav.AStart
{
    public abstract class EightWayAStarNavorMgrBase : FourWayAStarNavorMgrBase
    {
        protected override void Awake()
        {
            base.Awake();
            rounds = new List<Vector2Int>() {
                new Vector2Int(0, 1),
                new Vector2Int(1,1),
                new Vector2Int(1, 0),
                new Vector2Int(1, -1),
                new Vector2Int(-1, 0),
                new Vector2Int(-1, -1),
                new Vector2Int(0, -1) ,
                new Vector2Int(-1, 1) };
        }
    }
}