using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Text StageText;
    public Text DeckUI;
    public float SecondsToBattle;

    public int CurrentStage = 1;
    
    private EnemyController Enemy;
    private PlayerController Player;

    enum StateType { UNKNOWN, ENEMY_TURN, PLAYER_TURN, BATTLE, BATTLE_EVALUATE };
    private StateType gameState = StateType.ENEMY_TURN;
    private bool cardDropped = false;
    private float waitToBatTmr = 0.0f;

    enum PTurnState { WAIT_FOR_CARD_PLAY, DELAY_TO_END_TURN };
    private PTurnState playerState = PTurnState.WAIT_FOR_CARD_PLAY;


    // Start is called before the first frame update
    void Start()
    {
        Enemy = FindObjectOfType<EnemyController>();
        Player = FindObjectOfType<PlayerController>();

        CurrentStage = 1;
        SecondsToBattle = 2;
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

        StageText.text = "Stage: " + CurrentStage.ToString();
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
            if (CurrentStage == 3)
            {
                // TODO: Show a game won action

                SceneManager.LoadScene("Menu");
            }
            Enemy.Health = Enemy.MaxHealth;
            CurrentStage += 1;
        }
        if (Player.Health <= 0)
        {
            // TODO: Show a game lost action

            // Game Over
            SceneManager.LoadScene("Menu");
        }

        gameState = StateType.ENEMY_TURN;
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
}
