using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace MyTest
{
    //�۲���ģʽ���¼�����
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
        //����Ϊ��������ֱ��ʵ����
        public Dictionary<string, IEventInfo> eventMachine = new Dictionary<string, IEventInfo>();

        public void AddEventListener(string name, UnityAction action)
        {
            //�¼�������ί����ӵ���Ӧע���
            //�¼������������´����¼�
            if (eventMachine.ContainsKey(name))
            {
                (eventMachine[name] as EventInfo).actions += action;
            }
            else
            {
                //����ת������һ��Ҫװ��������Ϊʵ����
                eventMachine.Add(name, new EventInfo(action));
            }
        }
        //�в��¼������
        public void AddEventListener<T>(string name, UnityAction<T> action)
        {
            //�¼�������ί����ӵ���Ӧע���
            //�¼������������´����¼�
            if (eventMachine.ContainsKey(name))
            {
                (eventMachine[name] as EventInfo<T>).actions += action;
            }
            else
            {
                //����ת������һ��Ҫװ��������Ϊʵ����
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
    //��Ϊ�ӿ�,������չ�в����޲ε�EventInfo
    public interface IEventInfo
    {

    }
    //�޲ε�EventInfo
    public class EventInfo : IEventInfo
    {
        public UnityAction actions;

        public EventInfo(UnityAction action)
        {
            this.actions += action;
        }
    }
    //�в��¼������
    public class EventInfo<T> : IEventInfo
    {
        public UnityAction<T> actions;

        public EventInfo(UnityAction<T> action)
        {
            this.actions += action;
        }
    }
}