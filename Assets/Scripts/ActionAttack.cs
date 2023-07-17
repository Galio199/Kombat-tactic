using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAttack : Action
{
    //Enum para identificar de cual personaje es el ataque especial
    public enum SpecialAtacck
    {
        HUNTRESS, WARRIOR, WIZARD, NONE
    }

    [Header("Range effect")]
    [SerializeField] private Vector2[] range;

    [Header("Special Attack")]
    [SerializeField] private bool special = false;
    [SerializeField] private SpecialAtacck specialAtacck = SpecialAtacck.NONE;

    [Header("Settings")]
    [SerializeField] private Vector2 cellDistance;
    [SerializeField] private Vector2 offsetPointAttack;
    [SerializeField] private LayerMask character;
    [SerializeField] private LayerMask borderMap;
    [SerializeField] private float radius;

    private bool affects = false;

    public override void Execute()
    {
        Vector2 boxSize = new Vector2(radius * 2, radius * 2);

        //Cuadrar el offset del ataque
        Vector2 offsetPointAttackTemporal;
        if (myCharacter.GetPositionInCell() == PositionInCell.RIGHT)
        {
            offsetPointAttackTemporal = Vector2.Scale(offsetPointAttack, new Vector2(-1, 0));
        }
        else
        {
            offsetPointAttackTemporal = offsetPointAttack;
        }

        //Aplicar el offset respecto a la posicion del jugador
        Vector2 pointCharacter = myCharacter.transform.position;
        pointCharacter += offsetPointAttackTemporal;
        foreach (Vector2 point in range)
        {
            Vector2 pointRange = Vector2.Scale(point, cellDistance);

            //Ajustar el rango con respecto a la posicion del jugador
            pointRange += pointCharacter;

            //Comprobar si el rango esta dentro del mapa y mostrar el rango en pantalla
            if (!Physics2D.OverlapBox(pointRange, boxSize, 0f, borderMap))
            {
                ShowEffectCell(pointRange - offsetPointAttackTemporal + offsetEffectCell);
            }

            //Comprobar si el enemigo esta en rango del ataque
            if (Physics2D.OverlapBox(pointRange, boxSize, 0f, character))
            {
                affects = true;
            }
        }
        StartCoroutine(DestroyEffectCell());
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(durationEffectCell+0.2f);
        myCharacter.gameObject.GetComponent<Animator>().SetBool("Attack", true);
        if (affects)
        {
            //Llamar a la funcion healthSystem para realizar los cambios a la vida del oponente
            HealthSystem();
        }
        yield return new WaitForSeconds(durationMessage+0.2f);
        myCharacter.gameObject.GetComponent<Animator>().SetBool("Attack", false);
        //Verificar y llamar la funcion de ataque especial
        if (special)
        {
            SpecialAttack();
        }
        affects = false;
        myCharacter.damageChange = 0;
        cooldown = baseCooldown + 1;
    }

    private void HealthSystem()
    {
        int damage = this.GetDamage(); ;
        damage -= oponentCharacter.guardChange;
        damage += myCharacter.damageChange;

        //comprobar que el daño no sea negativo
        if (damage < 0)
        {
            damage = 0;
        }

        //Comprobar si al recibir daño la vida del oponente queda por debajo de 0 y cambiar la vida del rival
        if (oponentCharacter.health - damage < 0)
        {
            StartCoroutine(ShowFloatingMessage("-" + oponentCharacter.health + " vida", Color.red, oponentCharacter.gameObject));
            oponentCharacter.health = 0;
            
        }
        else
        {
            StartCoroutine(ShowFloatingMessage("-" + damage + " vida", Color.red, oponentCharacter.gameObject));
            oponentCharacter.health -= damage;
            
        }

        float oponentHealth = oponentCharacter.health;
        StartCoroutine(UpdateFillAmount(oponentHealth / 100));

    }

    private IEnumerator UpdateFillAmount(float targetFillAmount)
    {
        float initialFillAmount = oponentCharacter.healthBar.fillAmount;
        float timeElapsed = 0f;

        while (timeElapsed < durationMessage)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / durationMessage);
            oponentCharacter.healthBar.fillAmount = Mathf.Lerp(initialFillAmount, targetFillAmount, t);
            yield return null;
        }

        oponentCharacter.healthBar.fillAmount = targetFillAmount;
    }

    private void SpecialAttack()
    {
        switch (this.specialAtacck)
        {
            case SpecialAtacck.HUNTRESS:
                Action[] actions = myCharacter.GetActions();
                float oponentPositionX = oponentCharacter.transform.position.x;
                float myPositionX = myCharacter.transform.position.x;
                if (oponentPositionX > myPositionX)
                {
                    actions[6].Execute();
                } else if (oponentPositionX < myPositionX)
                {
                    actions[8].Execute();
                }
                break;
            case SpecialAtacck.WARRIOR:
                if (affects) 
                { 
                    myCharacter.priorityChange = 1;
                    StartCoroutine(ShowFloatingMessage("+velocidad", Color.blue, myCharacter.gameObject));
                }
                break;
            case SpecialAtacck.WIZARD:
                if (affects) 
                { 
                    oponentCharacter.priorityChange = -1;
                    StartCoroutine(ShowFloatingMessage("-velocidad", Color.magenta, oponentCharacter.gameObject));
                }
                
                break;
        }
    }
}
