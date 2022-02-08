using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToggle : MonoBehaviour
{
    public GameObject target;

    public void Toggle()
    {
        target.SetActive(!target.activeSelf);
    }
}
