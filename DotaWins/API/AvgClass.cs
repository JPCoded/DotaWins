using System.Collections.Generic;

namespace DotaWins
{
    public static class AvgClass 
    {
        public static List<double> Average(IEnumerable<double> number, int nElement)
        {
            var currentElement = 0;
            var currentSum = 0.0;

            var newList = new List<double>();

            foreach (var item in number)
            {
                currentSum += item;
                currentElement++;

                if (currentElement == nElement)
                {
                    newList.Add(currentSum / nElement);
                    currentElement = 0;
                    currentSum = 0.0;
                }
            }
            // Maybe the array element count is not the same to the asked, so average the last sum. You can remove this condition if you want
            if (currentElement > 0)
            {
                newList.Add(currentSum / currentElement);
            }

            return newList;
        }

       
    }
}