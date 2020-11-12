using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public Text StageText;
    public int TotalStages;
    public int CurrentStage = 1;
    public Text DeckUI;
    public float SecondsToBattle;
    public Image BattleZoneArea;
    public Image PlayerManaBarHighlightImage;
    public Sprite BossSprite, BossSprite2, BossSprite3;

    private EnemyController Enemy;
    private PlayerController Player;

    enum StateType { UNKNOWN, ENEMY_TURN, PLAYER_TURN, BATTLE, BATTLE_EVALUATE };
    private StateType gameState = StateType.ENEMY_TURN;
    private bool cardDropped = false;
    private float waitToBatTmr = 0.0f;

    enum PTurnState { WAIT_FOR_CARD_PLAY, DELAY_TO_END_TURN };
    private PTurnState playerState = PTurnState.WAIT_FOR_CARD_PLAY;

    private PersistentGameSettings gameSettings;
    private Sprite BZAreaArtwork;
    private Sprite PMBarArtwork;

    private ScoreSystem ScoreSystem;

    // Start is called before the first frame update
    void Start()
    {
        Enemy = FindObjectOfType<EnemyController>();
        Player = FindObjectOfType<PlayerController>();
        gameSettings = FindObjectOfType<PersistentGameSettings>();
        ScoreSystem = FindObjectOfType<ScoreSystem>();
        BZAreaArtwork = BattleZoneArea.sprite;
        PMBarArtwork = PlayerManaBarHighlightImage.sprite;

        CurrentStage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        RenderBossSprites();
        switch (gameState)
        {
            case StateType.UNKNOWN:
            default:
                Debug.Log("Game Controller in Unknonwn State");
                break;

            case StateType.ENEMY_TURN:
                Execute_EnemyTurn();
                break;

            case StateType.PLAYER_TURN:
                Execute_PlayerTurn();
                break;

            case StateType.BATTLE:
                Execute_Battle();
                break;

            case StateType.BATTLE_EVALUATE:
                Execute_BattleEvaluate();
                break;
        }
       
        StageText.text = string.Format("Stage: {0} of {1}", CurrentStage, TotalStages);
        DeckUI.text = Player.cardsInDeck.ToString();
    }

    void RenderBossSprites()
    {
        if (CurrentStage == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = BossSprite;

        }
        if (CurrentStage == 2)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = BossSprite2;

        }
        if (CurrentStage == 3)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = BossSprite3;

        }
    }
    void Execute_EnemyTurn()
    {
        if (!Enemy.IsTurn)
        {
            Enemy.StartTurn();
        }
        else
        {
            Enemy.EndTurn();
            gameState = StateType.PLAYER_TURN;
        }
    }

    void Execute_PlayerTurn()
    {
        switch (playerState)
        {
            // Waiting for a card to be dropped in the dropzone
            case PTurnState.WAIT_FOR_CARD_PLAY:
                if (cardDropped)
                {
                    cardDropped = false;

                    // Set timer to wait perioid
                    waitToBatTmr = 0.0f;

                    playerState = PTurnState.DELAY_TO_END_TURN;
                }
                break;

            case PTurnState.DELAY_TO_END_TURN:
                // Has battle timer wait hit?
                if (SecondsToBattle > 0.0f)
                {
                    waitToBatTmr += Time.deltaTime;

                    if (waitToBatTmr > SecondsToBattle)
                    {
                        // Timer done reset player state machine move to battle state
                        waitToBatTmr = 0.0f;
                        playerState = PTurnState.WAIT_FOR_CARD_PLAY;
                        gameState = StateType.BATTLE;
                        Player.EndTurn();
                    }
                }
                else
                {
                    // No Seconds to Battle set so immediately move to battle state
                    playerState = PTurnState.WAIT_FOR_CARD_PLAY;
                    gameState = StateType.BATTLE;
                }
                break;

            default:
                playerState = PTurnState.WAIT_FOR_CARD_PLAY;
                gameState = StateType.BATTLE;
                Debug.Log("Game Controller - PlayerTurn State INVALID");
                break;
        }
    }


    void Execute_Battle()
    {
        // Start the card battle by analyzing who won and apply results
        Player.Health = Enemy.StartBattle(Player.Health, ref Player.Shield);
        Enemy.Health = Player.StartBattle(Enemy.Health, ref Enemy.Shield);

        // Clean up the battle cards and anything else
        Enemy.EndBattle();
        Player.EndBattle();

        gameState = StateType.BATTLE_EVALUATE;
    }

    void Execute_BattleEvaluate()
    {
        if (Enemy.Health <= 0)
        {
            ScoreSystem.showResult();
        }
        if (Player.Health <= 0)
        {
            // Game Over Player LOST
            gameSettings.Level1Outcome = PersistentGameSettings.OutcomeType.LOST;
            SceneManager.LoadScene("Menu");
        }

        if ( gameSettings.RequiresSave )
        {
            // Requires a save so we can use the values over game instances or between scenes
            gameSettings.SaveProperties();
        }

        gameState = StateType.ENEMY_TURN;
    }

    public void PlayerHandDragBegin(PointerEventData eventData)
    {
        // Turn on highlight for available drop zones. Don't forget to save current to restore
        Sprite artwork = Resources.Load<Sprite>("Sprites/BZ-Highlight");

        BattleZoneArea.sprite = artwork;
        Color PMBOrigColor = PlayerManaBarHighlightImage.color;
        PMBOrigColor.a = 255f;
        PlayerManaBarHighlightImage.color = PMBOrigColor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // TODO : Play it safe turn off highlight for available drop zones
        BattleZoneArea.sprite = BZAreaArtwork;
        Color PMBOrigColor = PlayerManaBarHighlightImage.color;
        PMBOrigColor.a = 0f;
        PlayerManaBarHighlightImage.color = PMBOrigColor;
    }

    public bool CanDropPlayerCard(Draggable.Slot slot)
    {
        bool canDrop = true;

        if (playerState == PTurnState.WAIT_FOR_CARD_PLAY)
        {
            switch (slot)
            {
                case Draggable.Slot.BATTLE:
                    // Any necessary processing can be done to confirm drop
                    break;

                case Draggable.Slot.MANA:
                    if (Player.Mana - Player.ManaUseCost < 0)
                    {
                        Debug.Log("Not enough mana to enhance");
                        canDrop = false;
                    }
                    break;

                default:
                    canDrop = false;
                    break;
            }

            // Do not change notification of card dropped if one was already dropped. 
            // Multiple drops will be picked up
            if (!cardDropped)
            {
                cardDropped = canDrop;
            }
        }
        else
        {
            canDrop = false;
            Debug.Log("Cannot play a card until its your turn");
        }

        return canDrop;
    }

    public void nextStage()
    {
        if (CurrentStage >= TotalStages)
        {
            // Game Over Player Won
            gameSettings.Level1Outcome = PersistentGameSettings.OutcomeType.WON;
            SceneManager.LoadScene("Menu");
        }
        Enemy.Health = Enemy.MaxHealth;
        CurrentStage += 1;
    }
}
