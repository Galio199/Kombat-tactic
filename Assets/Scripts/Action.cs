using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public int idAction;
    public void PointerClick()
    {
        Debug.Log("Cliked" + this.idAction);
    }
}
