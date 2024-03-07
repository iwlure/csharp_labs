namespace Lab1;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Вариант 11. Автор: Суслова Евгения");
        RunFirstTask();
        RunSecondTask();
    }

    private static void PrintVector(IEnumerable<double> vector)
    {
        Console.WriteLine($"[{string.Join(" ", vector)}]");
    }

    private static void PrintMatrix(int[,] matrix)
    {
        Console.Write("\t");
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write("{0,5}", matrix[i, j]);
            }

            Console.WriteLine();
            Console.Write("\t");
        }

        Console.WriteLine();
    }

    private static void PrintMatrixRow(int[,] matrix, int row)
    {
        Console.Write("\t");
        for (var j = 0; j < matrix.GetLength(1); j++)
        {
            Console.Write("{0,5}", matrix[row, j]);
        }

        Console.WriteLine();
    }

    private static void RunFirstTask()
    {
        Console.WriteLine("Первая часть:");
        Console.Write("\tВведите размер массива: ");
        var size = int.Parse(Console.ReadLine() ?? "0");
        if (size <= 0)
        {
            Console.WriteLine("\tРазмер не может быть отрицательным или равным нулю!");
            throw new ArgumentException(nameof(size));
        }

        var task1 = new Task1(size);

        Console.WriteLine("\tИсходный массив:");
        Console.Write("\t");
        PrintVector(task1.Doubles);

        var maxValueIndex = task1.GetMaxAbsValueIndex();
        Console.WriteLine($"\tНомер максимального по модулю элемента массива: {maxValueIndex}");
        Console.WriteLine($"\tЗначение {maxValueIndex} элемента:");
        Console.WriteLine($"\t\t- Исходное: {task1.Doubles[maxValueIndex]}");
        Console.WriteLine($"\t\t- По модулю: {Math.Abs(task1.Doubles[maxValueIndex])}");
        Console.WriteLine($"\tСумма элементов после первого положительного: {task1.GetSumAfterFirstPositive()}");
        Console.Write("\tВведите значение 'a': ");
        var a = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("\tВведите значение 'b': ");
        var b = int.Parse(Console.ReadLine() ?? "0");
        if (a > b)
        {
            Console.WriteLine("\tГраница 'a' не может быть больше 'b'! Меняем местами...");
            (a, b) = (b, a);
        }

        Console.WriteLine("\tПереносим значения попавшие в интервал [a; b] в начало массива...");
        Console.WriteLine("\tПреобразованный массив:");
        Console.Write("\t");
        PrintVector(task1.MoveValuesFromAbIntervalToTheBeginning(a, b));
    }

    private static void RunSecondTask()
    {
        Console.WriteLine("Вторая часть:");
        Console.Write("\tВведите количество рядов матрицы: ");
        var rows = int.Parse(Console.ReadLine() ?? "0");
        if (rows <= 0)
        {
            Console.WriteLine("\tРазмер не может быть отрицательным или равным нулю!");
            throw new ArgumentException(nameof(rows));
        }

        Console.Write("\tВведите количество столбцов матрицы: ");
        var cols = int.Parse(Console.ReadLine() ?? "0");
        if (cols <= 0)
        {
            Console.WriteLine("\tРазмер не может быть отрицательным или равным нулю!");
            throw new ArgumentException(nameof(cols));
        }

        var task2 = new Task2(rows, cols);

        Console.WriteLine("\tИсходная матрица:");
        PrintMatrix(task2.Matrix);

        task2.SetRandomRowOrColumnWithZeroes();

        Console.WriteLine("\tМатрица с нулевой строкой и столбцом:");
        PrintMatrix(task2.Matrix);

        task2.RemoveZeroes();

        Console.WriteLine("\tУплотненная матрица:");
        PrintMatrix(task2.Matrix);

        try
        {
             var positiveRowIndex = task2.FindIndexOfFirstRowContainingPositiveElement();
             Console.WriteLine($"\tНомер первой строки, содержащей хотя бы один положительный элемент: {positiveRowIndex}");
             PrintMatrixRow(task2.Matrix, positiveRowIndex);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
