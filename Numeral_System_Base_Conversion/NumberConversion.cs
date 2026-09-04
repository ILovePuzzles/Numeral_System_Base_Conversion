using System.Numerics;
using System.Text;

namespace NumeralSystemBaseConversion
{
    public static class NumberConversion
    {
        public const string initialBaseMsg = "\nEnter the initial base to convert from. The base must be an integer from 2 to 36.";
        public const string finalBaseMsg = "\nEnter the final base to convert to. The base must be an integer from 2 to 36.";
        public const int digitsLowerBound = 6;
        public const int digitsUpperBound = 8192;

        /// <summary>
        /// Contains all the available digits for number system base conversion from 2 to 36.
        /// </summary>
        public static char[] digitsLibrary = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
                'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        /// <summary>
        /// Coordinates the number system base conversion.
        /// </summary>
        public static void ConvertNumber()
        {
            // Stores whether or not the user wants to escape an input menu
            bool escapeMenu;
            // Stores whether or not the number to convert contains valid digits
            bool validDigits;

            do
            {
                // Resets the boolean value to false
                validDigits = false;

                // Validates the initial base value (the base to convert from)
                int initialBase = ValidateBase(initialBaseMsg, out escapeMenu);
                if (escapeMenu) { break; }

                // Validates the final base value (the base to convert to)
                int finalBase = ValidateBase(finalBaseMsg, out escapeMenu);
                if (escapeMenu) { break; }

                // Validates the desired precision in binary digits
                int digitsLength = ValidatePrecision(out escapeMenu);
                if (escapeMenu) { break; }

                do
                {
                    // The precision limit is derived from the maximal capacity of 256 bits, converted
                    // using the value of the initial base
                    int precisionLimit = (int)(digitsLength / Math.Log2(initialBase));
                    // The cutoff limit is computed to match the precision limit, converted
                    // using the value of the final base
                    int cutoffLimit = (int)(digitsLength / Math.Log2(finalBase));

                    // Stores whether or not the value to convert is negative
                    bool negativeSign;
                    // Stores whether the separator used between the integer and fractional parts of the value
                    // is a dot or a comma
                    bool commaSeparator;
                    // Stores whether or not a blank space is used to separate groups of 3 digits
                    bool spaceSeparator;

                    // Validates the integer part, the aperiodic part of the fractional part and the periodic part of the fractional part
                    // from the value to convert
                    string[] values = ValidateInput(precisionLimit, out negativeSign, out commaSeparator, out spaceSeparator, out escapeMenu);
                    if (escapeMenu) { break; }

                    // Allows to choose whether or not to separate every group of 3 digits by a blank space separator
                    spaceSeparator = ChooseSpaceSeparator(out escapeMenu);
                    if (escapeMenu) { break; }




                    // Stores the integer value to convert
                    BigInteger integer = 0;
                    // Stores the numerator value to convert
                    BigInteger numerator = 0;
                    // Stores the denominator value to convert
                    BigInteger denominator = 1;

                    // Evaluates if the digits of the input string are valid or not, and computes the values of the integer and fractional part
                    validDigits = ConvertStringsToValues(values, initialBase, ref integer, ref numerator, ref denominator);

                    // If the values' digit content is valid
                    if (validDigits)
                    {
                        // Stores the period length
                        int periodLength = 0;
                        // Stores whether or not the converted value is exact
                        bool exactValue = true;
                        // Stores the output string
                        string outputValue = "";

                        if (numerator != 0)
                        {
                            // Stores where the periodic value begins
                            int cycleStartIndex = 0;

                            // Computes the individual digit value(s) from the fractional part
                            List<BigInteger> digitValues = ConvertFractionToDigits(numerator, denominator, finalBase, cutoffLimit,
                                    ref cycleStartIndex, ref periodLength, ref exactValue);

                            // Converts the individual digit values from the fraction to their final string representation
                            outputValue = ConvertDigitsToString(digitValues, finalBase, cycleStartIndex, periodLength,
                                exactValue, spaceSeparator, ref integer);
                        }

                        // Converts the integer part to its final string representation, and combines it with the fractional part
                        outputValue = ConvertIntegerToString(integer, finalBase, outputValue, negativeSign, commaSeparator, spaceSeparator);

                        // Prints the result of the number conversion
                        PrintOutput(outputValue, periodLength, exactValue);

                        break;
                    }
                }
                while (true);

                // If the user wants to exit the loop, or if the number conversion is successful, breaks the loop
                if (escapeMenu || validDigits) { break; }
            }
            while (true);
        }

