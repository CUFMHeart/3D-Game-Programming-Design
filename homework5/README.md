#3D Game 5 - 与游戏世界交互

> **It freed me to enter one of the most creative periods of my life**
>
> *— Steve Jobs, Stanford Report, June 14, 2005*

<!--more-->

## README

**博客地址**：https://sentimentalswordsman.github.io/2019/10/09/3dG5-与游戏世界交互/

**HitUFO视频链接**：https://www.bilibili.com/video/av70621955/

## 与游戏世界交互

### 游戏交互与创新

游戏交互是玩家体验的基础。这里**交互模型**定义为建立输入设备的输入与游戏世界中的对象行为之间的关系，以及游戏世界状态与玩家感知（视觉、听觉、触觉、嗅觉等之间的关系）。

游戏**典型的交互模型**包括：

- 角色扮演模型。即玩家扮演游戏世界中的一个对象，并实现游戏操纵杆及其按钮的操作与游戏对象行为之间的映射。这样，玩家就可以通过一个或多个设备控制游戏对象。
- 多视角交互模型。即为游戏世界构建多个视图，每个视图反映不同观察视角或游戏的状态，使得玩家可以在不同视图中选择合适的方式（如菜单、软件工具等）与游戏世界交互。
- 团队交互模型。在社交类游戏中，通常有视图反映协同玩家状态，如队友列表等，玩家可通过广播、视频、聊天室、私聊等即时沟通渠道实现协作。
- 竞技交互模式。具备竞技模式一般需要支持更多的游戏设备，以满足发烧玩家的需要。使用 180 度屏幕、7 声道杜比立体声、游戏鼠标等的玩家，显然都是游戏公司争夺的 VIP。
- 桌面、移动交互模式。以大众客户为目标，通常是小游戏或商业游戏。

**游戏创新**理念：

- 第一层：交互装备创新
- 第二层：机制创新
- 第三层：以客户为中心的创新

## 作业与练习

### 1 编程实践

#### 1.1 技术要求

- 使用带缓存的工厂模式管理不同飞碟的生产与回收，该工厂必须是场景单实例的！具体实现见参考资源 Singleton 模板类
- 近可能使用前面 MVC 结构实现人机交互与游戏模型分离

#### 1.2 游戏说明

**游戏内容**

一个简单的鼠标打飞碟（Hit UFO）游戏

**游戏规则与设定**

1. 游戏有 4 个 round，每个 round 都包括10 次 trial；
2. 每个 trial 的飞碟的色彩、大小、发射位置、速度、角度、同时出现的个数都可能不同。它们由该 round 的 ruler 控制；
3. 每个 trial 的飞碟有随机性，总体难度随 round 上升；
4. 鼠标点中得分，得分规则按色彩、大小、速度不同计算，规则可自由设定。

#### 1.3 完成情况

游戏界面如下：

![](./image/2.png)

![](./image/3.png)


#### 1.4 项目设计

首先创建需要的游戏元素，如飞碟的预制、Terrain等：

![](./image/5.png)

![](./image/1.png)

最终项目的文件结构如下：

![](./image/4.png)

关于代码编写，首先是继承上一次项目“牧师与魔鬼-动作分离版”的成功，实现部分的代码复用：

SSDirector.cs：实现动作基类。

```C#
public class SSDirector : System.Object {
    //  singltion instance
    private static SSDirector _instance;

    public ISceneController currentScenceController { get; set; }
    public bool running { get; set; }
    //  get instance anytime anywhere!
    public static SSDirector getInstance()
    {
        if (_instance == null)
        {
            _instance = new SSDirector();
        }
        return _instance;
    }

    public int getFPS()
    {
        return Application.targetFrameRate;
    }

    public void setFPS(int fps)
    {
        Application.targetFrameRate = fps;
    }
}
```

SSAction.cs：实现SSAction、CCMoveToAction和SSActionManager类。

