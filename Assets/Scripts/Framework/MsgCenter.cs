using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.Event;
using Client.Core;
using Client.Data;
using System;

namespace Client.Framework
{
    public class MsgCenter : MonoSingleton<MsgCenter>
    {
        private Dictionary<enEventID, EventNode> eventTree = new Dictionary<enEventID, EventNode>();
        /// <summary>
        /// 单个脚本注册一个消息
        /// </summary>
        /// <param name="script">要注册的脚本</param>
        /// <param name="msgId">消息id</param>
        public void RegistMsg(IProcessEvent script, enEventID msgId)
        {
            EventNode eventNode = new EventNode(script);
            if (!eventTree.ContainsKey(msgId))
            {
                eventTree.Add(msgId, eventNode);
            }
            else
            {
                var tmp = eventTree[msgId];
                while (eventTree[msgId].next != null)
                {
                    tmp = tmp.next;
                }
                tmp.next = eventNode;
            }
        }
        /// <summary>
        /// 一个脚本注册多个msg
        /// </summary>
        /// <param name="script">要注册的脚本</param>
        /// <param name="msgIds">多个msg</param>
        public void RegistMsg(IProcessEvent script, params enEventID[] msgIds)
        {
            for (int i = 0; i < msgIds.Length; i++)
            {
                RegistMsg(script, msgIds[i]);
            }
        }
        /// <summary>
        /// 注销单个脚本的一个消息事件
        /// </summary>
        /// <param name="script">要注销的脚本</param>
        /// <param name="msgId">消息id</param>
        public void UnRegistMsg(IProcessEvent script, enEventID msgId)
        {
            if (!eventTree.ContainsKey(msgId))
            {
                Debug.LogWarning("not contains id=" + msgId);
                return;
            }
            else
            {
                var tmp = eventTree[msgId];
                if (tmp.script == script)
                {
                    EventNode header = tmp;
                    if (header.next != null)
                    {
                        header.script = tmp.next.script;
                        header.next = tmp.next.next;
                    }
                    else
                    {
                        eventTree.Remove(msgId);
                    }
                }
                else
                {
                    while (tmp.next != null && tmp.next.script != script)
                    {
                        tmp = tmp.next;
                    }
                    if (tmp.next.next != null)
                    {
                        tmp.next = tmp.next.next;
                    }
                    else
                    {
                        tmp.next = null;
                    }
                }
            }
        }
        /// <summary>
        /// 注销一个脚本的多个msg
        /// </summary>
        /// <param name="script">要注销的脚本</param>
        /// <param name="msgIds">多个msg</param>
        public void UnRegistMsg(IProcessEvent script, params enEventID[] msgIds)
        {
            for (int i = 0; i < msgIds.Length; i++)
            {
                UnRegistMsg(script, msgIds[i]);
            }
        }
        // public  void ProcessEvent(MsgBase msg,Action<MsgBase> CallBack)
        // {
        //     if (!eventTree.ContainsKey(msg.msgId))
        //     {
        //         return;
        //     }
        //     else
        //     {
        //         EventNode tmp = eventTree[msg.msgId];
        //         while (tmp!=null)
        //         {
        //             tmp.data.ProcessEvent(msg, CallBack);
        //             tmp = tmp.next;
        //         }
        //     }
        // }
    }
}
