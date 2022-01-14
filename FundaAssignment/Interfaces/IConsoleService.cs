using FundaAssignment.Model;
using System.Collections.Generic;

namespace FundaAssignment.Interfaces
{
    public interface IConsoleService
    {
        public void WriteToConsole(IEnumerable<MakelaarNameHouseCount> makelaarNameHouseCounts, bool withTuin);
    }
}