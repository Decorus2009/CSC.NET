using System;
using System.Text;

namespace hw2
{
    /*
    Результат тестирования:
        Статус
        Опциональное сообщение (если был аргумент Ignore, то запишется текст
        в описании аргумента Ignore. Если было исключение, неважно, expected или нет, то запишется его message)
        Опциональное перехваченное expected исключение
        Опциональное время работы метода (если Status.Failed или Status.Ignored, то null)
    */
    public class TestResult
    {
        /* Я бы вместо null использовал тут Optional<>, но в C# такого нет? */
        public TestResult(Status status, string message = null, Type caughtException = null, string elapsedTime = null)
        {
            Status = status;
            Message = message;
            CaughtException = caughtException;
            ElapsedTime = elapsedTime;
        }

        public Status Status { get; }
        public string Message { get; }
        public Type CaughtException { get; }
        public string ElapsedTime { get; }

        public override string ToString()
        {
            var result = new StringBuilder();
            switch (Status)
            {
                case Status.Passed:
                    result.Append($"PASSED\nrun time: {ElapsedTime}\n");
                    break;
                case Status.Failed:
                    result.Append($"FAILED\nException message: {Message}\n");
                    break;
                case Status.Ignored:
                    result.Append($"IGNORED\nMessage: \"{Message}\"\n");
                    break;
            }
            return result.ToString();
        }
    }

    public enum Status
    {
        Passed,
        Failed,
        Ignored
    }
}