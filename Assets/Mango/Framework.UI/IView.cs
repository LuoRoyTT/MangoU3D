using Mango.Framework.Core;
using Mango.Framework.Resource;
using Mango.Framework.UI.Component;
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