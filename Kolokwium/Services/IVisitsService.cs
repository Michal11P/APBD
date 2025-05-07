using Kolokwium.Models.DTO;

namespace Kolokwium.Services;

public interface IVisitsService
{
    Task<VisitHistoryDTO> GetVisitHistory(int visitId);
    //Task AddNewVisit(int visitId,CreateNewVisitDTO visit);
}