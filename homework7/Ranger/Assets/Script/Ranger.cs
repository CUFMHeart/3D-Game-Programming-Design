using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  巡逻兵 基本属性
public class Ranger : MonoBehaviour
{
    //  巡逻兵所在的区块计数
    public int posNum;
    //  巡逻兵1-追踪/0-巡逻标志
    public bool isTracerting = false;
    //  刚体性质
    private void Start()
    {
        if (gameObject.GetComponent<Rigidbody>())
        {
            gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        }
    }
    //  处理碰撞
    void Update()
    {
        if (this.gameObject.transform.localEulerAngles.x != 0 || gameObject.transform.localEulerAngles.z != 0)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, gameObject.transform.localEulerAngles.y, 0);
        }
        if (gameObject.transform.position.y != 0)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        }
    }
}

//  巡逻兵 工厂模式
public class RangerFactory
{
    //  init
    public static RangerFactory ranger_factory = new RangerFactory();
    //  地图上的巡逻兵列表
    private Dictionary<int, GameObject> ranger_working = new Dictionary<int, GameObject>();
    //  九宫格地图上，巡逻兵的位置坐标
    int[] positionX = { -10, 2, 10 };
    int[] positionZ = { 10, 1, -10 };
    //  实质上在GetRanger()中完成
    private RangerFactory()
    {
    }
    //  创建巡逻兵
    public Dictionary<int, GameObject> GetRanger()
    {
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                GameObject ranger_temp = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Ranger"));
                //  给巡逻兵添加组件
                ranger_temp.AddComponent<Ranger>();
                //  决定巡逻兵的位置
                ranger_temp.transform.position = new Vector3(positionX[j], 0, positionZ[i]);
                ranger_temp.GetComponent<Ranger>().posNum = i * 3 + j;
                ranger_temp.SetActive(true);
                ranger_working.Add(i * 3 + j, ranger_temp);
            }
        }
        return ranger_working;
    }
    //  结束巡逻工作
    public void PatrolFinished()
    {
        for (int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                ranger_working[i * 3 + j].transform.position = new Vector3(positionX[j], 0, positionZ[i]);
            }
        }
    }
}
