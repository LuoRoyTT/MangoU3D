
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mango.Framework.UI
{
    public delegate void ValueChangeAction(object oldValue,object newValue);
    public class BindableProperty:IEnumerable 
    {

        private ValueChangeAction OnValueChanged;
        private ValueChangeAction OnChildValueChanged;
        //private UnityEvent<T,T> OnValueChanged;
        private List<BindableProperty> chlidCollection;
        public int Count
        {
            get
            {
                return chlidCollection.Count;
            }
        }
        public BindableProperty GetChlid(int index)
        {
            return chlidCollection[index];
        }
        public int IndexOf(BindableProperty item)
        {
            return chlidCollection.IndexOf(item);
        }
        public int IndexOf(object item)
        {
            return chlidCollection.FindIndex(a=>a.Equals(item));
        }
        public void Add(object item)
        {
            BindableProperty bindableProperty = new BindableProperty(item,OnChildValueChanged);
            chlidCollection.Add(bindableProperty);
        }
        public void Remove(BindableProperty item)
        {
            item.Clear();
            chlidCollection.Remove(item);
        }
        public void RemoveAt(int index)
        {
            BindableProperty item = GetChlid(index);
            item.Clear();
            chlidCollection.RemoveAt(index);
        }
        public void RemoveAll()
        {
            foreach (var item in chlidCollection)
            {
                item.Clear();
            }
            chlidCollection.Clear();
        }
        public void RemoveAll(Predicate<BindableProperty> match)
        {
            foreach (var item in chlidCollection.FindAll(match))
            {
                item.Clear();
            }
            chlidCollection.RemoveAll(match);
        }
        public BindableProperty(ValueChangeAction onValueChanged)
        {
            OnValueChanged+=onValueChanged;
        }
        public BindableProperty(object value,ValueChangeAction onValueChanged)
        {
            _value = value;
            OnValueChanged+=onValueChanged;
        }
        public BindableProperty(object value,ValueChangeAction onValueChanged,ValueChangeAction onChildValueChanged)
        {
            if(value is IEnumerable)
            {
                Clear();
                OnValueChanged+=onValueChanged;
                Value=value;
                OnChildValueChanged=onChildValueChanged;
                foreach (var item in value as IEnumerable)
                {
                    BindableProperty bindableProperty = new BindableProperty(item,OnChildValueChanged);
                    chlidCollection.Add(bindableProperty);
                }
            }   
        }
        private object _value = default(object);
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!Equals(_value, value))
                {
                    var oldValue = _value;
                    _value = value;
                    if (OnValueChanged != null)
                        OnValueChanged.Invoke(oldValue,value);
                }
            }
        }

        public void AddListener(ValueChangeAction onValueChanged)
        {
            OnValueChanged+=onValueChanged;
        }

        public void RemoveListener(ValueChangeAction onValueChanged)
        {
            OnValueChanged-=onValueChanged;
        }

        public void RemoveAllListeners()
        {
            OnValueChanged=null;
        }

        public override string ToString()
        {
            return (Value != null ? Value.ToString() : "null");
        }

        public void Clear()
        {
            _value = default(object);
            if(OnValueChanged!=null) OnValueChanged=null;
            chlidCollection.Clear();
            if(OnChildValueChanged!=null) OnChildValueChanged=null;

        }

        public IEnumerator GetEnumerator()
        {
            return chlidCollection.GetEnumerator();
        }

    }
}