        /// <summary>
        /// Validates the input base values.
        /// </summary>
        /// <param name="message">The message related to the initial base or the final base.</param>
        /// <param name="escapeMenu">The escape or not boolean.</param>
        /// <returns>Returns the validated base value.</returns>
        public static int ValidateBase(string message, out bool escapeMenu)
        {
            escapeMenu = false;
            int validBase = 0;



            do
            {
                string inputBase = ProcessKeyboardInputs(message, 2, out escapeMenu);

                if (escapeMenu)
                { break; }

                else if (!int.TryParse(inputBase, out validBase))
                { Console.WriteLine("\nError: the input value is not an integer.\n"); }

                else
                {
                    if (validBase >= 2 && validBase <= 36)
                    { break; }

                    else
                    {
                        Console.WriteLine("\nError: the input value is not a" +
                            " value from 2 to 36.\n");
                    }
                }
            }
            while (true);

            return validBase;
        }

        /// <summary>
        /// Validates the precision limit in terms of binary digits.
        /// </summary>
        /// <param name="escapeMenu">The escape or not boolean.</param>
        /// <returns>Returns the validated precision limit.</returns>
        public static int ValidatePrecision(out bool escapeMenu)
        {
            escapeMenu = false;
            int digitsLength = 0;

            string message = $"\nEnter the desired precision limit, in terms of binary digits. The values can" +
                $" range from {digitsLowerBound} digits up to {digitsUpperBound}.";



            do
            {
                string digitsLengthString = ProcessKeyboardInputs(message, 4, out escapeMenu);

                if (escapeMenu)
                { break; }

                else if (!int.TryParse(digitsLengthString, out digitsLength))
                { Console.WriteLine("\nError: the input value is not an integer.\n"); }

                else
                {
                    if (digitsLength >= digitsLowerBound && digitsLength <= digitsUpperBound)
                    { break; }

                    else
                    {
                        Console.WriteLine("\nError: the input value is not a" +
                            $" value from {digitsLowerBound} to {digitsUpperBound}.\n");
                    }
                }
            }
            while (true);

            return digitsLength;
        }

