using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using TMPro;
using UnityEngine.UI;

public class ActionCard : MonoBehaviour
{
    //Confirmar si esta seleccionada o no
    public bool isSelected;

    //Posicion iinicial de la carta
    public Vector3 initialPosition;

    //Posicion de la carta cuando es elegida
    public Vector3[] positionSelectedActionCards;


    public TextMeshProUGUI damageText;
    public TextMeshProUGUI coolDownText;
    public TextMeshProUGUI actionText;
    public Image attackArea;
    public Image image;

    void Start()
    {
        //Buscar, almacenar y oordenar los objetos que tienen la poosicion donde van las tarjetas elejidas
        GameObject[] selectedCardsPosition = GameObject.FindGameObjectsWithTag("SelectedAction");
        selectedCardsPosition = selectedCardsPosition.OrderBy(player => player.name).ToArray();

        //Abstraer la posicion de los objetos y almacenarlo en una variable
        for (int i = 0; i < selectedCardsPosition.Length; i++)
        {
            positionSelectedActionCards[i] = selectedCardsPosition[i].GetComponent<RectTransform>().position;
        }
    }


    //Al dar click sobre la tarjeta la elige
    public void PointerClick()
    {

        //Comprobar que no se haya pasado del limite maximo dee acciones
        if (GameManager.selectedActions < GameManager.maxActions)
        {

            if (isSelected == false)
            {
              
                this.GetComponent<RectTransform>().position = positionSelectedActionCards[GameManager.selectedActions];


                //Añadir a una lista temporal que luego se vacia si se lee da click al botón
                //remove cards o ready
                GameManager.temporaryActionCards.Add(gameObject);


                //Marcar acción seleccionada
                isSelected = true;


                //Sumar una acción
                GameManager.selectedActions += 1;

                Debug.Log("La accion esta " + isSelected + " y hay " + GameManager.selectedActions + " acciones elegidas");
            }
        }
        else
        {
            Debug.Log("Acciones elegidas, no puedes seleccionar más");
        }
    }


    //Posición inicial de la carta
    public void InitialPosition()
    {
        this.GetComponent<RectTransform>().position = initialPosition;
    }
}
