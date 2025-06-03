using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTest;
public class TestSample : MonoBehaviour
{
    //使用需要消耗的阳光数
    public int useSum = 25;

    public GameObject bkGround;

    private void Start()
    {
        EventCenter.Instance.AddEventListener<int>("阳光增加", CheckUnLock);
    }

    private void CheckUnLock(int num)
    {
        if(TestMgr.Instance.currentSum>=useSum)
        {
            bkGround.SetActive(true);
        }
        else
        {
            bkGround.SetActive(false);
        }
    }
}
