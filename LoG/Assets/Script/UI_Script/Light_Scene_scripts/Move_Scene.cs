using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move_Scene : MonoBehaviour
{
    public int sceneCount;

    public void MoveScene()
    {
        SceneManager.LoadScene(sceneCount);
    }
}