```C#
// SSAction\CCMoveToAction\SSActionManager
// ppt-04、05

public class SSAction : ScriptableObject {

    public bool enable = true;
    public bool destroy = false;

    public GameObject gameObject;
    public Transform transform;
    public SSActionCallback CallBack;

    //
    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }

    //
    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}

public class CCMoveToAction : SSAction
{
    public float speedX;
    public float speedY = 0;

    private CCMoveToAction() { }
    public static CCMoveToAction getAction(float speed)
    {
        CCMoveToAction action = CreateInstance<CCMoveToAction>();
        action.speedX = speed;
        return action;
    }

    public override void Update()
    {
        //  抛物线运动，参照hw3
        this.transform.position += new Vector3(speedX * Time.deltaTime, -speedY * Time.deltaTime + (float)-0.5 * 10 * Time.deltaTime * Time.deltaTime, 0);
        speedY += 10 * Time.deltaTime;
        //  落地则摧毁
        if (transform.position.y <= 1)
        {
            destroy = true;
            CallBack.SSActionCallback(this);
        }
    }

    public override void Start()
    {
    }
}

public class SSActionManager : MonoBehaviour
{
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingToAdd = new List<SSAction>();
    private List<int> watingToDelete = new List<int>();

    protected void Update()
    {
        foreach (SSAction ac in waitingToAdd)
        {
            actions[ac.GetInstanceID()] = ac;
        }
        waitingToAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.destroy)
            {
                watingToDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable)
            {
                ac.Update();
            }
        }

        foreach (int key in watingToDelete)
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        watingToDelete.Clear();
    }

    public void addAction(GameObject gameObject, SSAction action, SSActionCallback ICallBack)
    {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.CallBack = ICallBack;
        waitingToAdd.Add(action);
        action.Start();
    }
}
```

Action.cs：该脚本需添加到 Empty 物体上，刻画了UFO的动作。

```C#
public class Action : SSActionManager, SSActionCallback
{
    //  EventType
    public SSActionEventType comp = SSActionEventType.Completed;
    //  UFO 计数器
    int MoveUfoCount = 0;
    //  UFO 的运动
    public void UfoMove(UFO ufo)
    {
        MoveUfoCount++;
        comp = SSActionEventType.Started;
        CCMoveToAction action = CCMoveToAction.getAction(ufo.speed);
        addAction(ufo.gameObject, action, this);
    }
    //  确保结束
    public bool IsAllFinished()
    {
        if (MoveUfoCount == 0)
            return true;
        else
            return false;
    }
    //  SSActionCallback
    public void SSActionCallback(SSAction source)
    {
        MoveUfoCount--;
        comp = SSActionEventType.Completed;
        source.gameObject.SetActive(false);
    }
}
```

FirstController.cs：场记，该脚本需添加到 Empty 物体上，实现了游戏的开始、进行、结束和点击事件。

```C#
public class FirstController : MonoBehaviour, ISceneController, UserAction
{
    int score = 0;
    int miss = 0;
    int ufoNum = 0;
    int hitNum =0;
    //  游戏有 4 个 round，每个 round 都包括 10 次 trial
    int round = 1;
    int trial = 0;
    //  计数器
    int updateCount = 0;
    bool state = false; //  1-playing

    private Action action;
    private UFOFactory factory;

    void Awake()
    {
        //  GetComponent 需在外添加组件
        action = GetComponent<Action>();
        SSDirector director = SSDirector.getInstance();
        director.setFPS(60);
        director.currentScenceController = this;
        factory = UFOFactory.factory;
        LoadResources();
    }

    public void LoadResources()
    {   
        GenGameObjects();
    }

    public void GenGameObjects()
    {

    }

    public int GetRound()
    {
        return round;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetMiss()
    {
        return miss;
    }

    void Start ()
    {
  
    }

    void Update ()
    {
        if (state)
        {
            updateCount++;
            if (updateCount >= 150)
            {
                if(factory == null)
                {
                    return;
                }
                trial++;
                UFO ufoOfThisRound = factory.GetUFO(round);
                action.UfoMove(ufoOfThisRound);
                ufoNum++;
                //  每个 round 都包括 10 次 trial
                if (trial == 10)
                {
                    round++;
                    trial = 0;
                    miss = ufoNum - hitNum;
                }
                updateCount = 0;
            }
        }
	}

    public void Hit(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] raycastHits;
        raycastHits = Physics.RaycastAll(ray);
        for (int i = 0; i < raycastHits.Length; i++)
        {
            RaycastHit hit = raycastHits[i];
            if (hit.collider.gameObject.GetComponent<UFO>() != null)
            {
                hitNum++;
                //  颜色不同，得分不同
                Color c = hit.collider.gameObject.GetComponent<Renderer>().material.color;
                if (c == Color.red)
                    score += 1;
                if (c == Color.yellow)
                    score += 2;
                if (c == Color.black)
                    score += 3;
                //  根据 round 可以加分
                // if (round > 2)
                //     score += 1;
                hit.collider.gameObject.transform.position = new Vector3(0, -100, 0);
            }
        }
    }

    public bool GameFinish()
    {
        if (round > 4)
        {
            state = false;
            return action.IsAllFinished();
        }
        else
            return false;
    }

    public void Restart()
    {
        score = 0;
        round = 1;
        state = true;
    }
}
```

