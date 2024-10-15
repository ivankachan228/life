using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ��������� ��� ������ � UI
using TMPro;


public class FieldGenerator : MonoBehaviour
{
    public GameObject squarePrefab; // ������ ��� ��������
    private GameObject[,] field; // ��������� ������ ��� �������� ������ �� ��������
    public TMP_Text generationText;
    private int generationCount = 0; // ����� �������� ���������
    public bool[,] colors; // true - alive, false - dead/sleep
    public int width = 30; // ������ ����
    public int height = 30;
    private bool isAutoGenerating = false; // ���� ��� �������������� ���������

    private bool isUserMode = false; // ���� ��� ����������� ������ ������
    private bool isGaming = false;
    private float generationTimer = 0f; // ������ ��� ������������ �������

    void Start()
    {
        // �������������� ��������� ������
        field = new GameObject[width, height];
        colors = new bool[width, height];

        // ��������� ���� ��� ������
        GenerateField();

        // ����������� ������ � �������
        

        //ButtonRandom.onClick.
        //ButtonUserMode.onClick.AddListener(SwitchToUserMode);
    }


    void Update()
    {
        int aliveCellsCount = CountAliveCells(); // ������� ���������� ����� ������

        // ��������� ����� �� ������
        generationText.text = "Epoch: " + generationCount + "\nAlive Cells: " + aliveCellsCount;
        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateRandomField();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SwitchToUserMode();
        }
        // ���� ���� �� �����, ��������� ������ ��������� ����� ������
        if (!isGaming)
        {
            HandleUserInput(); // ��������� ������������ ��������� ������
        }
        else
        {
            Debug.Log("IN TRUE");
            // ���� ���� �������, ����������� ����� ��� ���������� ���������
            generationTimer += Time.deltaTime;

            if (generationTimer >= 2f) // ������ 2 �������
            {
                Debug.Log("A");
                NextGeneration();      // ������ ������ ���������
                generationTimer = 0f;  // ���������� ������
            }
        }

        // ����� � ������ ���� �� �������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(isGaming);
            TogglePause(); // ����������� ����� � ������
        }
    }
    int CountAliveCells()
    {
        int aliveCount = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (colors[x, y]) // ���� ������ �����
                {
                    aliveCount++;
                }
            }
        }

        return aliveCount;
    }
    void TogglePause()
    {
        isGaming = !isGaming; // ����������� ��������� ����

        if (!isGaming)
        {
            generationTimer = 0f; // ���������� ������, ���� ���� �� �����
        }
    }


    // ��������� ����
    void GenerateField()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newSquare = Instantiate(squarePrefab, new Vector3(x, y, 0), Quaternion.identity);
                newSquare.AddComponent<ClickableSquare>(); // ��������� ������ ������������� ��������
                field[x, y] = newSquare;
                colors[x, y] = false; // ��� ������ �������� ��� "�������"
                ColorSquareAt(x, y, false); // ������������� ��������� ���� (�������)
            }
        }
    }

    // ����� ��� ��������� ��������� ����
    void GenerateRandomField()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                colors[x, y] = Random.value > 0.5f; // 50% �����������, ��� ������ ����� �����
                ColorSquareAt(x, y, colors[x, y]); // ��������� ���� ��������
            }
        }

        isUserMode = false; // ��������� ���������������� �����
    }

    // ����� ��� ������������ � ���������������� �����
    void SwitchToUserMode()
    {
        isUserMode = true; // ���������� ���������������� �����
    }

    // ����� ��� ��������� ������ ������������ �� ���������
    void HandleUserInput()
    {
        if (Input.GetMouseButtonDown(0)) // ��������� ������� ����� ������ ����
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // �������� ���������� ����
            int x = Mathf.FloorToInt(mousePos.x);
            int y = Mathf.FloorToInt(mousePos.y);

            // ���������, ��� ���������� � �������� ����
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                OnClick(x, y); // ������ ��������� ������
            }
        }
    }

    // ����� ��� ��������� ����� ��������
    void ColorSquareAt(int x, int y, bool alive)
    {
        GameObject square = field[x, y];
        SpriteRenderer renderer = square.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = alive ? Color.red : Color.white; // ������� - �����, ����� - �������
        }
    }

    // ������� ���������� ����� �������
    int CountAliveNeighbors(int x, int y)
    {
        int num_of_alive = 0;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // ���������� ���� ������
                int neighborX = x + dx;
                int neighborY = y + dy;
                if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
                {
                    if (colors[neighborX, neighborY]) num_of_alive++;
                }
            }
        }
        return num_of_alive;
    }

    // ����� ��� ����� �� �������
    public void OnClick(int x, int y)
    {
        Debug.Log($"Clicked on square at ({x}, {y}). Current state: {colors[x, y]}");
        colors[x, y] = !colors[x, y]; // ������ ��������� ������
        ColorSquareAt(x, y, colors[x, y]); // ��������� ���� ������
    }

    // ������� � ���������� ���������
    void NextGeneration()
    {
        generationCount++; // ����������� ������� ���������
        bool[,] nextColors = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int numOfAlive = CountAliveNeighbors(x, y);
                nextColors[x, y] = colors[x, y] ? numOfAlive == 2 || numOfAlive == 3 : numOfAlive == 3;
            }
        }

        // ��������� ���������
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                colors[x, y] = nextColors[x, y];
                ColorSquareAt(x, y, colors[x, y]);
            }
        }
    }
}
