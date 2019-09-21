# 3D Game 3 - 空间与运动

> **I stumbled into by following my curiosity and intuition turned out to be priceless later on** 
>
> *— Steve Jobs, Stanford Report, June 14, 2005*

## 说明

**博客地址**：https://sentimentalswordsman.github.io/2019/09/20/3dG3-空间与运动/

**视频链接**： https://www.bilibili.com/video/av67477674/

说明：博客包含简答题（包括太阳系项目）、编程实践（包括牧师与魔鬼游戏）、选做题。

## 空间与运动

### 游戏的设计维度

- **空间维度**：自由度、尺度、边界等；
- **时间维度**：年代、时段等；
- **环境维度**：时代与文化背景、艺术风格与形式、场景与物体搭配等
- 情感维度、道德维度等

### 游戏世界空间模型

- **世界坐标**：一个游戏或游戏场景的**绝对坐标**系统。每个游戏对象的位置、角度、比例的值都这个坐标系下是唯一的。
- **对象坐标**：游戏对象相对父游戏对象的位置、角度、比例。又称为**相对坐标**。
- 3D空间（左手、右手坐标系统）、2D空间等。

### 坐标变化与运动

- 游戏运动本质就是使用矩阵变换（平移、旋转、缩放）改变游戏对象的空间属性。
- 在游戏层次视图中，游戏对象按树组织似乎天经地义。事实上，游戏对象是按它的空间关系组织设计。对象设计图如下：

![](./3dg-hw3/1.png)

### 基于职责的设计与游戏的MVC总体框架

- 面向对象设计的核心：**基于职责的设计**。即：模拟人类组织管理社会的方法，根据不同人拥有资源、知识与技能的不同，赋予不同人（或对象）特定的职责。再按一定结构（如设计模式），将它们组织起来。
- **门面（Fasàde）模式**：外部与一个子系统的通信必须通过一个统一的门面(Facade)对象进行。
- **MVC** 是界面人机交互程序设计的一种架构模式。它把程序分为三个部分：
  - 模型（Model）：数据对象及关系
    - 游戏对象、空间关系
  - 控制器（Controller）：接受用户事件，控制模型的变化
    - 一个场景一个主控制器
    - 至少实现与玩家交互的接口（IPlayerAction）
    - 实现或管理运动
  - 界面（View）：显示模型，将人机交互事件交给控制器处理
    - 处收 Input 事件
    - 渲染 GUI ，接收事件

## 作业与练习

### 1 简答题

#### Question 1.1

> 游戏对象运动的本质是什么？

游戏对象运动的本质是游戏对象的相对位置的改变。

实现游戏对象运动的方法一般是矩阵的变化，如平移、旋转、缩放等。

#### Question 1.2

> 请用三种方法以上方法，实现物体的抛物线运动。

方法一：transform.position

结合方程运用Transform，直接改变物体的位置。

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // x=0
    // z=v*t
    // y=4-a*t*t
    private float x;
    private float y;
    private float z;
    private float v;
    private float a;
    private float t;
    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        y = 4;
        z = 0;
        v = (float)1.6;
        a = (float)0.3;
        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(x, y, z);
        t += Time.deltaTime;
        z = v * t;
        y = 4 - a * t * t;
    }
}
```

方法二：transform.Translate

运用Translate方法分解抛物线运动的位移。

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private float vx;
    private float vy;
    private float a;
    // Start is called before the first frame update
    void Start()
    {
        vx = (float)1.6;
        a = (float)0.3;
        vy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        vy -= a * Time.deltaTime;
        this.transform.Translate(Vector3.forward * vx * Time.deltaTime);
        this.transform.Translate(Vector3.up * vy * Time.deltaTime);
    }
}
```

方法三：Vector3.Slerp

此方法用于返回参数点一到参数点二的球形插值向量，参数t范围为[0,1]。

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Vector3 v1 = new Vector3(0, 0, 2);
    Vector3 v2 = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
    }
            
    // Update is called once per frame
    void Update ()
    {
        Vector3 p = transform.position + v1 * Time.deltaTime - v2 * Time.deltaTime;
        v2.y += 1 * (Time.deltaTime);
        p.y -= 0.5F * 1 * (Time.deltaTime) * (Time.deltaTime);
        this.transform.position = Vector3.Slerp(transform.position, p, 1);
    }
}
```

方法四：Rigidbody

将Object定义为刚体，赋予重力和初速度属性。

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Use this for initialization
    void Start ()
    { 
    }
    
    // Update is called once per frame
    void Update ()
    {
        Rigidbody r = this.gameObject.AddComponent<Rigidbody>();
        r.useGravity = true;
        r.velocity = Vector3.left * 2;
    }
}
```

#### Question 1.3

> 写一个程序，实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上。

首先创建出九个星体，即太阳、月亮、地球即其他七个行星，设置好其初始位置和大小，完成结果如下图所示：

![](./3dg-hw3/2.png)

