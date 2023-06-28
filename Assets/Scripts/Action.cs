using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Action : MonoBehaviour
{
    public int idAction;
    public bool isSelected;
    public Vector3 intialPosition;
    private int idSelection;

    //action features
    public float damage;
    public float coolDown;
    public string action;

    //text and Images fields
    public TextMeshProUGUI damageTextField;
    public TextMeshProUGUI coolDownTextField;
    public TextMeshProUGUI actionTextField;
    public Image image;

    //Spawns selected actions
    //public RectTransform[] spawns;
    public static Vector3[] positionSelectActions;


    void Start()
    {
        damageTextField.text = damage.ToString();
        coolDownTextField.text = coolDown.ToString();
        actionTextField.text = action;
        GameObject[] spawnsObjects = GameObject.FindGameObjectsWithTag("SelectedAction");
        Debug.Log(spawnsObjects.Length); 
        //for (int i = 0; i < spawnsObjects.Length; i++)
        //{
        //    positionSelectActions[i] = spawnsObjects[i].GetComponent<RectTransform>().position;
        //}
    }

    //public void PointerClick()
    //{
    //    if (GameManager.selectedActions < GameManager.maxActions || isSelected == true)
    //    {
            
    //        if (isSelected == false)
    //        {
    //            Debug.Log("Cliked" + this.idAction);

    //            idSelection = GameManager.selectedActions;

    //            //Ubicar en las respectivas casillas de acciones seleccionadas
    //            //RectTransform rectTransform = spawns[idSelection];
    //            //this.GetComponent<RectTransform>().position = rectTransform.position;
    //            this.GetComponent<RectTransform>().position = positionSelectActions[idSelection];

    //            //Marcar acción seleccionada
    //            isSelected = true;


    //            //Sumar una acción
    //            GameManager.selectedActions += 1;

    //            Debug.Log("La aaaccion esta " + isSelected + " y hay " + GameManager.selectedActions + "acciones elegidas");
    //        }
    //        //else
    //        //{
    //        //    //Deseleccionar casillas
    //        //    isSelected = false;

    //        //    //Moverlas a la posición inicial
    //        //    this.GetComponent<RectTransform>().position = intialPosition;

    //        //    //Restar una accion seleccionada
    //        //    //GameManager.selectedActions = idSelection;
    //        //    GameManager.selectedActions -= 1;

    //        //    //action.transform.SetParent(panel.transform, true);
    //        //    Debug.Log("La aaaccion esta " + isSelected + " y hay " + GameManager.selectedActions + "acciones elegidas");
    //        //    //Debug.Log("Ya estan las 3 accciones elegidas");
    //        //}
    //    }
    //    else
    //    {
    //        Debug.Log("Acciones elegidas");
    //    }
    //}

    //public void RemoveCard()
    //{
    //    this.GetComponent<RectTransform>().position = intialPosition;
    //    GameManager.selectedActions =0;
    //}
}
