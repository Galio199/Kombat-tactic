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

    public override void execute()
    {
        switch (this.actionType)
        {
            case ActionType.PROTECT:
                myCharacter.guardChange = 10;
                break;
            case ActionType.BLOCK:
                myCharacter.guardChange = 100;
                break;
            case ActionType.ENHANCE:
                myCharacter.damageChange = 10;
                break;

        }

    }
}
