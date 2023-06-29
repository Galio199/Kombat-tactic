using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string CharacterName;
    public int health=100;
    public int priorityChange=0;
    public int damageChange=0;
    public int guardChange=0;
    public Character oponent;

    [SerializeField] protected Action[] actions;

    // Inicializar los valores de las estadisticas
    public void RestarStats()
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

}

