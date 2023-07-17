using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public static List<GameObject> temporaryActionCards;

    [Header("UI Elements")]
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject winnerTextTitle;
    [SerializeField] private GameObject winnerText;
    [SerializeField] private GameObject viewCardsButton;
    [SerializeField] private GameObject scoreContainer;
    [SerializeField] private GameObject[] scores = new GameObject[2];
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject cardsInGameContainer;
    [SerializeField] private GameObject[] cardsInGame;
    [SerializeField] private GameObject[] actionCardsInGame;
    [SerializeField] private GameObject[] messagePlayers;
    [SerializeField] private GameObject roundContainer;
    [SerializeField] private Image[] healthBars;
    [SerializeField] private Image[] iconCharacters;
    [SerializeField] private TMP_Text roundCount;


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

        //Asignar barra de vida a los personajes
        characters[0].healthBar = healthBars[0];
        characters[1].healthBar = healthBars[1];

        //Cargar el icono de los personajes
        iconCharacters[0].sprite = characters[0].miniature;
        iconCharacters[1].sprite = characters[1].miniature;

        //Asignar las posiciones en las celdas
        characters[0].SetPositionInCell(PositionInCell.LEFT);
        characters[1].SetPositionInCell(PositionInCell.RIGHT);

        //Hacerle flip al jugador 2
        characters[1].gameObject.GetComponent<SpriteRenderer>().flipX = true;

        //Asignar los oponentes a los character
        characters[0].SetOponent(characters[1]);
        characters[1].SetOponent(characters[0]);
        Debug.Log("Oponentes asignados");

        //Mostrar mensaje del jugador que debe elegir
        StartCoroutine(MessagePlayer());

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

        //Mostrar el mensaje indicando el jugador que elije la accion
        StartCoroutine(MessagePlayer());

        //Instanciar las cartas en caso de que sea la primera vez que le toque elegir cartas
        if (players[currentPlayerIndex].GetComponent<PlayerController>().instanceCards == false)
        {
            InstantiateActionCards();
        }

        //Mostrar cartas del jugador y activar sus componentes
        players[currentPlayerIndex].GetComponent<PlayerController>().EnableCards();
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
        mainMenuButton.SetActive(true);

        StartNextTurnCardSelection();
    }
    public void ViewMap()
    {
        actionCardsContainer.SetActive(false);
        mainMenuButton.SetActive(false);
        viewCardsButton.SetActive(true);
    }

    public void ViewCards()
    {
        viewCardsButton.SetActive(false);
        actionCardsContainer.SetActive(true);
        mainMenuButton.SetActive(true);
    }

    private IEnumerator MessagePlayer()
    {
        grid.SetActive(false);
        actionCardsContainer.SetActive(false);
        mainMenuButton.SetActive(false);
        messagePlayers[currentPlayerIndex].SetActive(true);
        characters[0].gameObject.SetActive(false);
        characters[1].gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        messagePlayers[currentPlayerIndex].SetActive(false);
        grid.SetActive(true);
        actionCardsContainer.SetActive(true);
        mainMenuButton.SetActive(true);
        characters[0].gameObject.SetActive(true);
        characters[1].gameObject.SetActive(true);

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
        mainMenuButton.SetActive(false);
        actionCardsContainer.SetActive(false);
        cardsInGameContainer.SetActive(true);

        //Activar todas las cartas
        foreach (GameObject card in cardsInGame){
            card.SetActive(true);
        }

        //Obtener las actions escogidas 
        List<Action> actions1 = players[0].GetComponent<PlayerController>().selectedActions;
        List<Action> actions2 = players[1].GetComponent<PlayerController>().selectedActions;

        Debug.Log("Acciones escogidas obtenidas");
        for (int i=0; i<3; i++)
        {
            //Asignar la accion al actionCard
            actionCardsInGame[0].GetComponent<ActionCard>().action = actions1[i];
            actionCardsInGame[0].GetComponent<ActionCard>().SetParameters();
            actionCardsInGame[1].GetComponent<ActionCard>().action = actions2[i];
            actionCardsInGame[1].GetComponent<ActionCard>().SetParameters();

            yield return new WaitForSeconds(0.2f);
            //Mostrar la ActionCard que se va a ejecutar
            StartCoroutine(moveActionCard(actionCardsInGame[0],1));
            StartCoroutine(moveActionCard(actionCardsInGame[1],-1));

            yield return new WaitForSeconds(2.5f);
            Debug.Log("Empieza la ejecucion");
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

            yield return new WaitForSeconds(0.1f);

            //Ocultar las cartas ya usadas
            if (i != 2)
            {
                cardsInGame[i].SetActive(false);
                cardsInGame[i + 2].SetActive(false);
            }

            //Reestablecer la posicision de las ActionCard
            actionCardsInGame[0].GetComponent<RectTransform>().anchoredPosition += Vector2.right * -60;
            actionCardsInGame[1].GetComponent<RectTransform>().anchoredPosition += Vector2.right * 60;

        }
        Debug.Log("Se acabo el turno");

        cardsInGameContainer.SetActive(false);

        //Aumentar numero de turno y comprobar condicion de victoria
        numTurn += 1;
        if (VictorySystem(0)) { EndGame(); yield break; }

        //Actualizar el numero numero de la ronda en pantalla
        roundCount.text = numTurn.ToString();

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

        //Activar la eleccion de acciones
        ChooseActionsCards();

    }

    private IEnumerator moveActionCard(GameObject actionCard, int direction)
    {
        RectTransform rectTransform = actionCard.GetComponent<RectTransform>();
        Vector2 targetPosition = rectTransform.anchoredPosition + Vector2.right * 60f * direction;

        while (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) > 0)
        {
            rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, 50f * Time.deltaTime);
            yield return null;
        }
    }
    
    private Action[] PrioritySystem(Action action0, Action action1)
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

    private bool VictorySystem(int i)
    {
        if (i == 0)
        {
            //Comprobar si alguno de los personajes tiene su vida en 0
            if (characters[0].health == 0)
            {
                winner = characters[1];
                winnerPlayer = "Jugador 2";
                players[1].GetComponent<PlayerController>().victoryCount += 1;
                return true;
            }
            else if (characters[1].health == 0)
            {
                winner = characters[0];
                winnerPlayer = "Jugador 1";
                players[0].GetComponent<PlayerController>().victoryCount += 1;
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
                    players[1].GetComponent<PlayerController>().victoryCount += 1;
                    return true;
                }
                else if (characters[0].health > characters[1].health)
                {
                    winner = characters[0];
                    winnerPlayer = "Jugador 1";
                    players[0].GetComponent<PlayerController>().victoryCount += 1;
                    return true;
                }
            }
        }
        return false;
    }
    #endregion

    #region End/New game
    public void EndGame()
    {
        foreach (Character character in characters)
        {
            character.gameObject.SetActive(false);
        }

        Debug.Log("Gano el" + winnerPlayer);

        //Destroy(actionCardsContainer);

        Camera.main.backgroundColor = Color.black;

        cardsInGameContainer.SetActive(false);
        grid.SetActive(false);
        roundContainer.SetActive(false);
        newGameButton.SetActive(true);
        mainMenuButton.SetActive(true);
        winnerText.SetActive(true);
        winnerTextTitle.SetActive(true);

        //Mostrar Personaje que ganó la partida y centrarlo
        winner.gameObject.SetActive(true);
        winner.gameObject.GetComponent<Transform>().position = new Vector3(0,0);

        winnerText.GetComponent<TextMeshProUGUI>().text = winnerPlayer;

        scoreContainer.SetActive(true);
        scores[0].GetComponent <TextMeshProUGUI>().text = players[0].GetComponent<PlayerController>().victoryCount.ToString();
        scores[1].GetComponent <TextMeshProUGUI>().text = players[1].GetComponent<PlayerController>().victoryCount.ToString();

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

    public void NewGame()
    {
        foreach (GameObject player in players)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.character = null;
            playerController.selectedCharacter = false;
            playerController.actionCards.Clear();
            playerController.selectedCards.Clear();
            playerController.selectedActions.Clear();
            playerController.instanceCards = false;
        }
        SceneManager.LoadScene(1);
    }
    #endregion

}
