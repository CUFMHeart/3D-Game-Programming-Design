using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class FirstController : MonoBehaviour, ISceneController, UserAction
{
    private GameObjects[] GameObjects;
    // InteracteGUI UserGUI;
    public CoastSceneController coast1;
    public CoastSceneController coast2;
    public BoatSceneController boat;
    private Action action;

    void Awake()
    {
        //  GetComponent 需在外添加组件
        action = GetComponent<Action>();
        SSDirector director = SSDirector.getInstance();
        director.setFPS(60);
        director.currentScenceController = this;
        //  GameObjects 存放三个牧师与三个魔鬼
        // UserGUI = gameObject.AddComponent<InteracteGUI>() as InteracteGUI;
        GameObjects = new GameObjects[6];
        //  直接调用 FirstController 中的 LoadResources()
        //  director.currentScenceController.LoadResources();
        LoadResources();
    }

    public void LoadResources()
    {   
        GenGameObjects();
    }

    public void GenGameObjects()
    {
        //  boat
        boat = new BoatSceneController();
        //  coast
        coast1 = new CoastSceneController(1);
        coast2 = new CoastSceneController(2);
        //  priests and devils
        //  type: p-1, d-0
        for (int i = 0; i < 6; i++)
        {
            GameObjects s;
            if (i < 3)
            {
                s = new GameObjects(1); // priests
            }
            else
            {
                s = new GameObjects(0); // devils
            }
            s.setName(i);
            //  放在第一个河岸上
            s.setPosition(coast1.getEmptyPosition());
            s.getOnCoast(coast1);
            coast1.getOnCoast(s);
            GameObjects[i] = s;
        }
        //  river
        //  直接生成预制体Prefabs
        GameObject river = Object.Instantiate(Resources.Load<GameObject>("Prefabs/river"), new Vector3(0, 0.5F, 0), Quaternion.identity); 
        river.name = "river";
    }

    public void ClickBoat()
    {
        if (action.comp == SSActionEventType.Started || boat.isEmpty())
            return; // if (boat.isEmpty()) return;
        action.BoatMove(boat); // boat.boatMove();
        //  每次开船后检查一次胜负
        // UserGUI.SetState = Check();
    }

    public void ClickObject(GameObjects PorD)
    {
        if (action.comp == SSActionEventType.Started)
            return;
        //  下船
        if (PorD.isOnBoat())
        {
            CoastSceneController coast;
            //  boat 1-coast1, 2-coast2
            if (boat.GetState() == 1)
            {
                coast = coast1;
            }
            else
            {
                coast = coast2;
            }
            boat.GetOffBoat(PorD.getName());
            action.GameObjectsMove(PorD, coast.getEmptyPosition());
            //  PorD.moveToPosition(coast.getEmptyPosition());
            PorD.getOnCoast(coast);
            coast.getOnCoast(PorD);
        }
        //  上船
        else
        {                                   
            CoastSceneController coast = PorD.getCoastSceneController();
            //  船上没位置或者船不在这边，什么都不做
            if (boat.getEmptyIndex() == -1 || coast.GetState() != boat.GetState())
            {   
                return;
            }
            coast.getOffCoast(PorD.getName());
            action.GameObjectsMove(PorD, boat.getEmptyPosition());
            //  PorD.moveToPosition(boat.getEmptyPosition());
            PorD.getOnBoat(boat);
            boat.GetOnBoat(PorD);
        }
        //  在游戏胜利的条件下，不用开船也应该检查出胜负
        // UserGUI.SetState = Check();
    }

    int Check()
    {
        // 0-play, 1-win, 2-lose
        int p_coast1_num = 0;
        int d_coast1_num = 0;
        int p_coast2_num = 0;
        int d_coast2_num = 0;
        int[] coast1_arr = coast1.GetobjectsNumber();
        int[] coast2_arr = coast2.GetobjectsNumber();
        int[] boat_arr = boat.GetobjectsNumber();
        //  分别计算两侧岸上的人数
        d_coast1_num += coast1_arr[0];
        p_coast1_num += coast1_arr[1];
        d_coast2_num += coast2_arr[0];
        p_coast2_num += coast2_arr[1];
        //  判断是否胜利
        if (p_coast2_num + d_coast2_num == 6)      // win
            return 1;
        //  计算船上人数，累加进累加器
        if (boat.GetState() == 1)
        {
            d_coast1_num += boat_arr[0];
            p_coast1_num += boat_arr[1];
        }
        else
        {
            d_coast2_num += boat_arr[0];
            p_coast2_num += boat_arr[1];
        }
        //  判断是否失败
        //  检查coast1
        if (p_coast1_num < d_coast1_num && p_coast1_num > 0)
        {
            return 2;
        }
        //  检查coast2
        if (p_coast2_num < d_coast2_num && p_coast2_num > 0)
        {
            return 2;
        }
        return 0;
    }

    public void Restart()
    {
        // 重置每个对象
        boat.Reset();
        coast1.Reset();
        coast2.Reset();
        for (int i = 0; i < GameObjects.Length; i++)
        {
            GameObjects[i].Reset();
        }
    }
}