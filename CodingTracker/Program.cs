namespace CodingTracker;

class Program
{
    static void Main(string[] args)
    {
        const string help = @"
# Welcome to Code Time!
  It's a simple code time manager to measure your progress!
* exit or 0: stop the program
* show: display logs
* add [hours]: insert data into the database
* update [id] [hours]: change existing data
* remove [id]: delete a log
";
        SqlAccess.CreateTable();
        Console.WriteLine(help);

        while (true)
        {
            var rawCommand = Console.ReadLine()!;
            var command = rawCommand.ToLower().Trim();

            if (command is "exit" or "0") break;

            else if (command is "help") Console.WriteLine(help);

            else if (command.StartsWith("add"))
            {
                var duration = Helpers.SplitTime(rawCommand, "add", Helpers.AddFormatErrorMessage);
                if (duration is null) continue;
                SqlAccess.AddLog(duration.Value);
            }

            else if (command.StartsWith("remove"))
            {
                if (rawCommand == "remove")
                {
                    SqlAccess.RemoveLastLog();
                    continue;
                }

                int id = rawCommand.GetNumber("remove", Helpers.RemoveFormatErrorMessage);

                if (id == 0) Console.WriteLine("Please enter a valid number");
                SqlAccess.RemoveLog(id);
            }

            else if (command == "show") SqlAccess.GetLogs();

            else if (command.StartsWith("update"))
            {
                //TODO: Update log doesn't work

                var duration = Helpers.SplitTime("", "update", Helpers.UpdateFormatErrorMessage);
                var id = command.RemoveKeyword("update", Helpers.UpdateFormatErrorMessage)!.GetNumber("");

                SqlAccess.UpdateLog(id, duration!.Value);
            }

            else if (string.IsNullOrWhiteSpace(command)) continue;
            else Console.WriteLine("Not a command. Use 'help' if required.");
        }
    }
}
