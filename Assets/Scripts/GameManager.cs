using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using System.Linq;


public class GameManager : MonoBehaviour
{
    public static int maxActions = 3;
    public static int selectedActions;
    //public static int indexActions;

    public GameObject[] players;
    private int currentPlayerIndex = 0;

    public RectTransform[] spawns;

    public GameObject actionCardPrefab;
    public int numInstances;

    public RectTransform actionCardsContainer;

    public static List<GameObject> temporaryActionCards;


    void Start()
    {
        //selectedActions = 0;
        //players = GameObject.FindGameObjectsWithTag("Player");

        //Buscar a los jugaadores, almacenarlos y ordenarlos
        players = GameObject.FindGameObjectsWithTag("Player");
        players = players.OrderBy(player => player.name).ToArray();

        InstantiateActionCards();
        temporaryActionCards = new List<GameObject>();
    }

    public void StartNextTurn()
    {
        //Reiniciar contador de acciones para el turno del jugador
        selectedActions = 0;

        //Indice del jugador que le toca el turno
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        Debug.Log("Turno del jugador " + (currentPlayerIndex + 1));

        //Instanciar las cartas en caso de que sea la primera vez que le toque elegir cartas
        if (players[currentPlayerIndex].GetComponent<PlayerController>().instanceCards == false)
        {
            InstantiateActionCards();
        }

        //Mostrar cartas del jugador y activar sus componentes
        players[currentPlayerIndex].GetComponent<PlayerController>().EnableCards();
        players[currentPlayerIndex].GetComponent<PlayerController>().enabled = true;
    }

    public void EndTurn()
    {
        //Asignar cartas de acciones al jugadoor actual
        SelectActionCards();

        //comprobar que haya elegido 3 aaccioones
        if (players[currentPlayerIndex].GetComponent<PlayerController>().selectedCards.Count != 3) return;

        //reiniciar tarjetas para el proximo Round
        foreach (GameObject card in temporaryActionCards)
        {
            card.GetComponent<ActionCard>().InitialPosition();
            card.GetComponent<ActionCard>().isSelected = false;
        }

         //Eliminar el "buffer" del jugador actual
        temporaryActionCards.Clear();

        //Desactivar componentes del jugador actual
        players[currentPlayerIndex].GetComponent<PlayerController>().DisableCards();
        players[currentPlayerIndex].GetComponent<PlayerController>().enabled = false;


        //Nueva ronda de seleccioon de acciones
        if (currentPlayerIndex == players.Length - 1)
        {
            Debug.Log("Nueva ronda de seleccioon de acciones");
            NewRound();
        }


        //Siguiente turno
        StartNextTurn();
    }


    private void InstantiateActionCards()
    {
        for (int i = 0; i < numInstances; i++)
        {
            GameObject instanciaPrefab = Instantiate(actionCardPrefab);
            instanciaPrefab.transform.SetParent(actionCardsContainer.transform, false);
            instanciaPrefab.GetComponent<RectTransform>().position = spawns[i].position;
            instanciaPrefab.GetComponent<ActionCard>().initialPosition = spawns[i].position;
            players[currentPlayerIndex].GetComponent<PlayerController>().SetCards(instanciaPrefab);
        }
        players[currentPlayerIndex].GetComponent<PlayerController>().instanceCards = true;
    }


    //Asignar acciones elegidas al jugador actual
    public void SelectActionCards()
    {
        //lista de todas las acciones que puedee elegir el jugador
        List<GameObject> listActionCards = players[currentPlayerIndex].GetComponent<PlayerController>().actionCards;

        //Lista de las acciones que el jugador ya eligió que va a realizar su personaje
        List<GameObject> listSelectedActionCards = players[currentPlayerIndex].GetComponent<PlayerController>().selectedCards;

        //foreach (GameObject card in listActionCards)
        foreach (GameObject card in temporaryActionCards)
        {
            //Es verdadero si la card ya está en las dos listas anteriormente creadas
            bool cardIsSelected = listActionCards.Contains(card) && listSelectedActionCards.Contains(card);

            //Si card está en las dos listas quiere decir que ya está elegido y no puede eleguir el mismo
            //movimiento en el mismo turno
            if (card.GetComponent<ActionCard>().isSelected && cardIsSelected == false)
            {
                players[currentPlayerIndex].GetComponent<PlayerController>().SetSelectedCard(card);
            }
        }
    }


    //Funcion que usa el boton Remove Cards para borrar las cartas escogidas y escoger otras
    public void RemoveSelectedCards()
    {
        foreach (GameObject card in temporaryActionCards)
        {
            card.GetComponent<ActionCard>().InitialPosition();
            card.GetComponent<ActionCard>().isSelected = false;
        }
        selectedActions = 0;
        temporaryActionCards.Clear();
        players[currentPlayerIndex].GetComponent<PlayerController>().UnsetSelectedCard();
    }


    //Eliminar las acciones elegidas de cada jugador para la proximaa ronda
    //de seleccion de acciones
    public void NewRound()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().UnsetSelectedCard();
        }
    }

}
