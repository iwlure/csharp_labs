namespace Lab1;

public class Task2
{
    public int[,] Matrix { get; set; }

    public Task2(int rows, int cols)
    {
        if (rows <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rows));
        }

        if (cols <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cols));
        }

        var random = new Random();

        Matrix = new int[rows, cols];
        for (var i = 0; i < Matrix.GetLength(0); i++)
        {
            for (var j = 0; j < Matrix.GetLength(1); j++)
            {
                Matrix[i, j] = random.Next(-100, 4);
            }
        }
    }

    // заменить нулями случайный ряд или столбец
    public void SetRandomRowOrColumnWithZeroes()
    {
        var random = new Random();
        var rowIndex = random.Next(Matrix.GetLength(0));
        var colIndex = random.Next(Matrix.GetLength(1));


        for (var i = 0; i < Matrix.GetLength(0); i++)
        {
            Matrix[rowIndex, i] = 0;
        }

        for (var i = 0; i < Matrix.GetLength(0); i++)
        {
            Matrix[i, colIndex] = 0;
        }
    }

    // Уплотнить матрицу (удалить ряды и/или столбцы состоящие из нулей)
    public void RemoveZeroes()
    {
        RemoveRows();
        RemoveColumns();
    }

    // Найти номер первой из строк, содержащих хотя бы один положительный элемент
    public int FindIndexOfFirstRowContainingPositiveElement()
    {
        for (var i = 0; i < Matrix.GetLength(0); i++)
        {
            for (var j = 0; j < Matrix.GetLength(1); j++)
            {
                if (Matrix[i, j] > 0)
                {
                    return i;
                }
            }
        }

        throw new Exception("No positive elements found in matrix");
    }

    private void RemoveRows()
    {
        var rowIndexesToDelete = GetRowIndexesToDelete();
        var matrixRows = Matrix.GetLength(0);
        var matrixColumns = Matrix.GetLength(1);
        var result = new int[matrixRows - rowIndexesToDelete.Count, matrixColumns];

        var newRowIndex = 0;
        for (var i = 0; i < matrixRows; i++)
        {
            if (rowIndexesToDelete.Contains(i)) continue;
            for (var j = 0; j < matrixColumns; j++)
            {
                result[newRowIndex, j] = Matrix[i, j];
            }

            newRowIndex++;
        }

        Matrix = result;
    }

    private void RemoveColumns()
    {
        var columnIndexesToDelete = GetColumnIndexesToDelete();
        var matrixRows = Matrix.GetLength(0);
        var matrixColumns = Matrix.GetLength(1);
        var result = new int[matrixRows, matrixColumns - columnIndexesToDelete.Count];

        for (var i = 0; i < matrixRows; i++)
        {
            var newColumnIndex = 0;
            for (var j = 0; j < matrixColumns; j++)
            {
                if (columnIndexesToDelete.Contains(j)) continue;
                result[i, newColumnIndex] = Matrix[i, j];
                newColumnIndex++;
            }
        }

        Matrix = result;
    }

    private List<int> GetRowIndexesToDelete()
    {
        var indexesToDelete = new List<int>();
        for (var i = 0; i < Matrix.GetLength(0); i++)
        {
            var onlyZeroes = true;
            for (var j = 0; j < Matrix.GetLength(1); j++)
            {
                if (Matrix[i, j] == 0) continue;
                onlyZeroes = false;
                break;
            }

            if (onlyZeroes) indexesToDelete.Add(i);
        }

        return indexesToDelete;
    }

    private List<int> GetColumnIndexesToDelete()
    {
        var indexesToDelete = new List<int>();
        for (var i = 0; i < Matrix.GetLength(1); i++)
        {
            var onlyZeroes = true;
            for (var j = 0; j < Matrix.GetLength(0); j++)
            {
                if (Matrix[j, i] == 0) continue;
                onlyZeroes = false;
                break;
            }

            if (onlyZeroes) indexesToDelete.Add(i);
        }

        return indexesToDelete;
    }
}