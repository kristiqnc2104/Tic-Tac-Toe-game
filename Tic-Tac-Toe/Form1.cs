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
        class Game
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

            }

            public char GetCell(int row, int col)
            {
                return 'a';
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