UFO.cs：采用了工厂模式来实现UFO的生产以及复用。

```C#
//  UFO 基本属性
public class UFO : MonoBehaviour
{
    public float speed 
    { 
        get;
        set; 
    }
    public Color color 
    {
        get { return gameObject.GetComponent<Renderer>().material.color; }
        set { gameObject.GetComponent<Renderer>().material.color = value; }
    }
    public Vector3 StartPoint
    {
        get { return gameObject.transform.position; } 
        set { gameObject.transform.position = value; } 
    }
    public Vector3 Direction
    {
        get { return Direction; }
        set { gameObject.transform.Rotate(value); }
    }
}

//  UFO 工厂模式
public class UFOFactory
{
    //  init
    public static UFOFactory factory = new UFOFactory();
    public GameObject ufotemp;
    //  outside of UFOFactory
    private Dictionary<int, UFO> outUFO = new Dictionary<int, UFO>();
    //  inside of  UFOFactory
    private List<UFO> inUFO = new List<UFO>();

    private UFOFactory()
    {
        ufotemp = Object.Instantiate(Resources.Load<GameObject>("Prefabs/UFO"));
        //  给 UFO 添加上组件
        ufotemp.AddComponent<UFO>();
        ufotemp.SetActive(false);
    }

    //  根据 round 值，拿到对应的 UFO
    public UFO GetUFO(int round)  
    {
        //  round: 1, 2, 3, 4
        int ufoType;    //  0~4, easy~hard
        float dire;
        GetInSide();
        GameObject objectTemp = null;
        UFO ufo;
        //  从 UFOFactory 中，拿出一个 UFO
        if (inUFO.Count > 0)
        {
            objectTemp = inUFO[0].gameObject;
            inUFO.Remove(inUFO[0]);
            objectTemp.SetActive(true);
            ufo = objectTemp.AddComponent<UFO>();
        }
        else
        {
            objectTemp = Object.Instantiate(ufotemp, Vector3.zero, Quaternion.identity);
            objectTemp.SetActive(true);
            ufo = objectTemp.AddComponent<UFO>();
        }
        //  round -> UFO Type
        if (round == 1)
        {
            ufoType = Random.Range(0, 2);   //  0,1
        }
        else if (round == 2)
        {
            ufoType = Random.Range(0, 3);   //  0,1,2
        }
        else if (round == 3)
        {
            ufoType = Random.Range(1, 4);   //  1,2,3
        }
        else
        {
            ufoType = Random.Range(2, 5);   //  2,3,4
        } 
        //  UFO Type -> UFO
        switch (ufoType)  
        {          
            case 0:  
                {  
                    ufo.color = Color.red;  
                    ufo.speed = Random.Range(20, 30);  
                    ufo.StartPoint = new Vector3(Random.Range(-120, -90), Random.Range(30, 90), Random.Range(50, 70));
                    break;  
                }  
            case 1:  
                {  
                    ufo.color = Color.yellow;  
                    ufo.speed = -Random.Range(20, 30);  
                    ufo.StartPoint = new Vector3(Random.Range(90, 120), Random.Range(40, 80), Random.Range(40, 60));
                    break;  
                }  
            case 2:  
                {  
                    ufo.color = Color.black;  
                    ufo.speed = Random.Range(25, 35);  
                    ufo.StartPoint = new Vector3(Random.Range(-120, -190), Random.Range(30, 90), Random.Range(50, 70));
                    break;  
                }
            case 3:
                {
                    ufo.color = Color.red;
                    ufo.speed = -Random.Range(30, 40); 
                    ufo.StartPoint = new Vector3(Random.Range(90, 120), Random.Range(40, 80), Random.Range(40, 60));
                    break;
                }
            default:
                {
                    ufo.color = Color.red;  
                    ufo.speed = Random.Range(30, 40);  
                    ufo.StartPoint = new Vector3(Random.Range(-120, -190), Random.Range(30, 90), Random.Range(50, 70));
                    break;
                }
        }
        dire = UnityEngine.Random.Range(-1f, 1f) < 0 ? -2 : 2;
        ufo.Direction = new Vector3(dire, 0, 10);
        outUFO.Add(ufo.GetInstanceID(), ufo);
        ufo.name = ufo.GetInstanceID().ToString();
        return ufo;  
    }

    //  放回 UFOFactory
    public void GetInSide()
    {
        foreach (UFO ufo in outUFO.Values)
        {
            if (!ufo.gameObject.activeSelf)
            {
                inUFO.Add(ufo);
                outUFO.Remove(ufo.GetInstanceID());
                return;
            }
        }
    }
}
```