        /// <summary>
        /// Validates the number to convert.
        /// </summary>
        /// <param name="precisionLimit">The precision limit value.</param>
        /// <param name="negativeSign">The negative or positive sign boolean.</param>
        /// <param name="commaSeparator">The comma or dot separator boolean.</param>
        /// <param name="spaceSeparator">The blank space separator boolean.</param>
        /// <param name="escapeMenu">The escape or not boolean.</param>
        /// <returns>Returns the validated strings in a string array.</returns>
        public static string[] ValidateInput(int precisionLimit, out bool negativeSign, out bool commaSeparator,
            out bool spaceSeparator, out bool escapeMenu)
        {
            negativeSign = false;
            commaSeparator = false;
            spaceSeparator = false;
            escapeMenu = false;

            string[] values = ["", "", ""];
            string message = $"\nEnter the number to convert. The integer part and the fractional part of the input value may contain up to" +
                $" {precisionLimit} digits each. You can tag a value as being periodic in the fractional part; to do so, you must contain the" +
                " periodic value between one opening parenthesis and one closing parenthesis. The parentheses will not count in the" +
                " digit limit. Also, for an integer part and/or a fractional part with more than 3 digits, the blank space character can be" +
                " used to separate groups of 3 digits, in order to make the value easier to read.";

            do
            {
                int maximalSpaceCount = precisionLimit / 3;

                // The maximal string length is computed as follows:
                // If the number has a sign, it counts as 1 character;
                // The maximal number of digits accepted for the integer part is the precision limit;
                // The separator can be either a dot or a comma, which counts as 1 character;
                // The maximal number of digits accepted for the fractional part is precision limit;
                // There can be 2 parentheses, which count as 2 characters.
                // 1 + precisionLimit + 1 + precisionLimit + 2 + maximalSpaceCount = 2 * precisionLimit + 4 + maximalSpaceCount
                string inputValue = ProcessKeyboardInputs(message, 2 * precisionLimit + 4 + maximalSpaceCount, out escapeMenu);



                // If the user wants to espace the menu, breaks the loop
                if (escapeMenu)
                { break; }



                commaSeparator = inputValue.Contains(",");

                // Validates the separator content of the input value
                if (!ValidateSeparator(inputValue, commaSeparator))
                { continue; }



                // If the value starts with a minus sign, stores the sign of the value in the
                // negativeSign boolean, then ignores the character
                if (inputValue.StartsWith('-'))
                {
                    inputValue = inputValue.Substring(1);
                    negativeSign = true;
                }

                // If the value starts with a plus sign, ignores the character
                else if (inputValue.StartsWith("+"))
                { inputValue = inputValue.Substring(1); }

                // Formats the characters from the string to uppercase to match the digit library characters
                inputValue = inputValue.ToUpperInvariant();



                // Splits the input value string using the separator as the delimiting character
                var splitStrings = (commaSeparator ? inputValue.Split(',') : inputValue.Split('.'));

                // Validates the content of the integer part of the input value
                if (!ValidateIntegerPart(splitStrings[0]))
                { continue; }

                values[0] = splitStrings[0];



                // Evaluates if the input value string contains a fractional part or not;
                // If the input value string contains a fractional part, validates and
                // processes the fractional part value
                if (splitStrings.Length == 2)
                {
                    // Validates the content of the fractional part of the input value
                    if (!ValidateFractionalPart(ref splitStrings[1]))
                    { continue; }

                    var splitFraction = splitStrings[1].Split(':');

                    values[1] = (splitFraction[0] != "" ? splitFraction[0] : "");
                    values[2] = (splitFraction.Length == 2 ? splitFraction[1] : "");
                }



                bool isValid = true;

                for (int i = 0; i < 3; i++)
                {
                    if (values[i].Contains(' '))
                    {
                        // If the value contains a blank space, yet as less than 5 characters, the value is invalid
                        // Explanation: a value must contain at least 4 digits to be separated by a space, plus the space itself
                        // = 5 characters
                        // If the value starts or ends with a blank space, it is also invalid
                        if (values[i].Length < 5 || values[i].StartsWith(" ") || values[i].EndsWith(" "))
                        {
                            isValid = false;

                            if (i != 0)
                            { Console.WriteLine("\nError: the fractional part contains an invalid blank space pattern.\n"); }

                            else
                            { Console.WriteLine("\nError: the integer part contains an invalid blank space pattern.\n"); }

                            break;
                        }

                        // Validates the blank space content of the input value part,
                        // depending on whether or not the part considered is the integer
                        // part (i == 0), or the fractional part (i != 0)
                        isValid = ValidateBlankSpace(ref values[i], i == 0);

                        // If the value is invalid, break the for loop
                        if (!isValid)
                        { break; }

                        spaceSeparator = true;
                    }
                }

                if (!isValid)
                { continue; }



                int integerValueExponent = values[0].Length;
                int fractionalValueExponent = values[1].Length + values[2].Length;

                // Evaluates if the length of the integer part is longer than the precision limit
                if (integerValueExponent > precisionLimit)
                {
                    Console.WriteLine("\nError: the input value's integer part contains too many digits." +
                        $" It contains {integerValueExponent} digits.\n");
                }

                // Evaluates if the length of the fractional part is longer than the fractional precision limit
                if (fractionalValueExponent > precisionLimit)
                {
                    Console.WriteLine("\nError: the input value's fractional part contains too many digits." +
                        $" It contains {fractionalValueExponent} digits.\n");
                }

                // If the integer part and the fractional part lengths are smaller or equal to their respective limits,
                // breaks the loop
                if (integerValueExponent <= precisionLimit && fractionalValueExponent <= precisionLimit)
                { break; }
            }
            while (true);

            return values;
        }

        /// <summary>
        /// Allows the user to choose whether or not to separate every group of 3 digits by a blank space separator.
        /// </summary>
        /// <param name="escapeMenu">The escape or not boolean.</param>
        /// <returns>Returns a boolean that represents the user's choice.</returns>
        public static bool ChooseSpaceSeparator(out bool escapeMenu)
        {
            escapeMenu = false;
            int choice = 0;

            string message = $"\nIf you want the converted value to be formated using blank spaces to separate" +
                " every group of 3 digits, type the value 1. Else, type the value 0. Next, press Enter to continue.";



            do
            {
                string choiceString = ProcessKeyboardInputs(message, 1, out escapeMenu);

                if (escapeMenu)
                { break; }

                else if (!int.TryParse(choiceString, out choice))
                { Console.WriteLine("\nError: the input value is not an integer.\n"); }

                else
                {
                    if (choice == 0 || choice == 1)
                    { break; }

                    else
                    {
                        Console.WriteLine("\nError: the input value is not a" +
                            $" value from {digitsLowerBound} to {digitsUpperBound}.\n");
                    }
                }
            }
            while (true);

            return (choice == 0 ? false : true);
        }

