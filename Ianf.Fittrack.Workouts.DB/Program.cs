using System;
using System.Linq;
using DbUp;
using System.Reflection;

namespace Ianf.Fittrack.Workouts.DB
{
    class Program
    {
        static int Main(string[] args)
        { 
            var connectionString =
                args.FirstOrDefault()
                ?? "Server=192.168.1.73; Database=Fittrack; User Id=SA; Password=31Freeble$";

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}
