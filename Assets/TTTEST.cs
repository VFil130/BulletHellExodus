using UnityEngine;

public class TTTEST : MonoBehaviour
{
    public float data1;
    public float data2;
    public float data3;
    public float data4;
    public void InitData()
    {
        data1 = 1;
        data4 = 1;
        data2 = 1;
        data3 = 1;
        ChangeData();
    }
    public void ChangeData()
    {

    }
}
public class Pers : MonoBehaviour
{
    TTTEST test;
    public void DA()
    {
        test.ChangeData();
    }
    public void ChangeData()
    {
        test.data1 = 2;
    }
}