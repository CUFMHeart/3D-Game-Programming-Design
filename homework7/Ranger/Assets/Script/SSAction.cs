using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Interfaces;

// SSAction\CCMoveToAction\SSActionManager\CCTracertAction
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
    public Vector3 pos;
    public float speed;
    public int count;

    private CCMoveToAction() { }

    public static CCMoveToAction getAction(int count, float speed, Vector3 position)
    {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.pos = position;
        action.speed = speed;
        action.count = count;
        return action;
    }

    public override void Start()
    {
        Quaternion rotation = Quaternion.LookRotation(pos - transform.position, Vector3.up);
        transform.rotation = rotation;
    }

    public override void Update()
    {
        //  移动
        if (this.transform.position == pos)
        {
            destroy = true;
            CallBack.SSActionCallback(this);
        }
        this.transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
    }
}

public class SSActionManager : MonoBehaviour
{
    private Dictionary<int, SSAction> actionDict = new Dictionary<int, SSAction>();
    private List<SSAction> actionWating = new List<SSAction>();
    private List<int> actionFinishing = new List<int>();

    public void addAction(GameObject gameObject, SSAction action, SSActionCallback ICallBack)
    {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.CallBack = ICallBack;
        actionWating.Add(action);
        action.Start();
    }

    protected void Update()
    {
        foreach (SSAction actionTemp in actionWating)
        {
            actionDict[actionTemp.GetInstanceID()] = actionTemp;
        }
        actionWating.Clear();
        foreach (KeyValuePair<int, SSAction> kv in actionDict)
        {
            SSAction actionTemp = kv.Value;
            if (actionTemp.destroy)
            {
                actionFinishing.Add(actionTemp.GetInstanceID());
            }
            else if (actionTemp.enable)
            {
                actionTemp.Update();
            }
        }
        foreach (int index in actionFinishing)
        {
            SSAction actionTemp = actionDict[index];
            actionDict.Remove(index);
            DestroyObject(actionTemp);
        }
        actionFinishing.Clear();
    }
}

public class CCTracertAction : SSAction
{
    public GameObject gameObejectTemp;
    public float speed;

    private CCTracertAction() { }
    public static CCTracertAction getAction(GameObject gameObejectTemp, float speed)
    {
        CCTracertAction action = ScriptableObject.CreateInstance<CCTracertAction>();
        action.gameObejectTemp = gameObejectTemp;
        action.speed = speed;
        return action;
    }

    public override void Start()
    {
    }

    public override void Update()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, gameObejectTemp.transform.position, speed * Time.deltaTime);
        Quaternion rotation = Quaternion.LookRotation(gameObejectTemp.transform.position - gameObject.transform.position, Vector3.up);
        gameObject.transform.rotation = rotation;
        if (gameObject.GetComponent<Ranger>().isTracerting == false||transform.position == gameObejectTemp.transform.position)
        {
            destroy = true;
            CallBack.SSActionCallback(this);
        }
    }
}
