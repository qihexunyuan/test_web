using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTest;
public class TestMgr
{
    

    private static TestMgr instance;

    public static TestMgr Instance
    {
        get
        {
            if(instance==null)
            {
                instance = new TestMgr();
            }
            return instance;
        }
    }
    //��ǰ��������
    public int currentSum = 0;
    //����ҵ���������ʱ�����㲥
    public void AddSumNum(int value)
    {
        currentSum += value;
        EventCenter.Instance.EventTrigger("��������", currentSum);
    }

}
