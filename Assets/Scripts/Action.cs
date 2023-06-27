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
    //public GameObject action;
    //public GameObject parent;
    //public GameObject panel;

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
    public RectTransform[] spawns;


    void Start()
    {

    }



    public void PointerClick()
    {
        if (GameManager.selectedActions < GameManager.maxActions || isSelected == true)
        {
            
            if (isSelected == false)
            {
                Debug.Log("Cliked" + this.idAction);

                idSelection = GameManager.selectedActions;

                //Ubicar en las respectivas casillas de acciones seleccionadas
                RectTransform rectTransform = spawns[idSelection];
                this.GetComponent<RectTransform>().position = rectTransform.position;

                //Marcar acción seleccionada
                isSelected = true;

                //action.transform.SetParent(parent.transform,true);

                //Sumar una acción
                GameManager.selectedActions += 1;

                Debug.Log("La aaaccion esta " + isSelected + " y hay " + GameManager.selectedActions + "acciones elegidas");
            }
            //else
            //{
            //    //Deseleccionar casillas
            //    isSelected = false;

            //    //Moverlas a la posición inicial
            //    this.GetComponent<RectTransform>().position = intialPosition;

            //    //Restar una accion seleccionada
            //    //GameManager.selectedActions = idSelection;
            //    GameManager.selectedActions -= 1;

            //    //action.transform.SetParent(panel.transform, true);
            //    Debug.Log("La aaaccion esta " + isSelected + " y hay " + GameManager.selectedActions + "acciones elegidas");
            //    //Debug.Log("Ya estan las 3 accciones elegidas");
            //}
        }
        else
        {
            Debug.Log("Acciones elegidas");
        }
    }

    public void RemoveCard()
    {
        this.GetComponent<RectTransform>().position = intialPosition;
        GameManager.selectedActions =0;
    }
}
