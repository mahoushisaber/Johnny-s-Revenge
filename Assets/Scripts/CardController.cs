using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class CardController : MonoBehaviour
{
    public int Id { get { return ThisCard.Id; } } // To set use InitWithCard(Card)
    public string Name { get { return ThisCard.Name; } }
    public int Power { get { return Enhanced ? ThisCard.Power * EnhancedMultiplier : ThisCard.Power; } }
    public string Description { get { return ThisCard.Description; } }
    public bool Enhanced { get { return ThisCard.Enhanced; } set { ThisCard.Enhanced = value; } }
    public Card.OwnerType Owner { get { return ThisCard.Owner; } set { ThisCard.Owner = value; } }
    public Sprite ImageSprite { get { return artwork; } }

    public int EnhancedMultiplier { get { return EnhanceMulti; } set { EnhanceMulti = (value > 1) ? value : 1; } }

    public Image ImageToColour;
    public Image ImageForSprite;
    public Text NameText;
    public Text PowerText;
    public Text DescriptionText;
    public Sprite artwork;

    private Card ThisCard = new Card();
    private int EnhanceMulti = 2;

    private void Update()
    {
        NameText.text = Name;
        PowerText.text = Power.ToString();
        DescriptionText.text = Description;
        artwork = ImageSprite;

        if (ThisCard.Colour == "Red")
        {
            ImageToColour.GetComponent<Image>().color = new Color32(115, 15, 45, 255);
        } 
        if (ThisCard.Colour == "Blue")
        {
            ImageToColour.GetComponent<Image>().color = new Color32(25, 90, 140, 255);
        }
        if (ThisCard.Colour == "Green")
        {
            ImageToColour.GetComponent<Image>().color = new Color32(30, 125, 20, 255);
        }
        if (ThisCard.Colour == "None")
        {
            ImageToColour.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        }


    }

    public void InitWithCard(Card ToLoad)
    {
        ThisCard.Copy(ToLoad);
        artwork = Resources.Load<Sprite>(ThisCard.ImageName);
        ImageForSprite.sprite = artwork;
    }
}
