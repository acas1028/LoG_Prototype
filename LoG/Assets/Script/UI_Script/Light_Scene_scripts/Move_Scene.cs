using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Move_Scene : MonoBehaviour
{
    public enum ENUM_SCENE {
        STARTING_SCENE,
        MAINLOBBY_SCENE,
        ARRAYMENT_SCENE,
        BATTLE_SCENE,
        DECK_SCENE,
        PVE_SCENE,
        PVE_CSVTESTSCENE2
    }

    public ENUM_SCENE targetScene;

    private void Start() {
        GetComponent<Button>().onClick.AddListener(MoveScene);
    }

    public void MoveScene()
    {
        SceneManager.LoadScene((int)targetScene);
    }
}
