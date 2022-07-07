using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class HighscoreManager : MonoBehaviour
{
    private string connectionString;
    private List<Highscore> highscores = new List<Highscore>();
    private List<string> profanity = new List<string>();

    public GameObject ScorePrefab;
    public Transform scoreParent;
    public int topRank;
    public int dbMax;

    public Button enter;
    public TMP_InputField name;

    // Start is called before the first frame update
    void Start()
    {
        connectionString = "URI=file:" + Application.dataPath + "/HighscoresDB.sqlite";
        createTable();
        checkTable();
        populateProfanity();
    }
    public void ButtonUpdate()
    {
        highscores.Clear();
        int score = int.Parse(GameManager.score.ToString());
        string inputName = name.text.ToLower();
        Debug.Log(inputName);
        foreach (string badName in profanity)
        {
            Debug.Log(badName);
            if (inputName.Equals(badName))
            {
                inputName = "#$%";
                break;
            }
            continue;
        }
        insertScore(inputName, score);
        showScores();
    }
    
    private void populateProfanity()
    {
        profanity.Add("ass");
        profanity.Add("a$$");
        profanity.Add("a55");
        profanity.Add("azz");
        profanity.Add("bod");
        profanity.Add("bra");
        profanity.Add("br@");
        profanity.Add("cum");
        profanity.Add("fag");
        profanity.Add("f@g");
        profanity.Add("fuk");
        profanity.Add("gae");
        profanity.Add("gay");
        profanity.Add("g@y");
        profanity.Add("gey");
        profanity.Add("gfy");
        profanity.Add("hiv");
        profanity.Add("jiz");
        profanity.Add("jlz");
        profanity.Add("kkk");
        profanity.Add("lez");
        profanity.Add("nad");
        profanity.Add("pee");
        profanity.Add("pot");
        profanity.Add("poo");
        profanity.Add("rum");
        profanity.Add("sex");
        profanity.Add("t1t");
        profanity.Add("uzi");
        profanity.Add("vag");
        profanity.Add("wad");
        profanity.Add("wtf");
        profanity.Add("xxx");
    }

    private void checkTable()
    {
        int num;

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT COUNT(*) FROM Highscores";

                dbCmd.CommandText = sqlQuery;

                num = Convert.ToInt32(dbCmd.ExecuteScalar());
                Debug.Log("Total count" + num);
                dbConnection.Close();
            }
        }
         if (num == 0)
         {
             insertScore("dum", -1);
             insertScore("dum", -1);
             insertScore("dum", -1);
         }
    }

    public void clearScores()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "DELETE FROM Highscores";

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();
                dbConnection.Close();

            }
        }
        highscores.Clear();
        showScores();
        insertScore("dum", -1);
        insertScore("dum", -1);
        insertScore("dum", -1);
    }

    private void getScores()
    {
        highscores.Clear();
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("SELECT * FROM Highscores LIMIT 3,\"{0}\"",dbMax);

                dbCmd.CommandText = sqlQuery;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        highscores.Add(new Highscore(reader.GetInt32(0), reader.GetInt32(2), reader.GetString(1)));
                     
                    }
                dbConnection.Close();
                reader.Close();
                }
            }
        }
        highscores.Sort();
        int num;
        int id;

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT COUNT(*) FROM Highscores";

                dbCmd.CommandText = sqlQuery;

                num = Convert.ToInt32(dbCmd.ExecuteScalar());
                Debug.Log("Total count" + num);
                dbConnection.Close();
            }
        }
        if (num > topRank+10)
        {
            id = highscores[topRank+7].getid();
            deleteScore(id);
        }
    }
    public void insertScore(string name, int newScore)
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("INSERT INTO Highscores(Name,Score) VALUES(\"{0}\",\"{1}\")",name,newScore);
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }

    private void createTable()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("CREATE TABLE IF NOT EXISTS Highscores(PlayerID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE," +
                    " Name TEXT NOT NULL, Score INTEGER NOT NULL);");
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
        showScores();

    }
    private void deleteScore(int id)
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("DELETE FROM Highscores WHERE PlayerID = \"{0}\"", id);

                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
        showScores();
    }
    // Have not implemented it, will be used for making sure the computer doesnt crash with all the memory loading into it

    public void showScores()
    {
        getScores();
        foreach (GameObject score in GameObject.FindGameObjectsWithTag("Score"))
        {
            Destroy(score);
        }
        for (int i = 0; i < topRank; i++)
        {
            GameObject tmp = Instantiate(ScorePrefab);
            if (highscores.Count()-1 < i)
            {
                break;
            }
            Highscore tmpscore = highscores[i];

            tmp.GetComponent<HighscoreScript>().setScore(tmpscore.Name, tmpscore.Score.ToString(), "#" + (i+1).ToString());

            tmp.transform.SetParent(scoreParent);
            tmp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
}
