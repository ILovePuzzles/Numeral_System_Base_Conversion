namespace NumeralSystemBaseConversion
{
    internal class Program
    {
        static void Main()
        {
            DisplayMainMenu();
        }



        public const string initialMenuMsg = "***** Number System Base Converter, for any integer base from 2 to 36 *****\n\n" +
        "Note: if you want to escape any input menu, simply press Esc.\n\n";
        public const string finalMenuMsg = "\n\nDo you want to continue? Press Esc to exit, Backspace to clear the console and" +
            " continue, or press any other key to continue.\n";

        /// <summary>
        /// Displays the main menu for the number conversion class
        /// </summary>
        public static void DisplayMainMenu()
        {
            // Stores whether or not the user wants to exit the main menu
            bool exitBool = false;



            Console.WriteLine(initialMenuMsg);

            do
            {
                NumberConversion.ConvertNumber();



                Console.WriteLine(finalMenuMsg);

                // Command menu: The Enter command repeats the method's loop and the Esc command breaks the method's loop
                ConsoleKeyInfo keyValue = Console.ReadKey(true);

                if (keyValue.Key == ConsoleKey.Escape)
                { exitBool = true; }

                else if (keyValue.Key == ConsoleKey.Backspace)
                {
                    // Clears the console
                    Console.Write("\u001bc");
                    Console.WriteLine(initialMenuMsg);
                }

                Console.Write("\n");
            }
            while (!exitBool);
        }
    }
}
