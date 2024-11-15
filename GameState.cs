namespace Snake
{
    public class GameState
    {
        public int Rows { get; }
        public int Columns { get; }
        public GridValue[,] Grid { get; }
        public Direction SnakeDirection { get; private set; }
        public int score { get; private set; }
        public bool GameOver { get; private set; }
        private readonly LinkedList<Position> snakePosition = new LinkedList<Position>();
        private readonly LinkedList<Direction> DirectionChanges = new LinkedList<Direction>();
        private readonly Random random = new Random();

        public GameState(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Grid = new GridValue[Rows, Columns];
            SnakeDirection = Direction.Right;
            AddSnakeToGame();
            AddFoodToGame();
        }
        private void AddSnakeToGame()
        {
            int Startingrow = Rows / 2;
            for (int i = 1; i <= 3; i++)
            {
                Grid[Startingrow, i] = GridValue.Snake;
                snakePosition.AddFirst(new Position(Startingrow, i));
            }
        }
        private IEnumerable<Position> GetEmptyPositions()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    if (Grid[row, column] == GridValue.Empty)
                    {
                        yield return new Position(row, column);
                    }
                }
            }
        }
        private void AddFoodToGame()
        {
            List<Position> empty = new List<Position>(GetEmptyPositions());
            if (empty.Count == 0)
            {
                GameOver = true; //mabye gonna change this
                return;
            }
            Position foodPosition = empty[random.Next(empty.Count)];
            Grid[foodPosition.Row, foodPosition.Column] = GridValue.Food;
        }
        public Position HeadPosition() => snakePosition.First.Value;
        public Position TailPosition() => snakePosition.Last.Value;
        public IEnumerable<Position> SnakePosition() => snakePosition;
        private void addHead(Position HeadPos)
        {
            snakePosition.AddFirst(HeadPos);
            Grid[HeadPos.Row, HeadPos.Column] = GridValue.Snake;
        }
        private void removeTail()
        {
            Position TailPos = snakePosition.Last.Value;
            Grid[TailPos.Row, TailPos.Column] = GridValue.Empty;
            snakePosition.RemoveLast();
        }
        private bool CanChangeDirection(Direction newDirection)
        {
            if (DirectionChanges.Count == 2)
            {
                return false;
            }
            Direction lastDirection = GetLastDirection();
            return ((newDirection != lastDirection) && (newDirection != lastDirection.oposite()));
        }
        private Direction GetLastDirection()
        {
            if (DirectionChanges.Count == 0)
            {
                return SnakeDirection;
            }
            return DirectionChanges.Last.Value;
        }
        public void ChangeDirection(Direction newDirection)
        {
            if (CanChangeDirection(newDirection))
            {
                DirectionChanges.AddLast(newDirection);
            }
            
        }
        private bool IsOutOfBounds(Position position)
        {
            return position.Row < 0 || position.Row >= Rows || position.Column < 0 || position.Column >= Columns;
        }
        private GridValue WillHit(Position newHeadposition)
        {
            if (IsOutOfBounds(newHeadposition)) 
                return GridValue.OutOfBounds;
            if (newHeadposition == TailPosition()) 
                return GridValue.Empty;
            return Grid[newHeadposition.Row, newHeadposition.Column];
        }
        public void Move() 
        {
            if (DirectionChanges.Count > 0)
            {
                SnakeDirection = DirectionChanges.First.Value;
                DirectionChanges.RemoveFirst();
            }
            Position newHeadPosition = HeadPosition().Traslate(SnakeDirection);
            GridValue hit = WillHit(newHeadPosition);
            switch (hit)
            {
                case GridValue.Empty:
                    removeTail();
                    addHead(newHeadPosition);
                    break;
                case GridValue.Food:
                    addHead(newHeadPosition);
                    AddFoodToGame();
                    score++;
                    break;
                case GridValue.OutOfBounds:
                    GameOver = true;
                    break;
                default:
                    GameOver = true;
                    break;
            }
        }
    }
  
}