        /// <summary>
        /// Evaluates if the individual digits of the input string are valid or not, and computes the values of the integer and fractional part.
        /// </summary>
        /// <param name="values">The input strings array.</param>
        /// <param name="initialBase">The initial base value.</param>
        /// <param name="integer">The integer part of the value.</param>
        /// <param name="numerator">The numerator of the fractional part of the value.</param>
        /// <param name="denominator">The denominator of the fractional part of the value.</param>
        /// <returns>Returns a boolean that specifies whether or not the values are valid.</returns>
        public static bool ConvertStringsToValues(string[] values, int initialBase, ref BigInteger integer, ref BigInteger numerator,
            ref BigInteger denominator)
        {
            string integerPart = values[0];
            string aperiodicFractionPart = values[1];
            string periodicFractionPart = values[2];

            int periodicFractionLength = periodicFractionPart.Length;

            // The for loop for the aperiodic part of the fraction
            for (int position = aperiodicFractionPart.Length - 1; position >= 0; position--)
            {
                char c = aperiodicFractionPart[position];
                int multiplier;

                int digitIndex = Array.IndexOf(digitsLibrary, c, 0, initialBase);

                if (digitIndex != -1)
                { multiplier = digitIndex; }

                else
                {
                    Console.WriteLine("\nError: the input value's fractional part contains invalid digits (aperiodic part).\n");

                    return false;
                }

                numerator += multiplier * denominator;
                denominator *= initialBase;
            }

            BigInteger periodicNumerator = 0;
            BigInteger periodicDenominator = 1;

            // The for loop for the periodic part of the fraction
            for (int position = periodicFractionLength - 1; position >= 0; position--)
            {
                char c = periodicFractionPart[position];
                int multiplier;

                int digitIndex = Array.IndexOf(digitsLibrary, c, 0, initialBase);

                if (digitIndex != -1)
                { multiplier = digitIndex; }

                else
                {
                    Console.WriteLine("\nError: the input value's fractional part contains invalid digits (periodic part).\n");

                    return false;
                }

                periodicNumerator += multiplier * periodicDenominator;
                periodicDenominator *= initialBase;
            }

            // If the periodic part is not empty, and the periodic value is non-zero, tries to simplify the fraction
            if (periodicFractionLength != 0 && periodicNumerator != 0)
            {
                periodicDenominator--;
                numerator *= periodicDenominator;
                numerator += periodicNumerator;
                denominator *= periodicDenominator;

                BigInteger gcd = GetGCD(numerator, denominator);
                numerator /= gcd;
                denominator /= gcd;
            }

            BigInteger integerExponential = 1;

            // The for loop for the integer part
            for (int position = integerPart.Length - 1; position >= 0; position--)
            {
                char c = integerPart[position];
                int multiplier;

                int digitIndex = Array.IndexOf(digitsLibrary, c, 0, initialBase);

                if (digitIndex != -1)
                { multiplier = digitIndex; }

                else
                {
                    Console.WriteLine("\nError: the input value's integer part contains invalid digits.\n");

                    return false;
                }

                integer += multiplier * integerExponential;
                integerExponential *= initialBase;
            }

            return true;
        }

        /// <summary>
        /// Computes the individual digits of the fractional part.
        /// </summary>
        /// <param name="numerator">The numerator of the fractional part of the value.</param>
        /// <param name="denominator">The denominator of the fractional part of the value.</param>
        /// <param name="finalBase">The final base value.</param>
        /// <param name="cutoffLimit">The cutoff limit value for the fractional part expansion.</param>
        /// <param name="cycleStartIndex">The index where the periodic value begins.</param>
        /// <param name="periodLength">The period length of the periodic value.</param>
        /// <param name="exactValue">The exact value or not boolean.</param>
        /// <returns>Returns a list of the individual digits from the fractional part.</returns>
        public static List<BigInteger> ConvertFractionToDigits(BigInteger numerator, BigInteger denominator, int finalBase, int cutoffLimit,
            ref int cycleStartIndex, ref int periodLength, ref bool exactValue)
        {
            int counter;
            Dictionary<BigInteger, int> numeratorsDictionary = new Dictionary<BigInteger, int>();
            BigInteger value = 0;
            List<BigInteger> valuesList = new List<BigInteger>();



            // If the numerator is smaller than the denominator, converts the fraction to digits
            if (numerator < denominator)
            {
                // Computes the digit values, while the counter is smaller than the cutoff limit
                for (counter = 0; counter < cutoffLimit; counter++)
                {
                    numerator *= finalBase;

                    // If a digit pattern has been found before the cutoff limit has been reached,
                    // the value is periodic and exact. Gets the cycle start index, computes the period
                    // length, then breaks the for loop
                    if (numeratorsDictionary.TryGetValue(numerator, out cycleStartIndex))
                    {
                        periodLength = valuesList.Count - cycleStartIndex;

                        break;
                    }

                    // Stores the numerator values in the dictionary, in an attempt to find a digit pattern
                    numeratorsDictionary.Add(numerator, counter);

                    value = numerator / denominator;
                    valuesList.Add(value);
                    numerator -= value * denominator;

                    // If the numerator is equal to 0 before the cutoff limit has been reached,
                    // the value is aperiodic and exact. Breaks the loop
                    if (numerator == 0)
                    { break; }

                    // If the numerator is not equal to 0 and the cutoff limit has been reached,
                    // the value is periodic and approximate. Extracts an extra digit for the
                    // rounding process that will follow
                    else if (counter == cutoffLimit - 1)
                    {
                        numerator *= finalBase;
                        value = numerator / denominator;
                        valuesList.Add(value);
                    }
                }
            }

            // If the numerator is equal to the denominator, assumes the fraction is equal to 1
            else
            {
                // Fills the list with digits equal to the final base minus 1. Adds an extra
                // digit for the rounding process that will follow
                for (counter = 0; counter < cutoffLimit + 1; counter++)
                { valuesList.Add(finalBase - 1); }
            }

            // If the numerator is non-zero and the period length has not been determined,
            // then the value is approximate
            if (numerator != 0 && periodLength == 0)
            { exactValue = false; }

            return valuesList;
        }

