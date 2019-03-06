  
  namespace Mango.Framework.Event
  {
      

    public class MangoEvent
    {
        private MangoDelegate action;
        public void AddListener(MangoDelegate listener)
        {
            action += listener; 
        }
        public void RemoveListener(MangoDelegate listener)
        {
            action -= listener;
        }
        public void RemoveAllListeners()
        {
            action = null;
        }
        public void Call()
        {
            action();
        }
    }

    public class MangoEvent<T>
    {
        private MangoDelegate<T> action;
        public void AddListener(MangoDelegate<T> listener)
        {
            action += listener; 
        }
        public void RemoveListener(MangoDelegate<T> listener)
        {
            action -= listener;
        }
        public void RemoveAllListeners()
        {
            action = null;
        }
        public void Call(T arg)
        {
            action(arg);
        }
    }

    public class MangoEvent<T1,T2>
    {
        private MangoDelegate<T1,T2> action;
        public void AddListener(MangoDelegate<T1,T2> listener)
        {
            action += listener; 
        }
        public void RemoveListener(MangoDelegate<T1,T2> listener)
        {
            action -= listener;
        }
        public void RemoveAllListeners()
        {
            action = null;
        }
        public void Call(T1 arg1,T2 arg2)
        {
            action(arg1,arg2);
        }
    }
  }