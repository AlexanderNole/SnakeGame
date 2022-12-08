namespace SnakeGame
{
    public partial class ViewMain : Form
    {
        // Fields
        private List<Circle> snake = new List<Circle>();
        private Circle food = new Circle();
        private Random rand = new Random();

        private int maxWidth;
        private int maxHeight;

        private int score;
        private int highScore;

        private bool isLeft;
        private bool isRight;
        private bool isDown;
        private bool isUp;

        // Constructor
        public ViewMain()
        {
            InitializeComponent();
            new Settings();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && Settings.Directions != "right")
                isLeft = true;
            if (e.KeyCode == Keys.Right && Settings.Directions != "left")
                isRight = true;
            if (e.KeyCode == Keys.Up && Settings.Directions != "down")
                isUp = true;
            if (e.KeyCode == Keys.Down && Settings.Directions != "up")
                isDown = true;
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                isLeft = false;

            if (e.KeyCode == Keys.Right)
                isRight = false;

            if (e.KeyCode == Keys.Up)
                isUp = false;

            if (e.KeyCode == Keys.Down)
                isDown = false;
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // Configuración de las direcciones
            if (isLeft)
                Settings.Directions = "left";

            if (isRight)
                Settings.Directions = "right";

            if (isDown)
                Settings.Directions = "down";

            if (isUp)
                Settings.Directions = "up";


            for (int i = snake.Count - 1; i >= 0; i--) {
                if (i == 0) {
                    switch (Settings.Directions) {
                        case "left":
                            snake[i].X--;
                            break;
                        case "right":
                            snake[i].X++;
                            break;
                        case "down":
                            snake[i].Y++;
                            break;
                        case "up":
                            snake[i].Y--;
                            break;
                    }

                    if (snake[i].X < 0)
                        snake[i].X = maxWidth;

                    if (snake[i].X > maxWidth)
                        snake[i].X = 0;

                    if (snake[i].Y < 0)
                        snake[i].Y = maxHeight;

                    if (snake[i].Y > maxHeight)
                        snake[i].Y = 0;

                    if (snake[i].X == food.X && snake[i].Y == food.Y)
                        EatFood();

                    for (int j = 1; j < snake.Count; j++) {
                        if (snake[i].X == snake[j].X && snake[i].Y == snake[j].Y)
                            GameOver();
                    }

                } else {
                    snake[i].X = snake[i - 1].X;
                    snake[i].Y = snake[i - 1].Y;
                }
            }

            Canvas.Invalidate();
        }

        private void UpdateCanvasGraphics(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Brush snakeColour;

            // Este ciclo pinta a la serpiente en el lienzo
            for (int i = 0; i < snake.Count; i++) {
                snakeColour = i == 0 ? Brushes.Green : Brushes.Black;

                graphics.FillEllipse(snakeColour, new Rectangle(
                    snake[i].X * Settings.Width,
                    snake[i].Y * Settings.Height,
                    Settings.Width,
                    Settings.Height
                    ));
            }

            // Pinta la comida en el lienzo
            graphics.FillEllipse(Brushes.Red, new Rectangle(
                    food.X * Settings.Width,
                    food.Y * Settings.Height,
                    Settings.Width,
                    Settings.Height
                    ));
        }

        private void RestartGame()
        {
            // Configuraciones para el inicio del juego
            maxWidth = Canvas.Width / Settings.Width - 1;
            maxHeight = Canvas.Height / Settings.Height - 1;
            score = 0;

            snake.Clear();

            BtnStart.Enabled = false;
            LblScore.Text = "Score: " + score;

            // Se crea la cabeza de la serpiente y se agrega a la lista
            Circle head = new Circle { X = 10, Y = 5 };
            snake.Add(head);

            // Este ciclo crea el cuerpo de la serpiente y lo agrega a la lista
            for (int i = 0; i < 6; i++) {
                Circle body = new Circle();
                snake.Add(body);
            }

            // Creación del alimento de manera aleatoria
            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };

            // Inicializamos el temporizador
            gameTimer.Start();
        }

        private void EatFood()
        {
            score += 1;
            LblScore.Text = "Score: " + score;

            Circle body = new Circle() {
                X = snake[snake.Count - 1].X,
                Y = snake[snake.Count - 1].Y
            };

            snake.Add(body);

            if (gameTimer.Interval > 60)
                gameTimer.Interval -= 3;
            else if (gameTimer.Interval > 40)
                gameTimer.Interval --;
            else gameTimer.Interval = 40;

            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
        }

        private void GameOver()
        {
            gameTimer.Stop();
            BtnStart.Enabled = true;

            if (score > highScore) {
                highScore = score;
                LblHighScore.Text = "High Score: " + highScore;
                LblHighScore.ForeColor = Color.Blue;
            }
        }
    }
}