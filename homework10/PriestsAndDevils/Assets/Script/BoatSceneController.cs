using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class BoatSceneController
{
    readonly GameObject boat;
    // readonly Moveable move;
    readonly Vector3[] position1;
    readonly Vector3[] position2;
    readonly Vector3 boatPosition1;
    readonly Vector3 boatPosition2;
    readonly float speed = 30;
    int State; // coast1-1,coast2-2
    int moveState;  // static-0, boat-moving-1
    public GameObjects[] objectsOnBoat = new GameObjects[2];

    public BoatSceneController()
    {
        State = 1;
        moveState = 0;
        //  存储coast1位置的两个船上的空位-物理位置
        position1 = new Vector3[] { new Vector3(4.5F, 1.5F, 0), new Vector3(5.5F, 1.5F, 0) };
        //  存储coast2位置的两个船上的空位-物理位置
        position2 = new Vector3[] { new Vector3(-5.5F, 1.5F, 0), new Vector3(-4.5F, 1.5F, 0) };
        //  存储coast1位置的船上的物理位置
        boatPosition1 = new Vector3(5, 1, 0);
        boatPosition2 = new Vector3(-5, 1, 0);
        boat = Object.Instantiate(Resources.Load<GameObject>("Prefabs/boat"), new Vector3(5, 1, 0), Quaternion.identity);
        boat.name = "boat";
        // move = boat.AddComponent(typeof(Moveable)) as Moveable;
        boat.AddComponent(typeof(ClickGUI));
    }

    //  update
    public int GetState()
    {
        return State;
    }

    public int GetMoveState()
    {
        return moveState;
    }

    public void ChangeState()
    {
        State = 3 - State;
    }

    public void ChangeMoveState()
    {
        moveState = 1 - moveState;
    }

    public Vector3 GetDestination()
    {
        if (State == 1)
            return boatPosition2;
        else
            return boatPosition1;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public bool isEmpty()
    {
        //  返回船上是否为空
        //  注意可能第一个位置上空，第二个位置上有
        for (int i = 0; i < objectsOnBoat.Length; i++)
        {
            if (objectsOnBoat[i] != null)
            {
                return false;
            }
        }
        return true;
    }

    public int getEmptyIndex()
    {
        //  返回船上的空位位置，0位或1位
        for (int i = 0; i < objectsOnBoat.Length; i++)
        {
            if (objectsOnBoat[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public Vector3 getEmptyPosition()
    {
        //  返回空位的物理位置
        Vector3 pos;
        int emptyIndex = getEmptyIndex();
        if (State == 1)
        {
            pos = position1[emptyIndex];    // coast1
        }
        else
        {
            pos = position2[emptyIndex];    // coast2
        }
        return pos;
    }

    public void GetOnBoat(GameObjects item)
    {
        //  坐船，把object放到数组里
        int index = getEmptyIndex();
        objectsOnBoat[index] = item;
    }

    public GameObjects GetOffBoat(string item_name)
    {
        //  下船，把object从数组里拿出
        for (int i = 0; i < objectsOnBoat.Length; i++)
        {
            if (objectsOnBoat[i] != null && objectsOnBoat[i].getName() == item_name)
            {
                GameObjects item = objectsOnBoat[i];
                objectsOnBoat[i] = null;
                return item;
            }
        }
        return null;
    }

    public GameObject GetGameobject()
    {
        //  船
        return boat;
    }

    public int[] GetobjectsNumber()
    {
        //  计算船上的牧师数目和魔鬼数目
        //  0-d，1-p
        int[] count = { 0, 0 };
        for (int i = 0; i < objectsOnBoat.Length; i++)
        {
            if (objectsOnBoat[i] == null)
                continue;
            if (objectsOnBoat[i].getType() == 0)
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

    // public void boatMove()
    // {
    //     if (State == 2)
    //     {
    //         move.SetDest(new Vector3(5, 1, 0));
    //         State = 1;
    //     }
    //     else
    //     {
    //         move.SetDest(new Vector3(-5, 1, 0));
    //         State = 2;
    //     }
    // }

    public void Reset()
    {
        State = 1;
        moveState = 0;
        boat.transform.position = boatPosition1;
        objectsOnBoat = new GameObjects[2];
    }
}