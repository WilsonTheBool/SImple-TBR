using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Базовый класс для всех талантов
/// </summary>
public class TraitBase: MonoBehaviour, IConvertToInfoText
{

    public Character owner;

    public string traitName;

    [TextArea]
    public string traitDiscriprion;

    public TraitType type;

    public enum TraitType
    {
        positive,
        negative,
        other,
    }

    public virtual void Start()
    {
        SetUp();
    }

    protected virtual void SetUp()
    {

    }

    public string GetInfoValueDiscription()
    {
        return traitDiscriprion;
    }

    public string GetInfoValueName()
    {
        return traitName;
    }

    public TraitType GetInfoType()
    {
        return type;
    }
}

