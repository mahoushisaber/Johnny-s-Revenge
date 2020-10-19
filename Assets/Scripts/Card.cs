using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Card
{
    public int Id;
    public string Name;
    public int Power;
    public string Description;
    public bool Enhanced;
    public string Colour;
    public string ImageName;

    public enum OwnerType { UNKNOWN, ENEMY, PLAYER };
    public OwnerType Owner;

    public Card()
    {
        Owner = OwnerType.UNKNOWN;
    }

    public Card(int ID, string AName, int PowerLevel, string ADescription, bool IsEnhanced, string AColour, OwnerType AOwner, string AImageName)
    {
        Id = ID;
        Name = AName;
        Power = PowerLevel;
        Description = ADescription;
        Enhanced = IsEnhanced;
        Colour = AColour;
        Owner = AOwner;
        ImageName = AImageName;
    }

    public Card(Card ToCopy)
    {
        Id = ToCopy.Id;
        Name = ToCopy.Name;
        Power = ToCopy.Power;
        Description = ToCopy.Description;
        Enhanced = ToCopy.Enhanced;
        Colour = ToCopy.Colour;
        Owner = ToCopy.Owner;
        ImageName = ToCopy.ImageName;
    }

    public void Copy(Card ToCopy)
    {
        Id = ToCopy.Id;
        Name = ToCopy.Name;
        Power = ToCopy.Power;
        Description = ToCopy.Description;
        Enhanced = ToCopy.Enhanced;
        Colour = ToCopy.Colour;
        Owner = ToCopy.Owner;
        ImageName = ToCopy.ImageName;
    }
}
