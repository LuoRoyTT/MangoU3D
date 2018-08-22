using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Client.Async
{
    public class CoroutineAsyncObject : IAsyncObject
    {
        private IEnumerator ie;
        public IAsyncObject Next { get; set; }

        public void Init()
        {

        }
        public void Compelete()
        {
            if(Next!=null)
            {
                Next.Start();
            }
        }

        public void Start()
        {
            AsyncCenter.Instance.GotoNext(ie,Compelete);
        }
    }
}

