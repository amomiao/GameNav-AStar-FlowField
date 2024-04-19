注意: 流场导航的向量箭头的可视化需要打开Gizmos

目录:
Scenes: Demo场景
Scripts:
 Astar:A星寻路底层
 FlowFieldPathfinding:流场导航底层
 NavTest:Demo的导航管理器类
 System:Demo运行的依赖

A*原理:
1.AStartNavor:A星导航器
	// 具体逻辑需要看AStartNavor源码中的注释
	// 给予起点 终点 out记录所有访问过的点 return寻路路径
	public Vector2Int[] Nav(Vector2Int startIndex, Vector2Int targetIndex, out Vector2Int[] routes)
2.NavPoint:A星路径点
	NavPoint是导航计算时的一个存储体
	当作为一个被确定使用的点时:它使用self提供路线，
	当作为一个不被确定使用的点时:它使用f 给导航器提供权重,继承的IComparable<NavPoint>接口方便Sort
3.nWayAStarNavorMgrBase:n向A星导航管理器
	创建一个AStartNavor作为运行环境,需要为这个环境配置:
	1) Func<List<Vector2Int>> Rounds: 说明一个点(x,y)相邻的点分别相对于这个坐标的偏移
		如(x+1,y)则偏移为(1,0)
	2) Func<Vector2Int, bool> Exist: 检查点(x,y)是否存在
		使用(x,y)访问到具体的物理存储,以检测存在与否
	3) Func<Vector2Int, Vector2Int, Vector2Int, float> GetF: 长度权重计算
		A*的权重计算和 起点、终点、当前点位置有关
		通常 = self与start距离+self与end距离,这个距离可以是勾股定理后不开方的。

流场导航原理:https://zhuanlan.zhihu.com/p/391459932
1.FFPNavor:流场导航器
	// 具体逻辑需要看FFPNavor源码中的注释
	// 流场尺寸 流场指向点 out对应点位权 return对应点位流向
	public Vector2[,] Nav(Vector2Int size, Vector2Int startIndex, out Point[,] points)
2.FFPNavorMgrBase:流场导航管理器
	1) 创建一个FFPNavor作为运行环境,需要为这个环境配置:
	Func<Vector2Int, bool> Exist: 检查点(x,y)是否存在
		使用(x,y)访问到具体的物理存储,以检测存在与否
	// Round被Navor内置了
	2) API方法
	// 流场尺寸 流场指向点 out对应点位权 return对应点位流向
	public Vector2[,] Nav(Vector2Int size, Vector2Int startIndex, out Point[,] points)