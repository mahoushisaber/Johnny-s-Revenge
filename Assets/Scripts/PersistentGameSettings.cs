using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGameSettings : MonoBehaviour
{
    // Property
    public int CurrentLevel
    {
        get
        {
            return curLevel;
        }
        set
        {
            if (curLevel != value)
            {
                reqSave = true;
            }
            curLevel = value;
        }
    }

    public int ActiveLevel
    {
        get
        {
            return activeLevel;
        }
        set
        {
            if (activeLevel != value)
            {
                reqSave = true;
            }
            activeLevel = value;
        }
    }

    // Property
    public int Level1Score 
    { 
        get 
        { 
            return level1Score; 
        } 
        set 
        {
            if (level1Score != value)
            {
                reqSave = true;
            }
            level1Score = value; 
        } 
    }

    // Property
    public OutcomeType Level1Outcome
    {
        get
        {
            OutcomeType retVal = (OutcomeType)level1Outcome;
            return retVal;
        }
        set
        {
            if (level1Outcome != (int)value)
            {
                reqSave = true;
            }

            level1Outcome = (int)value;
        }
    }

    // Property
    public int Level2Score
    {
        get
        {
            return level2Score;
        }
        set
        {
            if (level2Score != value)
            {
                reqSave = true;
            }
            level2Score = value;
        }
    }

    // Property
    public OutcomeType Level2Outcome
    {
        get
        {
            OutcomeType retVal = (OutcomeType)level2Outcome;
            return retVal;
        }
        set
        {
            if (level2Outcome != (int)value)
            {
                reqSave = true;
            }

            level2Outcome = (int)value;
        }
    }

    // Property
    public int Level3Score
    {
        get
        {
            return level3Score;
        }
        set
        {
            if (level3Score != value)
            {
                reqSave = true;
            }
            level3Score = value;
        }
    }

    // Property
    public OutcomeType Level3Outcome
    {
        get
        {
            OutcomeType retVal = (OutcomeType)level3Outcome;
            return retVal;
        }
        set
        {
            if (level3Outcome != (int)value)
            {
                reqSave = true;
            }

            level3Outcome = (int)value;
        }
    }

    // Properties
    public bool RequiresSave { get { return reqSave; } }
    public bool HasBeenLoaded { get { return loadOnWakeDone; } }

    private int curLevel;
    private int activeLevel;
    private int level1Outcome;
    private int level1Score;
    private int level2Outcome;
    private int level2Score;
    private int level3Outcome;
    private int level3Score;
    private bool reqSave = false;
    private bool loadOnWakeDone = false;

    // Do not change these keys or previous data will be lost
    private const string CURRENT_LEVEL_KEY = "CURRENT_LEVEL";
    private const string ACTIVE_LEVEL_KEY = "ACTIVE_LEVEL";
    private const string LEVEL_1_OUTCOME_KEY = "LEVEL_1_OUTCOME";
    private const string LEVEL_1_SCORE_KEY = "LEVEL_1_SCORE";
    private const string LEVEL_2_OUTCOME_KEY = "LEVEL_2_OUTCOME";
    private const string LEVEL_2_SCORE_KEY = "LEVEL_2_SCORE";
    private const string LEVEL_3_OUTCOME_KEY = "LEVEL_3_OUTCOME";
    private const string LEVEL_3_SCORE_KEY = "LEVEL_3_SCORE";

    public enum OutcomeType { LOST, WON, UNKNOWN = 255 };

    private void Awake()
    {
        loadOnWakeDone = LoadProperties();
        reqSave = false;
    }

    public bool LoadProperties()
    {
        bool success = true;

        try 
        {
            if (PlayerPrefs.HasKey(CURRENT_LEVEL_KEY))
            {
                // Key exists so load the value
                curLevel = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY);
            }

            if (PlayerPrefs.HasKey(ACTIVE_LEVEL_KEY))
            {
                // Key exists so load the value
                activeLevel = PlayerPrefs.GetInt(ACTIVE_LEVEL_KEY);
            }

            if (PlayerPrefs.HasKey(LEVEL_1_OUTCOME_KEY))
            {
                // Key exists so load the value
                level1Outcome = PlayerPrefs.GetInt(LEVEL_1_OUTCOME_KEY);
            }

            if (PlayerPrefs.HasKey(LEVEL_1_SCORE_KEY))
            {
                // Key exists so load the value
                level1Score = PlayerPrefs.GetInt(LEVEL_1_SCORE_KEY);
            }

            if (PlayerPrefs.HasKey(LEVEL_2_OUTCOME_KEY))
            {
                // Key exists so load the value
                level2Outcome = PlayerPrefs.GetInt(LEVEL_2_OUTCOME_KEY);
            }

            if (PlayerPrefs.HasKey(LEVEL_2_SCORE_KEY))
            {
                // Key exists so load the value
                level2Score = PlayerPrefs.GetInt(LEVEL_2_SCORE_KEY);
            }

            if (PlayerPrefs.HasKey(LEVEL_3_OUTCOME_KEY))
            {
                // Key exists so load the value
                level3Outcome = PlayerPrefs.GetInt(LEVEL_3_OUTCOME_KEY);
            }

            if (PlayerPrefs.HasKey(LEVEL_3_SCORE_KEY))
            {
                // Key exists so load the value
                level3Score = PlayerPrefs.GetInt(LEVEL_3_SCORE_KEY);
            }
        }
        catch (System.Exception e)
        {
            success = false;
            Debug.Log(e.Message);
        }

        reqSave = success;

        return success;
    }

    public bool SaveProperties()
    {
        bool success = true;

        try
        {
            PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, curLevel);
            PlayerPrefs.SetInt(ACTIVE_LEVEL_KEY, activeLevel);
            PlayerPrefs.SetInt(LEVEL_1_OUTCOME_KEY, level1Outcome);
            PlayerPrefs.SetInt(LEVEL_1_SCORE_KEY, level1Score);
            PlayerPrefs.SetInt(LEVEL_2_OUTCOME_KEY, level2Outcome);
            PlayerPrefs.SetInt(LEVEL_2_SCORE_KEY, level2Score);
            PlayerPrefs.SetInt(LEVEL_3_OUTCOME_KEY, level2Outcome);
            PlayerPrefs.SetInt(LEVEL_3_SCORE_KEY, level2Score);
        }
        catch (System.Exception e)
        { 
            success = false;
            Debug.Log(e.Message);
        }

        reqSave = !success;

        return success;
    }

    public bool DeleteProperties()
    {
        bool success = true;

        try
        {
            if (PlayerPrefs.HasKey(ACTIVE_LEVEL_KEY))
            {
                PlayerPrefs.DeleteKey(ACTIVE_LEVEL_KEY);
            }
            if (PlayerPrefs.HasKey(LEVEL_1_OUTCOME_KEY))
            {
                PlayerPrefs.DeleteKey(LEVEL_1_OUTCOME_KEY);
            }
            if (PlayerPrefs.HasKey(LEVEL_1_SCORE_KEY))
            {
                PlayerPrefs.DeleteKey(LEVEL_1_SCORE_KEY);
            }

            if (PlayerPrefs.HasKey(LEVEL_2_OUTCOME_KEY))
            {
                PlayerPrefs.DeleteKey(LEVEL_2_OUTCOME_KEY);
            }
            if (PlayerPrefs.HasKey(LEVEL_2_SCORE_KEY))
            {
                PlayerPrefs.DeleteKey(LEVEL_2_SCORE_KEY);
            }

            if (PlayerPrefs.HasKey(LEVEL_3_OUTCOME_KEY))
            {
                PlayerPrefs.DeleteKey(LEVEL_3_OUTCOME_KEY);
            }
            if (PlayerPrefs.HasKey(LEVEL_3_SCORE_KEY))
            {
                PlayerPrefs.DeleteKey(LEVEL_3_SCORE_KEY);
            }
        }
        catch (System.Exception e)
        {
            success = false;
            Debug.Log(e.Message);
        }

        reqSave = success;

        return success;
    }
}
