using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

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