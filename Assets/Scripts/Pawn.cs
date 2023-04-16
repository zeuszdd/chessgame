using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Chessman
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        Chessman c, c2;
        int[] e = BoardManager.Instance.EnPassantMove;

        // Világos lépése
        if (isWhite)
        {
            // Átlósan balra
            if (CurrentX != 0 && CurrentY != 7)
            {
                if (e[0] == CurrentX - 1 && e[1] == CurrentY + 1)
                {
                    r[CurrentX - 1, CurrentY + 1] = true;
                }
                c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY + 1];
                if (c!=null&&!c.isWhite)
                {
                    r[CurrentX - 1, CurrentY + 1] = true;
                }
            }
            // Átlósan jobbra
            if (CurrentX != 7 && CurrentY != 7)
            {
                if (e[0] == CurrentX + 1 && e[1] == CurrentY + 1)
                {
                    r[CurrentX + 1, CurrentY + 1] = true;
                }
                c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY + 1];
                if (c != null && !c.isWhite)
                {
                    r[CurrentX + 1, CurrentY + 1] = true;
                }
            }
            // Előre
            if (CurrentY != 7)
            {
                c = BoardManager.Instance.Chessmans[CurrentX, CurrentY + 1];
                if (c == null)
                {
                    r[CurrentX, CurrentY + 1] = true;
                }
            }
            // Első lépés előre
            if (CurrentY == 1)
            {
                c = BoardManager.Instance.Chessmans[CurrentX, CurrentY + 1];
                c2 = BoardManager.Instance.Chessmans[CurrentX, CurrentY + 2];
                if (c == null && c2 == null)
                {
                    r[CurrentX, CurrentY + 1] = true;
                }
            }
        }
        else // Sötét
        {
            // Átlósan balra
            if (CurrentX != 0 && CurrentY != 0)
            {
                if (e[0] == CurrentX - 1 && e[1] == CurrentY - 1)
                {
                    r[CurrentX - 1, CurrentY - 1] = true;
                }
                c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY - 1];
                if (c != null && c.isWhite)
                {
                    r[CurrentX - 1, CurrentY - 1] = true;
                }
            }
            // Átlósan jobbra
            if (CurrentX != 7 && CurrentY != 0)
            {
                if (e[0] == CurrentX + 1 && e[1] == CurrentY - 1)
                {
                    r[CurrentX + 1, CurrentY - 1] = true;
                }
                c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY - 1];
                if (c != null && c.isWhite)
                {
                    r[CurrentX + 1, CurrentY - 1] = true;
                }
            }
            // Előre
            if (CurrentY != 0)
            {
                c = BoardManager.Instance.Chessmans[CurrentX, CurrentY - 1];
                if (c == null)
                {
                    r[CurrentX, CurrentY - 1] = true;
                }
            }
            // Első lépés előre
            if (CurrentY == 6)
            {
                c = BoardManager.Instance.Chessmans[CurrentX, CurrentY - 1];
                c2 = BoardManager.Instance.Chessmans[CurrentX, CurrentY - 2];
                if (c == null && c2 == null)
                {
                    r[CurrentX, CurrentY - 2] = true;
                }
            }
        }
        r[3, 3] = true;
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