        /// <summary>
        /// Converts the digits of the fractional part to its final string representation.
        /// </summary>
        /// <param name="digitValues">The digits of the fractional part.</param>
        /// <param name="finalBase">The final base value.</param>
        /// <param name="cycleStartIndex">The index where the periodic value begins.</param>
        /// <param name="periodLength">The period length of the periodic value.</param>
        /// <param name="exactValue">The exact value or not boolean.</param>
        /// <param name="spaceSeparator">The blank space separator boolean.</param>
        /// <param name="integer">The integer part of the value to convert.</param>
        /// <returns>Returns the final string representation of the fractional part.</returns>
        public static string ConvertDigitsToString(List<BigInteger> digitValues, int finalBase,
            int cycleStartIndex, int periodLength, bool exactValue, bool spaceSeparator, ref BigInteger integer)
        {
            string outputValue = "";

            // In the case the value is exact and periodic
            if (exactValue && periodLength != 0)
            {
                string aperiodicPart = "";
                string periodicPart = "";

                for (int i = 0; i < cycleStartIndex; i++)
                { aperiodicPart += digitsLibrary[(int)digitValues[i]]; }

                for (int i = cycleStartIndex; i < digitValues.Count; i++)
                { periodicPart += digitsLibrary[(int)digitValues[i]]; }

                // If the input contained space separators, inserts blank spaces
                if (spaceSeparator)
                {
                    for (int i = 3, j = 3; i < cycleStartIndex; i += 3, j += 4)
                    { aperiodicPart = aperiodicPart.Insert(j, " "); }

                    for (int i = 3, j = 3; i < digitValues.Count - cycleStartIndex; i += 3, j += 4)
                    { periodicPart = periodicPart.Insert(j, " "); }
                }

                return aperiodicPart + "(" + periodicPart + ")";
            }

            // In the case the value is approximate and periodic, tries to round the value
            else if (!exactValue)
            {
                int counter = digitValues.Count - 1;
                // Defines the threshold value that determines whether to round the digits' value
                // or not
                bool evenBase = (finalBase % 2 == 0 ? true : false);
                int halfBase = (evenBase ? finalBase / 2 : (finalBase + 1) / 2);
                // Gets the extra digit at the end of the list
                BigInteger value = digitValues[counter];
                // Removes the extra digit from the list
                digitValues.RemoveAt(digitValues.Count - 1);

                // Evaluates if the extra digit from the list is larger than or equal to
                // the halfBase value. If it is, starts the rounding process
                if (value >= halfBase)
                {
                    // Adds 1 to the least significant digit of the fractional part, then evaluates
                    // if the resulting value is equal to the final base. If it is, then it adds 1
                    // to the next digit and evaluates if the resulting value is equal to the final
                    // base again, and so on, up to the most significant digit of the fractional
                    // part. The process continues while the rounded value evaluates to the value
                    // of the final base, or while the counter value is not zero
                    do
                    {
                        // Decrements the counter
                        counter--;
                        // Gets the value of the sum of the actual digit value with the value 1
                        value = ++digitValues[counter];
                        // Sets the new digit value to the remainder of the digit value modulo
                        // the final base
                        digitValues[counter] %= finalBase;
                    }
                    while (value == finalBase && counter > 0);

                    // In the case that the counter is 0, but the sum of the most
                    // significant digit of the fractional part with 1 is equal to
                    // the final base, adds 1 to the integer part
                    if (value == finalBase)
                    { integer++; }
                }
            }

            // In the case the value is aperiodic and exact, or period and approximate
            foreach (int digit in digitValues)
            { outputValue += digitsLibrary[digit]; }

            // If the input contained space separators, inserts blank spaces
            if (spaceSeparator)
            {
                for (int i = 3, j = 3; i < digitValues.Count; i += 3, j += 4)
                { outputValue = outputValue.Insert(j, " "); }
            }

            return outputValue;
        }

