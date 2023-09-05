using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    private static int SCREEN_WIDTH = 96;
    private static int SCREEN_HEIGHT = 54;

    public float speed = 0.1f;
    private float timer = 0;

    public bool simulationEnabled = false;

    Cell[,] grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT];


    void Start()
    {
        PlaceCells();
    }


    void Update()
    {
        if (simulationEnabled)
        {
            if (timer >= speed)
            {
                timer = 0f;

                CountNeighbors();
                PopulationControl();
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        UserInput();
    }


    void UserInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Mathf.RoundToInt(mousePoint.x);
            int y = Mathf.RoundToInt(mousePoint.y);

            if (x >= 0 && y >= 0 && x < SCREEN_WIDTH && y < SCREEN_HEIGHT)
            {
                grid[x, y].SetAlive(!grid[x, y].isAlive);
            }
        }

        // Play/Pause Simulation
        if (Input.GetKeyUp(KeyCode.P))
        {
            simulationEnabled = !simulationEnabled;
        }

        // Clear Grid
        if (Input.GetKeyUp(KeyCode.C))
        {            
            for (int y = 0; y < SCREEN_HEIGHT; y++)
            {
                for (int x = 0; x < SCREEN_WIDTH; x++)
                {
                    grid[x, y].SetAlive(false);

                    simulationEnabled = false;
                }
            }
        }

        if (!simulationEnabled)
        {
            // Random
            if (Input.GetKeyUp(KeyCode.R))
            {
                for (int y = 0; y < SCREEN_HEIGHT; y++)
                {
                    for (int x = 0; x < SCREEN_WIDTH; x++)
                    {
                        grid[x, y].SetAlive(RandomAliveCell());
                    }
                }
            }
        }

        // Quit
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    void PlaceCells()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                Cell cell = Instantiate(Resources.Load("Prefabs/tile", typeof(Cell)), new Vector2(x,y), Quaternion.identity) as Cell;
                grid[x, y] = cell;
                grid[x, y].SetAlive(false);
            }
        }
    }


    void CountNeighbors()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                int numNeighbors = 0;

                // North
                if (y + 1 < SCREEN_HEIGHT)
                {
                    if (grid[x, y + 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                // East
                if (x + 1 < SCREEN_WIDTH)
                {
                    if (grid[x + 1, y].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                // South
                if (y - 1 >= 0)
                {
                    if (grid[x, y - 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                // West
                if (x - 1 >= 0)
                {
                    if (grid[x - 1, y].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                // NorthEast
                if (x + 1 < SCREEN_WIDTH && y + 1 < SCREEN_HEIGHT)
                {
                    if (grid[x + 1, y + 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                // NorthWest
                if (x - 1 >= 0 && y + 1 < SCREEN_HEIGHT)
                {
                    if (grid[x - 1, y + 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                // SouthEast
                if (x + 1 < SCREEN_WIDTH && y - 1 >= 0)
                {
                    if (grid[x + 1, y - 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                // SouthWest
                if (x - 1 >= 0 && y - 1 >= 0)
                {
                    if (grid[x - 1, y - 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                grid[x, y].numNeighbors = numNeighbors;
            }
        }
    }


    void PopulationControl()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                if (grid[x, y].isAlive)
                {
                    if (grid[x, y].numNeighbors != 2 && grid[x, y].numNeighbors != 3)
                    {
                        grid[x, y].SetAlive(false);
                    }
                }
                else
                {
                    if (grid[x, y].numNeighbors == 3)
                    {
                        grid[x, y].SetAlive(true);
                    }
                }
            }
        }
    }


    bool RandomAliveCell()
    {
        int rand = UnityEngine.Random.Range(0, 10);

        if (rand > 7)
        {
            return true;
        }

        return false;
    }
}
