using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        dire = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;
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