        /// <summary>
        /// Converts the integer part to its final value, and formats the string to its final representation.
        /// </summary>
        /// <param name="integer">The integer part of the value.</param>
        /// <param name="finalBase">The final base value.</param>
        /// <param name="outputValue">The output value as a string.</param>
        /// <param name="negativeSign">The negative or positive sign boolean.</param>
        /// <param name="commaSeparator">The comma or dot separator boolean.</param>
        /// <param name="spaceSeparator">The blank space separator boolean.</param>
        /// <returns>Returns the final string representation of the value.</returns>
        public static string ConvertIntegerToString(BigInteger integer, int finalBase, string outputValue,
            bool negativeSign, bool commaSeparator, bool spaceSeparator)
        {
            BigInteger value;
            string integerPart = "";

            do
            {
                value = integer % finalBase;
                integerPart = digitsLibrary[(int)value] + integerPart;
                integer /= finalBase;
            }
            while (integer > 0);

            // If the input contained space separators, inserts blank spaces
            if (spaceSeparator)
            {
                int integerPartLength = integerPart.Length;
                int remainder = integerPartLength % 3;

                // If the remainder is 0, then the for loop will begin inserting at the
                // beginning of the string, which is not the purpose of this loop. Hence,
                // in this case it sets the remainder to the value 3 (the first step has to
                // be 3)
                if (remainder == 0)
                { remainder = 3; }

                for (int i = remainder, j = remainder; i < integerPartLength; i += 3, j += 4)
                { integerPart = integerPart.Insert(j, " "); }
            }

            // If the output value is not empty, combines the fractional part with the
            // integer part, and separates the values using the adopted separator convention
            if (outputValue != "")
            {
                char c = (commaSeparator ? ',' : '.');

                outputValue = integerPart + c + outputValue;
            }

            // Else, set the output string equal to the integer part of the value
            else
            { outputValue = integerPart; }

            if (negativeSign)
            { outputValue = "-" + outputValue; }

            return outputValue;
        }

        /// <summary>
        /// Prints the converted value.
        /// </summary>
        /// <param name="outputValue">The output string value.</param>
        /// <param name="periodLength">The period length of the converted value.</param>
        /// <param name="exactValue">The exact value or not boolean.</param>
        public static void PrintOutput(string outputValue, int periodLength, bool exactValue)
        {
            if (exactValue)
            {
                if (periodLength == 0)
                {
                    Console.WriteLine($"\nThe converted value is:\n{outputValue}\n\nThe value is exact" +
                        $" and aperiodic.\n");
                }

                else
                {
                    Console.WriteLine($"\nThe converted value is:\n{outputValue}\n\nThe value is exact" +
                        $" and periodic, with a period length of {periodLength} digit(s).\n");
                }
            }

            else
            {
                Console.WriteLine($"\nThe converted value is:\n{outputValue}\n\nThe value is approximate" +
                    " and periodic, and has been rounded.\n");
            }
        }





