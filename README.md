# Numeral_System_Base_Conversion
This program allows to convert the representation of a floating point number from an integer base to another, from base 2 to base 36. A new feature allows the user to input periodic values directly. Also, the algorithm now specifies if the converted value has a period length that exceeds the cutoff limit of the algorithm.

You can compare the results of this algorithm with Wolfram Alpha's base conversion:
https://www.wolframalpha.com/examples/mathematics/numbers/base-conversions

Example of value conversion:
<img width="1226" height="962" alt="image" src="https://github.com/user-attachments/assets/c4c619ac-62ee-4f06-9de7-1b409a82064c" />


Important note: the newest version of this algorithm has virtually unlimited precision, but the precision has been limited in order to prevent memory saturation (OutOfMemoryException).
