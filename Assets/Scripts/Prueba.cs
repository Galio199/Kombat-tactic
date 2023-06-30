using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prueba : MonoBehaviour
{
    [SerializeField] private Character character1;
    [SerializeField] private Character character2;

    void Start()
    {
        StartCoroutine(ExecutePrueba());
    }

    IEnumerator ExecutePrueba()
    {
        yield return new WaitForSeconds(0.2f);

        Action[] actions1 = character1.GetActions();
        Action[] actions2 = character2.GetActions();
        character1.SetOponent(character2);
        character2.SetOponent(character1);

        // Probar movimiento
        yield return ExecuteActionAndWait(actions1[3], "Se ejecutó el movimiento derecha J1");

        // Probar potenciar
        yield return ExecuteActionAndWait(actions1[6], "Se ejecutó el potenciar J1");

        // Probar movimiento
        yield return ExecuteActionAndWait(actions1[2], "Se ejecutó el movimiento izquierda J1");

        // Probar ataque
        yield return ExecuteActionAndWait(actions1[9], "Se ejecutó el ataque J1");

        // Probar movimiento
        yield return ExecuteActionAndWait(actions2[3], "Se ejecutó el movimiento derecha J2");

        // Probar movimiento
        yield return ExecuteActionAndWait(actions1[0], "Se ejecutó el movimiento arriba J1");

        // Probar potenciar
        yield return ExecuteActionAndWait(actions2[5], "Se ejecutó el bloquear J2");

        // Ataque especial
        yield return ExecuteActionAndWait(actions1[10], "Se ejecutó el ataque especial J1");

        // Probar movimiento
        yield return ExecuteActionAndWait(actions1[2], "Se ejecutó el movimiento izquierda J1");
    }

    IEnumerator ExecuteActionAndWait(Action action, string logMessage)
    {
        action.execute();
        Debug.Log(logMessage);
        yield return new WaitForSeconds(5f); // Tiempo de espera entre acciones
    }
}