其次是设置星体的自转，自转速度是随机的，代码编写完成后将脚本挂载到行星上：

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//  完成自转过程
public class Rotation : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
		
	}
	// Update is called once per frame
	void Update ()
    {
        //  自转速度随机
        this.transform.RotateAround(this.transform.position, Vector3.up, Random.Range(1, 2));
	}
}
```

第三，是设置太阳系八大行星的公转，设定每个行星的公转法平面为（0，ry，rz），且 ry，rz 以及公转速度 v 是随机取定的，需把旋转中心设置为太阳；除此之外，还需添加、设置TrailRenderer，使星体的运动轨迹更为清晰：

```C#

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolution : MonoBehaviour
{
    //  设定每个行星的公转法平面为（0，ry，rz）
    //  ry，rz，公转速度v 随机取定
    public Transform center;
    public float v;
    float ry, rz;
    // Use this for initialization  
    void Start()
    {
        //  设置轨道性质
        this.transform.gameObject.GetComponent<TrailRenderer>();
        TrailRenderer tr = this.transform.gameObject.GetComponent<TrailRenderer>();
        tr.time = 7;
        tr.startWidth = 0.01f;
        tr.endWidth = 0.01f;
        tr.material = new Material(Shader.Find("Sprites/Default"));
        tr.startColor = Color.blue;
        tr.endColor = Color.green;
        //  设置公转速度和公转法平面
        v = Random.Range(60, 100);
        ry = Random.Range(15, 45);
        rz = Random.Range(15, 45);
    }
    // Update is called once per frame  
    void Update()
    {
        this.transform.RotateAround(center.position, new Vector3(0, ry, rz), v * Time.deltaTime);
    }
}
```

第四，是设置月球相对于地球进行的公转，需把旋转中心设置为地球，且月亮是归属于地球的子对象：

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon_revolution : MonoBehaviour
{
    //  设定月球公转法平面为（0，ry，rz）
    //  ry，rz，公转速度v 随机取定
    public Transform center;
    public float v;
    float ry, rz;
    // Use this for initialization  
    void Start()
    {
        v = Random.Range(100, 120);
        ry = Random.Range(15, 25);
        rz = Random.Range(15, 25);
    }
    // Update is called once per frame  
    void Update()
    {
        this.transform.RotateAround(center.position, new Vector3(0, ry, rz), v * Time.deltaTime);
    }
}
```

第五，是设置星空的背景，添加UI image，并放置在相对于摄像机的合适位置上：

![](./3dg-hw3/3.png)

运行结果截图如下：

![](./3dg-hw3/4.png)

**代码地址：**

**视频地址：**

### 2 编程实践

#### 2.1 游戏脚本阅读

>Priests and Devils
>
>Priests and Devils is a puzzle game in which you will help the Priests and Devils to cross the river within the time limit. There are 3 priests and 3 devils at one side of the river. They all want to get to the other side of this river, but there is only one boat and this boat can only carry two persons each time. And there must be one person steering the boat from one side to the other side. In the flash game, you can click on them to move them and click the go button to move the boat to the other direction. If the priests are out numbered by the devils on either side of the river, they get killed and the game is over. You can try it in many > ways. Keep all priests alive! Good luck!

#### 2.2 技术要求

- play the game ( http://www.flash-game.net/game/2535/priests-and-devils.html )
- 列出游戏中提及的事物（Objects）
- 用表格列出玩家动作表（规则表），注意，动作越少越好
- 请将游戏中对象做成预制
- 在 GenGameObjects 中创建 长方形、正方形、球 及其色彩代表游戏中的对象
- 使用 C# 集合类型 有效组织对象
- 整个游戏仅 主摄像机 和 一个 Empty 对象，**其他对象必须代码动态生成**。整个游戏不许出现 Find 游戏对象， SendMessage 这类突破程序结构的通讯耦合语句
- 请使用课件架构图编程，**不接受非 MVC 结构程序**
- 注意细节，例如：船未靠岸，牧师与魔鬼上下船运动中，均不能接受用户事件

#### 2.3 游戏说明

**游戏规则与设定**

- 帮助三个牧师和三个魔鬼渡河。
- 船上最多可以载两名游戏角色。
- 船上需要有游戏对象才可移动。
- 当有一侧岸的魔鬼数多余牧师数时，魔鬼就会失去控制，吃掉牧师。
- 一侧岸的魔鬼数和牧师数数量统计包括岸上的以及靠岸船上的。
- 若有牧师被吃掉，则游戏失败。
- 所有游戏角色都到达对岸，则游戏胜利。

**游戏对象**

船（褐色长方体），牧师（白色方块），魔鬼（黑色圆球）、岸、河

**玩家动作表**

| 玩家动作 | 发生条件                           |
| :------: | ---------------------------------- |
|   上船   | 船上有空位时，点击上船对象可以上船 |
|   下船   | 船上有对象时，点击下船对象可以下船 |
|   开船   | 船在一岸时点击船可以开船到另一岸   |
| 重新开始 | 点击 Restart 按钮可重新开始        |

#### 2.4 完成情况

游戏界面如下：



项目结构如下：



核心代码如下：



#### 2.5 项目地址

**代码地址：**

**视频地址：**

### 3 思考题

> 使用向量与变换，实现并扩展 Tranform 提供的方法，如 Rotate、RotateAround 等

Rotate:

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float a = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion q = Quaternion.AngleAxis(a * Time.deltaTime, Vector3.up);
        transform.localRotation *= q;
    }
}
```

RotateAround:

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 v3 = Vector3(55,5,5);
        float a = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion T = Quaternion.LookRotation(v3 - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, T, Time.deltaTime * a);
    }
}
```

## 参考资料

[1] [空间与运动_教学讲义](https://pmlpml.github.io/unity3d-learning/03-space-and-motion)

[2] [Maunal](https://docs.unity3d.com/Manual/index.html)

[3] [Vector3.Slerp_1](http://ask.manew.com/question/37261)

[4]  [Vector3.Slerp_2](http://www.manew.com/thread-43314-1-1.html)