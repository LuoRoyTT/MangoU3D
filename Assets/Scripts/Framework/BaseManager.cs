using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.Core;
using Client.Data;
using Client.Msg;
using System;

namespace Client.Framework
{
    public class BaseManager<T> : MonoSingleton<T> where T : MonoSingleton<T>
    {
        private Dictionary<ushort, EventNode> eventTree = new Dictionary<ushort, EventNode>();
        /// <summary>
        /// 单个脚本注册一个消息
        /// </summary>
        /// <param name="data">要注册的脚本</param>
        /// <param name="msgId">消息id</param>
        public void RegistMsg(DataBase data, ushort msgId)
        {
            EventNode eventNode = new EventNode(data);
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
        /// <param name="data">要注册的脚本</param>
        /// <param name="msgIds">多个msg</param>
        public void RegistMsg(DataBase data, params ushort[] msgIds)
        {
            for (int i = 0; i < msgIds.Length; i++)
            {
                RegistMsg(data, msgIds[i]);
            }
        }
        /// <summary>
        /// 注销单个脚本的一个消息事件
        /// </summary>
        /// <param name="data">要注销的脚本</param>
        /// <param name="msgId">消息id</param>
        public void UnRegistMsg(DataBase data, ushort msgId)
        {
            if (!eventTree.ContainsKey(msgId))
            {
                Debug.LogWarning("not contains id=" + msgId);
                return;
            }
            else
            {
                var tmp = eventTree[msgId];
                if (tmp.data == data)
                {
                    EventNode header = tmp;
                    if (header.next != null)
                    {
                        header.data = tmp.next.data;
                        header.next = tmp.next.next;
                    }
                    else
                    {
                        eventTree.Remove(msgId);
                    }
                }
                else
                {
                    while (tmp.next != null && tmp.next.data != data)
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
        /// <param name="data">要注销的脚本</param>
        /// <param name="msgIds">多个msg</param>
        public void UnRegistMsg(DataBase data, params ushort[] msgIds)
        {
            for (int i = 0; i < msgIds.Length; i++)
            {
                UnRegistMsg(data, msgIds[i]);
            }
        }
        public  void ProcessEvent(MsgBase msg,Action<MsgBase> CallBack)
        {
            if (!eventTree.ContainsKey(msg.msgId))
            {
                return;
            }
            else
            {
                EventNode tmp = eventTree[msg.msgId];
                while (tmp!=null)
                {
                    tmp.data.ProcessEvent(msg, CallBack);
                    tmp = tmp.next;
                }
            }
        }
    }
}
