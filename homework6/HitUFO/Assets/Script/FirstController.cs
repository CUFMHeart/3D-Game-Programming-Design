using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

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

    public bool PhysicEngineFlag = false;
    public bool PhysicEngineState = false;

    private PhysicsEngineManager action;
    private UFOFactory factory;

    void Awake()
    {
        //  GetComponent 需在外添加组件
        action = GetComponent<PhysicsEngineManager>();
        SSDirector director = SSDirector.getInstance();
        director.setFPS(60);
        director.currentScenceController = this;
        factory = UFOFactory.factory;
        GenGameObjects();
    }

    public void LoadResources()
    {   
        PhysicEngineState = PhysicEngineFlag;
        if (PhysicEngineFlag)
        {
            action = this.gameObject.AddComponent<PhysicsEngineAction>() as PhysicsEngineManager;
        }
        else
        {
            action = this.gameObject.AddComponent<Action>() as PhysicsEngineManager;
        }
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
                Debug.Log(round);
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
        //  homework6 改进
        if (state)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 pos = Input.mousePosition;
                Hit(pos);
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
                // Debug.Log("score:"+score+"  color:"+c);
                // Debug.Log("hit:" + hit.collider.gameObject + " stacktrace: " + new System.Exception());
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
        if (round > 4 && action.IsAllFinished())
        {
            if (PhysicEngineState)
            {
                Destroy(this.gameObject.GetComponent<PhysicsEngineAction>());
            }
            else
            {
                Destroy(this.gameObject.GetComponent<Action>());
            }
            factory.GetInSide();
            return true;
        }
        return false;
    }

    public void Restart()
    {
        score = 0;
        round = 1;
        state = true;
    }
}