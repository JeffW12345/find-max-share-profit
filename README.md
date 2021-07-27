Imports a series of share prices, and finds what the optimal buy and sell dates would have been.

The program imports a list of comma-separated numbers, where each number represents the opening share price on a given day (the first number is day 1 of the month's share price, the second is
day 2's share price, etc).
  
The program determines what the optimal day to buy and sell the shares would have been, to maximise profit.
  
It produces a console output as follows:
 
buyDayOfMonth(price),sellDayOfMonth(price)
 
Example:
5(15.95),6(19.03)
 
The dates are not zero-indexed when presented in the output, i.e. the first of the month is 1, not 0.
 
The repository contains:

- SharePrice.cs, which contains the C# program that the user runs.
- ChallengeSampleDataSet1.txt and ChallengeSampleDataSet2.txt, which are sample data files
