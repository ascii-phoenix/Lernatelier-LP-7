using DocumentFormat.OpenXml.Drawing.Charts;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValue, ImageSource> gridValToImage = new()
        {
            {GridValue.Empty, Images.Empty},
            {GridValue.Snake, Images.SnakeBody},
            {GridValue.Food, Images.Food}
        };
        private readonly Dictionary<Direction, int> directionToRotate = new()
        {
            { Direction.Up, 0},
            { Direction.Right, 90},
            { Direction.Down, 180},
            { Direction.Left, 270},
        };
        private readonly int rows = 15;
        private readonly int columns = 15;
        private const int Startingspeed = 160;
        private readonly Image[,] gridImages;
        private GameState gameState;
        private bool gameRunning;
        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetUpgrid();
            gameState = new GameState(rows, columns);
        }
        private Image[,] SetUpgrid()
        {
            Image[,] images = new Image[rows, columns];
            GameGrid.Rows = rows;
            GameGrid.Columns = columns;
            GameGrid.Width = GameGrid.Height* (columns/(double) rows);
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    };
                    images[row, column] = image;
                    GameGrid.Children.Add(image);

                }
            }
            return images;
        }
        private void Draw()
        {
            DrawGrid();
            DrawSnakeHead();
            ScoreText.Text = $"Score: {gameState.score}";
        }
        private void DrawGrid()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    GridValue gridValue = gameState.Grid[r, c];
                    gridImages[r, c].Source = gridValToImage[gridValue];
                    gridImages[r, c].RenderTransform = Transform.Identity;
                }
            }
        }
        private void DrawSnakeHead()
        {
            Position HeadPosition = gameState.HeadPosition();
            Image headImage = gridImages[HeadPosition.Row, HeadPosition.Column];
            headImage.Source = Images.SnakeHead;
            int rotation = directionToRotate[gameState.SnakeDirection];
            headImage.RenderTransform = new RotateTransform(rotation);
        }
        private async Task DrawDeadSnake()
        {
            List<Position> snakePosition = new List<Position>(gameState.SnakePosition());
            for (int i = 0; i < snakePosition.Count; i++)
            {
                Position position = snakePosition[i];
                ImageSource image = (i == 0) ? Images.DeadHead : Images.DeadBody;
                gridImages[position.Row, position.Column].Source = image;
                await Task.Delay(50);
            }
        }
        private async Task RunGame()
        {
            Draw();
            await ShowContdown();
            OverLay.Visibility = Visibility.Hidden;
            await GameLoop();
            await ShowGameOver();
            gameState = new GameState(rows, columns);
        }
        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (OverLay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }
            if (!gameRunning)
            {
                gameRunning = true;
                await RunGame();
                gameRunning = false;
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.W:
                    gameState.ChangeDirection(Direction.Up);
                    break;
                case Key.S:
                    gameState.ChangeDirection(Direction.Down);
                    break;
                case Key.A:
                    gameState.ChangeDirection(Direction.Left);
                    break;
                case Key.D:
                    gameState.ChangeDirection(Direction.Right);
                    break;
            }
        }
        private async Task GameLoop()
        {
            while (!gameState.GameOver)
            {
                await Task.Delay(Startingspeed);
                gameState.Move();
                Draw();
            }
        }

        private async Task ShowContdown()
        {
            for (int i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }
        }
        private async Task ShowGameOver()
        {
            await DrawDeadSnake();
            await Task.Delay(1000);
            OverLay.Visibility = Visibility.Visible;
            OverlayText.Text = "Press any key to restart";
        }
    }
    
}