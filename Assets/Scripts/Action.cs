using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    [Header("General")]
    public string ActionName;
    [SerializeField] private int priority;
    public int cooldown;
    [SerializeField] private int damage = 0;
    [SerializeField] private string effect = "";

    [Header("Range Image")]
    [SerializeField] public Texture2D image;

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
        this.oponentCharacter = myCharacter.GetOponent();
    }

    public abstract void execute();

}
