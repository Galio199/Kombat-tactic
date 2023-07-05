using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMove : Action
{
    [Header("Direction")]
    [SerializeField] private Vector2 direction;

    [Header("Settings")]
    [SerializeField] private float cellDistance = 1f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private Vector2 offsetPointMove = new Vector2(0f,0.5f);
    [SerializeField] private LayerMask obstacles;
    [SerializeField] private float radius = 0.4f;

    private Vector2 pointMove;

    public override void Execute()
    {
        Vector2 boxSize = new Vector2(radius * 2, radius * 2);
        Vector2 directionTemporal = direction;
        pointMove = myCharacter.transform.position;
        directionTemporal *= cellDistance;
        pointMove += directionTemporal;

        //Comprobar si la casilla esta ocupada por otro jugador o esta fuera del mapa
        if (!Physics2D.OverlapBox(pointMove + offsetPointMove, boxSize, 0f, obstacles))
        {
            //Ejecutar el movimineto
            StartCoroutine(MoveCoroutine());
        }

        cooldown = baseCooldown+1;
    }
    private IEnumerator MoveCoroutine()
    {
        while (Vector2.Distance(myCharacter.transform.position, pointMove) > 0)
        {
            myCharacter.transform.position = Vector2.MoveTowards(myCharacter.transform.position, pointMove, speed * Time.deltaTime);
            yield return null;
        }

    }
}
