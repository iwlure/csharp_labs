namespace Lab2;

public static class Calculator
{
    private static readonly List<double> Results = new();

    public static void Main(string[] args)
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("\twhen a first symbol on line is '>' – enter operand (number)");
        Console.WriteLine("\twhen a first symbol on line is '@' – enter operation");
        Console.WriteLine("\toperation is one of '+', '-', '/', '*' or");
        Console.WriteLine("\t'#' followed with number of evaluation step");
        Console.WriteLine("\t'q' to exit");
        Console.WriteLine("Example:");
        Console.WriteLine("\t>1");
        Console.WriteLine("\t[#1] = 1");
        Console.WriteLine("\t>2");
        Console.WriteLine("\t>[#2] = 2");
        Console.WriteLine("\t@+");
        Console.WriteLine("\t[#1] = 3");
        Console.WriteLine("\t");

        while (true)
        {
            var input = Console.ReadLine() ?? throw new InvalidOperationException();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            var firstChar = input[0];

            if (firstChar == 'q')
            {
                break;
            }

            switch (firstChar)
            {
                case '>':
                    HandleOperand(input[1..]);
                    break;
                case '@':
                    HandleOperation(input[1..]);
                    break;
                default:
                    Console.WriteLine("Invalid input. Please start with '>' or '@'.");
                    break;
            }
        }
    }

    private static void HandleOperand(string operandStr)
    {
        if (double.TryParse(operandStr, out var operand))
        {
            Results.Add(operand);
            var step = Results.Count;
            Console.WriteLine($"[#{step}] = {operand}");
        }
        else
        {
            Console.WriteLine("Invalid operand. Please enter a valid number.");
        }
    }

    private static void HandleOperation(string operationStr)
    {
        if (operationStr.StartsWith("#"))
        {
            HandleEvaluationStep(operationStr[1..]);
        }
        else if (operationStr.Length == 1 && IsBasicOperation(operationStr[0]))
        {
            HandleBasicOperation(operationStr[0]);
        }
        else
        {
            Console.WriteLine("Invalid operation. Please enter a valid operation.");
        }
    }

    private static void HandleBasicOperation(char operation)
    {
        if (Results.Count < 2)
        {
            Console.WriteLine("Not enough operands for the operation.");
            return;
        }

        var operand2 = Results[^1];
        var operand1 = Results[^2];

        var result = 0d;

        switch (operation)
        {
            case '+':
                result = operand1 + operand2;
                break;
            case '-':
                result = operand1 - operand2;
                break;
            case '*':
                result = operand1 * operand2;
                break;
            case '/':
                if (operand2 != 0)
                {
                    result = operand1 / operand2;
                }
                else
                {
                    Console.WriteLine("Cannot divide by zero.");
                    return;
                }

                break;
        }
        Results.Add(result);

        var step = Results.Count;
        Console.WriteLine($"[#{step}] = {result}");
    }

    private static void HandleEvaluationStep(string stepStr)
    {
        if (int.TryParse(stepStr, out var step) && step > 0 && step <= Results.Count)
        {
            var result = Results[step - 1];
            Results.Add(result);
            Console.WriteLine($"[#{Results.Count}] = {result}");
        }
        else
        {
            Console.WriteLine("Invalid evaluation step. Please enter a valid step number.");
        }
    }

    private static bool IsBasicOperation(char operation)
    {
        return operation is '+' or '-' or '*' or '/';
    }
}