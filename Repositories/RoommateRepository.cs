using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roommates.Models;
using Microsoft.Data.SqlClient;


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
                    cmd.CommandText = "SELECT rm.FirstName, rm.LastName, rm.RentPortion, rm.MoveInDate, r.Id, r.Name, r.MaxOccupancy FROM Roommate rm JOIN Room r ON rm.RoomId = r.Id WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;
                    
                    

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("rm.FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("rm.LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("rm.RentPortion")),
                            MovedInDate = reader.GetDateTime(reader.GetOrdinal("rm.MoveInDate")),
                            Room = new Room
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("r.id")),
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
    }
}
