using System;
using System.Collections.Generic;

namespace SnakeGame
{
    //класс, предостовляющий змею в игре
    public class Snake
    {
        //цвет головы змеи
        private readonly ConsoleColor _headColor;

        //цвет тела змеи
        private readonly ConsoleColor _bodyColor;

        // Конструктор, создающий змею с заданными начальными координатами,
        //цветом головы и тела, а также длиной тела.
        public Snake(int initialX,
            int initialY,
            ConsoleColor headColor,
            ConsoleColor bodyColor,
            int bodyLength = 3)
        {
            _headColor = headColor; //задаём цвет головы
            _bodyColor = bodyColor; //задаём цвет тела

            Head = new Pixel(initialX, initialY, headColor); //создаём голову

            //создаём начальное тело змеи
            for (int i = bodyLength; i >= 0; i--)
            {
                Body.Enqueue(new Pixel(Head.X - i - 1, initialY, _bodyColor));
            }

            Draw(); //отрисовываем змею
        }

        //свойство для головы змеи
        public Pixel Head { get; private set; }

        //очередь для хранения пикселей, составляющих тело змеи
        public Queue<Pixel> Body { get; } = new Queue<Pixel>();

        //метод для перемищения змеи в заданном направлении
        public void Move(Direction direction, bool eat = false)
        {
            Clear(); //очистка текущего позиции

            //добавляем текущую голову в тело
            Body.Enqueue(new Pixel(Head.X, Head.Y, _bodyColor));
            if (!eat)
                Body.Dequeue(); //убираем последний пиксель, если змея не ест

            //обновляем позицию головы в зависимости от направления движения
            Head = direction switch
            {
                Direction.Right => new Pixel(Head.X + 1, Head.Y, _headColor),
                Direction.Left => new Pixel(Head.X - 1, Head.Y, _headColor),
                Direction.Up => new Pixel(Head.X, Head.Y - 1, _headColor),
                Direction.Down => new Pixel(Head.X, Head.Y + 1, _headColor),
                _ => Head
            };

            Draw(); //отрисовка змеи на новом месте
        }

        //метод для отрисовки змеи на экране
        public void Draw()
        {
            Head.Draw(); //рисуем голову

            foreach (Pixel pixel in Body) //рисуем тело
            {
                pixel.Draw();
            }
        }

        //метод для очистки змеи с экрана
        public void Clear()
        {
            Head.Clear(); //очистка головы

            foreach (Pixel pixel in Body) //очистка тела
            {
                pixel.Clear();
            }
        }
    }
}