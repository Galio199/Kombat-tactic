using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PositionInCell
{
    LEFT, RIGHT
}

public class Character : MonoBehaviour
{
    public string CharacterName;

    [Header("Stats")]
    public int health=100;
    public int priorityChange=0;
    public int damageChange=0;
    public int guardChange=0;

    [Header("Actions")]
    [SerializeField] private Action[] actions;

    public Sprite miniature;

    [SerializeField] private Character oponent;
    public Image healthBar;

    private PositionInCell positionInCell = PositionInCell.LEFT;

    // Inicializar los valores de las estadisticas
    public void ResetStats()
    {
        health = 100;
        priorityChange = 0;
        damageChange = 0;
        guardChange = 0;
    }

    public Action[] GetActions()
    {
        return actions;
    }

    public void SetOponent(Character oponent)
    {
        this.oponent = oponent;
        SetCharactersInActions();
    }

    public Character GetOponent()
    {
        return oponent;
    }

    private void SetCharactersInActions()
    {
        foreach(Action action in actions)
        {
            action.SetCharacters();
            action.OffsetEffectCell();
        }
    }

    public void SetPositionInCell(PositionInCell position)
    {
        this.positionInCell = position;
    }

    public PositionInCell GetPositionInCell()
    {
        return positionInCell;
    }

}

