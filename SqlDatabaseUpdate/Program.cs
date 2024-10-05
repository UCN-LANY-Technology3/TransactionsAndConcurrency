using DbUp;
using System.Reflection;

namespace SqlDatabaseUpdate
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Cafe Sanchez Database Update");

            var connectionString = args.FirstOrDefault() ?? "Server=192.168.56.101; Database=CafeSanchez; User Id=sa; Password=P@$$w0rd; TrustServerCertificate=True";

            EnsureDatabase.For.SqlDatabase(connectionString);

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
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}
