using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int maxActions = 3;
    public static int selectedActions;


    [Header("General")]
    [SerializeField] private GameObject[] players;
    [SerializeField] private int currentPlayerIndex = 0;

    public List<Character> characters;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private int numTurn = 1;
    [SerializeField] private int maxTurns = 50;
    [SerializeField] private Character winner;
    [SerializeField] private string winnerPlayer;

    [Header("Choose Action Cards")]
    [SerializeField] private RectTransform[] spawns;

    [SerializeField] private GameObject actionCardPrefab;
    [SerializeField] private int numCards;

    [SerializeField] private GameObject actionCardsContainer;
    //[SerializeField] private GameObject executeActionContainer;

    public static List<GameObject> temporaryActionCards;

    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject winnerTextTitle;
    [SerializeField] private GameObject winnerText;


    void Start()
    {
        currentPlayerIndex = 0;
        //Buscar a los jugaadores, almacenarlos y ordenarlos
        players = GameObject.FindGameObjectsWithTag("Player");
        players = players.OrderBy(player => player.name).ToArray();

        //Obetener e instanciar los characters de los player a partir de los prefabs
        for (int i = 0; i < players.Length; i++)
        {
            GameObject characterPrefab = players[i].GetComponent<PlayerController>().character;
            Transform spawnPosition = spawnPositions[i];

            GameObject characterInstance = Instantiate(characterPrefab, spawnPosition.position, spawnPosition.rotation);
            Character character = characterInstance.GetComponent<Character>();

            characters[i]=character;

            players[i].GetComponent<PlayerController>().character = characterInstance;

            Debug.Log("Personaje agregado");
        }

        //Asignar las posiciones en las celdas
        characters[0].SetPositionInCell(PositionInCell.LEFT);
        characters[1].SetPositionInCell(PositionInCell.RIGHT);

        //Asignar los oponentes a los character
        characters[0].SetOponent(characters[1]);
        characters[1].SetOponent(characters[0]);
        Debug.Log("Oponentes asignados");

        //Iniciar la eleccion de acciones
        InstantiateActionCards();
        temporaryActionCards = new List<GameObject>();

        
    }

    #region Choose Action
    public void StartNextTurnCardSelection()
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

    public void EndTurnCardSelection()
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


        //Pasar a ejecutar las acciones
        if (currentPlayerIndex == players.Length - 1)
        {
            Debug.Log("Pasando a ejecutar acciones");
            StartCoroutine(ExecuteActions());
            return;
        }

        StartNextTurnCardSelection();
    }


    private void InstantiateActionCards()
    {
        //Obtener personaje y acciones del personaje
        GameObject character = players[currentPlayerIndex].GetComponent<PlayerController>().character;
        Action[] actions = character.GetComponent<Character>().GetActions();

        //Asignar cartas respectivas del personaje escogido al jugador
        for (int i = 0; i < numCards; i++)
        {
            //Instancear carta
            GameObject instanciaPrefab = Instantiate(actionCardPrefab);
            //Posicionar carta y guardar posición de inicio
            instanciaPrefab.transform.SetParent(actionCardsContainer.transform, false);
            instanciaPrefab.GetComponent<RectTransform>().position = spawns[i].position;
            instanciaPrefab.GetComponent<ActionCard>().initialPosition = spawns[i].position;

            //Asignar acción de la carta
            instanciaPrefab.GetComponent<ActionCard>().action = actions[i];

            //Modificar aspecto de la carta para que contenga los parametros de la accion
            instanciaPrefab.GetComponent<ActionCard>().SetParameters();

            //Asignar carta a jugador
            players[currentPlayerIndex].GetComponent<PlayerController>().SetCard(instanciaPrefab);
        }
        //Marcar que el jugador ya tiene cartas asignadas
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

    //Activar la seleccion de cartas
    public void ChooseActionsCards()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().CoolDownActionCards();
            player.GetComponent<PlayerController>().UnsetSelectedCard();
        }
        actionCardsContainer.SetActive(true);

        StartNextTurnCardSelection();
    }
    #endregion

    #region Execute Actions
    IEnumerator ExecuteActions()
    {
        //Guardar las acciones en el player, ocultar la interfaz de eleccion y mostrar la de seleccion
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().SelectedActions();
        }
        actionCardsContainer.SetActive(false);

        List<Action> actions1 = players[0].GetComponent<PlayerController>().selectedActions;
        List<Action> actions2 = players[1].GetComponent<PlayerController>().selectedActions;
        Debug.Log("Acciones escogidas obtenidas");
        for (int i=0; i<3; i++)
        {
            yield return new WaitForSeconds(2f);
            //Comprobar prioridad y reestablecer los cambios a la prioridad
            Action[] actions = PrioritySystem(actions1[i],actions2[i]);
            characters[0].priorityChange = 0;
            characters[1].priorityChange = 0;

            //Ejecutar acciones y comprobar condiciones de victoria
            foreach (Action action in actions)
            {
                Debug.Log("Se ejecuto la accion " + action.ActionName);
                action.Execute();
                yield return new WaitForSeconds(action.durationAnimation);
                if (VictorySystem(0)) { EndGame(); yield break; }
            }

            //Reestablecer los cambios al guard de los personajes
            characters[0].guardChange = 0;
            characters[1].guardChange = 0;

            Debug.Log("Se ejecuto el par");

            yield return null;
        }

        Debug.Log("Se acabo el turno");

        //Aumentar numero de turno y comprobar condicion de victoria
        numTurn += 1;
        if (VictorySystem(0)) { EndGame(); yield break; }

        //Reestablecer cambios al daño
        characters[0].damageChange = 0;
        characters[1].damageChange = 0;

        //Reducir el cooldown
        for (int i = 0; i < 2; i++)
        {
            Action[] actions = characters[i].GetActions();
            foreach(Action action in actions)
            {
                if (action.cooldown > 0) { action.cooldown -= 1; }
            }
        }
        
        ChooseActionsCards();

    }
    
    public Action[] PrioritySystem(Action action0, Action action1)
    {
        Action[] actions = new Action[2];

        //Comprobar si los personajes tienen cambios en su prioridad
        if (characters[0].priorityChange > characters[1].priorityChange)
        {
            actions[0] = action0;
            actions[1] = action1;
        } 
        else if (characters[0].priorityChange < characters[1].priorityChange)
        {
            actions[0] = action1;
            actions[1] = action0;
        }
        else
        {
            //Comprobar la prioridad de las acciones
            if (action0.GetPriotiy() > action1.GetPriotiy())
            {
                actions[0] = action0;
                actions[1] = action1;
            }
            else if (action0.GetPriotiy() < action1.GetPriotiy())
            {
                actions[0] = action1;
                actions[1] = action0;
            }
            else
            {
                //Elegir aleatoriamente cual accion va primero
                if (Random.Range(0f, 1f) > 0.5f)
                {
                    actions[0] = action0;
                    actions[1] = action1;
                }
                else
                {
                    actions[0] = action1;
                    actions[1] = action0;
                }
            }
        }
        Debug.Log("Prioridad comprobada");
        return actions;
    }

    public bool VictorySystem(int i)
    {
        if (i == 0)
        {
            //Comprobar si alguno de los personajes tiene su vida en 0
            if (characters[0].health == 0)
            {
                winner = characters[1];
                winnerPlayer = "Jugador 2";
                return true;
            }
            else if (characters[1].health == 0)
            {
                winner = characters[0];
                winnerPlayer = "Jugador 1";
                return true;
            }
        } 
        else
        {
            //Comprobar si el numero de turnos es mayor al maximo permitido
            if (numTurn < maxTurns)
            {
                //Asignar como ganador al jugador con mayor vida
                if (characters[0].health < characters[1].health)
                {
                    winner = characters[1];
                    winnerPlayer = "Jugador 2";
                    return true;
                }
                else if (characters[0].health > characters[1].health)
                {
                    winner = characters[0];
                    winnerPlayer = "Jugador 1";
                    return true;
                }
            }
        }
        return false;
    }
    #endregion

    #region End game
    public void EndGame()
    {
        foreach (Character character in characters)
        {
            character.gameObject.SetActive(false);
        }

        Debug.Log("Gano el" + winnerPlayer);

        //Destroy(actionCardsContainer);

        Camera.main.backgroundColor = Color.black;

        newGameButton.SetActive(true);
        mainMenuButton.SetActive(true);
        winnerText.SetActive(true);
        winnerTextTitle.SetActive(true);

        //Mostrar Personaje que ganó la partida y centrarlo
        winner.gameObject.SetActive(true);
        winner.gameObject.GetComponent<Transform>().position = new Vector3(0,0);

        winnerText.GetComponent<TextMeshProUGUI>().text = winnerPlayer;

        selectedActions = 0;
    }

    public void GoMainMenu()
    {
        foreach (GameObject player in players)
        {
            Destroy(player);
        }
        SceneManager.LoadScene(0);
    }
    #endregion

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
}
