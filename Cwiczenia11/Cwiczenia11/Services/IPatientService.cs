using Cwiczenia11.DTOs;

namespace Cwiczenia11.Services;

public interface IPatientService
{
    Task<PatientDetailsDTO> GetPatientDetails(int patientId);
}