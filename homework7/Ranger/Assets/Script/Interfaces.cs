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
        void Restart();
        int GetScore();
        bool GetGameState();
        void PlayerMove(float move_x, float move_z);
    }
    
    public interface SSActionCallback
    {
        void SSActionCallback(SSAction source);
    }
}