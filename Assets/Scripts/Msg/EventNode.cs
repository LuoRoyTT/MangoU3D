using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.Data;

namespace Client.Msg
{
    public class EventNode
    {
        public DataBase data;
        public EventNode next;
        public EventNode(DataBase data)
        {
            this.data = data;
            next = null;
        }
    }
}
