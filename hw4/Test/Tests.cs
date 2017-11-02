using Mini_Roguelike;
using NUnit.Framework;

namespace Test
{
    /*
    Game требует путь к файлу с полем. При тестировании надо указать корректный путь в текущей файловой системе 
    */
    public class Tests
    {
        private Game _game;

        [Test]
        public void GameFieldContainsOnlyWallsAndCorridors()
        {
            _game = new Game("/Users/decorus/Dropbox/CSC/Courses/2017-2/C#/repo/CSC.NET/hw4/Mini-Roguelike/field/generated_field.txt");
            _game.Field.ForEach(
                row => { row.ForEach(cell => { Assert.That(cell == Cell.Free || cell == Cell.Wall); }); });
        }

        /*
        Тут привязка к приложенному файлу generated_field.txt, в котором в клетке [1, 0] стенка, 
        т.е. из [0, 0] вниз шагнуть нельзя. 
        При перегенерации поля этот тест может упасть, если под собакой не окажется стенки
        */
        [Test]
        public void CannotMoveThroughWall()
        {
            _game = new Game("/Users/decorus/Dropbox/CSC/Courses/2017-2/C#/repo/CSC.NET/hw4/Mini-Roguelike/field/generated_field.txt");
            _game.OnDown();
            Assert.AreEqual(_game.Current, _game.Previous);
        }

        
        [Test]
        public void CannotMoveUpInEmptyFieldWhenAtFirstRow() 
        {
            _game = new Game("/Users/decorus/Dropbox/CSC/Courses/2017-2/C#/repo/CSC.NET/hw4/Mini-Roguelike/field/empty_field.txt");
            _game.OnUp();
            Assert.AreEqual(_game.Current, _game.Previous);
        }

        [Test]
        public void CannotMoveDownInEmptyFieldWhenAtLastRow()
        {
            _game = new Game("/Users/decorus/Dropbox/CSC/Courses/2017-2/C#/repo/CSC.NET/hw4/Mini-Roguelike/field/empty_field.txt");
            var rows = _game.Field.Count;

            _game.Current = (0, rows - 1);
            _game.Previous = _game.Current;
            _game.OnDown();
            Assert.AreEqual(_game.Current, _game.Previous);
        }

        [Test]
        public void CannotMoveLeftInEmptyFieldWhenAtFirstColumn()
        {
            _game = new Game("/Users/decorus/Dropbox/CSC/Courses/2017-2/C#/repo/CSC.NET/hw4/Mini-Roguelike/field/empty_field.txt");
            _game.OnLeft();
            Assert.AreEqual(_game.Current, _game.Previous);
        }
        
        [Test]
        public void CannotMoveRightInEmptyFieldWhenAtLastColumn()
        {
            _game = new Game("/Users/decorus/Dropbox/CSC/Courses/2017-2/C#/repo/CSC.NET/hw4/Mini-Roguelike/field/empty_field.txt");
            var columns = _game.Field[0].Count;

            _game.Current = (columns - 1, 0);
            _game.Previous = _game.Current;
            _game.OnRight();
            Assert.AreEqual(_game.Current, _game.Previous);
        }

        
        [Test]
        public void CanMoveUpInEmptyFieldWhenNotAtFirstRow()
        {
            _game = new Game("/Users/decorus/Dropbox/CSC/Courses/2017-2/C#/repo/CSC.NET/hw4/Mini-Roguelike/field/empty_field.txt");
            var rows = _game.Field.Count;

            _game.Current = (0, rows - 1);
            _game.Previous = _game.Current;

            _game.OnUp();
            Assert.AreEqual(_game.Current.x, _game.Previous.x);
            Assert.AreEqual(_game.Current.y, _game.Previous.y - 1);
        }

        [Test]
        public void CanMoveDownInEmptyFieldWhenNotAtLastRow()
        {
            _game = new Game("/Users/decorus/Dropbox/CSC/Courses/2017-2/C#/repo/CSC.NET/hw4/Mini-Roguelike/field/empty_field.txt");
            _game.OnDown();
            Assert.AreEqual(_game.Current.x, _game.Previous.x);
            Assert.AreEqual(_game.Current.y, _game.Previous.y + 1);
        }
        
        [Test]
        public void CanMoveLeftInEmptyFieldWhenNotAtFirstColumn()
        {
            _game = new Game("/Users/decorus/Dropbox/CSC/Courses/2017-2/C#/repo/CSC.NET/hw4/Mini-Roguelike/field/empty_field.txt");
            var columns = _game.Field[0].Count;

            _game.Current = (columns - 1, 0);
            _game.Previous = _game.Current;

            _game.OnLeft();
            Assert.AreEqual(_game.Current.x, _game.Previous.x - 1);
            Assert.AreEqual(_game.Current.y, _game.Previous.y);
        }

        [Test]
        public void CanMoveRightInEmptyFieldWhenNotAtLastColumn()
        {
            _game = new Game("/Users/decorus/Dropbox/CSC/Courses/2017-2/C#/repo/CSC.NET/hw4/Mini-Roguelike/field/empty_field.txt");
            _game.OnRight();
            Assert.AreEqual(_game.Current.x, _game.Previous.x + 1);
            Assert.AreEqual(_game.Current.y, _game.Previous.y);
        }
    }

}