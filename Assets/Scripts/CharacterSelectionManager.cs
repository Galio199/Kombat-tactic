using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    private PlayerController[] players;
    public GameObject playerPrefab;
    private int playerIndex;
    [SerializeField] private GameObject[] messagePlayers;
    [SerializeField] private GameObject characterSelector;
    void Start()
    {

        Debug.Log("Entro START");
        this.playerIndex = 0;
        players = new PlayerController[2];
        GetPlayers();
        StartCoroutine(MessagePlayer());
    }

    public void GetPlayers()
    {
        if (GameObject.Find("Player1") == null && GameObject.Find("Player2") == null)
        {
            GameObject player = Instantiate(playerPrefab);
            player.name = "Player1";
            player = Instantiate(playerPrefab);
            player.name = "Player2";
        }

        GameObject[] playerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        playerGameObjects = playerGameObjects.OrderBy(player => player.name).ToArray();

        for (int i = 0; i < playerGameObjects.Length; i++)
        {
            players[i] = playerGameObjects[i].GetComponent<PlayerController>();
        }
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
                return;
            }
            this.playerIndex = (this.playerIndex + 1) % players.Length;

            StartCoroutine(MessagePlayer());
        }
    }

    private IEnumerator MessagePlayer()
    {
        characterSelector.SetActive(false);
        messagePlayers[playerIndex].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        messagePlayers[playerIndex].SetActive(false);
        characterSelector.SetActive(true);

    }
}
