using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class Action : SSActionManager, SSActionCallback
{
    public SSActionEventType comp = SSActionEventType.Completed;
    Dictionary<int, CCMoveToAction> actionDict = new Dictionary<int, CCMoveToAction>();
    //  SSActionCallback
    public void SSActionCallback(SSAction source)
    {
        if(actionDict.ContainsKey(source.gameObject.GetComponent<Ranger>().posNum)) actionDict.Remove(source.gameObject.GetComponent<Ranger>().posNum);
        Patrol(source.gameObject);
    }
    //  追踪
    public void Tracert(GameObject rangerTemp, GameObject player)
    {
        if (actionDict.ContainsKey(rangerTemp.GetComponent<Ranger>().posNum)) actionDict[rangerTemp.GetComponent<Ranger>().posNum].destroy = true;
        CCTracertAction actionTemp = CCTracertAction.getAction(player, 0.8f);
        addAction(rangerTemp.gameObject, actionTemp, this);
    }
    //  巡逻
    public void Patrol(GameObject rangerTemp)
    {
        CCMoveToAction actionTemp = CCMoveToAction.getAction(rangerTemp.GetComponent<Ranger>().posNum, 1.5f, GetNewPos(rangerTemp));
        actionDict.Add(rangerTemp.GetComponent<Ranger>().posNum, actionTemp);
        addAction(rangerTemp.gameObject, actionTemp, this);
    }
    //  位置更新
    private Vector3 GetNewPos(GameObject rangerTemp)
    {
        Vector3 pos = rangerTemp.transform.position;
        Vector3 posAdd = pos;
        Vector3 posNew = pos;
        int posNum = rangerTemp.GetComponent<Ranger>().posNum;
        float x1 = -5f + (posNum % 3) * 9f;
        float x2 = -13f + (posNum % 3) * 10f;
        float z1 = 13f - (posNum / 3) * 9.5f;
        float z2 = 5f - (posNum / 3) * 9.5f;
        posAdd = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        posNew = pos + posAdd;
        while (!(posNew.x<x1 && posNew.x>x2 && posNew.z<z1 && posNew.z > z2))
        {
            posAdd = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            posNew = pos + posAdd;
        }
        return posNew;
    }
    //  结束
    public void AllFinished()
    {
        foreach(CCMoveToAction x in actionDict.Values)
        {
            x.destroy = true;
        }
        actionDict.Clear();
    }
}