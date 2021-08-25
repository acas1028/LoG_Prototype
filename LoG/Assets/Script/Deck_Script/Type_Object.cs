using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Type_Object : MonoBehaviour
{
    public enum Type_Num
    {
        Attacker = 1,
        Balance,
        Defender
    };
    public Type_Num Type;
}
