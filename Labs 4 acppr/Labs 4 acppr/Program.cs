using System;
using System.Globalization;
using System.Collections.Generic;

class Program
{
    static int rows, cols;
    static double[,] U;
    static double alpha;
    static double[] P;

    static List<int> allWinners = new List<int>();

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        while (true)
        {
            Console.WriteLine("\nПРАКТИЧНА РОБОТА №4");
            Console.WriteLine("Ігри з природою");
            Console.WriteLine("1 - Ввести матрицю вручну");
            Console.WriteLine("2 - Завантажити мій варіант");
            Console.WriteLine("3 - Завантажити тестовий приклад 1");
            Console.WriteLine("4 - Завантажити тестовий приклад 2");
            Console.WriteLine("5 - Показати матрицю");
            Console.WriteLine("6 - Знайти оптимальні стратегії");
            Console.WriteLine("0 - Вихід");
            Console.Write("Ваш вибір: ");

            int choice = int.Parse(Console.ReadLine());

            if (choice == 0) break;

            switch (choice)
            {
                case 1:
                    InputData();
                    break;
                case 2:
                    LoadVariant12();
                    break;
                case 3:
                    LoadTest1();
                    break;
                case 4:
                    LoadTest2();
                    break;
                case 5:
                    if (CheckData()) PrintMatrix(U);
                    break;
                case 6:
                    if (CheckData()) SolveAllCriteria();
                    break;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }
    }

