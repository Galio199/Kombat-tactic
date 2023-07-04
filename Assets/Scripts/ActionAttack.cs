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
    [SerializeField] private float cellDistance = 1f;
    [SerializeField] private Vector2 offsetPointMove = new Vector2(0f, 0.5f);
    [SerializeField] private LayerMask character;
    [SerializeField] private float radius = 0.4f;

    private bool affects = false;

    public override void Execute()
    {
        Vector2 boxSize = new Vector2(radius * 2, radius * 2);

        //Aplicar el offset respecto a la posicion del jugador
        Vector2 pointCharacter = myCharacter.transform.position;
        pointCharacter += offsetPointMove;
        foreach (Vector2 point in range)
        {
            Vector2 pointRange = point * cellDistance;
            //Ajustar el rango con respecto a la posicion del jugador
            pointRange += pointCharacter;

            //Comprobar si el enemigo esta en rango del ataque
            if (Physics2D.OverlapBox(pointRange, boxSize, 0f, character))
            {
                affects = true;
            }
        }

        if (affects)
        {
            //Llamar a la funcion healthSystem para realizar los cambios a la vida del oponente
            HealthSystem();
        }

        //Verificar y llamar la funcion de ataque especial
        if (special)
        {
            SpecialAttack();
        }

        myCharacter.damageChange = 0;
        cooldown = baseCooldown+1;

    }

    public void HealthSystem()
    {
        int damage = this.GetDamage(); ;
        damage -= oponentCharacter.guardChange;
        damage += myCharacter.damageChange;

        //comprobar que el daño no sea negativo
        if (damage < 0)
        {
            damage = 0;
        }

        int oponentHealth = oponentCharacter.health;
        //Comprobar si al recibir daño la vida del oponente queda por debajo de 0 y cambiar la vida del rival
        if (oponentHealth - damage < 0)
        {
            oponentCharacter.health = 0;
        }
        else
        {
            oponentCharacter.health = oponentHealth - damage;
        }
    }

    public void SpecialAttack()
    {
        switch (this.specialAtacck)
        {
            case SpecialAtacck.HUNTRESS:
                Action[] actions = myCharacter.GetActions();
                float oponentPositionX = oponentCharacter.transform.position.x;
                float myPositionX = myCharacter.transform.position.x;
                if (oponentPositionX > myPositionX)
                {
                    actions[2].Execute();
                } else if (oponentPositionX < myPositionX)
                {
                    actions[3].Execute();
                } 
                break;
            case SpecialAtacck.WARRIOR:
                myCharacter.priorityChange = 1;
                break;
            case SpecialAtacck.WIZARD:
                if (affects)
                {
                    oponentCharacter.priorityChange = -1;
                }
                break;
        }
    }
}
