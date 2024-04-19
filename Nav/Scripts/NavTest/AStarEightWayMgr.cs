using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nav.AStart
{
    public class AStarEightWayMgr : EightWayAStarNavorMgrBase
    {
        private SceneGridManager Mgr => SceneGridManager.instance;
        protected override bool Exist(Vector2Int index)
        {
            SceneGrid grid = Mgr.GetCell(index);
            return grid != null && grid.nowState != SceneGrid.E_GridState.Obstacle;
        }
        protected override float GetF(Vector2Int self, Vector2Int start, Vector2Int end)
        {
            return Mgr.GetCellPos(self).DisSquare(Mgr.GetCellPos(start)) + Mgr.GetCellPos(self).DisSquare(Mgr.GetCellPos(end));
        }

        public Button startBtn;
        private Vector2Int startPos = -Vector2Int.one;
        private Vector2Int endPos = -Vector2Int.one;

        private void Start()
        {
            Mgr.setStartPoint += (value) => { startPos = value; };
            Mgr.setEndPoint += (value) => { endPos = value; };
            startBtn.onClick.AddListener(NavorStart);
        }

        public void NavorStart()
        {
            if (startPos == -Vector2Int.one)
                Debug.Log("起点未赋值");
            else if (endPos == -Vector2Int.one)
                Debug.Log("终点未赋值");
            else
            {
                Vector2Int[] routes;
                Vector2Int[] nav = Nav(startPos, endPos, out routes);
                for (int i = 1; routes != null && i < routes.Length; i++)
                {
                    if (routes[i] != endPos)
                        Mgr.ChangeState(routes[i], SceneGrid.E_GridState.RoutePoint);
                }
                for (int i = 1; nav != null && i < nav.Length - 1; i++)
                {
                    Mgr.ChangeState(nav[i], SceneGrid.E_GridState.TrueRoutePoint);
                }
            }
        }
    }
}