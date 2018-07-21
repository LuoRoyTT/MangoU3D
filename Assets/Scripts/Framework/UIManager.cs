using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Client.UI;
using Client.Event;
using Client.Core;

namespace Client.Framework
{
    public class UIManager : MonoSingleton<UIManager>
    {
        protected override void Awake()
        {
            Init();
        }
        protected override void Init()
        {
            base.Init();
        }

        
    }
}