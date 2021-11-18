using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Array_Click_Move : MonoBehaviour
{
    public Arrayment_Manager Manager;
    private bool overlab = false;
    private void OnMouseDown()
    {
        if (Manager.move_object == this.gameObject && overlab == true)
        {
            Manager.move_object = null;
            for (int i = 0; i < 9; i++)
            {
                if (this.gameObject == Manager.Grids[i])
                    Manager.GridsOffHighLight(i);
            }
            overlab = false;
            return;
        }
        if (Manager.move_object == null && gameObject.tag == "Character")
        {
            Manager.move_object = this.gameObject;
            for (int i = 0; i < 9; i++)
            {
                if (this.gameObject == Manager.Grids[i])
                    Manager.GridsOnHighLight(i);
            }
            overlab = true;
        }
        if (Manager.arive_object == null && Manager.move_object != this.gameObject && Manager.move_object != null)
        {
            for (int i = 0; i < 9; i++)
            {
                if (Manager.move_object == Manager.Grids[i])
                    Manager.GridsOffHighLight(i);
            }
            Manager.arive_object = this.gameObject;
            Manager.Move_Grid();
            Manager.arive_object = null;
            Manager.move_object = null;
            overlab = false;
        }

        // 클릭시 프리펩 색 변경.
        // transform.Getchild(1) -> 프리펩 가져올 수 있음.
        // 가져온 프리펩 색 변경.
    }
}
