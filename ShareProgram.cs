using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharesApp
{
    class SharesProgram
    {
        private static string filePath = "";
        private static void AskUserForFilePath()
        {
            Console.WriteLine("What is the filepath of your data file, including the file extension (like .txt?)");
            Console.WriteLine("For example, " + @"C:\Users\yourname\OneDrive\Desktop\ChallengeSampleDataSet1.txt");
            string userInput = Console.ReadLine();
            bool colonPresent = false;
            if (userInput.Contains(':'))
            {
                colonPresent = true;
            }
            bool slashPresent = false;
            if (userInput.Contains('\\') || userInput.Contains('/'))
            {
                slashPresent = true;
            }
            if (colonPresent && slashPresent)
            {
                filePath = @userInput;
            }
            else
            {
                Console.WriteLine("That selection is invalid.\nThe location must contain a colon and a back slash or a forwards slash.\nPlease try again.\n\n");
                AskUserForFilePath();
            }
        }
        private static string GetDataAsString(string filePath)
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        builder.Append(sr.ReadLine());
                    }
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\nPlease check your file name and file path and try again.");
            }
            // Console.WriteLine("Test - this is the string that is returned: " + builder.ToString());
            return builder.ToString();
        }

        private static List<double> GetListOfPrices(string dataAsString)
        {
            string[] arrrayOfPrices = dataAsString.Split(','); // The data is stored in the file as a comma-separated list of numbers
            List<double> tempList = new List<double>();
            for (int index = 0; index < arrrayOfPrices.Length; index++)
            {
                if (double.TryParse(arrrayOfPrices[index], out double number))
                {
                    tempList.Add(number);
                }
                else
                {
                    throw new Exception("Invalid item in data file - please check your data");
                }
            }
            return tempList;
        }

        private static Dictionary<int, double> GetDict(List<double> listOfPrices)
        {
            Dictionary<int, double> tempDateToPriceDict = new Dictionary<int, double>();
            for (int index = 0; index < listOfPrices.Count; index++)
            {
                tempDateToPriceDict[index] = listOfPrices[index];
            }
            return tempDateToPriceDict;
        }

        private static List<int> GetBuyAndSellDates(Dictionary<int, double> dayToPriceDict)
        {
            // In the dictionary below, List<int> key stores the opening date [0] and the closing date [1].
            Dictionary<List<int>, double> openAndCloseDayToProfitDictionary = new Dictionary<List<int>, double>();
            // The 'for' loop below obtains the profit or loss from buying on every possible opening day and selling on 
            // every possible corresponding closing day. That figure is stored as a value in 'openAndCloseDayToDifferenceDict'.
            // The resulting data is then used to determine which combination of buy and sell dates would have been most profitable.
            int numDays = dayToPriceDict.Keys.Count;
            for (int openingDay = 0; openingDay < numDays; openingDay++)
            {
                for (int closingDay = openingDay + 1; closingDay < numDays; closingDay++)
                {
                    List<int> openDayCloseDayInstance = new List<int> { openingDay, closingDay }; // Stores the open and close day indices for a trade
                    double profitOrLoss = dayToPriceDict[closingDay] - dayToPriceDict[openingDay]; // Positive if the trade is profitable, negative if a loss.
                    openAndCloseDayToProfitDictionary[openDayCloseDayInstance] = profitOrLoss;
                }
            }
            double mostProfitable = openAndCloseDayToProfitDictionary.Values.ToList().Max();
            int openDay = openAndCloseDayToProfitDictionary.FirstOrDefault(x => x.Value == mostProfitable).Key[0];
            int closeDay = openAndCloseDayToProfitDictionary.FirstOrDefault(x => x.Value == mostProfitable).Key[1];
            return new List<int> { openDay, closeDay };
        }
        private static void PrintOutputToConsole(List<int> buyAndSellDates, Dictionary<int, double> dayToPriceDict)
        {
            int buyDayOfMonth = buyAndSellDates[0];
            double buyPrice = dayToPriceDict[buyDayOfMonth];
            int sellDayOfMonth = buyAndSellDates[1];
            double sellPrice = dayToPriceDict[sellDayOfMonth];
            // The + 1 below is because the dates are not zero-indexed when presented in the output, i.e. the first of the month is 1, not 0.
            Console.WriteLine((buyDayOfMonth + 1) + "(" + buyPrice + ")," + (sellDayOfMonth + 1) + "(" + sellPrice + ")");
        }

        static void Main(string[] args)
        {
            AskUserForFilePath();
            string dataAsString = GetDataAsString(filePath);
            List<double> listOfPrices = GetListOfPrices(dataAsString); // Converts the imported data into a List of doubles. 
            Dictionary<int, double> dayToPriceDict = GetDict(listOfPrices); // Zero-based indexing is used - The first imported entry is day 0, the next day 1, etc.
            List<int> buyAndSellDates = GetBuyAndSellDates(dayToPriceDict); // Returns a List of two integers - best date to open trade [0], best date to close trade [1]
            PrintOutputToConsole(buyAndSellDates, dayToPriceDict); // The printed dates are 1 for the first of the month, 2 for the second, etc (i.e. not zero indexed). 
        }
    }
}
