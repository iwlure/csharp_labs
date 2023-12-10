namespace Lab1;

public class Task1
{
    public double[] Doubles { get; }

    public Task1(int size)
    {
        if (size <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size));
        }

        var random = new Random();

        Doubles = new Double[size];
        for (var i = 0; i < Doubles.Length; i++)
        {
            Doubles[i] = random.NextDouble() * 100 - 50;
        }
    }

    // номер максимального по модулю элемента массива
    public int GetMaxAbsValueIndex()
    {
        var indexOfMaxAbsValue = 0;
        for (var i = 1; i < Doubles.Length; i++)
        {
            if (Math.Abs(Doubles[i]) > Math.Abs(Doubles[indexOfMaxAbsValue]))
            {
                indexOfMaxAbsValue = i;
            }
        }

        return indexOfMaxAbsValue;
    }

    // сумму элементов массива, расположенных после первого положительного элемента
    public double GetSumAfterFirstPositive()
    {
        var sum = 0d;
        var firstPositiveValueIndex = -1;
        for (var i = 0; i < Doubles.Length; i++)
        {
            if (!(Doubles[i] >= 0)) continue;
            firstPositiveValueIndex = i;
            break;
        }

        if (firstPositiveValueIndex == -1) return sum;

        for (var i = firstPositiveValueIndex; i < Doubles.Length; i++)
        {
            sum += Doubles[i];
        }

        return sum;
    }

    // Преобразовать массив таким образом, чтобы сначала располагались все элементы,
    // целая часть которых лежит в интервале [а, b], а потом — все остальные.
    public IEnumerable<double> MoveValuesFromAbIntervalToTheBeginning(int a, int b)
    {
        var valuesInInterval = new List<double>();
        var otherValues = new List<double>();

        foreach (var value in Doubles)
        {
            if (Math.Truncate(value) >= a && Math.Truncate(value) <= b)
            {
                valuesInInterval.Add(value);
            }
            else
            {
                otherValues.Add(value);
            }
        }

        valuesInInterval.AddRange(otherValues);
        return valuesInInterval.ToArray();
    }
}