using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public string ActionName;
    [SerializeField] private int priority;
    public int cooldown;
    [SerializeField] private int damage = 0;
    [SerializeField] private string effect = "";

    protected Character myCharacter;
    protected Character oponentCharacter;
    public int GetPriotiy()
    {
        return this.priority;
    }

    public int GetDamage()
    {
        return this.damage;
    }

    public string GetEffect()
    {
        return this.effect;
    }

    //Agregar las referencias de los personajes
    public void SetCharacters()
    {
        this.myCharacter = GetComponentInParent<Character>();
        this.oponentCharacter = myCharacter.oponent;

    }

    public abstract void execute();

}
