using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public string idName;
    public bool selectedCharacter;

    public GameObject character;

    
    public List<GameObject> actionCards;
    public bool instanceCards;


    public List<GameObject> selectedCards;
    public List<Action> selectedActions;


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        instanceCards = false;   
        selectedCharacter = false;
        actionCards = new List<GameObject>();
        selectedCards = new List<GameObject>();
    }

    public void SetCard(GameObject card)
    {
        //Debug.Log("SetCards");
        actionCards.Add(card);
    }

    public void DisableCards()
    {
        foreach (GameObject card in actionCards)
        {
            card.SetActive(false);
        }
    }

    public void EnableCards()
    {
        foreach (GameObject card in actionCards)
        {
            card.SetActive(true);
        }
    }

    public void SetSelectedCard(GameObject card)
    {
        selectedCards.Add(card);
    }

    public void UnsetSelectedCard()
    {
        selectedCards.Clear();
        selectedActions.Clear();
    }

    public void SelectedActions()
    {
        foreach (GameObject selectedCard in selectedCards)
        {
            selectedActions.Add(selectedCard.GetComponent<ActionCard>().action);
        }
    }

    public void CoolDownActionCards()
    {
        foreach (GameObject actionCard in actionCards)
        {
            actionCard.GetComponent<ActionCard>().CoolDown();
        }
    }
}
