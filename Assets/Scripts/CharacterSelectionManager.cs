using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    public PlayerController[] players;
    private int playerIndex;

    void Start()
    {
        this.playerIndex = 0;
    }

    public void characterSelection()
    {
        if (players[playerIndex].selectedCharacter == false)
        {
            players[playerIndex].character = PlayerStorage.playerPrefab;
            players[playerIndex].selectedCharacter = true;
            Debug.Log("Personaje del jugador " + playerIndex + " seleccionado");
            if (playerIndex == players.Length - 1)
            {
                SceneManager.LoadScene("Fight");
                //GameObject.Find("SceneController").GetComponent<SceneController>().LoadScene("Fight");
            }
            this.playerIndex = (this.playerIndex + 1) % players.Length;
        }
    }
}
