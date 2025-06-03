using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace MyTest
{
    //观察者模式的事件中心
    public class EventCenter
    {
        private static EventCenter instance;

        public static EventCenter Instance
        {
            get
            {
                if (instance == null)
                {
                    return new EventCenter();
                }
                return instance;
            }
        }
        protected EventCenter() { }
        //设置为单例可以直接实例化
        public Dictionary<string, IEventInfo> eventMachine = new Dictionary<string, IEventInfo>();

        public void AddEventListener(string name, UnityAction action)
        {
            //事件存在则将委托添加到对应注册表
            //事件不存在则重新创造事件
            if (eventMachine.ContainsKey(name))
            {
                (eventMachine[name] as EventInfo).actions += action;
            }
            else
            {
                //向下转化父类一定要装载子类作为实例化
                eventMachine.Add(name, new EventInfo(action));
            }
        }
        //有参事件的添加
        public void AddEventListener<T>(string name, UnityAction<T> action)
        {
            //事件存在则将委托添加到对应注册表
            //事件不存在则重新创造事件
            if (eventMachine.ContainsKey(name))
            {
                (eventMachine[name] as EventInfo<T>).actions += action;
            }
            else
            {
                //向下转化父类一定要装载子类作为实例化
                eventMachine.Add(name, new EventInfo<T>(action));
            }
        }

        public void EventTrigger(string name)
        {
            if (eventMachine.ContainsKey(name))
            {
                (eventMachine[name] as EventInfo)?.actions.Invoke();
            }
        }
        public void EventTrigger<T>(string name, T info)
        {
            if (eventMachine.ContainsKey(name))
            {
                (eventMachine[name] as EventInfo<T>)?.actions.Invoke(info);
            }
        }

        public void ClearEventListener(string name, UnityAction action)
        {
            if (eventMachine.ContainsKey(name))
            {
                if ((eventMachine[name] as EventInfo).actions != null)
                { (eventMachine[name] as EventInfo).actions -= action; }
            }
        }
        public void ClearEventListener<T>(string name, UnityAction<T> action)
        {
            if (eventMachine.ContainsKey(name))
            {
                if ((eventMachine[name] as EventInfo<T>).actions != null)
                { (eventMachine[name] as EventInfo<T>).actions -= action; }
            }
        }

        public void ClearAll()
        {
            eventMachine.Clear();
        }


    }
    //作为接口,方便扩展有参与无参的EventInfo
    public interface IEventInfo
    {

    }
    //无参的EventInfo
    public class EventInfo : IEventInfo
    {
        public UnityAction actions;

        public EventInfo(UnityAction action)
        {
            this.actions += action;
        }
    }
    //有参事件的添加
    public class EventInfo<T> : IEventInfo
    {
        public UnityAction<T> actions;

        public EventInfo(UnityAction<T> action)
        {
            this.actions += action;
        }
    }
}