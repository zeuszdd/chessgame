﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Chessman
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        Chessman c;
        int i, j;

        // Balra fel
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j++;
            if (i < 0 || j >= 8)
            {
                break;
            }
            c = BoardManager.Instance.Chessmans[i, j];
            if (c==null)
            {
                r[i, j] = true;
            }
            else
            {
                if (isWhite!=c.isWhite)
                {
                    r[i, j] = true;
                }
                break;
            }
        }
        // Jobbra fel
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j++;
            if (i >= 8 || j >= 8)
            {
                break;
            }
            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
            {
                r[i, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite)
                {
                    r[i, j] = true;
                }
                break;
            }
        }
        // Balra le
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j--;
            if (i < 0 || j < 8)
            {
                break;
            }
            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
            {
                r[i, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite)
                {
                    r[i, j] = true;
                }
                break;
            }
        }
        // Jobbra le
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j--;
            if (i >= 8 || j < 0)
            {
                break;
            }
            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
            {
                r[i, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite)
                {
                    r[i, j] = true;
                }
                break;
            }
        }
        return r;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
