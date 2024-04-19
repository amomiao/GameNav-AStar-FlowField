using Nav.FlowFieldPathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nav.FlowFieldPathfinding
{
    public class FFPNavMgr : FFPNavorMgrBase
    {
        private SceneGridManager Mgr => SceneGridManager.instance;
        protected override bool Exist(Vector2Int index)
        {
            SceneGrid grid = Mgr.GetCell(index);
            return grid != null && grid.nowState != SceneGrid.E_GridState.Obstacle;
        }

        public RectTransform rect;
        public Button startBtn;
        public Button resetBtn;
        private Vector2Int startPos = -Vector2Int.one;
        private Vector2[,] vectors;
        private GameObject[,] numbers;
        private void Start()
        {
            Mgr.setStartPoint += (value) => { startPos = value; };
            startBtn.onClick.AddListener(NavorStart);
            resetBtn.onClick.AddListener(() =>
            {
                vectors = null;
                if (numbers != null)
                {
                    for (int i = 0; i < numbers.GetLength(0); i++)
                        for (int j = 0; j < numbers.GetLength(1); j++)
                            Destroy(numbers[i, j]);
                    numbers = null;
                }
            });
        }
        public void NavorStart()
        {
            vectors = null;
            if (numbers != null)
            {
                for (int i = 0; i < numbers.GetLength(0); i++)
                    for (int j = 0; j < numbers.GetLength(1); j++)
                        Destroy(numbers[i, j]);
                numbers = null;
            }
            if (startPos == -Vector2Int.one)
                Debug.Log("起点未赋值");
            else
            {
                FFPNavor.Point[,] points;
                vectors = Nav(Mgr.gridSize, startPos, out points);
                numbers = new GameObject[vectors.GetLength(0), vectors.GetLength(1)];
                GameObject obj;
                for (int i = 0; i < vectors.GetLength(0); i++)
                    for (int j = 0; j < vectors.GetLength(1); j++)
                    {
                        obj = GameObject.Instantiate(rect.gameObject, rect.parent);
                        obj.GetComponent<RectTransform>().anchoredPosition = Mgr.GetCellPosCenter(i, j);
                        obj.GetComponent<Text>().text = points[i, j].value.ToString();
                        numbers[i, j] = obj;
                    }
            }
        }

        private void Update()
        {
            if (vectors != null)
            {
                for (int i = 0; i < vectors.GetLength(0); i++)
                    for (int j = 0; j < vectors.GetLength(1); j++)
                    {
                        rect.anchoredPosition = Mgr.GetCellPosCenter(i, j);
                        Debug.DrawRay(rect.position, vectors[i, j] * 0.2f, Color.red);
                    }
            }
        }
    }
}