using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGameSettings : MonoBehaviour
{
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

    // Properties
    public bool RequiresSave { get { return reqSave; } }
    public bool HasBeenLoaded { get { return loadOnWakeDone; } }


    private int level1Outcome;
    private int level1Score;
    private bool reqSave = false;
    private bool loadOnWakeDone = false;

    // Do not change these keys or previous data will be lost
    private const string LEVEL_1_OUTCOME_KEY = "LEVEL_1_OUTCOME";
    private const string LEVEL_1_SCORE_KEY = "LEVEL_1_SCORE";

    public enum OutcomeType { LOST, WON, UNKNOWN = 255 };

    private void Awake()
    {
        loadOnWakeDone = LoadProperties();
        reqSave = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        ; // Put any init code you may neef for your property
    }

    public bool LoadProperties()
    {
        bool success = true;

        try 
        {
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
            PlayerPrefs.SetInt(LEVEL_1_OUTCOME_KEY, level1Outcome);
            PlayerPrefs.SetInt(LEVEL_1_SCORE_KEY, level1Score);
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
            if (PlayerPrefs.HasKey(LEVEL_1_OUTCOME_KEY))
            {
                PlayerPrefs.DeleteKey(LEVEL_1_OUTCOME_KEY);
            }
            if (PlayerPrefs.HasKey(LEVEL_1_SCORE_KEY))
            {
                PlayerPrefs.DeleteKey(LEVEL_1_SCORE_KEY);
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
