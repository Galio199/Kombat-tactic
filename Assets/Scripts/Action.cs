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
    

    [Header("Range Image")]
    public Texture2D image;

    [Header("Animation")]
    public float durationAnimation;
    [SerializeField] protected float durationMessage;
    [SerializeField] protected float durationEffectCell;

    [Header("EffectCells")]
    [SerializeField] protected GameObject effectCell;
    protected List<GameObject> effectCells = new List<GameObject>();
    protected Vector2 offsetEffectCell;

    protected Character myCharacter;
    protected Character oponentCharacter;
    



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
    protected IEnumerator ShowFloatingMessage(string message, Color color, GameObject character)
    {
        TMP_Text textComponent = character.GetComponentInChildren<TMP_Text>(true);
        textComponent.text = message;
        textComponent.color = color;
        textComponent.gameObject.SetActive(true);
        yield return new WaitForSeconds(durationMessage);
        textComponent.gameObject.SetActive(false);
    }

    //Funcion para mostrar el rango de casillas que afecta el ataque
    protected void ShowEffectCell(Vector2 position)
    {
        GameObject effectObject = Instantiate(effectCell, position, Quaternion.identity);
        effectCells.Add(effectObject);
    }

    protected IEnumerator DestroyEffectCell()
    {
        yield return new WaitForSeconds(durationEffectCell);
        foreach (GameObject cell in effectCells)
        {
            Destroy(cell);
        }
        effectCells.Clear();
    }
    public abstract void Execute();

}
