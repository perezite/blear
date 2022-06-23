using System.Collections;

using UnityEngine;

public static class MathHelper
{
    // Get n'th digit from a given integer
    public static int GetDigit(int number, int digitPos)
    {
        number = Mathf.Abs(number);

        int numDigits = GetNumDigits(number);
        if (digitPos > numDigits)
        {
            throw new System.ArgumentException();
        }

        int nextLargerBaseNumber = (int)Mathf.Pow(10, digitPos);

        while (number >= nextLargerBaseNumber)
        {
            number /= 10;
        }

        return number % 10;
    }

    public static int GetNumDigits(int number)
    {
        number = Mathf.Abs(number);

        if (number == 0)
        {
            return 1;
        }

        return (int)Mathf.Floor(Mathf.Log10(number) + 1);
    }
}