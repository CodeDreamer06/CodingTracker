using System.Globalization;

namespace CodingTracker;

class Helpers
{
    public static string AddFormatErrorMessage = "Add commands should be in this format: 'add [duration]'. \nFor example: 'add 5:30' means 5 hours and 30 minutes.";

    public static string RemoveFormatErrorMessage = "Remove commands should be in this format: 'remove [id]'. \nFor example: 'remove 3' deletes the third log.";

    public static string UpdateFormatErrorMessage = "Update commands should be in this format: 'update [log id] [hours]''. \nFor example: 'update 3 8' changes the number of hours in row 3.";

    public static TimeSpan? SplitTime(string command, string keyword, string errorMessage)
    {
        try
        {
            var time = command.RemoveKeyword(keyword, errorMessage);
            if (string.IsNullOrEmpty(time)) return null;

            if (time.Contains(':'))
            {
                var splitTime = time.Split(":");
                return new TimeSpan(Convert.ToInt32(splitTime[0]), Convert.ToInt32(splitTime[1]), 0);
            }

            else return new TimeSpan(Convert.ToInt32(time), 0, 0);
        }

        catch (FormatException)
        {
            Console.WriteLine(errorMessage);
            return null;
        }
    }
}

public static class Extensions
{
    public static int GetNumber(this string str, string keyword, string errorMessage = "")
    {
        var command = str.RemoveKeyword(keyword, errorMessage);
        _ = int.TryParse(
              Convert.ToString(command),
              NumberStyles.Any,
              NumberFormatInfo.InvariantInfo,
              out int number);

        return number;
    }

    public static string? RemoveKeyword(this string str, string keyword, string errorMessage = "")
    {
        try
        {
            return str.Replace(keyword + " ", "");
        }

        catch (FormatException)
        {
            if (!string.IsNullOrEmpty(errorMessage))
                Console.WriteLine(errorMessage);

            return string.Empty;
        }
    }
}