        /// <summary>
        /// Processes the keyboard inputs and commands.
        /// </summary>
        /// <param name="message">The messages related to the initial base, the final base, or the value to convert.</param>
        /// <param name="maximalStringLength">The maximal string length value.</param>
        /// <param name="escapeMenu">The escape or not boolean.</param>
        /// <returns>Returns the input string.</returns>
        public static string ProcessKeyboardInputs(string message, int maximalStringLength, out bool escapeMenu)
        {
            escapeMenu = false;
            string stringLengthMsg = $" The maximal string length is {maximalStringLength} ASCII characters.\n";
            message += stringLengthMsg;

            StringBuilder input = new StringBuilder();
            ConsoleKeyInfo keyValue;
            int inputLength = 0;



            Console.WriteLine(message);

            do
            {
                // Reads the value of the key pressed, yet does not display the value of the key pressed
                keyValue = Console.ReadKey(true);
                inputLength = input.Length;

                switch (keyValue.Key)
                {
                    // If the Enter key has been pressed
                    case (ConsoleKey.Enter):
                        // If the input string length is non-zero and smaller or equal to the maximal string length,
                        // prints a new line and returns the input variable as a string
                        if (inputLength > 0)
                        {
                            Console.WriteLine();

                            return input.ToString();
                        }

                        Console.WriteLine("\nError: the input cannot be empty.\n");
                        Console.WriteLine(message);

                        // Empties the buffer of the pressed key(s)
                        while (Console.KeyAvailable)
                        { Console.ReadKey(true); }

                        break;

                    // If the Escape key has been pressed, turns the escape menu boolean to true
                    // and returns an empty string
                    case (ConsoleKey.Escape):
                        Console.WriteLine("\nReturning to the main menu.\n");

                        escapeMenu = true;

                        return "";

                    // If the Backspace key has been pressed
                    case (ConsoleKey.Backspace):
                        // If the input string length is non-zero
                        if (inputLength > 0)
                        {
                            // Checks if the cursor is at the start of a line
                            if (Console.CursorLeft == 0)
                            {
                                // Moves up one line, to the very end of the line
                                Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop - 1);

                                // Prints a blank space over the actual character
                                Console.Write(" ");
                            }

                            else
                            {
                                // Moves back from one character, prints a blank space over the actual character,
                                // then moves the cursor left one more time
                                Console.Write("\b \b");
                            }

                            // Removes the last character from the input variable
                            input.Remove(inputLength - 1, 1);
                        }

                        break;

                    // If any other key has been pressed
                    default:
                        // If the key value is not a null character
                        if (keyValue.KeyChar != '\0')
                        {
                            // If the input string length is smaller than the maximal string length before adding
                            // another character
                            if (inputLength < maximalStringLength)
                            {
                                char c = keyValue.KeyChar;

                                // If the actual character is either a standard blank space, a parenthese, a plus sign, a comma, a minus sign, a dot,
                                // a value from 0 to 9, or a letter from a to z (all letters can be uppercase or lowercase)
                                if (c == ' ' || c == '(' || c == ')' || (c >= '+' && c <= '.') || (c >= '0' && c <= '9') ||
                                    (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                                {
                                    // Prints the character in the console
                                    Console.Write(c);

                                    // Appends the character to the input variable
                                    input.Append(c);
                                }

                                // If the character is not an accepted character
                                else
                                {
                                    Console.WriteLine("\nThe character entered is invalid. The accepted characters are:\n" +
                                        "The standard blank space, parentheses, the minus sign, the plus sign, the dot, the comma," +
                                        " any number from 0 to 9, and any character from a to z (lowercase and uppercase).\n");

                                    // Empties the buffer of the pressed key(s)
                                    while (Console.KeyAvailable)
                                    { Console.ReadKey(true); }

                                    // Reprints the valid part of the input variable
                                    Console.Write($"{input}");
                                }
                            }

                            // If the input string length is equal or larger than the maximal string length before adding
                            // another character
                            else
                            {
                                Console.WriteLine("\nMaximal string length reached." + stringLengthMsg + "\n");

                                // Empties the buffer of the pressed key(s)
                                while (Console.KeyAvailable)
                                { Console.ReadKey(true); }

                                // Reprints the valid part of the input variable
                                Console.Write($"{input}");
                            }
                        }

                        break;
                }
            }
            while (true);
        }

        /// <summary>
        /// Validates the separator content of the input value.
        /// </summary>
        /// <param name="inputValue">The input value to convert.</param>
        /// <param name="commaSeparator">The comma or dot separator boolean.</param>
        /// <returns>Returns whether the separator content of the input value is valid or not.</returns>
        public static bool ValidateSeparator(string inputValue, bool commaSeparator)
        {
            switch (inputValue.Contains("."), commaSeparator)
            {
                // If the input value contains a dot and a comma, it is invalid
                case (true, true):
                    Console.WriteLine("\nError: the input value contains dots and commas.\n");

                    return false;

                // If the input value contains a dot or a comma, evaluates if it contains more than one separator.
                case (false, true):
                case (true, false):
                    char c = (commaSeparator ? ',' : '.');

                    // If the input value contains more than one separator, it is invalid
                    if (inputValue.IndexOf(c) != inputValue.LastIndexOf(c))
                    {
                        Console.WriteLine("\nError: the input value contains more than one dot or more than one comma.\n");

                        return false;
                    }

                    break;

                // If the input value does not contain a separator, it may be valid
                default:
                    break;
            }

            return true;
        }

        /// <summary>
        /// Validates the integer part of the input value.
        /// </summary>
        /// <param name="integerPart">The integer part of the input value.</param>
        /// <returns>Returns whether the integer part is valid or not.</returns>
        public static bool ValidateIntegerPart(string integerPart)
        {
            // If the input value's integer is empty, it is invalid
            if (integerPart == "")
            {
                Console.WriteLine("\nError: the input value's integer is empty.\n");

                return false;
            }

            // If the input value's integer is not empty, it may be valid
            else
            {
                // If the input value contains parentheses, it is invalid
                if (integerPart.Contains('(') || integerPart.Contains(')'))
                {
                    Console.WriteLine("\nError: the input value's integer part cannot contain parentheses.\n");

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates and processes the fractional part of the input value.
        /// </summary>
        /// <param name="fractionalPart">The fractional part of the input value.</param>
        /// <returns>Returns whether the fractional part is valid or not.</returns>
        public static bool ValidateFractionalPart(ref string fractionalPart)
        {
            // Finds the position of the first parentheses in the fractional part
            int openingParenthesisIndex = fractionalPart.IndexOf("(");
            int closingParenthesisIndex = fractionalPart.IndexOf(")");

            // If the input value's fractional part is empty, it is invalid
            if (fractionalPart == "")
            {
                Console.WriteLine("\nError: the input value contains a separator, yet the fractional part is empty.\n");

                return false;
            }

            switch (openingParenthesisIndex == -1, closingParenthesisIndex == -1)
            {
                // If the fractional part does not contain any parenthesis, keeps the value as it is
                case (true, true):
                    return true;

                // If the fractional part contains only an opening or a closing parenthesis, it is invalid
                case (true, false):
                case (false, true):
                    Console.WriteLine("\nError: the input value cannot contain a closing or an opening parenthesis.\n");

                    return false;

                // If the fractional part contains both opening and closing parentheses
                default:
                    // If a closing parenthesis appears before an opening parenthesis, the fractional part is invalid
                    if (closingParenthesisIndex < openingParenthesisIndex)
                    {
                        Console.WriteLine("\nError: the closing parenthesis must be after the opening parenthesis.\n");

                        return false;
                    }

                    // If the closing parenthesis is not at the end of the string, the fractional part is invalid
                    else if (closingParenthesisIndex != fractionalPart.Length - 1)
                    {
                        Console.WriteLine("\nError: the periodic value must be at the end of the input value.\n");

                        return false;
                    }

                    // If the fractional part contains only one opening parenthesis and one closing parenthesis,
                    // and the closing parenthesis is at the end of the string, the fractional part may be valid
                    else if (openingParenthesisIndex == fractionalPart.LastIndexOf("(") &&
                         closingParenthesisIndex == fractionalPart.Length - 1)
                    { break; }

                    // If the fractional part contains more than one opening parenthesis and closing parentheses,
                    // the fractional part is invalid
                    else
                    {
                        Console.WriteLine("\nError: the input value's contains an invalid parentheses pattern.\n");

                        return false;
                    }
            }



            // If the parentheses do not hold anything
            if (closingParenthesisIndex == openingParenthesisIndex + 1)
            {
                Console.WriteLine("\nError: the parentheses must hold at least one digit.\n");

                return false;
            }

            // If the parentheses hold at least one digit
            else
            {
                // Splits the fractional part in two strings at the opening parenthesis
                var fractionalPartTemp = fractionalPart.Split('(');

                // Sets the finite part of the fractional part as the first string
                string finitePart = fractionalPartTemp[0];

                // Extracts a substring from the second string, while excluding the closing parenthesis from the substring
                // Sets the extracted substring as the second string (the periodic part of the fractional part)
                string infinitePart = fractionalPartTemp[1].Substring(0, fractionalPartTemp[1].Length - 1);

                fractionalPart = finitePart + ":" + infinitePart;
            }

            return true;
        }

        /// <summary>
        /// Validates the blank space content of the input value's integer part, aperiodic part, or periodic part.
        /// </summary>
        /// <param name="inputValuePart">The integer part, the aperiodic part, or the periodic part of the value to convert.</param>
        /// <param name="isIntegerPart">The integer part or fractional part boolean.</param>
        /// <returns>Returns whether the part considered is valid or not.</returns>
        public static bool ValidateBlankSpace(ref string inputValuePart, bool isIntegerPart)
        {
            int partLength = inputValuePart.Length;
            int residue = (isIntegerPart ? partLength % 4 : 3);
            int startIndex = (isIntegerPart ? residue : 0);

            int spaceIndex;

            for (int numberOfSpaces = partLength / 4; numberOfSpaces > 0; numberOfSpaces--)
            {
                spaceIndex = inputValuePart.IndexOf(' ', startIndex, 4);

                switch (isIntegerPart, spaceIndex % 4 != residue)
                {
                    // In the case the integer part is being validated and contains an invalid pattern
                    case (true, true):
                        Console.WriteLine("\nError: the integer part contains an invalid blank space pattern.\n");

                        return false;

                    // In the case the fractional part is being validated and contains an invalid pattern
                    case (false, true):
                        Console.WriteLine("\nError: the fractional part contains an invalid blank space pattern.\n");

                        return false;

                    // Else, the pattern is valid up until now
                    default:
                        startIndex += 4;

                        break;
                }
            }

            inputValuePart = inputValuePart.Replace(" ", "");

            return true;
        }

        /// <summary>
        /// Calculates the greatest common divisor (GCD) using the Euclidean algorithm.
        /// </summary>
        /// <param name="a">The input value to compare with b.</param>
        /// <param name="b">The input value to compare with a.</param>
        /// <returns>Returns the greatest common divisor of the values a and b.</returns>
        public static BigInteger GetGCD(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }
    }
}
