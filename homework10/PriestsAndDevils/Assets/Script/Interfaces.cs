using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Interfaces
{
    // copy from ppt
    public enum SSActionEventType : int { Started, Completed }

    public interface ISceneController
    {
        void LoadResources();
    }

    public interface UserAction
    {
        void ClickBoat();
        void ClickObject(GameObjects Object);
        void NextStep();
        void Restart();
    }
    
    public interface SSActionCallback
    {
        void SSActionCallback(SSAction source);
    }
}