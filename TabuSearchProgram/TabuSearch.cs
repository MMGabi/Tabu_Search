using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt2
{
    internal class TabuSearch
    {
        public static void TabuSearchRun()
        {
            string[] csvLines = Program.loadFile();
            int[,] matrix = Program.LoadData(csvLines);
            tabuSearch(matrix);
           
        }
        static void tabuSearch(int[,] matrix)
        {
            Program.Showmatrix(matrix);
            int n = matrix.GetLength(0); //długość tabeli
            int m = matrix.GetLength(1); //szerokość tabeli
            int[] solution = new int[n]; //rozwiązanie
            int[] copy_of_solution = new int[n];//kopia najmniejszego rozwiązania wee wszystkich obiegach pętli
            int[,] Matrix_sum = new int[n, m]; // macierz sumy
            int length_of_tabu_matrix=6;//długość tablicy tabu
            int[,]tabu_matrix=new int[length_of_tabu_matrix, 3];//macierz tabu

            //WYZNACZANIE ROZWIĄZANIA POCZĄTKOWEGO  
            Matrix_sum = Program.Matrix_summed(matrix);
            solution = Program.GetSolution(matrix);
            int cost=Program.GetCost(Matrix_sum);//obliczanie kosztu aktualnego rozwiązania
            int copy_of_cost = cost;//kopia najmniejszego rozwiązania we wszystkich obiegach pętli
            Program.ShowSolution(solution);
            Console.WriteLine("Koszt: "+cost);
            Console.WriteLine("----------------");
            
            int help_costs;
            int[,] possibilities = new int[n * n *n, 3];//macierz zawierająca wszystkie możliwe zamiany (w kolumnie pierwszej i drugiej i różnicę w długości trasy w kolumnie trzeciej 
            int k = 100;//ilość iteracji
            int amount_of_not_better_iteration = 10;
            int copy_of_amount_of_not_better_iteration = amount_of_not_better_iteration;
            int p = 0;//do dopisywania do possibilities
            int[,] matrix_copy = new int[n, m];// będziemy działać na kopii macierzy początkowej
            int[,] matrix_help = new int[n, m];// macierz do wyznaczania aktualnego rozwiązania
            int[,] matrix_sum_help = new int[n, m];// macierz zsumowana dodatkowa do wyznaczania aktualnego rozwiązania
            matrix_copy = Program.CopyMatrix(matrix);
            while (k > 0)
            {
           // SZUKANIE MOŻLIWYCH ZAMIAN
                
                //Program.Showmatrix(matrix_copy);
                for (int i = 0; i < n - 1; i++)
                {
                    for (int j = i + 1; j < n - 1; j++)
                    {

                        matrix_help = Program.CopyMatrix(matrix_copy);
                        int x = matrix_help[i, 0];//[0,0]-->1
                        int z = matrix_help[j, 0];//[1,0]-->2
                        if (x * z == 0) break;
                        Program.SolutionChangeMatrix(matrix_help, x, z);
                       // Program.Showmatrix(matrix_help);
                        //Program.ShowSolution(Program.GetSolution(matrix_help));
                        matrix_sum_help = Program.Matrix_summed(matrix_help);
                        help_costs = Program.GetCost(matrix_sum_help);

                        possibilities[p, 0] = x;
                        possibilities[p, 1] = z;
                        possibilities[p, 2] = help_costs - cost;

                        p++;
                        // Program.Showmatrix(matrix_help);
                    }
                }
                //Program.Showmatrix(possibilities);

                //WYBIERANIE NAJLEPSZEJ MOŻLIWOŚCI
                bool IsItRight = false;

                int[] best_possible = new int[3];
                //wybieranie pierwszego możliwego rozwiązania (nieopytmalnego)
                // potrzebne jest nam to po to, aby później móc porównywać z tym rozwiązaniem inne rozwiązania i sprawdzać któe będzie najlepsze
                for (int i = 0; i < possibilities.Length; i++)
                {
                    if (possibilities[i, 0] == 0 || possibilities[i, 1] == 0 || possibilities[i, 2] == 0)
                    {
                        IsItRight = false;
                        continue;
                    }

                    for (int j = 0; j < length_of_tabu_matrix; j++)
                    {

                        if (possibilities[i, 0] != tabu_matrix[j, 0] && possibilities[i, 1] != tabu_matrix[j, 1])
                        {
                            if (possibilities[i, 1] != tabu_matrix[j, 0] && possibilities[i, 0] != tabu_matrix[j, 1])
                            {
                                IsItRight = true;
                            }
                            else IsItRight = false;
                        }
                        else IsItRight = false;


                    }
                    if (IsItRight == true)
                    {
                        best_possible[0] = possibilities[i, 0];
                        best_possible[1] = possibilities[i, 1];
                        best_possible[2] = possibilities[i, 2];
                        break;
                    }

                }
                // SZUKANIE NAJLEPSZEJ ZAMIANY
                for (int i = 0; i < possibilities.Length; i++)
                {
                    if (possibilities[i, 0] == 0 && possibilities[i, 1] == 0 && possibilities[i, 2] == 0) break;

                    for (int j = 0; j < length_of_tabu_matrix; j++)
                    {
                        if (possibilities[i, 0] != tabu_matrix[j, 0] && possibilities[i, 1] != tabu_matrix[j, 1])
                        {
                            if (possibilities[i, 1] != tabu_matrix[j, 0] && possibilities[i, 0] != tabu_matrix[j, 1])
                            {
                                IsItRight = true;//sprawdzanie czy na pewno tego rozwiązania nie ma w tabeli tabu
                            }
                            else IsItRight = false;
                        }
                        else IsItRight = false;

                        if (IsItRight == false) break;

                    }
                    if (IsItRight == true & best_possible[2] > possibilities[i, 2])
                    {
                        best_possible[0] = possibilities[i, 0];
                        best_possible[1] = possibilities[i, 1];
                        best_possible[2] = possibilities[i, 2];
                    }

                }

                //ZMNIEJSZANIE WARTOŚCI NA TABLICY TABU
                for (int i = 0; i < length_of_tabu_matrix; i++)
                {
                    if (tabu_matrix[i, 2] > 1)
                    {

                        tabu_matrix[i, 2] = tabu_matrix[i, 2] - 1;
                    }
                    else
                    {
                        tabu_matrix[i, 0] = 0;
                        tabu_matrix[i, 1] = 0;

                    }



                }
                //DOPISYWANIE AKTUALNEGO ROZWIĄZANIA DO LISTY TABU
                for (int i = 0; i < length_of_tabu_matrix; i++)
                {
                    if (tabu_matrix[i, 0] == 0 && tabu_matrix[i, 1] == 0)
                    {
                        tabu_matrix[i, 0] = best_possible[0];
                        tabu_matrix[i, 1] = best_possible[1];
                        tabu_matrix[i, 2] = length_of_tabu_matrix;
                        break;
                    }
                }
                //  ZMIANA AKTUALNEGO ROZWIĄZANIA NA NAJLEPSZE
                
                    Program.SolutionChangeMatrix(matrix_copy, best_possible[0], best_possible[1]);
                
                //Program.Showmatrix(matrix_copy);
                matrix_sum_help = Program.Matrix_summed(matrix_copy);
                solution = Program.GetSolution(matrix_copy);
                Console.WriteLine(best_possible[0] +" " + best_possible[1]+" " + best_possible[2]);
                //Program.ShowSolution(solution);
                //Program.Showmatrix(tabu_matrix);
                cost = Program.GetCost(matrix_sum_help);

                //PORÓWNYWANIE Z NAJMNIEJSZYM ROzWIĄzANIEM

                if (copy_of_cost > cost)
                {

                    for (int i = 0; i < n - 1; i++)
                    {
                        copy_of_solution[i] = solution[i];
                    }
                    copy_of_cost = cost;
                    copy_of_amount_of_not_better_iteration = amount_of_not_better_iteration;
                }
                
            p = 0;
            k--;
            }

            
            Console.Write("Najmniejsza długość trasy " + copy_of_cost + "\n Najlepsza trasa : \n");
            Program.ShowSolution(copy_of_solution);
        }
        

    }
   

}
