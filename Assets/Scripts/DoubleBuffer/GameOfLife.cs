using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public GameObject TileObject;
    private static readonly int Width = 10;
    private static readonly int Height = 10;
    private bool[,] grid_curr = new bool[Width, Height];
    private bool[,] grid_next = new bool[Width, Height];
    GameObject[,] tiles = new GameObject[Width, Height];

    public bool paused = false;
    
    private float TimeAccu = 0.0f;
    [SerializeField][Range(0.1f,3f)] private float RefreshRate = 1f;
    [SerializeField] private List<Vector2> start_reds = new();

    void Start()
    {
        // Generate tiles
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                // Clear the grid
                grid_curr[x, y] = false;
                // Instantiate the tile
                tiles[x, y] = Instantiate(TileObject,
                                        new Vector3(x, 5f, y) * 1.05f,
                                        TileObject.transform.rotation);
                //tiles[x, y].SetActive(false);
                tiles[x, y].GetComponent<MeshRenderer>().material.color = Color.black;
            }
        }

        // I just wanted the initial setup phase to be a bit smoother by adding them from the editor.
        foreach (Vector2 red in start_reds)
        {
            if (0 <= red.x && red.x < Width + 1 && 0 <= red.y && red.y < Height + 1)
            {
                grid_curr[(int)red.x, (int)red.y] = true;
                grid_next[(int)red.x, (int)red.y] = true;
                tiles[(int)red.x, (int)red.y].GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
        //
        // grid_curr[5, 5] = true;
        // tiles[5, 5].GetComponent<MeshRenderer>().material.color = Color.red;
        // grid_curr[3, 5] = true;
        // tiles[3, 5].GetComponent<MeshRenderer>().material.color = Color.red;
        // grid_curr[4, 4] = true;
        // tiles[4, 4].GetComponent<MeshRenderer>().material.color = Color.red;
        // grid_curr[1, 4] = true;
        // tiles[1, 4].GetComponent<MeshRenderer>().material.color = Color.red;
        // grid_curr[2, 3] = true;
        // tiles[2, 3].GetComponent<MeshRenderer>().material.color = Color.red;
        // grid_curr[1, 3] = true;
        // tiles[1, 3].GetComponent<MeshRenderer>().material.color = Color.red;
        // grid_curr[2, 2] = true;
        // tiles[2, 2].GetComponent<MeshRenderer>().material.color = Color.red;
    }

    private int GetLiveNeighbours(int x, int y, bool[,] grid)
    {
        int liveneighbours = 0;
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (!(i == x & j == y) && i >= 0 && j >= 0 && i < Width && j < Height)
                {
                    // current i,j is not x,y
                    if (grid[i, j] == true)
                    {
                        liveneighbours++;
                    }
                }
            }
        }
        return liveneighbours;
    }

    void Update()
    {
        if (!paused)
            TimeAccu += Time.deltaTime;

        if (TimeAccu > RefreshRate)
        {
            // RULES:
            // 1. Fewer than 2 live neighbours --> die
            // 2. 2 or 3 live neighbours -> live on
            // 3. more than 3 live neighbours -> die
            // 4. dead cell with 3 live neighbours -> rebirth
            
            UpdateNextGrid();
            StepGrids();
            UpdateTiles();
            
            TimeAccu = 0;

        }
    }
    
    private void UpdateNextGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                int live = GetLiveNeighbours(x, y, grid_curr);
                if (live < 2)
                {
                    grid_next[x, y] = false;
                }
                else if (live < 4 && grid_curr[x, y] == true)
                {
                    // live on... do nothing
                }
                else if (live > 3 && grid_curr[x, y] == true)
                {
                    grid_next[x, y] = false;
                }
                else if (live == 3 && grid_curr[x, y] == false)
                {
                    grid_next[x, y] = true;
                }
            }
        }
    }

    private void StepGrids()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                grid_curr[x, y] = grid_next[x, y];
            }
        }
    }

    private void UpdateTiles()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (grid_curr[x, y] == true)
                {
                    tiles[x, y].GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else
                {
                    tiles[x, y].GetComponent<MeshRenderer>().material.color = Color.black;
                }
            }
        }
    }
}

