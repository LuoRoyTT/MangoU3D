using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.Msg;
using Client.Core;

namespace Client.Framework
{
    public class MsgCenter : MonoSingleton<MsgCenter>
    {
        //委托：消息传递
        public void ProcessEvent(MsgBase msg)
        {
            throw new System.NotImplementedException();
        }
    }


}
