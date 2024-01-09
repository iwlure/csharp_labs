namespace Lab4;

public static class Calculator
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("\twhen a first symbol on line is '>' – enter operand (number)");
        Console.WriteLine("\twhen a first symbol on line is '@' – enter operation");
        Console.WriteLine("\toperation is one of '+', '-', '/', '*' or");
        Console.WriteLine("\t'#' followed with number of evaluation step");
        Console.WriteLine("\t'load' to load history from source");
        Console.WriteLine("\t'save' to save history to source");
        Console.WriteLine("\t'print' to print history to console");
        Console.WriteLine("\t'q' to exit");
        Console.WriteLine("Example:");
        Console.WriteLine("\t>1");
        Console.WriteLine("\t[#1] = 1");
        Console.WriteLine("\t>2");
        Console.WriteLine("\t>[#2] = 2");
        Console.WriteLine("\t@+");
        Console.WriteLine("\t[#3] = 3");
        Console.WriteLine("\t");

        while (true)
        {
            var input = Console.ReadLine() ?? throw new InvalidOperationException();

            switch (input)
            {
                case "":
                case null:
                    continue;
                case "save":
                    HistoryManager.SaveHistory();
                    break;
                case "load":
                    HistoryManager.LoadHistory();
                    break;
                case "print":
                    Console.WriteLine($"[{string.Join(", ", HistoryManager.UserHistory.Values)}]");
                    break;
                default:
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

                    break;
            }
        }
    }

    private static void HandleOperand(string operandStr)
    {
        if (double.TryParse(operandStr, out var operand))
        {
            HistoryManager.UserHistory.Values.Add(operand);
            var step = HistoryManager.UserHistory.Values.Count;
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
        if (HistoryManager.UserHistory.Values.Count < 2)
        {
            Console.WriteLine("Not enough operands for the operation.");
            return;
        }

        var operand2 = HistoryManager.UserHistory.Values[^1];
        var operand1 = HistoryManager.UserHistory.Values[^2];

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

        HistoryManager.UserHistory.Values.Add(result);

        var step = HistoryManager.UserHistory.Values.Count;
        Console.WriteLine($"[#{step}] = {result}");
    }

    private static void HandleEvaluationStep(string stepStr)
    {
        if (int.TryParse(stepStr, out var step) && step > 0 && step <= HistoryManager.UserHistory.Values.Count)
        {
            var result = HistoryManager.UserHistory.Values[step - 1];
            HistoryManager.UserHistory.Values.Add(result);
            Console.WriteLine($"[#{HistoryManager.UserHistory.Values.Count}] = {result}");
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