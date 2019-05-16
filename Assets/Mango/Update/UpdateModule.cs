using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Core;
using Mango.Event;
using UnityEngine;


namespace Mango.Update
{
    public class UpdateModule : GameModule
    {
        public override eModulePriority Priority
		{
			get
			{
				return eModulePriority.Last;
			}
		}
        private long currentFrame;
        private long currentStamp;
        private long lastStamp;
        private DateTime lastStampTime;
        private double standardStampDeltaTime;
        private double deltaStampTime;
        private bool doing;
        public MangoEvent onUpdate;
        public MangoEvent onSimulate;
        protected override void OnInit()
        {
            currentFrame = 0;
            currentStamp = 0;
            lastStamp = currentStamp;
            lastStampTime = DateTime.Now;
            doing = false;
        }
        public void GameUpdate()
        {
            Update();
        }
        private void Update()
        {
            currentFrame++;
            onUpdate.Call();
            if((DateTime.Now - lastStampTime).TotalMilliseconds < standardStampDeltaTime || doing)
            {
                return;
            }
            Simulate();
        }
        private void Simulate()
        {
            lastStamp = currentStamp;
            currentStamp++;
            onSimulate.Call();
            deltaStampTime = (DateTime.Now - lastStampTime).TotalMilliseconds;
            lastStampTime = DateTime.Now;
        }
    }
}

