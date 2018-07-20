using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.Msg;

namespace Client.Data
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DataBase
    {
        public abstract void ProcessEvent(MsgBase msg,Action<MsgBase> CallBack );
        public virtual void InitData() { }
    }
}