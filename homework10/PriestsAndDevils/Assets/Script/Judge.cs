using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class Judge : MonoBehaviour
{
    public int state;
    InteracteGUI UserGUI;
    private FirstController FC;

    void Awake()
    {
        state = 0; 
        UserGUI = gameObject.AddComponent<InteracteGUI>() as InteracteGUI;
        FC = GetComponent<FirstController>();
    }

    void Update()
    {
        Check();
        // Debug.Log("state: " + state);
        UserGUI.SetState = state;
    }

    public void setState(int sta)
    {
        state = sta;
    }

    public int getState()
    {
        return state;
    }

    public void Check()
    {
        // 0-play, 1-win, 2-lose
        int p_coast1_num = 0;
        int d_coast1_num = 0;
        int p_coast2_num = 0;
        int d_coast2_num = 0;
        int[] coast1_arr = FC.coast1.GetobjectsNumber();
        int[] coast2_arr = FC.coast2.GetobjectsNumber();
        int[] boat_arr = FC.boat.GetobjectsNumber();
        //  分别计算两侧岸上的人数
        d_coast1_num += coast1_arr[0];
        p_coast1_num += coast1_arr[1];
        d_coast2_num += coast2_arr[0];
        p_coast2_num += coast2_arr[1];
        //  判断是否胜利
        if (p_coast2_num + d_coast2_num == 6)      // win
        {
            state = 1;
            return;
        }
        //  计算船上人数，累加进累加器
        if (FC.boat.GetState() == 1)
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
            state = 2;
            return;
        }
        //  检查coast2
        if (p_coast2_num < d_coast2_num && p_coast2_num > 0)
        {
            state = 2;
            return;
        }
        state = 0;
    }
}