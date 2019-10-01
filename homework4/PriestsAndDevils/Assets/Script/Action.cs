using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class Action : SSActionManager, SSActionCallback
{
    public SSActionEventType comp = SSActionEventType.Completed;
    //  船的运动
    public void BoatMove(BoatSceneController boat)
    {
        // Debug.Log(boat.GetState());
        comp = SSActionEventType.Started;
        CCMoveToAction action = CCMoveToAction.getAction(boat.GetDestination(), boat.GetSpeed());
        addAction(boat.GetGameobject(), action, this);
        boat.ChangeState();
    }
    //  牧师和魔鬼的运动
    public void GameObjectsMove(GameObjects Object, Vector3 dest)
    {
        comp = SSActionEventType.Started;
        Vector3 pos1 = Object.getPosition();
        Vector3 pos2 = Object.getPosition();
        if (dest.y <= pos2.y)
        {
            pos2.x = dest.x;
        }
        else
        {
            pos2.y = dest.y;
        }
        SSAction action1 = CCMoveToAction.getAction(pos2, Object.getSpeed());
        SSAction action2 = CCMoveToAction.getAction(dest, Object.getSpeed());
        SSAction seq = CCSequenceAction.getAction(1, 0, new List<SSAction> { action1, action2 });
        this.addAction(Object.GetGameobject(), seq, this);
    }
    //  SSActionCallback
    public void SSActionCallback(SSAction source)
    {
        comp = SSActionEventType.Completed;
    }
}