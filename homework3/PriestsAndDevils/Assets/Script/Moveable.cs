using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class Moveable : MonoBehaviour
{
    readonly float Speed = 30;
    Vector3 position1;
    Vector3 position2;
    int state = 0;  // static-0, object-move-1, boat-moving-2
    bool flag = true;

    void Update()
    {
        if (state == 1)
        {
            if (flag)
            {
                transform.position = Vector3.MoveTowards(transform.position, position2, Speed * Time.deltaTime);
                if (transform.position == position2)
                {
                    flag = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, position1, Speed * Time.deltaTime);
                if (transform.position == position1)
                {
                    state = 0;
                }
            }
        }
        else if (state == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, position1, Speed * Time.deltaTime);
            if (transform.position == position1)
            {
                flag = true;
                state = 0;
            }
        }
    }

    public void SetDest(Vector3 pos)
    {
        if (state != 0) return;
        position1 = position2 = pos;
        flag = true;
        if (transform.position.y == position1.y)
        {
            state = 2;
        }
        else
        {
            state = 1;
            if (transform.position.y < position1.y)
            {
                position2.x = transform.position.x;
            }
            else if (transform.position.y > position1.y)
            {
                position2.y = transform.position.y;
            }
        }
    }

    public void Reset()
    {
        state = 0;
        flag = true;
    }
}