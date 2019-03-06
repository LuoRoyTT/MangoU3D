using Mango.Framework.Core;
using Mango.Framework.Resource;
using UnityEngine;

namespace Mango.Framework.UI
{
    public  interface IView 
    {
        bool Vaild{get;}
        void Enter();
        void Exit();
    }
}