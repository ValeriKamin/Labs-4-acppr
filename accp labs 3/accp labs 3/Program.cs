using System;
using System.Text;

namespace DualLinearProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            PrintOriginalProblem();
            SolvePrimal();
            PrintDualProblem();
            SolveDual();
            CheckResult();

            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }

        static void PrintOriginalProblem()
        {
            Console.WriteLine("1. Постановка прямої задачі:");
            Console.WriteLine("Z = 2*x1 - x2 -> max\n");

            Console.WriteLine("при обмеженнях:");
            Console.WriteLine("3*x1 - x2 = 1");
            Console.WriteLine("x1 - 2*x2 <= -3");
            Console.WriteLine("x1 >= 0, x2 >= 0\n");
        }

        static void SolvePrimal()
        {
            Console.WriteLine("2. Розв'язання прямої задачі Z:\n");

            Console.WriteLine("З першого обмеження:");
            Console.WriteLine("3*x1 - x2 = 1");
            Console.WriteLine("x2 = 3*x1 - 1\n");

            Console.WriteLine("Підставимо x2 у друге обмеження:");
            Console.WriteLine("x1 - 2*x2 <= -3");
            Console.WriteLine("x1 - 2*(3*x1 - 1) <= -3");
            Console.WriteLine("x1 - 6*x1 + 2 <= -3");
            Console.WriteLine("-5*x1 <= -5");
            Console.WriteLine("x1 >= 1\n");

            Console.WriteLine("Цільова функція:");
            Console.WriteLine("Z = 2*x1 - x2");
            Console.WriteLine("Z = 2*x1 - (3*x1 - 1)");
            Console.WriteLine("Z = -x1 + 1\n");

            Console.WriteLine("Оскільки x1 >= 1, максимум Z буде при мінімальному x1.");
            Console.WriteLine("Отже:");
            Console.WriteLine("x1 = 1");
            Console.WriteLine("x2 = 3*1 - 1 = 2\n");

            double x1 = 1;
            double x2 = 2;
            double z = 2 * x1 - x2;

            Console.WriteLine("Оптимальний розв'язок прямої задачі:");
            Console.WriteLine($"X = ({x1:F2}; {x2:F2})");
            Console.WriteLine($"Max(Z) = {z:F2}\n");
        }

        static void PrintDualProblem()
        {
            Console.WriteLine("3. Побудова двоїстої задачі:\n");

            Console.WriteLine("Пряма задача:");
            Console.WriteLine("Z = 2*x1 - x2 -> max");
            Console.WriteLine("3*x1 - x2 = 1");
            Console.WriteLine("x1 - 2*x2 <= -3\n");

            Console.WriteLine("Матриця коефіцієнтів:");
            Console.WriteLine("A = [  3  -1 ]");
            Console.WriteLine("    [  1  -2 ]\n");

            Console.WriteLine("Вектор правих частин:");
            Console.WriteLine("b = (1; -3)\n");

            Console.WriteLine("Вектор коефіцієнтів цільової функції:");
            Console.WriteLine("c = (2; -1)\n");

            Console.WriteLine("Двоїста задача має вигляд:");
            Console.WriteLine("W = u1 - 3*u2 -> min\n");

            Console.WriteLine("при обмеженнях:");
            Console.WriteLine("3*u1 + u2 >= 2");
            Console.WriteLine("-u1 - 2*u2 >= -1\n");

            Console.WriteLine("Умови на змінні:");
            Console.WriteLine("u1 - довільна за знаком, бо перше обмеження прямої задачі є рівністю;");
            Console.WriteLine("u2 >= 0, бо друге обмеження прямої задачі має знак <=.\n");
        }

        static void SolveDual()
        {
            Console.WriteLine("4. Розв'язання двоїстої задачі W:\n");

            Console.WriteLine("Оскільки в оптимальному розв'язку прямої задачі:");
            Console.WriteLine("x1 > 0, x2 > 0,");
            Console.WriteLine("то обмеження двоїстої задачі виконуються як рівності.\n");

            Console.WriteLine("Отримуємо систему:");
            Console.WriteLine("3*u1 + u2 = 2");
            Console.WriteLine("-u1 - 2*u2 = -1\n");

            Console.WriteLine("Перетворимо друге рівняння:");
            Console.WriteLine("u1 + 2*u2 = 1\n");

            Console.WriteLine("Розв'язуємо систему:");
            Console.WriteLine("3*u1 + u2 = 2");
            Console.WriteLine("u1 + 2*u2 = 1\n");

            double[,] A =
            {
                { 3, 1 },
                { 1, 2 }
            };

            double[] B = { 2, 1 };

            double[] result = Solve2x2(A, B);

            double u1 = result[0];
            double u2 = result[1];
            double w = u1 - 3 * u2;

            Console.WriteLine($"u1 = {u1:F2}");
            Console.WriteLine($"u2 = {u2:F2}\n");

            Console.WriteLine("Оптимальний розв'язок двоїстої задачі:");
            Console.WriteLine($"U = ({u1:F2}; {u2:F2})");
            Console.WriteLine($"Min(W) = {w:F2}\n");
        }

        static double[] Solve2x2(double[,] A, double[] B)
        {
            double a11 = A[0, 0];
            double a12 = A[0, 1];
            double a21 = A[1, 0];
            double a22 = A[1, 1];

            double b1 = B[0];
            double b2 = B[1];

            double det = a11 * a22 - a12 * a21;

            if (Math.Abs(det) < 1e-10)
                throw new Exception("Система не має єдиного розв'язку.");

            double detX = b1 * a22 - a12 * b2;
            double detY = a11 * b2 - b1 * a21;

            double x = detX / det;
            double y = detY / det;

            return new double[] { x, y };
        }

        static void CheckResult()
        {
            Console.WriteLine("5. Перевірка результатів:\n");

            double x1 = 1;
            double x2 = 2;
            double z = 2 * x1 - x2;

            double u1 = 0.6;
            double u2 = 0.2;
            double w = u1 - 3 * u2;

            Console.WriteLine("Для прямої задачі:");
            Console.WriteLine($"Z = 2*{x1:F2} - {x2:F2} = {z:F2}\n");

            Console.WriteLine("Для двоїстої задачі:");
            Console.WriteLine($"W = {u1:F2} - 3*{u2:F2} = {w:F2}\n");

            Console.WriteLine("Отже:");
            Console.WriteLine($"Max(Z) = {z:F2}");
            Console.WriteLine($"Min(W) = {w:F2}\n");

            if (Math.Abs(z - w) < 1e-10)
                Console.WriteLine("Max(Z) = Min(W), тому розв'язки знайдено правильно.");
            else
                Console.WriteLine("Результати не збігаються, потрібно перевірити обчислення.");
        }
    }
}
