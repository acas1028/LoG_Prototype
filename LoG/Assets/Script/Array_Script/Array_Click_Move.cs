using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Array_Click_Move : MonoBehaviour
{
    public Arrayment_Manager Manager;
    private void OnMouseDown()
    {
        if(Manager.move_object == null&&gameObject.tag=="Character")
        {
            Manager.move_object = this.gameObject;
        }
        if (Manager.arive_object == null && Manager.move_object != this.gameObject && Manager.move_object != null)
        {
            Manager.arive_object = this.gameObject;
            Manager.Move_Grid();
        }
    }
}
