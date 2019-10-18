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
        void Hit(Vector3 pos);
        int GetScore();
        int GetRound();
        int GetMiss();
        bool GameFinish();
        void Restart();
    }
    
    public interface PhysicsEngineManager
    {
        void UfoMove(UFO ufo);
        bool IsAllFinished();
    }

    public interface SSActionCallback
    {
        void SSActionCallback(SSAction source);
    }
}