开发文档

本次项目的碰撞检测均使用射线进行检测
每一个单位均是1*1的矩形，碰撞器体积为0.5*0.5

地面使用Tilemap绘制
检测物体是否在地面上：由物体发射一个y=0.1的射线，仅检测地面层（GroundLayer）如果检测到碰撞体即在地面上，否则不在地面上

检测洞口是否开启：Food类提供了一个IsFoodExist方法，返回当时是否有食物存在，Hole类在Update中调用此方法，若返回true则开启洞口

吃辣椒后的飞行：将蛇及其身体加入LinkedList，不断遍历LinkedList进行位置的移动，若飞行途中遇到Movable类的物体（食物，冰），则将其加入LinkedList并加入Set（防止重复加入）
		若碰到Obstacle类的物体（墙，木头）则停止飞行

吃香蕉后身体变长： 生成一个Body，然后蛇往香蕉的位置移动一次

冰块与木块的销毁：吃到辣椒后若前面一格有冰块或木块，则Destory

沙坑的检测：跟地面的检测方法相同，但仅检测BunkerLayer

UI界面：每个界面都是一个Scene，提供一个LevelControl类提供Scene切换的函数

R键重新开始：重新加载当前Scene

Esc菜单：打开时Time.Scale = 0，并使玩家无法控制蛇蛇。关闭时 Time.Scale = 0，并使玩家可以控制蛇蛇

从天而降动画效果：开始游戏时使每一个物体y+=25 再使其降落到原位置

洞口关闭到开启的变化：利用Animator实现

小蛇表情变化：利用Animator实现，设置Direction，isOnlyHead，FlyStop，startMove等变量

喷火的效果：在吃到辣椒后创建一个fire的对象，也加入不断遍历的LinkedList，飞行结束后Destory

蛇身体拐角处：每一个Body都记录一个Dir变量，每次移动时除第一个Body外均继承前一个Body形态即可
		第一个Body结合其当前Dir与蛇头的Dir改变其形态

进入洞口动画/掉入沙坑动画：尚未实现

尘土效果：尚未实现

上下浮动效果：尚未实现
