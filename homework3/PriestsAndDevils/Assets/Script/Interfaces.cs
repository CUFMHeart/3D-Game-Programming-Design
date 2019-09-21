using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Interfaces
{
    public interface ISceneController
    {
        void LoadResources();
    }

    public interface UserAction
    {
        void ClickBoat();
        void ClickObject(GameObjects Object);
        void Restart();
    }
}