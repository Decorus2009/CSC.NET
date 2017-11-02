using System;
using System.Collections.Generic;

namespace Mini_Roguelike
{
    public class Game
    {
        private (int x, int y) _current;
        private (int x, int y) _previous;

        public event Action<Game> MoveHandler;

        public (int x, int y) Previous
        {
            get => _previous;
            set
            {
                _previous.x = value.x;
                _previous.y = value.y;
            }
        }

        public (int x, int y) Current
        {
            get => _current;
            set
            {
                _current.x = value.x;
                _current.y = value.y;
            }
        }

        public List<List<Cell>> Field { get; }

        public Game(string file)
        {
            Current = (0, 0);
            Previous = (0, 0);
            Field = Parser.Parse(file);
        }

        public char CharUnderPreviousPosition()
        {
            switch (Field[Previous.y][Previous.x])
            {
                case Cell.Free:
                    return '.';
                case Cell.Wall:
                    return '*';
                default:
                    throw new ArgumentException(
                        "Only walls and corridors are supported in current version");
            }            
        }

        public void OnUp() => TryMove(Direction.Up);

        public void OnDown() => TryMove(Direction.Down);

        public void OnLeft() => TryMove(Direction.Left);

        public void OnRight() => TryMove(Direction.Right);

        
        private void TryMove(Direction direction)
        {
            if (!CanMove(direction))
            {
                return;
            }
            Previous = Current;
            switch (direction)
            {
                case Direction.Up:
                    _current.y--;
                    break;
                case Direction.Down:
                    _current.y++;
                    break;
                case Direction.Left:
                    _current.x--;
                    break;
                case Direction.Right:
                    _current.x++;
                    break;
                default:
                    throw new ArgumentException("Wrong direction");
            }
            MoveHandler?.Invoke(this);
        }

        private bool CanMove(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Current.y > 0 && Field[Current.y - 1][Current.x] == Cell.Free;
                case Direction.Down:
                    return Current.y < Field.Count - 1 && Field[Current.y + 1][Current.x] == Cell.Free;
                case Direction.Left:
                    return Current.x > 0 && Field[Current.y][Current.x - 1] == Cell.Free;
                case Direction.Right:
                    return Current.x < Field[Current.y].Count - 1 && Field[Current.y][Current.x + 1] == Cell.Free;
                default:
                    throw new ArgumentException("Wrong direction");
            }
        }

        private static class Parser
        {
            public static List<List<Cell>> Parse(string file)
            {
                var field = new List<List<Cell>>();
                var lines = System.IO.File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    var row = new List<Cell>();
                    foreach (var symbol in line)
                    {
                        switch (symbol)
                        {
                            case '.':
                                row.Add(Cell.Free);
                                break;
                            case '*':
                                row.Add(Cell.Wall);
                                break;
                            default:
                                throw new ArgumentException(
                                    "Only walls and corridors are supported in current version");
                        }
                    }
                    field.Add(row);
                }
                return field;
            }
        }
    }
}