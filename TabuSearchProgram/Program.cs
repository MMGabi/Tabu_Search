using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TabuSearch.TabuSearchRun();
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
        }
        public static string[] loadFile()
        {
            string[] csvLines = System.IO.File.ReadAllLines(@"C:\Users\gabry\Desktop\Semestr 5\Inteligencja Obliczeniowa\Projekt2\Dane_PFSP_100_10.csv");
            return csvLines;
        }
        public static int[,] LoadData(string[] csvLines)
        {
            int n = csvLines.Length;
            string[] rowData = csvLines[1].Split(';');
            int m = rowData.Length;
            int[,] matrix = new int[n, m];

            for (int i = 0; i < n-1; i++)
            {
                
                rowData = csvLines[i + 1].Split(';');
                for (int j = 0; j < rowData.Length; j++)
                {
                    matrix[i, j] = Convert.ToInt32(rowData[j]);
                        
                }
            }

           // for (int i = 0; i < n-1; i++)
            {
               // matrix[i, 0] = i+1;
            }

            return matrix;
        }
        public static void Showmatrix(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);
            for (int l = 0; l < n; l++)
            {
                for (int k = 0; k < m; k++)
                {
                    Console.Write(matrix[l, k]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        public static int[,] Matrix_summed(int[,] matrix)
        {

            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);
            int[,] MatrixHelp = CopyMatrix(matrix);
            int help;
            
            for (int j = 2; j < m; j++)
            {

                MatrixHelp[0, j] += MatrixHelp[0,j-1];
            }
            for (int i = 1; i < n; i++)
            {

                MatrixHelp[i,1] += MatrixHelp[i-1,1];
            }

            for (int i = 1; i < n; i++)
            {
                for (int j = 2; j < m; j++)
                {
                    if (MatrixHelp[i-1,j] > MatrixHelp[i,j-1]) help = MatrixHelp[i-1,j];
                    else help = MatrixHelp[i,j-1];
                    MatrixHelp[i, j] += help;
                }

            }

            return MatrixHelp;
        }
        public static int[] GetSolution(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int[] solution = new int[n];
            for (int i = 0; i < n-1; i++)
            {
                solution[i] = matrix[i,0];

            }
            return solution;
        }
        public static int GetCost(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);
            return matrix[n-1, m-1];
        }

        public static int[,] CopyMatrix(int[,] matrix)
        {

            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);
            int[,] Matrix = new int[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Matrix[i, j] = matrix[i, j];
                }

            }

            return Matrix;
        }
         public static void ShowSolution(int[] solution)
        {
            for (int i = 0; i < solution.Length-1; i++)
            {
                Console.Write(solution[i]);
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        public static void SolutionChangeMatrix(int[,]matrix,int x,int y)
        {
            int n=matrix.GetLength(0);
            int m = matrix.GetLength(1);
            int[] help1 = new int[m];
            int[] help2 = new int[m];
            for (int j = 0; j < n-1; j++)
            {
                if (matrix[j,0]==x)
                {
                    for (int i = 0; i < m; i++)
                    {
                        help1[i] = matrix[j, i];
                        
                    }
                }
                if (matrix[j,0] == y)
                {
                    for (int i = 0; i < m; i++)
                    {
                        help2[i] = matrix[j, i];

                    }
                }
                

            }
           //ShowSolution(help1);
           // ShowSolution(help2);
            for (int j = 0; j < n-1; j++)
            {
                if (matrix[j, 0] == y)
                {
                    for (int i = 0; i < m; i++)
                    {
                        matrix[j, i] = help1[i];

                    }
                }
                else if (matrix[j,0] == x)
                {
                    for (int i = 0; i < m; i++)
                    {
                        matrix[j, i] = help2[i];

                    }
                }

            }


        }
    }
}
