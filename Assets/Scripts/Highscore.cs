using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

class Highscore: IComparable<Highscore>
{
    public int Score { get; set; }
    public string Name { get; set; }
    public int ID { get; set; }

    public Highscore(int id, int score, string name)
    {
        this.Score = score;
        this.Name = name;
        this.ID = id;
    }

    public int getid()
    {
        return this.ID;
    }

    public int CompareTo(Highscore other)
    {
        if (other.Score < this.Score)
        {
            return -1;
        }
        else if (other.Score > this.Score)
        {
            return 1;
        }
        return 0;
    }
}
