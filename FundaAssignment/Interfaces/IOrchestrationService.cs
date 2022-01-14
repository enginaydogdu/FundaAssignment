using System.Threading.Tasks;

namespace FundaAssignment.Interfaces
{
    public interface IOrchestrationService
    {
        Task Execute(bool withTuin);
    }
}