using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Linq;

//事件的消息结构父类
public abstract class EventMessage
{
    protected EventMessage()
    {
    }
}

//非友元功能模块类之间事件监听管理
public class EventManage
{
    protected static EventManage instance;

    public static EventManage Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventManage();
            }
            return instance;
        }
    }

    protected Dictionary<object, Dictionary<System.Type, List<ICallback>>> listeners = new Dictionary<object, Dictionary<System.Type, List<ICallback>>>();
    protected int nFire = 0;
    protected List<CallbackData> pendingDeletes = new List<CallbackData>();
    protected List<CallbackData> pendingListens = new List<CallbackData>();

    public void AddListener<T>(GenericDelegate<T> func) where T : EventMessage
    {
        this.AddListener<T>(this, func);
    }

    public void AddListener<T>(object obj, GenericDelegate<T> func) where T : EventMessage
    {
        if (obj == null)
        {
            throw new ArgumentNullException("obj");
        }
        if (func == null)
        {
            throw new ArgumentNullException("func");
        }
        CallbackData item = new CallbackData
        {
            obj = obj,
            type = typeof(T),
            callback = new Callback<T>(func)
        };
        this.pendingListens.Add(item);
        this.ProcessPendingListens();
    }

    #region _Check
    public void DetectLeakedObjects()
    {
        foreach (object obj2 in this.Listeners.Keys)
        {
            if (!(obj2 is GameObject))
            {
                continue;
            }
            Dictionary<System.Type, List<ICallback>> dictionary = this.Listeners[obj2];
            GameObject obj3 = (GameObject)obj2;
            if (obj3 == null)
            {
                Debug.LogError("Deleted game object found in event table: ");
            }
            else
            {
                Debug.LogError("Active game object (" + obj3.name + ") found in event table");
            }
            foreach (KeyValuePair<System.Type, List<ICallback>> pair in dictionary)
            {
                foreach (ICallback callback in pair.Value)
                {
                    Debug.LogError("--> " + pair.Key.ToString() + " : " + callback.ReferencedFunction.ToString());
                }
            }
        }
    }

    public void DumpEventListeners()
    {
        object[] array = new object[this.Listeners.Count];
        this.Listeners.Keys.CopyTo(array, 0);
        Array.Sort(array, new ObjectNameComparer());
        foreach (object obj2 in array)
        {
            string name;
            Dictionary<System.Type, List<ICallback>> dictionary = this.Listeners[obj2];
            System.Type[] typeArray = new System.Type[dictionary.Count];
            dictionary.Keys.CopyTo(typeArray, 0);
            Array.Sort(typeArray, new ObjectNameComparer());
            GameObject obj3 = obj2 as GameObject;
            if (obj3 != null)
            {
                name = obj3.name;
            }
            else
            {
                name = obj2.ToString();
            }
            Debug.Log("Event Object:  " + name);
            foreach (System.Type type in typeArray)
            {
                foreach (ICallback callback in dictionary[type])
                {
                    Debug.Log("   --> " + type.ToString() + " == " + callback.ReferencedFunction.ToString());
                }
            }
        }
    }
    #endregion

    public void Fire(object sender, EventMessage msg)
    {
        this.FireEventWithFilter(this, sender, msg);
        this.FireEventWithFilter(sender, sender, msg);
        this.ProcessPendingDeletes();
        this.ProcessPendingListens();
    }

    protected void FireEventWithFilter(object filter, object sender, EventMessage msg)
    {
        this.nFire++;
        try
        {
            if (filter != null)
            {
                Dictionary<System.Type, List<ICallback>> dictionary = null;
                if (this.listeners.TryGetValue(filter, out dictionary))
                {
                    List<ICallback> list = null;
                    if (dictionary.TryGetValue(msg.GetType(), out list))
                    {
                        foreach (ICallback callback in list)
                        {
                            try
                            {
                                callback.Fire(msg);
                                continue;
                            }
                            catch (Exception exception)
                            {
                                Debug.LogError(exception);
                                continue;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception exception2)
        {
            Debug.LogError(exception2);
        }
        this.nFire--;
    }

    protected void ProcessPendingDeletes()
    {
        if ((this.nFire <= 0) && (this.pendingDeletes.Count > 0))
        {
            foreach (var deleteData in this.pendingDeletes)
            {
                Dictionary<System.Type, List<ICallback>> dictionary = null;
                if (this.listeners.TryGetValue(deleteData.obj, out dictionary))
                {
                    List<ICallback> list = null;
                    if (dictionary.TryGetValue(deleteData.type, out list))
                    {
                        list.RemoveAll((x) => x == deleteData.callback);
                        if (list.Count == 0)
                        {
                            dictionary.Remove(deleteData.type);
                        }
                    }
                    if (dictionary.Count == 0)
                    {
                        this.listeners.Remove(deleteData.obj);
                    }
                }
            }

            this.pendingDeletes.Clear();
        }
    }

    protected void ProcessPendingListens()
    {
        if ((this.nFire <= 0) && (this.pendingListens.Count > 0))
        {
            foreach (CallbackData data in this.pendingListens)
            {
                Dictionary<System.Type, List<ICallback>> dictionary = null;
                if (!this.listeners.TryGetValue(data.obj, out dictionary))
                {
                    dictionary = new Dictionary<System.Type, List<ICallback>>();
                    this.listeners.Add(data.obj, dictionary);
                }
                List<ICallback> list = null;
                if (!dictionary.TryGetValue(data.type, out list))
                {
                    list = new List<ICallback>();
                    dictionary.Add(data.type, list);
                }
                list.Add(data.callback);
            }
            this.pendingListens.Clear();
        }
    }

    public void RemoveListener<T>(GenericDelegate<T> func) where T : EventMessage
    {
        this.RemoveListener<T>(this, func);
    }

    public void RemoveListener<T>(object obj, GenericDelegate<T> func) where T : EventMessage
    {
        if (obj == null)
        {
            Debug.LogError("Null object in RemoveListener");
        }
        else
        {
            Dictionary<System.Type, List<ICallback>> dictionary = null;
            if (this.listeners.TryGetValue(obj, out dictionary))
            {
                List<ICallback> list = null;
                if (dictionary.TryGetValue(typeof(T), out list))
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        Callback<T> callback = list[i] as Callback<T>;
                        if ((callback != null) && (callback.func == func))
                        {
                            CallbackData item = new CallbackData
                            {
                                obj = obj,
                                type = typeof(T),
                                callback = list[i]
                            };
                            this.pendingDeletes.Add(item);
                        }
                    }
                }
            }
            this.ProcessPendingDeletes();
        }
    }

    public Dictionary<object, Dictionary<System.Type, List<ICallback>>> Listeners
    {
        get
        {
            return this.listeners;
        }
    }

    protected class Callback<T> : EventManage.ICallback where T : EventMessage
    {
        public EventManage.GenericDelegate<T> func;

        public Callback(EventManage.GenericDelegate<T> func)
        {
            this.func = func;
        }

        public void Fire(EventMessage msg)
        {
            this.func(msg as T);
        }

        public object ReferencedFunction
        {
            get
            {
#if !NETFX_CORE
                return (this.func.Target + ":  " + this.func.Method);
#else
                return (this.func.Target + ":  " + "unknow_func");
#endif
            }
        }
    }

    protected struct CallbackData
    {
        public object obj;
        public System.Type type;
        public EventManage.ICallback callback;
    }

    public delegate void GenericDelegate<T>(T arg) where T : EventMessage;

    public interface ICallback
    {
        void Fire(EventMessage msg);

        object ReferencedFunction { get; }
    }

    public class ObjectNameComparer : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            string name;
            string str2;
            GameObject obj2 = x as GameObject;
            if (obj2 != null)
            {
                name = obj2.name;
            }
            else
            {
                name = x.ToString();
            }
            obj2 = y as GameObject;
            if (obj2 != null)
            {
                str2 = obj2.name;
            }
            else
            {
                str2 = y.ToString();
            }
            return name.CompareTo(str2);
        }
    }
}