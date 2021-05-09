using System;

namespace NoAntivirus
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var guid = new Guid("B");
            Console.WriteLine($"Welcome to NoAntivirus, GUID:{guid}");
            if (args.Length <= 0)
            {
                switch (args[0].ToLower())
                {

                }
            }
            else
            {
                Console.WriteLine("Nothing to do.");
            }
        }
    }
}
