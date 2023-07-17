using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMove : Action
{
    [Header("Direction")]
    [SerializeField] private Vector2 direction;

    [Header("Settings")]
    [SerializeField] private Vector2 cellDistance;
    [SerializeField] private float speed;
    [SerializeField] private Vector2 offsetPointMove;
    [SerializeField] private LayerMask borderMap;
    [SerializeField] private float radius;

    private Vector2 pointMove;

    public override void Execute()
    {
        Vector2 boxSize = new Vector2(radius * 2, radius * 2);
        Vector2 directionTemporal = Vector2.Scale(direction, cellDistance);
        pointMove = myCharacter.transform.position;
        pointMove += directionTemporal;

        //Comprobar si la casilla esta fuera del mapa
        if (!Physics2D.OverlapBox(pointMove + offsetPointMove, boxSize, 0f, borderMap))
        {
            ShowEffectCell(pointMove + offsetEffectCell);
            StartCoroutine(DestroyEffectCell());
            //Ejecutar el movimineto
            StartCoroutine(MoveCoroutine());
        }
        else
        {
            Vector2 position = myCharacter.transform.position;
            ShowEffectCell( position + offsetEffectCell);
            StartCoroutine(DestroyEffectCell());
        }
        cooldown = baseCooldown+1;
    }
    private IEnumerator MoveCoroutine()
    {
        yield return new WaitForSeconds(durationEffectCell+0.2f);
        myCharacter.Orientation(pointMove.x);
        while (Vector2.Distance(myCharacter.transform.position, pointMove) > 0)
        {
            myCharacter.transform.position = Vector2.MoveTowards(myCharacter.transform.position, pointMove, speed * Time.deltaTime);
            yield return null;
        }
        myCharacter.Orientation();
        oponentCharacter.Orientation();
    }
}