    static void InputData()
    {
        Console.Write("Кількість стратегій: ");
        rows = int.Parse(Console.ReadLine());

        Console.Write("Кількість станів природи: ");
        cols = int.Parse(Console.ReadLine());

        U = new double[rows, cols];

        Console.WriteLine("\nВведіть матрицю корисності U:");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"U[{i + 1},{j + 1}] = ");
                U[i, j] = double.Parse(Console.ReadLine());
            }
        }

        Console.Write("Коефіцієнт Гурвіца alpha: ");
        alpha = double.Parse(Console.ReadLine());

        P = new double[cols];

        Console.WriteLine("Ймовірності для критерію Байєса:");
        for (int j = 0; j < cols; j++)
        {
            Console.Write($"P[{j + 1}] = ");
            P[j] = double.Parse(Console.ReadLine());
        }

        Console.WriteLine("Дані збережено.");
    }

    static void LoadVariant12()
    {
        rows = 3;
        cols = 4;

        U = new double[,]
        {
            {  1,  1, -2, 1 },
            {  2, -1,  2, 2 },
            { -1,  2,  1, 4 }
        };

        alpha = 0.3;
        P = new double[] { 0.2, 0.4, 0.1, 0.3 };

        Console.WriteLine("Завантажено варіант.");
    }

    static void LoadTest1()
    {
        rows = 3;
        cols = 4;

        U = new double[,]
        {
            { -1,  1, 1, 4 },
            { -1, -2, 2, 3 },
            {  3, -1, 3, 2 }
        };

        alpha = 0.3;
        P = new double[] { 0.2, 0.4, 0.1, 0.3 };

        Console.WriteLine("Завантажено тестовий приклад 1.");
    }

    static void LoadTest2()
    {
        rows = 3;
        cols = 4;

        U = new double[,]
        {
            {  2, -1, 3, 4 },
            { -1,  2, 3, 7 },
            {  5,  4, 6, 2 }
        };

        alpha = 0.4;
        P = new double[] { 0.4, 0.1, 0.2, 0.3 };

        Console.WriteLine("Завантажено тестовий приклад 2.");
    }

    static bool CheckData()
    {
        if (U == null)
        {
            Console.WriteLine("Спочатку введіть або завантажте дані.");
            return false;
        }

        return true;
    }

    static void SolveAllCriteria()
    {
        allWinners.Clear();

        Console.WriteLine("\nЗгенерований протокол обчислення:");
        Console.WriteLine("\nМатриця корисності результатів U:");
        PrintMatrix(U);

        List<int> wald = WaldCriterion();
        List<int> maxmax = OptimismCriterion();
        List<int> hurwicz = HurwiczCriterion();
        List<int> savage = SavageCriterion();
        List<int> bayes = BayesCriterion();
        List<int> laplace = LaplaceCriterion();

        Console.WriteLine("\nПОВНИЙ РЕЗУЛЬТАТ:");
        PrintResult("Критерій Вальда", wald);
        PrintResult("Критерій максимаксу", maxmax);
        PrintResult("Критерій Гурвіца", hurwicz);
        PrintResult("Критерій Севіджа", savage);
        PrintResult("Критерій Байєса", bayes);
        PrintResult("Критерій Лапласа", laplace);

        PrintMostFrequent();
    }

    static List<int> WaldCriterion()
    {
        Console.WriteLine("Критерій Вальда:");

        double[] values = new double[rows];

        for (int i = 0; i < rows; i++)
        {
            values[i] = RowMin(i);
            Console.WriteLine($"min в рядку {i + 1}: {values[i]:F2}");
        }

        double best = Max(values);
        List<int> winners = GetIndexes(values, best);

        Console.WriteLine($"Максимальний елемент: {best:F2}");
        Console.WriteLine($"Оптимальні стратегії: {StrategiesText(winners)}\n");

        AddWinners(winners);
        return winners;
    }

    static List<int> OptimismCriterion()
    {
        Console.WriteLine("Критерій максимаксу:");

        double[] values = new double[rows];

        for (int i = 0; i < rows; i++)
        {
            values[i] = RowMax(i);
            Console.WriteLine($"max в рядку {i + 1}: {values[i]:F2}");
        }

        double best = Max(values);
        List<int> winners = GetIndexes(values, best);

        Console.WriteLine($"Максимальний елемент: {best:F2}");
        Console.WriteLine($"Оптимальні стратегії: {StrategiesText(winners)}\n");

        AddWinners(winners);
        return winners;
    }

    static List<int> HurwiczCriterion()
    {
        Console.WriteLine("Критерій Гурвіца:");
        Console.WriteLine($"Коефіцієнт alpha = {alpha:F2}");

        double[] values = new double[rows];

        for (int i = 0; i < rows; i++)
        {
            double min = RowMin(i);
            double max = RowMax(i);

            values[i] = alpha * min + (1 - alpha) * max;

            Console.WriteLine($"s{i + 1} = {alpha:F2} * {min:F2} + (1 - {alpha:F2}) * {max:F2} = {values[i]:F2}");
        }

        double best = Max(values);
        List<int> winners = GetIndexes(values, best);

        Console.WriteLine($"Максимальний елемент: {best:F2}");
        Console.WriteLine($"Оптимальні стратегії: {StrategiesText(winners)}\n");

        AddWinners(winners);
        return winners;
    }

    static List<int> SavageCriterion()
    {
        Console.WriteLine("Критерій Севіджа:");

        double[,] R = new double[rows, cols];

        for (int j = 0; j < cols; j++)
        {
            double colMax = ColumnMax(j);

            for (int i = 0; i < rows; i++)
                R[i, j] = colMax - U[i, j];
        }

        Console.WriteLine("Матриця ризиків:");
        PrintMatrix(R);

        double[] values = new double[rows];

        for (int i = 0; i < rows; i++)
        {
            values[i] = MaxInRow(R, i);
            Console.WriteLine($"max в рядку {i + 1}: {values[i]:F2}");
        }

        double best = Min(values);
        List<int> winners = GetIndexes(values, best);

        Console.WriteLine($"Мінімальний елемент: {best:F2}");
        Console.WriteLine($"Оптимальні стратегії: {StrategiesText(winners)}\n");

        AddWinners(winners);
        return winners;
    }

    static List<int> BayesCriterion()
    {
        Console.WriteLine("Критерій Байєса:");

        Console.Write("Ймовірності: ");
        for (int j = 0; j < cols; j++)
        {
            Console.Write($"p{j + 1} = {P[j]:F2}");
            if (j < cols - 1) Console.Write("; ");
        }
        Console.WriteLine();

        double[] values = new double[rows];

        for (int i = 0; i < rows; i++)
        {
            Console.Write($"s{i + 1} = ");

            for (int j = 0; j < cols; j++)
            {
                values[i] += U[i, j] * P[j];
                Console.Write($"{U[i, j]:F2} * {P[j]:F2}");

                if (j < cols - 1)
                    Console.Write(" + ");
            }

            Console.WriteLine($" = {values[i]:F2}");
        }

        double best = Max(values);
        List<int> winners = GetIndexes(values, best);

        Console.WriteLine($"Максимальний елемент: {best:F2}");
        Console.WriteLine($"Оптимальні стратегії: {StrategiesText(winners)}\n");

        AddWinners(winners);
        return winners;
    }

    static List<int> LaplaceCriterion()
    {
        Console.WriteLine("Критерій Лапласа:");

        double probability = 1.0 / cols;
        double[] values = new double[rows];

        for (int i = 0; i < rows; i++)
        {
            Console.Write($"s{i + 1} = ");

            for (int j = 0; j < cols; j++)
            {
                values[i] += U[i, j] * probability;
                Console.Write($"{U[i, j]:F2} * {probability:F2}");

                if (j < cols - 1)
                    Console.Write(" + ");
            }

            Console.WriteLine($" = {values[i]:F2}");
        }

        double best = Max(values);
        List<int> winners = GetIndexes(values, best);

        Console.WriteLine($"Максимальний елемент: {best:F2}");
        Console.WriteLine($"Оптимальні стратегії: {StrategiesText(winners)}\n");

        AddWinners(winners);
        return winners;
    }

    static void PrintResult(string name, List<int> winners)
    {
        Console.WriteLine($"{name}: {StrategiesText(winners)}");
    }

    static void PrintMostFrequent()
    {
        int[] count = new int[rows];

        foreach (int index in allWinners)
            count[index]++;

        int maxCount = 0;

        for (int i = 0; i < rows; i++)
            if (count[i] > maxCount)
                maxCount = count[i];

        List<int> result = new List<int>();

        for (int i = 0; i < rows; i++)
            if (count[i] == maxCount)
                result.Add(i);

        Console.WriteLine("\nКількість виборів стратегій:");

        for (int i = 0; i < rows; i++)
            Console.WriteLine($"A{i + 1}: {count[i]}");

        Console.WriteLine($"\nНайчастіше були оптимальними стратегії: {StrategiesText(result)}");
    }

    static void AddWinners(List<int> winners)
    {
        foreach (int w in winners)
            allWinners.Add(w);
    }

    static string StrategiesText(List<int> indexes)
    {
        string result = "";

        for (int i = 0; i < indexes.Count; i++)
        {
            result += $"A{indexes[i] + 1}";

            if (i < indexes.Count - 1)
                result += " або ";
        }

        return result;
    }

    static List<int> GetIndexes(double[] values, double target)
    {
        List<int> indexes = new List<int>();

        for (int i = 0; i < values.Length; i++)
            if (Math.Abs(values[i] - target) < 1e-9)
                indexes.Add(i);

        return indexes;
    }

    static double RowMin(int row)
    {
        double min = U[row, 0];

        for (int j = 1; j < cols; j++)
            if (U[row, j] < min)
                min = U[row, j];

        return min;
    }

    static double RowMax(int row)
    {
        double max = U[row, 0];

        for (int j = 1; j < cols; j++)
            if (U[row, j] > max)
                max = U[row, j];

        return max;
    }

    static double ColumnMax(int col)
    {
        double max = U[0, col];

        for (int i = 1; i < rows; i++)
            if (U[i, col] > max)
                max = U[i, col];

        return max;
    }

    static double MaxInRow(double[,] matrix, int row)
    {
        double max = matrix[row, 0];

        for (int j = 1; j < cols; j++)
            if (matrix[row, j] > max)
                max = matrix[row, j];

        return max;
    }

    static double Max(double[] array)
    {
        double max = array[0];

        for (int i = 1; i < array.Length; i++)
            if (array[i] > max)
                max = array[i];

        return max;
    }

    static double Min(double[] array)
    {
        double min = array[0];

        for (int i = 1; i < array.Length; i++)
            if (array[i] < min)
                min = array[i];

        return min;
    }

    static void PrintMatrix(double[,] matrix)
    {
        int r = matrix.GetLength(0);
        int c = matrix.GetLength(1);

        for (int i = 0; i < r; i++)
        {
            for (int j = 0; j < c; j++)
                Console.Write($"{matrix[i, j],8:F2}");

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}