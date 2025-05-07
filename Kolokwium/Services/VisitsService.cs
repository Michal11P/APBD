using System.Data;
using Kolokwium.Models;
using Kolokwium.Models.DTO;
using Microsoft.Data.SqlClient;

namespace Kolokwium.Services;

public class VisitsService : IVisitsService
{
    private readonly string _connectionString;

    public VisitsService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? string.Empty;
    }

    public async Task<VisitHistoryDTO> GetVisitHistory(int visitId)
    {
        var query =
            @"SELECT v.date, c.first_name, c.last_name, c.date_of_birth, m.mechanic_id, m.licence_number, s.name, vs.service_fee
            FROM Visit v JOIN Client c ON c.client_id = v.client_id
            JOIN Mechanics m ON m.mechanic_id = v.mechanic_id
            JOIN Visit_Service vs ON vs.visit_id = v.visit_id
            JOIN Service s ON s.service_id = vs.service_id
            WHERE v.visit_id =@userId
            ";
        
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        await connection.OpenAsync();
        command.Parameters.Add(new SqlParameter("@userId", visitId));
        var reader = await command.ExecuteReaderAsync();
        
        VisitHistoryDTO? visitHistory = null;
        while (await reader.ReadAsync())
        {
            if (visitHistory == null)
            {
                visitHistory = new VisitHistoryDTO
                {
                    Date = reader.GetDateTime(0),
                    Client = new ClientDTO()
                    {
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        DateOfBirth = reader.GetDateTime(3),
                    },
                    Mechanic = new MechanicDTO()
                    {
                        MechanicId = reader.GetInt32(4),
                        LicenceNumber = reader.GetString(5),
                    },
                    Services = new List<ServiceDTO>()
                };
            }
            string serviceName = reader.GetString(7);
            var service = visitHistory.Services.FirstOrDefault(s => s.Name.Equals(serviceName));
            if (service == null)
            {
                service = new ServiceDTO()
                {
                    Name = serviceName,
                    ServiceFee = reader.GetInt32(8),

                };
                visitHistory.Services.Add(service);
            }
            
        }

        if (visitHistory == null)
        {
            throw new Exception("Visit not found");
        }
        return visitHistory;
    }

    //public async Task AddNewVisit(CreateNewVisitDTO visit)
   // {
        //await using SqlConnection connection = new SqlConnection(_connectionString);
       // await using SqlCommand command = new SqlCommand();
        
       // command.Connection = connection;
        //await connection.OpenAsync();
        
        //var query = @"INSERT INTO VISIT(visit_id, client_id) VALUES(@visitId, @clientId)";
        //command.CommandText = query;
        //command.Parameters.Add(new SqlParameter("@visitId", visit.VisitId));
        //command.Parameters.Add(new SqlParameter("@clientId", visit.ClientId));
        
        
   // }
}