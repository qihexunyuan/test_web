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
    //当前的阳光数
    public int currentSum = 0;
    //当玩家点击阳光添加时触发广播
    public void AddSumNum(int value)
    {
        currentSum += value;
        EventCenter.Instance.EventTrigger("阳光增加", currentSum);
    }

}
