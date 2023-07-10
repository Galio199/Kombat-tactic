using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    [Header("General")]
    public string ActionName;
    [SerializeField] private int priority;
    [SerializeField] protected int baseCooldown;
    [HideInInspector] public int cooldown = 0;
    [SerializeField] private int damage = 0;
    [SerializeField] private string effect = "";

    [Header("Range Image")]
    [SerializeField] public Texture2D image;

    [Header("Animation")]
    public float durationAnimation = 1;

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

    public int GetBaseCooldown()
    {
        return this.baseCooldown;
    }

    //Agregar las referencias de los personajes
    public void SetCharacters()
    {
        this.myCharacter = GetComponentInParent<Character>();
        this.oponentCharacter = myCharacter.GetOponent();
    }

    public abstract void Execute();

}
