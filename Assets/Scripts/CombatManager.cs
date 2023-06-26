using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    public Fighter[] fighters;
    private int fighterIndex;

    void Start()
    {
        this.fighterIndex = 0;
    }

    public void characterSelection()
    {
        if (fighters[fighterIndex].selectedCharacter == false)
        {
            fighters[fighterIndex].character = PlayerStorage.playerPrefab;
            fighters[fighterIndex].selectedCharacter = true;
            Debug.Log("Personaje del jugador " +  fighterIndex  + " seleccionado");
            if (fighterIndex == fighters.Length - 1)
            {
                SceneManager.LoadScene("Fight");
                //GameObject.Find("SceneController").GetComponent<SceneController>().LoadScene("Fight");
            }
            this.fighterIndex = (this.fighterIndex + 1) % fighters.Length;
        }
    }
}
