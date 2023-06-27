using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int maxActions = 3;
    public static int selectedActions;
    public static int indexActions;

    void Start()
    {
        selectedActions = 0;
    }
}
