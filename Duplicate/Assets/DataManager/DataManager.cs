using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// - Adding data to the manager
/// - Accessing the data from the manager
/// 
/// </summary>
public static class DataManager
{
    private static Dictionary<string, object> _data = new Dictionary<string, object>();

    public static void ToTheCloud(string key, object value)
    {
        if(_data.ContainsKey(key))
        {
            _data[key] = value;
        }
        else
        {
            _data.Add(key, value);
        }
    }

    public static AnyType MakeItRain<AnyType>(string key)
    {
        if(_data.ContainsKey(key))
        {
            return (AnyType) _data[key];
        }

        return default;
    }
}
