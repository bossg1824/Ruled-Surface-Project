using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherFuncs
{
    public static string ReturnArray<v>(v[] array)
    {
        string ret = "";
        for (int i = 0; i < array.Length; i++)
        {
            ret += array[i].ToString();
        }
        return ret;
    }
}
