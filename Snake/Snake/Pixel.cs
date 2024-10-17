using System;
using System.Drawing;

namespace SnakeGame
{
    //Структура, представляющая один пиксель на экране консоли.Хранит координаты, цвет и символ пикселя
    public readonly struct Pixel
    {
        //символы, используемые для отображения различных элементом
        private const char BorderChar = '█'; //границы
        private const char FoodChar = 'o'; //еда
        private const char BombChar = '*'; //бомба

        //конструктор для создания пикселя с заданными координатами, цветом и символом
        public Pixel(int x, int y, ConsoleColor color, char pixelChar = BorderChar)
        {
            X = x;
            Y = y;
            Color = color;
            PixelChar = pixelChar;
        }

        //свойства для координат и цвета пикселя
        public int X { get; } //координата Х

        public int Y { get; } //координата Y

        public ConsoleColor Color { get; } //цвет пикселя

        private char PixelChar { get; } //символ для отрисовки пикселя

        //метод для отрисовки пикселя на экране
        public void Draw()
        {
            Console.ForegroundColor = Color; //устанавливаем цвет пикселя

            Console.SetCursorPosition(X, Y); //перемещаем курсор на заданные координаты
            Console.Write(PixelChar); //рисуем пиксель
        }

        //метод для очистки пикселя
        public void Clear()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(' ');
        }
    }
}