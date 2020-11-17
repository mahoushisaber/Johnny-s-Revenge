using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HighScoreController : MonoBehaviour
{
    public int[] highScores = new int[highScoresCount];
    public string[] players = new string[highScoresCount];
    public static HighScoreController gCtrl;
    public const int highScoresCount = 5;

    const string fileName = "/highscore.dat";

    private enum StateType { SEARCHING, NEW_HIGH_SCORE };

    public void Awake()
    {
        // Initalize the players but don't replace ones that have something in them as we could be switching scenes
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null || players[i].Length == 0)
            {
                players[i] = "--";
            }
        }

        if (gCtrl == null)
        {
            gCtrl = this;
            LoadHighScore();
        }
    }

    private void ResetScores()
    {
        GameData data = new GameData();

        for (int i = 0; i < players.Length; i++)
        {
            players[i] = "--";
            data.players[i] = players[i];
        }

        for (int i = 0; i < highScores.Length; i++)
        {
            highScores[i] = 0;
            data.highScores[i] = 0;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(Application.persistentDataPath + fileName, FileMode.OpenOrCreate);
        bf.Serialize(fs, data);
        fs.Close();
    }

    public void LoadHighScore()
    {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + fileName, FileMode.Open, FileAccess.Read);
            GameData data = (GameData)bf.Deserialize(fs);
            fs.Close();
            for (int i = 0; i < highScores.Length; i++)
            {
                players[i] = data.players[i];
                highScores[i] = data.highScores[i];
            }
        }
    }

    public bool IsThisAHighScore(int Score)
    {
        bool IsHighScore = false;

        // Iterate through all the highscores to see if this score is higher
        for (int i = 0; i < highScores.Length; i++)
        {
            if (Score > highScores[i])
            {
                IsHighScore = true;
                break;
            }
        }
        
        return IsHighScore;
    }

    public void SaveHighScore(int Score, string PlayerName)
    {
        StateType state = StateType.SEARCHING;
        int lastScore = 0;
        string lastPlayer = "--";
        GameData data = new GameData();

        // Iterate through all the highscores to see if this score is higher
        for (int i = 0; i < highScores.Length; i++)
        {
            if (state == StateType.NEW_HIGH_SCORE)
            {
                // Save current score and replace with new high score then switch state to shiftdown
                int currScore = lastScore;
                string currPlayer = lastPlayer;
                lastScore = highScores[i];
                lastPlayer = players[i];
                highScores[i] = currScore;
                players[i] = currPlayer;
            }
            else if (state == StateType.SEARCHING && Score > highScores[i])
            {
                // Save current score and replace with new high score then switch state to shiftdown
                lastScore = highScores[i];
                lastPlayer = players[i];
                highScores[i] = Score;
                players[i] = PlayerName;
                data.highScores[i] = highScores[i];
                data.players[i] = players[i];
                state = StateType.NEW_HIGH_SCORE;
            }
            else
            {
                // In case we find a high score the ones befoer have to be intact
                data.highScores[i] = highScores[i];
                data.players[i] = players[i];
            }
        }

        // If we found a high score than save it out
        if (state == StateType.NEW_HIGH_SCORE)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + fileName, FileMode.OpenOrCreate);
            bf.Serialize(fs, data);
            fs.Close();
        }
    }

    public void ResetScoresPressed()
    {
        ResetScores();
    }
}

[Serializable]
class GameData
{
    public int[] highScores = new int[HighScoreController.highScoresCount];
    public string[] players = new string[HighScoreController.highScoresCount];
}