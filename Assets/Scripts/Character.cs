using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string CharacterName;

    [Header("Stats")]
    public int health=100;
    public int priorityChange=0;
    public int damageChange=0;
    public int guardChange=0;

    [Header("Actions")]
    [SerializeField] private Action[] actions;

    [SerializeField] private Character oponent;

    // Inicializar los valores de las estadisticas
    public void ResetStats()
    {
        this.health = 100;
        this.priorityChange = 0;
        this.damageChange = 0;
        this.guardChange = 0;
    }

    public Action[] GetActions()
    {
        return actions;
    }

    public void SetOponent(Character oponent)
    {
        this.oponent = oponent;
        SetCharactersInActions();
    }

    public Character GetOponent()
    {
        return oponent;
    }

    public void SetCharactersInActions()
    {
        foreach(Action action in actions)
        {
            action.SetCharacters();
        }
    }

}

