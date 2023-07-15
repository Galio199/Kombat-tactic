using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Action : MonoBehaviour
{
    [Header("General")]
    public string ActionName;
    [SerializeField] private int priority;
    [SerializeField] protected int baseCooldown;
    [HideInInspector] public int cooldown = 0;
    [SerializeField] private int damage = 0;
    [SerializeField] private string effect = "";
    [SerializeField] protected GameObject effectCell;

    [Header("Range Image")]
    public Texture2D image;

    [Header("Animation")]
    public float durationAnimation;
    public float durationMessage;

    protected Character myCharacter;
    protected Character oponentCharacter;
    protected Vector2 offsetEffectCell;



    public int GetPriotiy()
    {
        return this.priority;
    }

    public int GetDamage()
    {
        return this.damage;
    }

    public string GetEffect()
    {
        return this.effect;
    }

    public int GetBaseCooldown()
    {
        return this.baseCooldown;
    }

    //Agregar las referencias de los personajes
    public void SetCharacters()
    {
        this.myCharacter = GetComponentInParent<Character>();
        this.oponentCharacter = myCharacter.GetOponent();
    }

    public void OffsetEffectCell()
    {
        if (myCharacter.GetPositionInCell() == PositionInCell.RIGHT)
        {
            offsetEffectCell = new Vector2(-1, 0);
        }
        else
        {
            offsetEffectCell = new Vector2(1, 0);
        }
    }

    //Funcion para mostrar mensajes
    public IEnumerator ShowFloatingMessage(string message, Color color, GameObject character, float wait = 0f)
    {
        yield return new WaitForSeconds(wait);
        TMP_Text textComponent = character.GetComponentInChildren<TMP_Text>(true);
        textComponent.text = message;
        textComponent.color = color;
        textComponent.gameObject.SetActive(true);
        yield return new WaitForSeconds(durationMessage);
        textComponent.gameObject.SetActive(false);
    }

    public abstract void Execute();

}
