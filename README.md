下载:Nav_AStar_FlowField.unitypackage 即可
bilibili.com/video/BV1fm4y1V7wK<br>
目录:<br>
Scenes: Demo场景<br>
Scripts:<br>
 Astar:A星寻路底层<br>
 FlowFieldPathfinding:流场导航底层<br>
 NavTest:Demo的导航管理器类<br>
 System:Demo运行的依赖<br>

前言:<br>
	导航算法一般就是三步,<br>
	1) 遍历访问: 从起点开始迭代访问所有可能<br>
	2) 计算权: 每次迭代计算一些权<br>
	3) 导出路径<br>
	基于这三得三个要点: Rounds、Exist、GetF。<br>
	Rounds和Exist用来实现遍历访问(使用网格实现的情况,具体的路点Rounds的职能需要转给点Point)<br>
	GetF则为计算权,在流场导航中GetF即FFPNavor中的热力图Heatmap<br>

AStar原理:<br>
1.AStartNavor:A星导航器<br>
	// 具体逻辑需要看AStartNavor源码中的注释<br>
	// 给予起点 终点 out记录所有访问过的点 return寻路路径<br>
	public Vector2Int[] Nav(Vector2Int startIndex, Vector2Int targetIndex, out Vector2Int[] routes)<br>
2.NavPoint:A星路径点<br>
	NavPoint是导航计算时的一个存储体<br>
	当作为一个被确定使用的点时:它使用self提供路线，<br>
	当作为一个不被确定使用的点时:它使用f 给导航器提供权重,继承的IComparable<NavPoint>接口方便Sort<br>
3.nWayAStarNavorMgrBase:n向A星导航管理器<br>
	创建一个AStartNavor作为运行环境,需要为这个环境配置:<br>
	1) Func<List<Vector2Int>> Rounds: 说明一个点(x,y)相邻的点分别相对于这个坐标的偏移<br>
		如(x+1,y)则偏移为(1,0)<br>
	2) Func<Vector2Int, bool> Exist: 检查点(x,y)是否存在<br>
		使用(x,y)访问到具体的物理存储,以检测存在与否<br>
	3) Func<Vector2Int, Vector2Int, Vector2Int, float> GetF: 长度权重计算<br>
		AStar的权重计算和 起点、终点、当前点位置有关<br>
		通常 = self与start距离+self与end距离,这个距离可以是勾股定理后不开方的。<br>

流场导航原理:https://zhuanlan.zhihu.com/p/391459932<br>
注意: <br>
	1) 流场导航的向量箭头的可视化需要打开Gizmos<br>
	2) 流场导航可能会出现'撞墙'的情况,发生在 发生点、障碍点、终点 三点处于 同一条x或y轴线上时<br>

1.FFPNavor:流场导航器<br>
	// 具体逻辑需要看FFPNavor源码中的注释<br>
	// 流场尺寸 流场指向点 out对应点位权 return对应点位流向<br>
	public Vector2[,] Nav(Vector2Int size, Vector2Int startIndex, out Point[,] points)<br>
2.FFPNavorMgrBase:流场导航管理器<br>
	1) 创建一个FFPNavor作为运行环境,需要为这个环境配置:<br>
	Func<Vector2Int, bool> Exist: 检查点(x,y)是否存在<br>
		使用(x,y)访问到具体的物理存储,以检测存在与否<br>
	// Round被Navor内置了<br>
	2) API方法<br>
	// 流场尺寸 流场指向点 out对应点位权 return对应点位流向<br>
	public Vector2[,] Nav(Vector2Int size, Vector2Int startIndex, out Point[,] points)<br>
