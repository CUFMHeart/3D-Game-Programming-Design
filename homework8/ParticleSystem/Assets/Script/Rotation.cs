using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//  copy from homework2
//  完成行星的自转过程
public class Rotation : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
		
	}
	// Update is called once per frame
	void Update ()
    {
        //  自转速度随机
        this.transform.RotateAround(this.transform.position, Vector3.up, Random.Range(1, 2));
	}
}
