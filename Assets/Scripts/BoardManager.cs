using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; set; }
    private bool[,] allowedMoves { get; set; }

    public Chessman[,] Chessmans { get; set; }
    private Chessman selectedChessman;

    private const float tile_size = 1.0f; // csempe mérete
    private const float tile_offset = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman = new List<GameObject>();

    private Material previousMaterial;
    public Material selectedMaterial;

    public int[] EnPassantMove { get; set; }

    // Elforgatás x,y,z mentén
    private Quaternion orientation = Quaternion.Euler(0, 180, 0);

    // Világos kezd
    public bool isWhiteTurn = true;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        // Vector3.zero helyett a GetTileCenter függvényt hívjuk meg
        // SpawnChessman(0, GetTileCenter(3,0));
        SpawnAllChessmans();
    }

    // Update is called once per frame
    void Update()
    {
        // Sakktábla kirajzolása
        UpdateSelection();
        DrawChessboard();
        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedChessman == null)
                {
                    // sakkfigura kiválasztása
                    SelectChessman(selectionX, selectionY);
                }
                else
                {
                    // sakkfigurával lépünk
                    MoveChessman(selectionX, selectionY);
                }
            }
        }
    }

    private void SelectChessman(int x, int y)
    {
        if (Chessmans[x, y] == null)
        {
            return;
        }
        if (Chessmans[x, y].isWhite != isWhiteTurn)
        {
            return;
        }
        bool hasAtLeastOneMove = false;
        allowedMoves = Chessmans[x, y].PossibleMove();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j])
                {
                    hasAtLeastOneMove = true;
                }
            }
        }
        if (!hasAtLeastOneMove)
        {
            return;
        }
        selectedChessman = Chessmans[x, y];
        previousMaterial = selectedChessman.GetComponent<MeshRenderer>().material;
        selectedMaterial.mainTexture = previousMaterial.mainTexture;
        selectedChessman.GetComponent<MeshRenderer>().material = selectedMaterial;
        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
    }

    private void MoveChessman(int x, int y)
    {
        if (allowedMoves[x,y])
        {
            Chessman c = Chessmans[x, y];

            if (c != null && c.isWhite != isWhiteTurn)
            {
                // ütés, király esetén matt
                if (c.GetType() == typeof(King))
                {
                    EndGame();
                    return;
                }
                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            // En Passant ütés
            if (x == EnPassantMove[0] && y == EnPassantMove[1])
            {
                if (isWhiteTurn) // világos gyalog esetén
                {
                    c = Chessmans[x, y - 1];
                }
                else // sötét gyalog esetén
                {
                    c = Chessmans[x, y + 1];
                }
                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            EnPassantMove[0] = -1;
            EnPassantMove[1] = -1;
            if (selectedChessman.GetType() == typeof(Pawn))
            {
                // Gyalogból vezér lesz
                if (y == 7)
                {
                    activeChessman.Remove(selectedChessman.gameObject);
                    Destroy(selectedChessman.gameObject);
                    SpawnChessman(1, x, y);
                    selectedChessman = Chessmans[x, y];
                }
                else if (y == 0)
                {
                    activeChessman.Remove(selectedChessman.gameObject);
                    Destroy(selectedChessman.gameObject);
                    SpawnChessman(7, x, y);
                    selectedChessman = Chessmans[x, y];
                }

                if (selectedChessman.CurrentY == 1 && y == 3)
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y - 1;
                }
                else if (selectedChessman.CurrentY == 6 && y == 4)
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y + 1;
                }
            }

            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetTileCenter(x, y);
            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;
            isWhiteTurn = !isWhiteTurn;
        }
        selectedChessman.GetComponent<MeshRenderer>().material = previousMaterial;
        BoardHighlights.Instance.Hidehighlights();
        selectedChessman = null;
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)(hit.point.x);
            selectionY = (int)(hit.point.z);
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void SpawnChessman(int index, int x, int y)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], GetTileCenter(x,y), orientation) as GameObject;
        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);
    }

    private void SpawnAllChessmans()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];
        EnPassantMove = new int[2] { -1, -1 };

        // GetTileCenter(x,y) a 2. és 3. koordináta
        // világos
        SpawnChessman(0, 4, 0); // király
        SpawnChessman(1, 3, 0); // vezér
        SpawnChessman(2, 0, 0); // bástya
        SpawnChessman(2, 7, 0); // bástya
        SpawnChessman(3, 2, 0); // futó
        SpawnChessman(3, 5, 0); // futó
        SpawnChessman(4, 1, 0); // huszár
        SpawnChessman(4, 6, 0); // huszár
        // gyalog
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(5, i, 1);
        }

        // sötét
        SpawnChessman(6, 4, 7); // király
        SpawnChessman(7, 3, 7); // vezér
        SpawnChessman(8, 0, 7); // bástya
        SpawnChessman(8, 7, 7); // bástya
        SpawnChessman(9, 2, 7); // futó
        SpawnChessman(9, 5, 7); // futó
        SpawnChessman(10, 1, 7); // huszár
        SpawnChessman(10, 6, 7); // huszár
        // gyalog
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(11, i, 6);
        }
    }

    // A sakkfigurák középre rendezése az adott mezőn.
    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (tile_size * x) + tile_offset;
        origin.z += (tile_size * y) + tile_offset;
        return origin;
    }

    // Sakktábla kirajzolásának metódusa
    private void DrawChessboard()
    {
        // vízszintes vonal deklarációja
        Vector3 widthLine = Vector3.right * 8;
        // függőleges vonal deklarációja
        Vector3 heightLine = Vector3.forward * 8;
        // tábla rajzolása
        for (int i = 0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }
        // forward: előre, a "z" irányba;
        // right: jobbra, az "x" irányba.
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1)
            );
            Debug.DrawLine(
                Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1)
            );
        }
    }

    private void EndGame()
    {
        if (isWhiteTurn)
        {
            Debug.Log("Világos nyert!");
        }
        else
        {
            Debug.Log("Sötét nyert!");
        }
        foreach (GameObject go in activeChessman)
        {
            Destroy(go);
        }
        isWhiteTurn = true;
        BoardHighlights.Instance.Hidehighlights();
        SpawnAllChessmans();
    }
}
