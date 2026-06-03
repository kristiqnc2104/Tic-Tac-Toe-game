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
        public Form1()
        {
            InitializeComponent();
        }
        public class Game
        {
            public char[,] board;
            public char currentPlayer;
            public bool IsGameOver;

            public int PlayerWins { get; private set; }
            public int ComputerWins {  get; private set; }            
            public int Draws { get; private set; }

            public Game()
            {
                board = new char[3, 3];
                PlayerWins = 0;
                ComputerWins = 0;
                Draws = 0;
                currentPlayer = 'X';
                IsGameOver = false;
            }

            public void MakeMove(int row, int col)
            {
                if (!IsGameOver)
                {
                    board[row, col] = currentPlayer;
                    if (CheckWinner())
                    {
                        IsGameOver = true;
                        if (currentPlayer == 'X')
                        {
                            PlayerWins++;
                        }
                        else
                        {
                            ComputerWins++;
                        }
                    }
                    else if (IsDraw())
                    {
                        IsGameOver = true;
                        Draws++;
                    }
                    else
                    {
                        //Смяна на играч
                        currentPlayer = currentPlayer == 'X' ? 'O' : 'X'; 
                    }

                }
            }  
            public bool CheckWinner()
            {
                for (int i = 0; i < 3; i++)
                {
                    if (board[i, 0] != ' ' && board[i, 0] == board[i,1] && board[i, 1] == board[i, 2])
                    {
                        return true;
                    }
                    if (board[0, i] != ' ' && board[0,i] == board[1,i] && board[1,i] == board[2,i])
                    {
                        return true;
                    }
                }

                if (board[0, 0] != ' ' && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) 
                {
                    return true;
                }
                if (board[2, 0] != ' ' && board[2, 1] == board[1, 1] && board[1, 1] == board[0, 2])
                {
                    return true;
                }

                return false;
            }

            public bool IsDraw()
            {
                return !CheckWinner();
            }

            public void ResetGame()
            {
                for (int r = 0; r < 3; r++)
                    for (int c = 0; c < 3; c++)
                    {
                        board[r, c] = ' ';
                    }
                        
                currentPlayer = 'X';
                IsGameOver = false;
            }

            public char GetCell(int row, int col)
            {
                return board[row, col];
            }


        }

        public class AI
        {
            private char aiChar;
            private char humanChar;

            public AI(char aiSymbol = 'O', char humanSymbol = 'X')
            {
                aiChar = aiSymbol;
                humanChar = humanSymbol;
            }

            public int[] GetBestMove(char[,] board)
            {
                int bestScore = int.MinValue;
                int[] bestMove = new int[] {-1,-1};

                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        if (board[r, c] == ' ')
                        {
                            board[r, c] = aiChar;
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

            private int Minimax(char[,] board, int depth, bool isMaximizing)
            {
                if (CheckWin(board, aiChar)) return 10 - depth;
                if (CheckWin(board, humanChar)) return depth - 10;
                if (IsFull(board)) return 0;

                if (isMaximizing)
                {
                    int best = int.MinValue;
                    for (int r = 0; r < 3; r++)
                        for (int c = 0; c < 3; c++)
                            if (board[r, c] == ' ')
                            {
                                board[r, c] = aiChar;
                                best = Math.Max(best, Minimax(board, depth + 1, false));
                                board[r, c] = ' ';
                            }
                    return best;
                }
                else
                {
                    int best = int.MaxValue;
                    for (int r = 0; r < 3; r++)
                        for (int c = 0; c < 3; c++)
                            if (board[r, c] == ' ')
                            {
                                board[r, c] = humanChar;
                                best = Math.Min(best, Minimax(board, depth + 1, true));
                                board[r, c] = ' ';
                            }
                    return best;
                }
            }

            public bool CanWin(char[,] board, char player)
            {
                for (int r = 0; r < 3; r++)
                    for (int c = 0; c < 3; c++)
                        if (board[r, c] == ' ')
                        {
                            board[r, c] = player;
                            bool wins = CheckWin(board, player);
                            board[r, c] = ' ';
                            if (wins) return true;
                        }
                return false;
            }

            private bool CheckWin(char[,] b, char p)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (b[i, 0] == p && b[i, 1] == p && b[i, 2] == p) return true;
                    if (b[0, i] == p && b[1, i] == p && b[2, i] == p) return true;
                }
                if (b[0, 0] == p && b[1, 1] == p && b[2, 2] == p) return true;
                if (b[0, 2] == p && b[1, 1] == p && b[2, 0] == p) return true;
                return false;
            }

            private bool IsFull(char[,] b)
            {
                foreach (char c in b) if (c == ' ') return false;
                return true;
            }

            public int EvaluateMove(char[,] board, int row, int col, char player)
            {
                board[row, col] = player;
                int score = 0;
                if (CheckWin(board, player)) score = 10;
                board[row, col] = ' ';
                return score;
            }
        }
        public class Player
        {
            public string Name { get; set; }
            public char Symbol { get; set; }
            public bool IsHuman { get; set; }

            public Player(string name, char symbol, bool isHuman)
            {
                Name = name;
                Symbol = symbol;
                IsHuman = isHuman;
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
