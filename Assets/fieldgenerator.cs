using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Добавляем для работы с UI
using TMPro;


public class FieldGenerator : MonoBehaviour
{
    public GameObject squarePrefab; // Префаб для квадрата
    private GameObject[,] field; // Двумерный массив для хранения ссылок на квадраты
    public TMP_Text generationText;
    private int generationCount = 0; // Номер текущего поколения
    public bool[,] colors; // true - alive, false - dead/sleep
    public int width = 30; // Размер поля
    public int height = 30;
    private bool isAutoGenerating = false; // Флаг для автоматической генерации

    private bool isUserMode = false; // Флаг для определения режима работы
    private bool isGaming = false;
    private float generationTimer = 0f; // Таймер для отслеживания времени

    void Start()
    {
        // Инициализируем двумерный массив
        field = new GameObject[width, height];
        colors = new bool[width, height];

        // Генерация поля при старте
        GenerateField();

        // Привязываем кнопки к методам
        

        //ButtonRandom.onClick.
        //ButtonUserMode.onClick.AddListener(SwitchToUserMode);
    }


    void Update()
    {
        int aliveCellsCount = CountAliveCells(); // Считаем количество живых клеток

        // Обновляем текст на экране
        generationText.text = "Epoch: " + generationCount + "\nAlive Cells: " + aliveCellsCount;
        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateRandomField();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SwitchToUserMode();
        }
        // Если игра на паузе, позволяем игроку добавлять новые клетки
        if (!isGaming)
        {
            HandleUserInput(); // Позволяем пользователю добавлять клетки
        }
        else
        {
            Debug.Log("IN TRUE");
            // Если игра активна, отсчитываем время для следующего поколения
            generationTimer += Time.deltaTime;

            if (generationTimer >= 2f) // Прошло 2 секунды
            {
                Debug.Log("A");
                NextGeneration();      // Запуск нового поколения
                generationTimer = 0f;  // Сбрасываем таймер
            }
        }

        // Пауза и запуск игры по пробелу
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(isGaming);
            TogglePause(); // Переключаем паузу и запуск
        }
    }
    int CountAliveCells()
    {
        int aliveCount = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (colors[x, y]) // Если клетка живая
                {
                    aliveCount++;
                }
            }
        }

        return aliveCount;
    }
    void TogglePause()
    {
        isGaming = !isGaming; // Переключаем состояние игры

        if (!isGaming)
        {
            generationTimer = 0f; // Сбрасываем таймер, если игра на паузе
        }
    }


    // Генерация поля
    void GenerateField()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newSquare = Instantiate(squarePrefab, new Vector3(x, y, 0), Quaternion.identity);
                newSquare.AddComponent<ClickableSquare>(); // Добавляем скрипт кликабельного квадрата
                field[x, y] = newSquare;
                colors[x, y] = false; // Все клетки начинают как "мертвые"
                ColorSquareAt(x, y, false); // Устанавливаем начальный цвет (мертвый)
            }
        }
    }

    // Метод для случайной генерации поля
    void GenerateRandomField()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                colors[x, y] = Random.value > 0.5f; // 50% вероятность, что клетка будет живая
                ColorSquareAt(x, y, colors[x, y]); // Обновляем цвет квадрата
            }
        }

        isUserMode = false; // Отключаем пользовательский режим
    }

    // Метод для переключения в пользовательский режим
    void SwitchToUserMode()
    {
        isUserMode = true; // Активируем пользовательский режим
    }

    // Метод для обработки кликов пользователя по квадратам
    void HandleUserInput()
    {
        if (Input.GetMouseButtonDown(0)) // Проверяем нажатие левой кнопки мыши
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Получаем координаты мыши
            int x = Mathf.FloorToInt(mousePos.x);
            int y = Mathf.FloorToInt(mousePos.y);

            // Проверяем, что координаты в пределах поля
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                OnClick(x, y); // Меняем состояние клетки
            }
        }
    }

    // Метод для изменения цвета квадрата
    void ColorSquareAt(int x, int y, bool alive)
    {
        GameObject square = field[x, y];
        SpriteRenderer renderer = square.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = alive ? Color.red : Color.white; // Красный - живая, белый - мертвая
        }
    }

    // Подсчет количества живых соседей
    int CountAliveNeighbors(int x, int y)
    {
        int num_of_alive = 0;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // Пропускаем саму клетку
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

    // Метод при клике на квадрат
    public void OnClick(int x, int y)
    {
        Debug.Log($"Clicked on square at ({x}, {y}). Current state: {colors[x, y]}");
        colors[x, y] = !colors[x, y]; // Меняем состояние клетки
        ColorSquareAt(x, y, colors[x, y]); // Обновляем цвет клетки
    }

    // Переход к следующему поколению
    void NextGeneration()
    {
        generationCount++; // Увеличиваем счетчик поколений
        bool[,] nextColors = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int numOfAlive = CountAliveNeighbors(x, y);
                nextColors[x, y] = colors[x, y] ? numOfAlive == 2 || numOfAlive == 3 : numOfAlive == 3;
            }
        }

        // Применяем изменения
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
