/* Copyright 2026 Louis Thériault
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */





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
