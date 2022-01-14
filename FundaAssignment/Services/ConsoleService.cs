using FundaAssignment.Interfaces;
using FundaAssignment.Model;
using System;
using System.Collections.Generic;

namespace FundaAssignment.Services
{
    public class ConsoleService : IConsoleService
    {
        public void WriteToConsole(IEnumerable<MakelaarNameHouseCount> makelaarNameHouseCounts, bool withTuin)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (withTuin)
            {
                Console.WriteLine("-- Top 10 with Garden ----------------------------------------");
            }
            else
            {
                Console.WriteLine("-- Top 10 without Garden -------------------------------------");
            }
            Console.ForegroundColor = ConsoleColor.White;
            int order = 1;
            foreach (var item in makelaarNameHouseCounts)
            {
                Console.WriteLine($"{order,-5}|{item.MakelaarName,-50}|{item.TotalHouse,5}");
                Console.WriteLine("--------------------------------------------------------------");
                order++;
            }
            Console.WriteLine();
        }
    }
}