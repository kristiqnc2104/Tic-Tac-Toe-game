using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{
    public partial class Form1 : Form
    {
        public class Game
        {
            private char[,] board;
            private char currentPlayer;
            private bool isGameOver;
            private int playerWins;
            private int computerWins;
            private int draws;
            public char CurrentPlayer => currentPlayer;
            public bool IsGameOver => isGameOver;
            public int PlayerWins => playerWins;
            public int ComputerWins => computerWins;
            public int Draws => draws;

         
            public Game()
            {
                board = new char[3, 3];
                playerWins = 0;
                computerWins = 0;
                draws = 0;
                ResetGame('X'); 
            }

            public void ResetGame(char startingPlayer)
            {
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        board[r, c] = ' ';
                    }
                }
                currentPlayer = startingPlayer;
                isGameOver = false;
            }

            public char GetCell(int row, int col)
            {
                return board[row, col];
            }

            public void MakeMove(int row, int col)
            {
                if (row < 0 || row >= 3 || col < 0 || col >= 3)
                    throw new ArgumentOutOfRangeException("Индексът е извън границите на полето.");

                if (board[row, col] != ' ')
                    throw new InvalidOperationException("Клетката вече е заета!");

                if (isGameOver)
                    throw new InvalidOperationException("Играта вече е приключила!");

                board[row, col] = currentPlayer;

                if (CheckWinner())
                {
                    isGameOver = true;
                    if (currentPlayer == 'X') playerWins++;
                    else computerWins++;
                }
                else if (IsDraw())
                {
                    isGameOver = true;
                    draws++;
                }
                else
                {
                    
                    currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
                }
            }

           
            public bool CheckWinner()
            {
                
                for (int i = 0; i < 3; i++)
                    if (board[i, 0] != ' ' && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                        return true;

            
                for (int i = 0; i < 3; i++)
                    if (board[0, i] != ' ' && board[0, i] == board[1, i] && board[1, i] == board[2, i])
                        return true;

                if (board[0, 0] != ' ' && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                    return true;
                if (board[0, 2] != ' ' && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                    return true;

                return false;
            }

            public bool IsDraw()
            {
                if (CheckWinner()) return false;

                for (int r = 0; r < 3; r++)
                    for (int c = 0; c < 3; c++)
                        if (board[r, c] == ' ')
                            return false;

                return true;
            }

            public char[,] GetBoardState()
            {
                return (char[,])board.Clone();
            }
        }
        public static class AI
        {
            
            public static int[] GetBestMove(char[,] board)
            {
                int bestScore = int.MinValue;
                int[] bestMove = null;

                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        if (board[r, c] == ' ')
                        {
                            board[r, c] = 'O';

                            
                            int score = Minimax(board, 0, false);

                            board[r, c] = ' '; 

                            if (score > bestScore)
                            {
                                bestScore = score;
                                bestMove = new int[] { r, c };
                            }
                        }
                    }
                }

                return bestMove;
            }

            
            private static int Minimax(char[,] board, int depth, bool isMaximizing)
            {
                
                if (CheckWinnerSim(board, 'O')) return 10 - depth; 
                if (CheckWinnerSim(board, 'X')) return depth - 10; 
                if (IsDrawSim(board)) return 0;                  
                if (isMaximizing)
                {
                    int bestScore = int.MinValue;
                    for (int r = 0; r < 3; r++)
                    {
                        for (int c = 0; c < 3; c++)
                        {
                            if (board[r, c] == ' ')
                            {
                                board[r, c] = 'O';
                                int score = Minimax(board, depth + 1, false);
                                board[r, c] = ' ';
                                bestScore = Math.Max(score, bestScore);
                            }
                        }
                    }
                    return bestScore;
                }
                else
                {
                    int bestScore = int.MaxValue;
                    for (int r = 0; r < 3; r++)
                    {
                        for (int c = 0; c < 3; c++)
                        {
                            if (board[r, c] == ' ')
                            {
                                board[r, c] = 'X';
                                int score = Minimax(board, depth + 1, true);
                                board[r, c] = ' ';
                                bestScore = Math.Min(score, bestScore);
                            }
                        }
                    }
                    return bestScore;
                }
            }
            private static bool CheckWinnerSim(char[,] board, char player)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player) return true;
                    if (board[0, i] == player && board[1, i] == player && board[2, i] == player) return true;
                }
                if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) return true;
                if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player) return true;
                return false;
            }

            private static bool IsDrawSim(char[,] board)
            {
                for (int r = 0; r < 3; r++)
                    for (int c = 0; c < 3; c++)
                        if (board[r, c] == ' ')
                            return false;
                return true;
            }
        }



        private Game game;
        private Button[,] btnBoard;

        private Label lblStatus;
        private Label lblPlayerWins;
        private Label lblComputerWins;
        private Label lblDraws;
        private Button btnNewGame;
        private ComboBox cmbFirstPlayer;

        public Form1()
        {
            InitializeComponent();
            game = new Game();
            InitializeCustomComponents();
            UpdateUI();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Морски Шах с AI";
            this.Size = new Size(400, 500);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Label lblChoose = new Label() { Text = "Започва:", Location = new Point(20, 20), Width = 60 };
            cmbFirstPlayer = new ComboBox() { Location = new Point(85, 17), Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbFirstPlayer.Items.AddRange(new string[] { "Играч (X)", "Компютър (O)" });
            cmbFirstPlayer.SelectedIndex = 0;

            btnNewGame = new Button() { Text = "Нова игра", Location = new Point(200, 15), Width = 160 };
            btnNewGame.Click += BtnNewGame_Click;

            this.Controls.Add(lblChoose);
            this.Controls.Add(cmbFirstPlayer);
            this.Controls.Add(btnNewGame);

            lblStatus = new Label() { Text = "Ход на играча", Location = new Point(20, 55), Width = 340, Font = new Font("Arial", 12, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };
            this.Controls.Add(lblStatus);
            TableLayoutPanel panelBoard = new TableLayoutPanel();
            panelBoard.ColumnCount = 3;
            panelBoard.RowCount = 3;
            panelBoard.Location = new Point(50, 90);
            panelBoard.Size = new Size(270, 270);

            btnBoard = new Button[3, 3];

            for (int r = 0; r < 3; r++)
            {
                panelBoard.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
                panelBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));

                for (int c = 0; c < 3; c++)
                {
                    Button btn = new Button();
                    btn.Dock = DockStyle.Fill;
                    btn.Font = new Font("Arial", 24, FontStyle.Bold);
                    btn.Tag = new int[] { r, c };
                    btn.Click += GridButton_Click;

                    btnBoard[r, c] = btn;
                    panelBoard.Controls.Add(btn, c, r);
                }
            }
            this.Controls.Add(panelBoard);

            lblPlayerWins = new Label() { Text = "Играч: 0", Location = new Point(50, 380), Width = 80 };
            lblComputerWins = new Label() { Text = "Компютър: 0", Location = new Point(140, 380), Width = 100 };
            lblDraws = new Label() { Text = "Равенства: 0", Location = new Point(250, 380), Width = 90 };

            this.Controls.Add(lblPlayerWins);
            this.Controls.Add(lblComputerWins);
            this.Controls.Add(lblDraws);
        }

        private void GridButton_Click(object sender, EventArgs e)
        {
            if (game.IsGameOver || game.CurrentPlayer != 'X') return;

            Button clickedButton = (sender as Button);
            int[] coords = clickedButton.Tag as int[];
            int row = coords[0];
            int col = coords[1];

            try
            {
                game.MakeMove(row, col);
                UpdateUI();

                if (!game.IsGameOver)
                {
                    lblStatus.Text = "Ход на компютъра";
                    this.Refresh(); 
                    System.Threading.Thread.Sleep(400); 
                    ComputerTurn();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ComputerTurn()
        {
            int[] aiMove = AI.GetBestMove(game.GetBoardState());
            if (aiMove != null)
            {
                game.MakeMove(aiMove[0], aiMove[1]);
                UpdateUI();
            }
        }
        private void BtnNewGame_Click(object sender, EventArgs e)
        {
            char startingPlayer = cmbFirstPlayer.SelectedIndex == 0 ? 'X' : 'O';
            game.ResetGame(startingPlayer);
            UpdateUI();

            if (startingPlayer == 'O')
            {
                lblStatus.Text = "Ход на компютъра";
                this.Refresh();
                System.Threading.Thread.Sleep(400);
                ComputerTurn();
            }
        }

        private void UpdateUI()
        {
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    char cell = game.GetCell(r, c);
                    btnBoard[r, c].Text = cell == ' ' ? "" : cell.ToString();
                    btnBoard[r, c].Enabled = (cell == ' ' && !game.IsGameOver);
                }
            }

            lblPlayerWins.Text = $"Играч: {game.PlayerWins}";
            lblComputerWins.Text = $"Компютър: {game.ComputerWins}";
            lblDraws.Text = $"Равенства: {game.Draws}";

            if (game.IsGameOver)
            {
                if (game.CheckWinner())
                {
                    if (game.CurrentPlayer == 'X') 
                        lblStatus.Text = "Победа за Играча!";
                    else
                        lblStatus.Text = "Победа за Компютъра!";
                }
                else if (game.IsDraw())
                {
                    lblStatus.Text = "Равенство!";
                }
            }
            else
            {
                lblStatus.Text = game.CurrentPlayer == 'X' ? "Ход на играча" : "Ход на компютъра";
            }
        }
     
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
