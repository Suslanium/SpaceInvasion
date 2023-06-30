using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool isVisited = false;
        public bool[] status = new bool[4];
    }

    [SerializeField] private Vector2 size;
    [SerializeField] private GameObject[] roomPrefabs;
    [SerializeField] private int startPosition = 0;
    [SerializeField] private Vector2 offset;
    [SerializeField] private NavMeshGenerator generator;
    private List<Cell> board;

    void Start()
    {
        GenerateMaze();
    }

    void GenerateLevel()
    {
        List<RoomBehaviour> rooms = new List<RoomBehaviour>();
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                var currentRoomPrefab = roomPrefabs[roomPrefabs.Length - 1];

                if ((i == 0 && j == 0) || (i == 4 && j == 0) || (i == 0 && j == 4) || (i == 4 && j == 4))
                {
                    currentRoomPrefab = roomPrefabs[roomPrefabs.Length - 1];
                }
                else
                {
                    currentRoomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length - 1)];
                }

                var newRoom =
                    Instantiate(currentRoomPrefab, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform)
                        .GetComponent<RoomBehaviour>();
                rooms.Add(newRoom);
                newRoom.UpdateRoom(board[Mathf.FloorToInt(i + j * size.x)].status);

                newRoom.name += " " + i + "-" + j;
            }
        }
        generator.GenerateNavMesh();
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].DisableRoom();
        }
    }

    void GenerateMaze()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPosition;

        Stack<int> path = new Stack<int>();
        int k = 0;

        while (k < 1000)
        {
            k++;
            board[currentCell].isVisited = true;
            List<int> neighbours = CheckNeighbours(currentCell);

            if (neighbours.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }

                currentCell = path.Pop();
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbours[Random.Range(0, neighbours.Count)];

                if (newCell > currentCell)
                {
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }

        GenerateLevel();
    }

    List<int> CheckNeighbours(int cell)
    {
        List<int> neighbours = new List<int>();

        // Up
        if (cell - size.x >= 0 && !board[Mathf.FloorToInt(cell - size.x)].isVisited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - size.x));
        }

        // Down
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].isVisited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + size.x));
        }

        // Right
        if ((cell + 1) % size.x != 0 && !board[Mathf.FloorToInt(cell + 1)].isVisited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + 1));
        }

        // Left
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell - 1)].isVisited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbours;
    }
}