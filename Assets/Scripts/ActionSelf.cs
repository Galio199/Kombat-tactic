using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelf : Action
{
    //Enum que representa el tipo de accion autoinflinjida, es decir, cuando se proteje, bloquea o potencia su daño
    public enum ActionType
    {
        NONE, PROTECT, BLOCK, ENHANCE
    }

    [Header("Action")]
    [SerializeField] private ActionType actionType;

    public override void Execute()
    {
        switch (this.actionType)
        {
            case ActionType.PROTECT:
                myCharacter.guardChange = 10;
                StartCoroutine(ShowFloatingMessage("+10 Defensa", Color.green, myCharacter.gameObject));
                break;
            case ActionType.BLOCK:
                myCharacter.guardChange = 100;
                StartCoroutine(ShowFloatingMessage("+100 Defensa", Color.green, myCharacter.gameObject));
                break;
            case ActionType.ENHANCE:
                myCharacter.damageChange = 10;
                StartCoroutine(ShowFloatingMessage("+10 Ataque", Color.yellow, myCharacter.gameObject));
                break;
        }

        cooldown = baseCooldown+1;
    }
}
