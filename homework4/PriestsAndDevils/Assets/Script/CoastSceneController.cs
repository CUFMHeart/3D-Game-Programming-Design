using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class CoastSceneController
{
    readonly GameObject coast;
    readonly Vector3 coastPosition1;
    readonly Vector3 coastPosition2;
    readonly Vector3[] positions;
    readonly int State;    // 1-coast1， 2-coast2
    GameObjects[] obejcts;

    public CoastSceneController(int num)
    {
        //  存储两个coast的物理位置
        coastPosition1 = new Vector3(9, 1, 0);
        coastPosition2 = new Vector3(-9, 1, 0);
        //  存储coast1上6个object的物理位置
        positions = new Vector3[] {new Vector3(6.5F,2.25F,0), new Vector3(7.5F,2.25F,0), new Vector3(8.5F,2.25F,0), new Vector3(9.5F,2.25F,0), new Vector3(10.5F,2.25F,0), new Vector3(11.5F,2.25F,0)};
        //  存储6个object的位置
        obejcts = new GameObjects[6];
        //  init
        if (num == 1)
        {
            coast = Object.Instantiate(Resources.Load<GameObject>("Prefabs/coast"), new Vector3(9, 1, 0), Quaternion.identity);
            coast.name = "coast1";
            State = 1;
        }
        else
        {
            coast = Object.Instantiate(Resources.Load<GameObject>("Prefabs/coast"), new Vector3(-9, 1, 0), Quaternion.identity);
            coast.name = "coast2";
            State = 2;
        }
    }

    public int getEmptyIndex()
    {
        //  空位的索引号
        for (int i = 0; i < obejcts.Length; i++)
        {
            if (obejcts[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public Vector3 getEmptyPosition()
    {
        //  空位的物理位置
        Vector3 pos = positions[getEmptyIndex()];
        if (State == 2)
        {
            pos.x *= -1;
        }
        return pos;
    }

    public void getOnCoast(GameObjects Object)
    {
        //  上岸
        int index = getEmptyIndex();
        obejcts[index] = Object;
    }

    public GameObjects getOffCoast(string item_name)
    {
        //  上船
        for (int i = 0; i < obejcts.Length; i++)
        {
            if (obejcts[i] != null && obejcts[i].getName() == item_name)
            {
                GameObjects Obejct = obejcts[i];
                obejcts[i] = null;
                return Obejct;
            }
        }
        return null;
    }

    public int GetState()
    {
        return State;
    }

    public int[] GetobjectsNumber()
    {
        //  计算船上的牧师数目和魔鬼数目
        //  0-d，1-p
        int[] count = { 0, 0 };
        for (int i = 0; i < obejcts.Length; i++)
        {
            if (obejcts[i] == null)
                continue;
            if (obejcts[i].getType() == 0)
            {
                count[0]++;
            }
            else
            {
                count[1]++;
            }
        }
        return count;
    }

    public void Reset()
    {
        obejcts = new GameObjects[6];
    }
}