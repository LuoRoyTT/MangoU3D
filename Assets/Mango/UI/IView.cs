using Mango.Core;
using Mango.Resload;
using UnityEngine;

namespace Mango.UI
{
    public  interface IView 
    {
        bool Vaild{get;}
        void Enter();
        void Exit();
    }
}