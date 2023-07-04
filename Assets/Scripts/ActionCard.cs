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

    //Parametros de la carta de accion
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI coolDownText;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private Image attackArea;
    [SerializeField] private Image image;

    public Action action;

    //pruebas
    [SerializeField] private int cooldown;

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
        cooldown = action.cooldown;
    }


    //Al dar click sobre la tarjeta la elige
    public void PointerClick()
    {
        //comprobar si la carta está en cooldown
        if (action.cooldown != 0) return;
        //if (cooldown != 0) return; //pruebas


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


    //Poner parametros de la acción en la carta
    public void SetParameters()
    {
        actionText.text = action.ActionName;
        damageText.text = action.GetDamage().ToString();
        coolDownText.text = action.GetBaseCooldown().ToString();

        //Convertir imagen de tipo Texture2D a Sprite y asignar a la carta de accion
        Sprite imageSprite = Sprite.Create(action.image, new Rect(0, 0, action.image.width, action.image.height), Vector2.one * 0.5f);
        attackArea.sprite = imageSprite;
    }

    public void CoolDown()
    {
        cooldown = action.cooldown;
        if (cooldown != 0)
        {
            Debug.Log("Entro a oscurecerse, la variable cooldown es " + cooldown);
            //Color colorOpacity = gameObject.GetComponent<Image>().color;
            //colorOpacity.a = 0.5f;
            //gameObject.GetComponent<Image>().color = colorOpacity;

            Image[] images = gameObject.GetComponentsInChildren<Image>();

            foreach (Image image in images)
            {
                Color color = image.color;
                color.a = 0.5f;
                image.color = color;
            }
        }
        else
        {
            Image[] images = gameObject.GetComponentsInChildren<Image>();

            foreach (Image image in images)
            {
                Color color = image.color;
                color.a = 1f;
                image.color = color;
            }
        }
    }
}
