using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.Event
{

	public class MangoEvents 
	{
        public MangoEvents()
        {
            eventTree = new Dictionary<ushort, EventNode>();
        }
        private Dictionary<ushort, EventNode> eventTree;

        public void AddListener(ushort msgId,MangoDelegate callback)
        {
            if(Contains(msgId,callback))
            {
                return;
            }
            EventNode eventNode = new EventNode(callback);
            InternalAddListener(msgId,eventNode);
        }
        public void AddListener(ushort msgId,MangoDelegate<IMessage> callback1)
        {
            if(Contains(msgId,callback1))
            {
                return;
            }
            EventNode eventNode = new EventNode(callback1);
            InternalAddListener(msgId,eventNode);
        }
        
        private void InternalAddListener(ushort msgId,EventNode eventNode)
        {
            EventNode header;
            if(!eventTree.TryGetValue(msgId,out header))
            {
                eventTree.Add(msgId,eventNode);
            }
            else
            {
                EventNode tempNode = header;
                while(tempNode.next != null)
                {
                    tempNode = tempNode.next;
                }
                tempNode.next = eventNode;
            }
        }
        public void RemoveListener(ushort msgId,MangoDelegate callback)
        {
            if(!Contains(msgId,callback))
            {
                return;
            }
            EventNode eventNode = new EventNode(callback);
            InternalRemoveListener(msgId,eventNode);
        }
        public void RemoveListener(ushort msgId,MangoDelegate<IMessage> callback1)
        {
            if(!Contains(msgId,callback1))
            {
                return;
            }
            EventNode eventNode = new EventNode(callback1);
            InternalRemoveListener(msgId,eventNode);
        }
        private void InternalRemoveListener(ushort msgId,EventNode eventNode)
        {
            EventNode header;
            if(!eventTree.TryGetValue(msgId,out header))
            {
                eventTree.Add(msgId,eventNode);
            }
            else
            {
                if(header == eventNode)
                {
                    if(header.next != null)
                    {
                        header = header.next;
                    }
                    else
                    {
                        RemoveAllListeners(msgId);
                    }   
                }
                else
                {
                    EventNode tempNode = header;
                    while(tempNode.next != null)
                    {
                        if(tempNode.next == eventNode)
                        {
                            tempNode.next = tempNode.next.next;
                        }  
                    }
                    tempNode.next = eventNode;
                }
            }
        }
        public void RemoveAllListeners(ushort msgId)
        {
            eventTree.Remove(msgId);
        }

        public void Call(ushort msgId)
        {
            EventNode node;
            if(!eventTree.TryGetValue(msgId,out node))
            {
                Debug.LogWarning("not contains id=" + msgId);
                return;
            }
            else
            {
                node.Call();
                while(node.next != null)
                {
                    node = node.next;
                    node.Call();
                }
            }
        }

        public void Call(ushort msgId,IMessage message)
        {
            EventNode node;
            if(!eventTree.TryGetValue(msgId,out node))
            {
                Debug.LogWarning("not contains id=" + msgId);
                return;
            }
            else
            {
                node.Call(message);
                while(node.next != null)
                {
                    node = node.next;
                    node.Call(message);
                }
            }
        }
        
        public bool Contains(ushort msgId,MangoDelegate callback)
        {
            EventNode node;
            if(!eventTree.TryGetValue(msgId,out node))
            {
                Debug.LogWarning("not contains id=" + msgId);
                return false;
            }
            else
            {
                do
                {
                    if(node.SameAs(callback))
                    {
                        return true;
                    }
                    node = node.next;
                }
                while(node.next!=null);
                return node.SameAs(callback);
            }
        }
        public bool Contains(ushort msgId,MangoDelegate<IMessage> callback1)
        {
            EventNode node;
            if(!eventTree.TryGetValue(msgId,out node))
            {
                Debug.LogWarning("not contains id=" + msgId);
                return false;
            }
            else
            {
                do
                {
                    if(node.SameAs(callback1))
                    {
                        return true;
                    }
                    node = node.next;
                }
                while(node.next!=null);
                return node.SameAs(callback1);
            }  
        }
        public void Clear()
        {
            eventTree.Clear();
        }
	}
}
