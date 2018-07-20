using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Msg
{
    public class MsgBase
    {
        public ushort msgId;
        public ManagerID GetManager()
        {
            int tmpId = msgId / MsgHelper.MSG_SPAN;
            return (ManagerID)(tmpId * MsgHelper.MSG_SPAN);
        }
        public MsgBase(ushort msgId)
        {
            this.msgId = msgId;
        }
    }
    public class MsgHelper
    {
        public const int MSG_SPAN = 3000;
    }
}
