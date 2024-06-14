using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class UpgradeProperty
{
    public enum PropertyType
    {
        Int,
        Float,
        String,
        Bool,
        Sprite,
        Prefab
    }

    public string key;
    public PropertyType type;
    public int intValue;
    public float floatValue;
    public string stringValue;
    public bool boolValue;
    public Sprite spriteValue;
    public GameObject prefabValue;

    public object GetValue()
    {
        switch (type)
        {
            case PropertyType.Int:
                return intValue;
            case PropertyType.Float:
                return floatValue;
            case PropertyType.String:
                return stringValue;
            case PropertyType.Bool:
                return boolValue;
            case PropertyType.Sprite:
                return spriteValue;
            case PropertyType.Prefab:
                return prefabValue;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
