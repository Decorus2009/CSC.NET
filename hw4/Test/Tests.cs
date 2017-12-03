using Mini_Roguelike;
using NUnit.Framework;

namespace Test
{
    /*
    Game требует путь к файлу с полем. При тестировании надо указать корректный путь в текущей файловой системе 
    */
    public class Tests
    {
        private Game _generatedGame;
        private Game _emptyGame;


        [SetUp]
        public void SetUp()
        {
            _generatedGame = new Game("/Users/decorus/Dropbox/CSC/Courses/2017-2/C#/repo/CSC.NET/hw4/Mini-Roguelike/field/generated_field.txt");
            _emptyGame = new Game("/Users/decorus/Dropbox/CSC/Courses/2017-2/C#/repo/CSC.NET/hw4/Mini-Roguelike/field/empty_field.txt");
        }
        
        [Test]
        public void GameFieldContainsOnlyWallsAndCorridors()
        {
            _generatedGame.Field.ForEach(
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
            _generatedGame.OnDown();
            Assert.AreEqual(_generatedGame.Current, _generatedGame.Previous);
        }

        
        [Test]
        public void CannotMoveUpInEmptyFieldWhenAtFirstRow() 
        {
            _emptyGame.OnUp();
            Assert.AreEqual(_emptyGame.Current, _emptyGame.Previous);
        }

        [Test]
        public void CannotMoveDownInEmptyFieldWhenAtLastRow()
        {
            var rows = _emptyGame.Field.Count;

            _emptyGame.Current = (0, rows - 1);
            _emptyGame.Previous = _emptyGame.Current;
            _emptyGame.OnDown();
            Assert.AreEqual(_emptyGame.Current, _emptyGame.Previous);
        }

        [Test]
        public void CannotMoveLeftInEmptyFieldWhenAtFirstColumn()
        {
            _emptyGame.OnLeft();
            Assert.AreEqual(_emptyGame.Current, _emptyGame.Previous);
        }
        
        [Test]
        public void CannotMoveRightInEmptyFieldWhenAtLastColumn()
        {
            var columns = _emptyGame.Field[0].Count;

            _emptyGame.Current = (columns - 1, 0);
            _emptyGame.Previous = _emptyGame.Current;
            _emptyGame.OnRight();
            Assert.AreEqual(_emptyGame.Current, _emptyGame.Previous);
        }

        
        [Test]
        public void CanMoveUpInEmptyFieldWhenNotAtFirstRow()
        {
            var rows = _emptyGame.Field.Count;

            _emptyGame.Current = (0, rows - 1);
            _emptyGame.Previous = _emptyGame.Current;

            _emptyGame.OnUp();
            Assert.AreEqual(_emptyGame.Current.x, _emptyGame.Previous.x);
            Assert.AreEqual(_emptyGame.Current.y, _emptyGame.Previous.y - 1);
        }

        [Test]
        public void CanMoveDownInEmptyFieldWhenNotAtLastRow()
        {
            _emptyGame.OnDown();
            Assert.AreEqual(_emptyGame.Current.x, _emptyGame.Previous.x);
            Assert.AreEqual(_emptyGame.Current.y, _emptyGame.Previous.y + 1);
        }
        
        [Test]
        public void CanMoveLeftInEmptyFieldWhenNotAtFirstColumn()
        {
            var columns = _emptyGame.Field[0].Count;

            _emptyGame.Current = (columns - 1, 0);
            _emptyGame.Previous = _emptyGame.Current;

            _emptyGame.OnLeft();
            Assert.AreEqual(_emptyGame.Current.x, _emptyGame.Previous.x - 1);
            Assert.AreEqual(_emptyGame.Current.y, _emptyGame.Previous.y);
        }

        [Test]
        public void CanMoveRightInEmptyFieldWhenNotAtLastColumn()
        {
            _emptyGame.OnRight();
            Assert.AreEqual(_emptyGame.Current.x, _emptyGame.Previous.x + 1);
            Assert.AreEqual(_emptyGame.Current.y, _emptyGame.Previous.y);
        }
    }
}