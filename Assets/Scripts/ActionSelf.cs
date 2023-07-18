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
        Vector2 position = myCharacter.transform.position;
        ShowEffectCell(position + offsetEffectCell);
        StartCoroutine(ActionSelfCoroutine());
        StartCoroutine(DestroyEffectCell());
        cooldown = baseCooldown + 1;
    }

    private IEnumerator ActionSelfCoroutine()
    {
        yield return new WaitForSeconds(durationEffectCell+0.2f);
        switch (this.actionType)
        {
            case ActionType.PROTECT:
                myCharacter.guardChange = 10;

                //Activar animacion
                myCharacter.animationsEffects[1].Animation();

                //Mostrar mensaje
                StartCoroutine(ShowFloatingMessage("+10 Defensa", Color.green, myCharacter.gameObject));
                break;
            case ActionType.BLOCK:
                myCharacter.guardChange = 100;

                //Activar animacion 
                myCharacter.animationsEffects[1].Animation();

                //Mostrar mensaje
                StartCoroutine(ShowFloatingMessage("+100 Defensa", Color.green, myCharacter.gameObject));
                break;
            case ActionType.ENHANCE:
                myCharacter.damageChange = 10;

                //Activar animacion
                myCharacter.animationsEffects[2].Animation();

                //Mostrar mensaje
                StartCoroutine(ShowFloatingMessage("+10 Ataque", Color.yellow, myCharacter.gameObject));
                break;
        }
    }
}
