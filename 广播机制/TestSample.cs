using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTest;
public class TestSample : MonoBehaviour
{
    //ʹ����Ҫ���ĵ�������
    public int useSum = 25;

    public GameObject bkGround;

    private void Start()
    {
        EventCenter.Instance.AddEventListener<int>("��������", CheckUnLock);
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
