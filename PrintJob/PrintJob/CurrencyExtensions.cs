using System;

namespace PrintJob
{
    static class CurrencyExtensions
    {
        public static string ToEvenCent(this double finalTotal)
        {
            var cents = Math.Round(finalTotal % 2,2);
            var modVal = cents * 100 % 2;
            return modVal == 0 ? $"{finalTotal:$0.00}" : $"{finalTotal-0.01:$0.00}";
        }
    }
}
