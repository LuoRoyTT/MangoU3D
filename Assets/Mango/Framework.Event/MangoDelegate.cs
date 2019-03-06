  
  namespace Mango.Framework.Event
  {
    public delegate void MangoDelegate();
    public delegate void MangoDelegate<T>(T arg1);
    public delegate void MangoDelegate<T1,T2>(T1 arg1,T2 arg2);

 }