using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roommates.Models;
using Microsoft.Data.SqlClient;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }

        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> chores = new List<Chore>();

                    while (reader.Read())
                    {
                        int choreIdColumnPosition = reader.GetOrdinal("Id");

                        int choreId = reader.GetInt32(choreIdColumnPosition);

                        int choreNameColumnPosition = reader.GetOrdinal("Name");
                        string choreName = reader.GetString(choreNameColumnPosition);

                        Chore chore = new Chore
                        {
                            Id = choreId,
                            Name = choreName
                        };

                        chores.Add(chore);
                    }
                    reader.Close();

                    return chores;
                }
            }
        }

        public Chore GetChoreById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM chore where Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Chore chore = null;

                    if (reader.Read())
                    {
                        chore = new Chore
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }

                    reader.Close();
                    return chore;
                }
            }
        }

        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                      OUTPUT INSERTED.Id
                                      VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
 
                }
            }
        }

        public List<Chore> GetUnassignedChore()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT c.Id AS 'Chore Id', Name, rc.Id AS 'Roommate Chore Id' FROM Chore c LEFT JOIN RoommateChore rc ON c.Id = rc.ChoreId WHERE rc.Id is NULL";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> unassignedChore = new List<Chore>();

                    while (reader.Read())
                    {
                        int unassignedColumnIdPosition = reader.GetOrdinal("Chore Id");

                        int unassignedIdValue = reader.GetInt32(unassignedColumnIdPosition);

                        int unassignedColumnChorePosition = reader.GetOrdinal("Name");

                        string unassignedChoreValue = reader.GetString(unassignedColumnChorePosition);

                        Chore c = new Chore
                        {
                            Id = unassignedIdValue,
                            Name = unassignedChoreValue
                        };

                        unassignedChore.Add(c);
                    }

                    reader.Close();

                    return unassignedChore;
                }
            }
        }

        public void AssignChore(int choreId, int roommateId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();


                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO RoommateChore (ChoreId, RoommateId)
                                      OUTPUT INSERTED.Id
                                      VALUES (@choreId, @roommateId)";
                }
            }
        }
    }
}
