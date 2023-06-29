using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelf : Action
{
    //Enum que representa el tipo de accion autoinflinjida, es decir, cuando se proteje, bloquea o potencia su daño
    public enum ActionType
    {
        PROTECT, BLOCK, POWER
    }

    [SerializeField] private ActionType guardType;

    public override void execute()
    {
        SetCharacters();
        switch (this.guardType)
        {
            case ActionType.PROTECT:
                myCharacter.guardChange = 10;
                break;
            case ActionType.BLOCK:
                myCharacter.guardChange = 100;
                break;
            case ActionType.POWER:
                myCharacter.damageChange = 10;
                break;

        }

    }
}
