using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fighter : MonoBehaviour
{
    public string idName;
    public bool selectedCharacter;
    public GameObject character;
    public GameObject[] actions;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        selectedCharacter = false;
    }
}
