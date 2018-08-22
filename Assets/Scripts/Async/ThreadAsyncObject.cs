using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Client.Data;
using UnityEngine;

namespace Client.Async
{
    public delegate void AsyncThreadDelegateFull(object param, Action next);
    public class ThreadAsyncObject : IAsyncObject
    {
        private AsyncThreadDelegateFull threadAction;
        IAsyncObject IAsyncObject.Next { get; set; }
        public void Init()
        {
            
        }
        public void Compelete()
        {
            
        }

        public void Start()
        {
            
        }
    }
}