Interfaces.cs：接口。

```C#
namespace Interfaces
{
    // copy from ppt
    public enum SSActionEventType : int { Started, Completed }

    public interface ISceneController
    {
        void LoadResources();
    }

    public interface UserAction
    {
        void Hit(Vector3 pos);
        int GetScore();
        int GetRound();
        int GetMiss();
        bool GameFinish();
        void Restart();
    }
    
    public interface SSActionCallback
    {
        void SSActionCallback(SSAction source);
    }
}
```

InteracteGUI.cs：实现GUI。

```C#
public class InteracteGUI : MonoBehaviour
{
    UserAction user_act;
    private bool flag = false;
    private float slip;
    private float Now;
    int round = 1;
    private GUIStyle Style = new GUIStyle ();

	void Start ()
    {
        //  style init
        Style.fontSize = 16;
        Style.fontStyle = FontStyle.BoldAndItalic;
        Style.normal.textColor = Color.blue;
        Style.alignment = TextAnchor.MiddleCenter;
        //  user_act init
        user_act = SSDirector.getInstance().currentScenceController as UserAction;
        //  Time.time
        slip = Time.time;
    }

    private void OnGUI()
    {
        if (!flag)
            slip = Time.time;
        GUI.Label(new Rect(680, 30, 100, 70), "Score: " + user_act.GetScore().ToString(), Style);
        GUI.Label(new Rect(680, 45, 100, 70), "Time: " + ((int)(Time.time - slip)).ToString(), Style);
        GUI.Label(new Rect(680, 60, 100, 70), "Round: " + round, Style);
        // GUI.Label(new Rect(680, 75, 100, 70), "Miss: " + user_act.GetMiss(), Style);
        if (!flag)
        {
            if (GUI.Button(new Rect(380, 200, 140, 70), "Start"))
            {
                slip = Time.time;
                flag = true;
                user_act.Restart();
            }
        }
        if (flag)
        {
            round = user_act.GetRound();
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 pos = Input.mousePosition;
                user_act.Hit(pos);
            }
            //  限制 round
            if (round > 4)
            {
                round = 4;
                if (user_act.GameFinish())
                {
                    flag = false;
                }
            }
        }
    }
}
```

### 2 编写一个简单的自定义 Component 

> 用自定义组件定义几种飞碟，做成预制
>
> - 参考官方脚本手册 https://docs.unity3d.com/ScriptReference/Editor.html
> - 实现自定义组件，编辑并赋予飞碟一些属性

自定义组件的实现要求可以在 Unity3D 的 Inspector 界面对飞碟的大小颜色分数等进行编辑。需要继承 Editor 类，还需要显示当前的属性值，由 OnEnable() 实现，而修改属性值则通过 OnInspectorGUI() 实现，分数是一个区间所以设置为一个滑动条，大小可以在0～10内拖动，而大小、颜色则有自适应的调整方式。

代码如下：

```C#
using UnityEngine;
using UnityEditor;
using System.Collections;
[CustomEditor(typeof(UFO))]
[CanEditMultipleObjects]

public class UfoEditor : Editor
{
	SerializedProperty score;  
	SerializedProperty scale;
	SerializedProperty color;    

	void OnEnable()
	{
		score = serializedObject.FindProperty("score");
		color = serializedObject.FindProperty("color");
		scale = serializedObject.FindProperty("scale");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		//	大小可以在0～10内拖动，表示一个UFO的分数
		EditorGUILayout.IntSlider(score, 0, 4, new GUIContent("score"));
		//	有自适应的调整方式
		EditorGUILayout.PropertyField(scale);
		EditorGUILayout.PropertyField(color);
		serializedObject.ApplyModifiedProperties();
	}
}
```

## 参考资料

[1] [与游戏世界交互_教学讲义](https://pmlpml.github.io/unity3d-learning/05-interaction-with-gameworld)

[2] [Maunal](https://docs.unity3d.com/Manual/index.html)

