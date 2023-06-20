using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    public string idName;
    public bool selectedCharacter;
    public GameObject character;

    private void Start()
    {
        selectedCharacter = false;
    }
}
