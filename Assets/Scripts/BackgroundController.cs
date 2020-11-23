using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public Sprite Level1;
    public Sprite Level2;
    public Sprite Level3;
    
    public SpriteRenderer BackgroundSprite;

    private Sprite[] StageSprites;
    // Start is called before the first frame update
    void Start()
    {
        StageSprites = new Sprite[3];
        StageSprites[0] = Level1;
        StageSprites[1] = Level2;
        StageSprites[2] = Level3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setBackground(int levelNum)
    {
        BackgroundSprite.sprite = StageSprites[ levelNum - 1 ];
    }
}
