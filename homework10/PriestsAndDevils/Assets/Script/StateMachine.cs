using Interfaces;
using UnityEngine;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class StateMachine
{
    public static StateMachine stateMachine = new StateMachine();
    private enum OnBoat {empty, P, D, PP, DD, PD }
    public FirstController first_controller;
    private OnBoat nextOnBoat;
    private bool flag = true;
    // coast1-1,coast2-2
    private int count = 0;
    private int num = 0;
    private int ThisCoast;
    private int D_count;
    private int P_count;

    private StateMachine() {}

    public void move()
    {
        if (flag)
        {
            int[] coast1_num = first_controller.coast1.GetobjectsNumber();
            P_count = coast1_num[1];
            D_count = coast1_num[0];
            ThisCoast = first_controller.boat.GetState();
            if (count == 0)
            {
                nextOnBoat = getNextState();
                if ((int)nextOnBoat >= 3)
                    num = 2;
                else if ((int)nextOnBoat > 0)
                    num = 1;
                else
                    num = 0;
                count++;
            }
            Debug.Log(nextOnBoat);
            flag = false;
            getNextMove();
        }
    }

    public void restart()
    {
        count = 0;
        num = 0;
    }

    private void getNextMove()
    {
        if (num != 0 && count == 1)
        {
            if (nextOnBoat == OnBoat.D || nextOnBoat == OnBoat.DD)
            {
                D_boat();
            }
            else if (nextOnBoat == OnBoat.P || nextOnBoat == OnBoat.PP || nextOnBoat == OnBoat.PD)
            {
                P_boat();
            }
        }
        else if (num == 2 && count == 2)
        {
            if (nextOnBoat == OnBoat.DD || nextOnBoat == OnBoat.PD)
            {
                D_boat();
            }
            else if (nextOnBoat == OnBoat.PP)
            {
                P_boat();
            }
        }
        else if((num == 1 && count == 2) || (num == 2 && count == 3) || (num == 0 && count == 1))
        {
            first_controller.ClickBoat();
        }
        else if ((num == 1 && count >= 3) || (num == 2 && count >= 4) || (num == 0 && count >= 2))
        {
            OverMove();
            count--;
        }
        flag = true;
        count++;
    }

    private void OverMove()
    {
        if((P_count == 0 && D_count == 2) || (P_count == 0 && D_count == 0))
        {
            if (first_controller.boat.GetState() == 1)
                count = 0;
            else
            {
                foreach (var x in first_controller.boat.objectsOnBoat)
                {
                    if (x != null)
                    {
                        first_controller.ClickObject(x);
                        break;
                    }
                }
                if (first_controller.boat.isEmpty())
                    count = 0;
            }
        }
        else if (P_count == 0 && D_count == 1 && first_controller.boat.GetState() == 1)
        {
            count = 0;
        }
        else
        {
            foreach (var x in first_controller.boat.objectsOnBoat)
            {
                if (x != null && x.getType() == 0)
                {
                    first_controller.ClickObject(x);
                    count = 0;
                    break;
                }
            }
            if (count != 0)
            {
                foreach (var x in first_controller.boat.objectsOnBoat)
                {
                    if (x != null)
                    {
                        first_controller.ClickObject(x);
                        count = 0;
                        break;
                    }
                }
            }
        }
    }

    private OnBoat getNextState()
    {
        Debug.Log("DC:" + D_count);
        Debug.Log("PC:" + P_count);
        OnBoat next = OnBoat.empty;
        if (ThisCoast == 1)
        {
            if ((D_count == 3 && P_count == 3) || (D_count == 1 && P_count == 1))
            {
                next = OnBoat.PD;
            }
            else if ((D_count == 2 && P_count == 3) || (D_count == 3 && P_count == 0))
            {
                next = OnBoat.DD;
            }
            else if ((D_count == 1 && P_count == 3) || (D_count == 2 && P_count == 2))
            {
                next = OnBoat.PP;
            }
            else if ((D_count == 1 && P_count == 2) || (D_count == 2 && P_count == 1))
            {
                next = OnBoat.P;
            }
            else if ((D_count == 1 && P_count == 0) || (D_count == 3 && P_count == 2) || (D_count == 2 && P_count == 0))
            {
                Debug.Log("sssss");
                next = OnBoat.D;
            }
            else
                next = OnBoat.empty;
        }
        else
        {
            if ((D_count == 2 && P_count == 2) || (D_count == 1 && P_count == 3) || (D_count == 0 && P_count == 3) || (D_count == 1 && P_count == 0))
            {
                next = OnBoat.empty;
            }
            else if ((D_count == 2 && P_count == 3) || (D_count == 1 && P_count == 1) || (D_count == 2 && P_count == 0))
            {
                Debug.Log("sssss2");
                next = OnBoat.D;
            }
            else
                next = OnBoat.empty;
        }
        return next;
    }

    private void P_boat()
    {
        if(ThisCoast == 1)
        {
            foreach(var x in first_controller.coast1.obejcts)
            {
                if (x != null && x.getType() == 1)
                {
                    first_controller.ClickObject(x);
                    return;
                }
            }
        }
        else if(ThisCoast == 2)
        {
            foreach (var x in first_controller.coast2.obejcts)
            {
                if (x != null && x.getType() == 1)
                {
                    first_controller.ClickObject(x);
                    return;
                }
            }
        }
    }

    private void D_boat()
    {
        if (ThisCoast == 1)
        {
            foreach (var x in first_controller.coast1.obejcts)
            {
                if (x != null && x.getType() == 0)
                {
                    first_controller.ClickObject(x);
                    return;
                }
            }
        }
        else
        {
            foreach (var x in first_controller.coast2.obejcts)
            {
                if (x != null && x.getType() == 0)
                {
                    first_controller.ClickObject(x);
                    return;
                }
            }
        }
    }
}