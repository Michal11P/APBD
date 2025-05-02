using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class TripsService : ITripsService
{
    private readonly string _connectionString =
        "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;TrustServerCertificate=True;";

    public async Task<List<TripDTO>> GetTrips()
    {
        var trips = new List<TripDTO>();

        //Zapytanie pobierajace podstawowe informacje o wycieczkach
        string tripCommand = @"SELECT IdTrip, Name, Description, DateFrom, DateTo, MaxPeople FROM Trip";
        
        //Zapytanie pobierajace informacje o krajach przypisanych do danych wycieczek
        string countryCommand =
            @"Select ct.IdTrip, c.Name FROM Country c JOIN Country_Trip ct ON c.IdCountry = ct.IdCountry";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            
            //Wykonanie zapytania pobierajacego informacje o wycieczkach
            using (SqlCommand command = new SqlCommand(tripCommand, conn))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    //Dodanie kazdej wycieczki do listy trips
                    trips.Add(new TripDTO
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("IdTrip")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        DateFrom = reader.GetDateTime(reader.GetOrdinal("DateFrom")),
                        DateTo = reader.GetDateTime(reader.GetOrdinal("DateTo")),
                        MaxPeople = reader.GetInt32(reader.GetOrdinal("MaxPeople")),
                        Countries = new List<CountryDTO>()
                    });
                }
            }

            //Wykonanie zapytania przypisujacego kraje do odpowiednich wycieczek
            using (SqlCommand command = new SqlCommand(countryCommand, conn))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int tripId = reader.GetInt32(reader.GetOrdinal("IdTrip"));
                    string countryName = reader.GetString(reader.GetOrdinal("Name"));

                    //Wyszukuje wycieczke po id i dodajemy do niej kraj
                    var trip = trips.FirstOrDefault(t => t.Id == tripId);
                    if (trip != null)
                    {
                        trip.Countries.Add(new CountryDTO { Name = countryName });
                    }
                }
            }

            //Zwracamy liste wycieczek z przypisanymi krajami
            return trips;
        }
    }
}