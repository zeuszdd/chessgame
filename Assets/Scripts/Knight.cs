using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chessman
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        // Fel balra
        KnightMove(CurrentX - 1, CurrentY + 2, ref r);
        // Fel jobbra
        KnightMove(CurrentX + 1, CurrentY + 2, ref r);
        // Jobbra fel
        KnightMove(CurrentX + 2, CurrentY + 1, ref r);
        // Jobbra le
        KnightMove(CurrentX + 2, CurrentY - 1, ref r);
        // Le balra
        KnightMove(CurrentX - 1, CurrentY - 2, ref r);
        // Le Jobbra
        KnightMove(CurrentX + 1, CurrentY - 2, ref r);
        // Balra fel
        KnightMove(CurrentX - 2, CurrentY + 1, ref r);
        // Balra le
        KnightMove(CurrentX - 2, CurrentY - 1, ref r);
        return r;
    }

    public void KnightMove(int x, int y, ref bool[,] r)
    {
        Chessman c;
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            c = BoardManager.Instance.Chessmans[x, y];
            if (c == null)
            {
                r[x, y] = true;
            }
            else if (isWhite != c.isWhite)
            {
                r[x, y] = true;
            }
        }
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
