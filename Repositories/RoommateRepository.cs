using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;


namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public Roommate GetRoommateById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT FirstName, LastName, RentPortion, r.Id as 'Room Id', MoveInDate, Name, MaxOccupancy " +
                                      "FROM Roommate rm JOIN Room r ON rm.RoomId = r.Id " +
                                      "WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = new Room
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Room Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
                            }
                        };
                    }

                    reader.Close();
                    return roommate;
                }    
            }
        }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName FROM Roommate";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roomMates = new List<Roommate>();

                    while (reader.Read())
                    {
                        int idRoommateColumnPosition = reader.GetOrdinal("Id");
                        int idRoommateValue = reader.GetInt32(idRoommateColumnPosition);

                        int fnRoommateColumnPosition = reader.GetOrdinal("FirstName");
                        string fnRoommateValue = reader.GetString(fnRoommateColumnPosition);

                        int lnRoommateColumnPosition = reader.GetOrdinal("LastName");
                        string lnRoommateValue = reader.GetString(lnRoommateColumnPosition);

                        Roommate roomMate = new Roommate
                        {
                            Id = idRoommateValue,
                            FirstName = fnRoommateValue,
                            LastName = lnRoommateValue
                        };

                        roomMates.Add(roomMate);
                    }
                    reader.Close();

                    return roomMates;
                }
            }
        }
    }
}
