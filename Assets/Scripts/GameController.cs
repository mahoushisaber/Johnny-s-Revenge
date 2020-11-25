using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public readonly int TotalLevels = 3;
    public readonly int TotalStages = 3;
    public int CurrentLevel = 1;
    public int CurrentStage = 1;
    public float SecondsToBattle;
    public Text LevelText;
    public Text DeckUI;
    public Image BattleZoneArea;
    public Image PlayerManaBarHighlightImage;

    private EnemyController Enemy;
    private PlayerController Player;
    private BackgroundController Background;
    private ArrowSignifiers GameSignifiers;

    enum StateType { UNKNOWN, ENEMY_TURN, PLAYER_TURN, BATTLE, BATTLE_EVALUATE };
    private StateType gameState = StateType.ENEMY_TURN;
    private bool cardDropped = false;
    private float waitToBatTmr = 0.0f;
    private bool manaUsedThisTurn = false;

    enum PTurnState { START_TURN_INIT, WAIT_FOR_CARD_PLAY, DELAY_TO_END_TURN };
    private PTurnState playerState = PTurnState.START_TURN_INIT;

    private PersistentGameSettings gameSettings;
    private Sprite BZAreaArtwork;
    private Sprite PMBarArtwork;

    private ScoreSystem ScoreSystem;
    private int ActiveLevel;

    // Start is called before the first frame update
    void Start()
    {
        Enemy = FindObjectOfType<EnemyController>();
        Player = FindObjectOfType<PlayerController>();
        gameSettings = FindObjectOfType<PersistentGameSettings>();
        ScoreSystem = FindObjectOfType<ScoreSystem>();
        Background = FindObjectOfType<BackgroundController>();
        GameSignifiers = FindObjectOfType<ArrowSignifiers>();
        BZAreaArtwork = BattleZoneArea.sprite;
        PMBarArtwork = PlayerManaBarHighlightImage.sprite;

        CurrentStage = 1;
        CurrentLevel = gameSettings.CurrentLevel;
        ActiveLevel = gameSettings.ActiveLevel;

        if (CurrentLevel != ActiveLevel)
        {
            // Upon return they are not the same so we are starting a new level
            NextLevel();
        }
        if (gameSettings.CurrentLevel == 1)
        {
            FindObjectOfType<AudioManager>().Play("EpicString");
        }
        if (gameSettings.CurrentLevel == 2)
        {
            FindObjectOfType<AudioManager>().Play("Theme2");
        }
        if (gameSettings.CurrentLevel == 3)
        {
            FindObjectOfType<AudioManager>().Play("Theme4");
        }
       
    }

    // Update is called once per frame
    void Update()
    {
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

        LevelText.text = string.Format("Level: {0}.{1}", CurrentLevel, CurrentStage);
        DeckUI.text = Player.cardsInDeck.ToString();
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
            case PTurnState.START_TURN_INIT:
                // Do not move on to next turn until the score screen has disappeared!
                if (ScoreSystem.ResultScreenShowing == false)
                { 
                    // Set up the signifiers so they will properly manage
                    GameSignifiers.StartTurn(Player.Mana >= Player.ManaUseCost);
                    manaUsedThisTurn = false;

                    // Goto the new state
                    playerState = PTurnState.WAIT_FOR_CARD_PLAY;
                }
                break;

            // Waiting for a card to be dropped in the dropzone
            case PTurnState.WAIT_FOR_CARD_PLAY:
                if (cardDropped)
                {
                    cardDropped = false;

                    // Set timer to wait perioid
                    waitToBatTmr = 0.0f;

                    // Stop signifiers
                    GameSignifiers.EndTurn(manaUsedThisTurn);

                    // Goto the new state
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
                        playerState = PTurnState.START_TURN_INIT;
                        gameState = StateType.BATTLE;
                        Player.EndTurn();
                    }
                }
                else
                {
                    // No Seconds to Battle set so immediately move to battle state
                    playerState = PTurnState.START_TURN_INIT;
                    gameState = StateType.BATTLE;
                }
                break;

            default:
                playerState = PTurnState.START_TURN_INIT;
                gameState = StateType.BATTLE;
                Debug.Log("Game Controller - PlayerTurn State INVALID");
                break;
        }
    }


    void Execute_Battle()
    {
        // Start the card battle by analyzing who won and apply results
        Enemy.Health = Player.StartBattle(Enemy.Health, ref Enemy.Shield);
        
        if (Enemy.Health <= 0)
        {
            Enemy.Health = 0;
            Enemy.EndBattle();
            Player.EndBattle();
        }
        else
        {
            Player.Health = Enemy.StartBattle(Player.Health, ref Player.Shield);
            Enemy.EndBattle();
            Player.EndBattle();
        }
        
        // Clean up the battle cards and anything else

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
            // Game Over Player LOST save the result and move to menu screen
            if (CurrentLevel == 1)
            {
                gameSettings.Level1Outcome = PersistentGameSettings.OutcomeType.LOST;
                gameSettings.Level1Score = ScoreSystem.levelScore;
            }
            else if (CurrentLevel == 2)
            {
                gameSettings.Level2Outcome = PersistentGameSettings.OutcomeType.LOST;
                gameSettings.Level2Score = ScoreSystem.levelScore;
            }
            else if (CurrentLevel == 3)
            {
                gameSettings.Level3Outcome = PersistentGameSettings.OutcomeType.LOST;
                gameSettings.Level3Score = ScoreSystem.levelScore;
            }

            if (gameSettings.RequiresSave)
            {
                // Requires a save so we can use the values over game instances or between scenes
                gameSettings.SaveProperties();
            }

            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }

        gameState = StateType.ENEMY_TURN;
    }

    public void PlayerHandDragBegin(PointerEventData eventData)
    {
        // Turn on highlight for available drop zones. Don't forget to save current to restore
        Sprite artwork = Resources.Load<Sprite>("Sprites/BZ-Highlight");
        BattleZoneArea.sprite = artwork;

        // Don't turn on mana if there is not enough to use.
        if (Player.Mana >= Player.ManaUseCost)
        {
            Color PMBOrigColor = PlayerManaBarHighlightImage.color;
            PMBOrigColor.a = 255f;
            PlayerManaBarHighlightImage.color = PMBOrigColor;
        }
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
                    else
                    {
                        // Are using mana so record use for signifier tracking
                        manaUsedThisTurn = true;
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
            string nextScene = "AdventureMap";

            // Level Over Player WON save the result and move to menu screen
            if (CurrentLevel == 1)
            {
                gameSettings.Level1Outcome = PersistentGameSettings.OutcomeType.WON;
                gameSettings.Level1Score = ScoreSystem.levelScore;
                gameSettings.ActiveLevel = 2;
            }
            else if (CurrentLevel == 2)
            {
                gameSettings.Level2Outcome = PersistentGameSettings.OutcomeType.WON;
                gameSettings.Level2Score = ScoreSystem.levelScore;
                gameSettings.ActiveLevel = 3;
            }
            else if (CurrentLevel == 3)
            {
                gameSettings.Level3Outcome = PersistentGameSettings.OutcomeType.WON;
                gameSettings.Level3Score = ScoreSystem.levelScore;
                gameSettings.ActiveLevel = 0;
                nextScene = "Menu";
            }

            if (gameSettings.RequiresSave)
            {
                // Requires a save so we can use the values over game instances or between scenes
                gameSettings.SaveProperties();
            }

            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }
        else
        {
            Enemy.setEnemy(CurrentLevel, CurrentStage+1);
        }
        Enemy.Health = Enemy.MaxHealth;
        CurrentStage += 1;
        
    }

    public void NextLevel()
    {
        // Must save the change locally and to persistent data so progression
        // of stages and levels are picked up here and in other scenes.
        CurrentLevel = ActiveLevel;
        gameSettings.CurrentLevel = CurrentLevel;
        gameSettings.SaveProperties();

        // Load new settings for the CurrentLevel to play
        Background.setBackground(CurrentLevel);
        Enemy.setEnemy(CurrentLevel, CurrentStage);

        // !!!! NOTE NOR SURE HOW TO CHANGE SETTINGS BACK TO NEW GAME AFTER WIN !!!!
        Debug.LogFormat("New Level {0} Achieved", CurrentLevel);
    }
}
