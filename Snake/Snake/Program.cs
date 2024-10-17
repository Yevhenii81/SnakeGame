using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace SnakeGame
{
    internal class Program
    {
        private const int FrameMs = 200;

        //цвета для границ, головы и тела змеи, еды и бомбы
        private const ConsoleColor BorderColor = ConsoleColor.DarkRed;
        private const ConsoleColor HeadColor = ConsoleColor.DarkGreen;
        private const ConsoleColor BodyColor = ConsoleColor.Green;
        private const ConsoleColor FoodColor = ConsoleColor.Yellow;
        private const ConsoleColor BombColor = ConsoleColor.Red;

        //генератор для размещения еды и бомб
        private static readonly Random Random = new Random();

        static void Main()
        {
            Title = "Snake Game"; //заголовок консоли

            CursorVisible = false; //скрытие курсора

            var consoleHelper = new ConsoleHelper();
            consoleHelper.CenterConsoleWindow(); //центрирование консоли;

            while (true)
            {
                ShowLogo(); //показ логотипа игры
                ShowMenu(); //показ главного меню
                ReadKey(); //ожидания нажатия клавиши
            }
        }

        //метод отображения логотипа
        static void ShowLogo()
        {
            Clear();
            SetCursorPosition(WindowWidth / 2 - 14, WindowHeight / 2 - 4);
            WriteLine("  SSSSS  N   N   AAAAA   K   K  EEEEE ");
            SetCursorPosition(WindowWidth / 2 - 14, WindowHeight / 2 - 3);
            WriteLine(" S       NN  N   A   A   K  K   E     ");
            SetCursorPosition(WindowWidth / 2 - 14, WindowHeight / 2 - 2);
            WriteLine("  SSS    N N N   AAAAA   KKK    EEEEE ");
            SetCursorPosition(WindowWidth / 2 - 14, WindowHeight / 2 - 1);
            WriteLine("     S   N  NN   A   A   K  K   E     ");
            SetCursorPosition(WindowWidth / 2 - 14, WindowHeight / 2);
            WriteLine(" SSSSS   N   N   A   A   K   K  EEEEE ");
            SetCursorPosition(WindowWidth / 2 - 14, WindowHeight / 2 + 2);
            WriteLine("Press Enter to start...");

            while (true)
            {
                if (KeyAvailable && ReadKey(true).Key == ConsoleKey.Enter)
                    break;
            }
        }

        //метод для отображения главного меню
        static void ShowMenu()
        {
            Clear();
            string[] options = {
                "1. Правила",
                "2. Играть (Легкий уровень)",
                "3. Выбор уровня сложности",
                "4. Выйти"
            };

            int selectedOption = 0;

            while (true)
            {
                Clear();

                int menuWidth = options.Max(option => option.Length);
                int startX = (WindowWidth - menuWidth) / 2;
                int startY = (WindowHeight - options.Length) / 2;

                //отрисовка меню с подсветкой
                for (int i = 0; i < options.Length; i++)
                {
                    SetCursorPosition(startX, startY + i);
                    if (i == selectedOption)
                    {
                        ForegroundColor = ConsoleColor.Blue; //подсветка
                        WriteLine(" " + options[i]);
                        ResetColor();
                    }
                    else
                    {
                        WriteLine(options[i]);
                    }
                }

                ConsoleKey key = ReadKey(true).Key;

                //обработка нажатия клавиш
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedOption = (selectedOption == 0) ? options.Length - 1 : selectedOption - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedOption = (selectedOption == options.Length - 1) ? 0 : selectedOption + 1;
                        break;
                    case ConsoleKey.Enter:
                        ExecuteMenuChoice(selectedOption);
                        return;
                }
            }
        }

        //выполнение выбраного пункта меню
        static void ExecuteMenuChoice(int selectedOption)
        {
            switch (selectedOption)
            {
                case 0:
                    ShowRules(); //правила
                    break;
                case 1:
                    StartGame(40, 20, withBomb: false); //начать игру на легким уровнем
                    break;
                case 2:
                    ChooseDifficulty(); //выбор уровня сложности
                    break;
                case 3:
                    CloseConsole(); //закрыть консоль
                    break;
            }
        }

        //метод для отображения правил игры
        static void ShowRules()
        {
            Clear();
            WriteLine("Правила игры:");
            WriteLine("1. Управляйте змейкой с помощью стрелок.");
            WriteLine("2. Съедайте еду, чтобы расти и набирать очки.");
            WriteLine("3. Если съедите бомбу (в сложных уровнях), игра завершится.");
            WriteLine("\nНажмите любую клавишу для возврата в меню...");
            ReadKey();
            ShowMenu(); //возврат в меню
        }

        //метод для выбора уровня сложности
        static void ChooseDifficulty()
        {
            Clear();
            string[] difficultyOptions = {
                "1. Легкий (без бомб, поле 40x20)",
                "2. Средний (с бомбами, поле 60x30)",
                "3. Сложный (с бомбами, поле 80x40)",
                "4. Назад"
            };

            int selectedDifficulty = 0;

            while (true)
            {
                Clear();

                int menuWidth = difficultyOptions.Max(option => option.Length);
                int startX = (WindowWidth - menuWidth) / 2;
                int startY = (WindowHeight - difficultyOptions.Length) / 2;

                //отрисовка выбора уровня сложности
                for (int i = 0; i < difficultyOptions.Length; i++)
                {
                    SetCursorPosition(startX, startY + i);
                    if (i == selectedDifficulty)
                    {
                        ForegroundColor = ConsoleColor.Blue; //подсветка выбора
                        WriteLine(" " + difficultyOptions[i]);
                        ResetColor();
                    }
                    else
                    {
                        WriteLine(difficultyOptions[i]);
                    }
                }

                ConsoleKey key = ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedDifficulty = (selectedDifficulty == 0) ? difficultyOptions.Length - 1 : selectedDifficulty - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedDifficulty = (selectedDifficulty == difficultyOptions.Length - 1) ? 0 : selectedDifficulty + 1;
                        break;
                    case ConsoleKey.Enter:
                        switch (selectedDifficulty)
                        {
                            case 0:
                                StartGame(40, 20, withBomb: false); //легкий уровень
                                break;
                            case 1:
                                StartGame(60, 30, withBomb: true); //средний уровень
                                break;
                            case 2:
                                StartGame(80, 40, withBomb: true); //сложный уровень
                                break;
                            case 3:
                                ShowMenu(); //возврат в меню
                                return;
                        }
                        return;
                }
            }
        }

        //метод для закрытия консоли
        static void CloseConsole()
        {
            Clear();
            WriteLine("Игра завершена. Нажмите любую клавишу, чтобы закрыть.");
            ReadKey(true);
            Environment.Exit(0); //закрытие программы и консоли
        }

        //метод для начала игры
        static void StartGame(int mapWidth, int mapHeight, bool withBomb)
        {
            //установка размеров окна и буфера консоли
            SetWindowSize(mapWidth + 1, mapHeight + 1);
            SetBufferSize(mapWidth + 1, mapHeight + 1);

            Clear(); //очистка экрана
            DrawBorder(mapWidth, mapHeight); //отрисовка границ игрового поля

            Direction currentMovement = Direction.Right; //начальное направление движения змейки
            var snake = new Snake(10, 5, HeadColor, BodyColor); //создание змейки
            Pixel food = GenFood(snake, mapWidth, mapHeight); //генерация еды
            food.Draw(); //отрисовка еды

            Pixel? bomb = withBomb ? GenBomb(snake, food, mapWidth, mapHeight) : (Pixel?)null; //генерация бомбы
            bomb?.Draw(); //отрисовка бомбы

            int score = 0;
            int maxScore = (mapWidth - 2) * (mapHeight - 2) - snake.Body.Count - 1; //максимальный возможный счет
            Stopwatch sw = new Stopwatch();

            while (true)
            {
                sw.Restart();
                Direction oldMovement = currentMovement;

                while (sw.ElapsedMilliseconds <= FrameMs)
                {
                    if (currentMovement == oldMovement)
                    {
                        currentMovement = ReadMovement(currentMovement);
                    }
                }

                if (snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(currentMovement, true); //змейка съедает еду
                    food = GenFood(snake, mapWidth, mapHeight); //новая еда
                    food.Draw();

                    if (withBomb)
                    {
                        bomb?.Clear();
                        bomb = GenBomb(snake, food, mapWidth, mapHeight); //новая бомба
                        bomb?.Draw();
                    }

                    score++;

                    Task.Run(() => Beep(1200, 200)); //звук при съедании еды

                    if (score == maxScore)
                    {
                        WinGame(score); //победа
                        break;
                    }
                }
                else if (withBomb && bomb.HasValue && snake.Head.X == bomb.Value.X && snake.Head.Y == bomb.Value.Y)
                {
                    LoseGame(score); //проигрыш при столкновении с бомбой
                    break;
                }
                else
                {
                    snake.Move(currentMovement); //движение змейки
                }

                //проверка на столкновение с границей или своим телом
                if (snake.Head.X == mapWidth - 1
                   || snake.Head.X == 0
                   || snake.Head.Y == mapHeight - 1
                   || snake.Head.Y == 0
                   || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                {
                    LoseGame(score); //проигрыш
                    break;
                }
            }

            snake.Clear();
            food.Clear();
            bomb?.Clear();
        }

        //метод для отображения экрана проигрыша
        static void LoseGame(int score)
        {
            SetCursorPosition(WindowWidth / 3, WindowHeight / 2);
            WriteLine($"Game over, score: {score}");
            Task.Run(() => Beep(200, 600)); //звук проигрыша
            ShowGameOverMenu(); //показать меню окончания игры
        }

        //метод для отображения экрана победы
        static void WinGame(int score)
        {
            SetCursorPosition(WindowWidth / 3, WindowHeight / 2);
            WriteLine($"You Win! Final Score: {score}");
            Task.Run(() => Beep(1500, 600)); //звук победы
            ShowGameOverMenu(); //показать меню окончания игры
        }

        //метод для отображения меню окончания игры
        static void ShowGameOverMenu()
        {
            WriteLine("Press any key to return to the menu...");
            ReadKey(); //ожидание нажатия клавиши
            ShowMenu(); //возврат в меню
        }

        //генерация новой еды
        static Pixel GenFood(Snake snake, int mapWidth, int mapHeight)
        {
            Pixel food;
            do
            {
                food = new Pixel(Random.Next(2, mapWidth - 2), Random.Next(1, mapHeight - 1), FoodColor, 'o');
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y ||
                     snake.Body.Any(b => b.X == food.X && b.Y == food.Y));

            return food;
        }

        //генерация новой бомбы 
        static Pixel GenBomb(Snake snake, Pixel food, int mapWidth, int mapHeight)
        {
            Pixel bomb;
            do
            {
                bomb = new Pixel(Random.Next(2, mapWidth - 2), Random.Next(1, mapHeight - 1), BombColor, '*');
            } while (snake.Body.Any(b => b.X == bomb.X && b.Y == bomb.Y || (food.X == bomb.X && food.Y == bomb.Y)));

            return bomb;
        }

        //метод для обработки ввода пользователя и направления движения змейки
        static Direction ReadMovement(Direction currentDirection)
        {
            if(!KeyAvailable) //если нет доступного ввода, возвращаем текущее направление
                return currentDirection;

            ConsoleKey key = ReadKey(true).Key;

            //смена направления, если оно не противоположное текущему
            currentDirection = key switch
            {
                ConsoleKey.UpArrow when currentDirection != Direction.Down => Direction.Up,
                ConsoleKey.DownArrow when currentDirection != Direction.Up => Direction.Down,
                ConsoleKey.LeftArrow when currentDirection != Direction.Right => Direction.Left,
                ConsoleKey.RightArrow when currentDirection != Direction.Left => Direction.Right,
                _ => currentDirection
            };

            return currentDirection; //возвращаем обновленное направление
        }

        //метод по отрисовки границ игрового поля
        static void DrawBorder(int mapWidth, int mapHeight)
        {
            //отрисовка верхней границы
            for (int i = 0; i <= mapWidth; i++)
            {
                new Pixel(i, 0, BorderColor).Draw(); //верхний ряд
            }

            // Отрисовка боковых границ
            for (int i = 1; i <= mapHeight; i++)
            {
                new Pixel(0, i, BorderColor).Draw(); //левая граница
                new Pixel(1, i, BorderColor).Draw(); //вторая левая граница
                new Pixel(mapWidth, i, BorderColor).Draw(); //правая граница
                new Pixel(mapWidth - 1, i, BorderColor).Draw(); //вторая правая граница
            }

            //отрисовка нижней границы
            for (int i = 0; i <= mapWidth; i++)
            {
                new Pixel(i, mapHeight, BorderColor).Draw(); //нижний ряд
            }
        }
    }
}