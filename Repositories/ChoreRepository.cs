using System.Collections.Generic;
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
                    cmd.Parameters.AddWithValue("@choreId", choreId);
                    cmd.Parameters.AddWithValue("@roommateId", roommateId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Chore> GetChoreCounts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id AS 'Chore Id', Name, FirstName, LastName, rc.Id AS 'Roommate Chore Id' 
                                        FROM Chore c JOIN RoommateChore rc ON c.Id = rc.ChoreId 
                                        JOIN Roommate r ON rc.RoommateId = r.Id";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> choresList = new List<Chore>();

                    while (reader.Read())
                    {
                        int unassignedColumnIdPosition = reader.GetOrdinal("Chore Id");
                        int unassignedIdValue = reader.GetInt32(unassignedColumnIdPosition);

                        int unassignedColumnRmIdPosition = reader.GetOrdinal("Roommate Chore Id");
                        int unassignedRmIdValue = reader.GetInt32(unassignedColumnRmIdPosition);

                        int unassignedColumnChorePosition = reader.GetOrdinal("Name");
                        string unassignedChoreValue = reader.GetString(unassignedColumnChorePosition);

                        int unassignedColumnFnPosition = reader.GetOrdinal("FirstName");
                        string unassignedFnValue = reader.GetString(unassignedColumnFnPosition);

                        int unassignedColumnLnPosition = reader.GetOrdinal("LastName");
                        string unassignedLnValue = reader.GetString(unassignedColumnLnPosition);

                        Chore c = new Chore
                        {
                            Id = unassignedIdValue,
                            Name = unassignedChoreValue,
                            Roommate = new Roommate
                            {
                                Id = unassignedRmIdValue,
                                FirstName = unassignedFnValue,
                                LastName = unassignedLnValue
                            }
                        };

                        choresList.Add(c);
                    }

                    reader.Close();

                    return choresList;
                }
            }
        }

        public void UpdateChore(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Chore
                                        SET Name = @name
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    cmd.Parameters.AddWithValue("@id", chore.Id);

                    cmd.ExecuteNonQuery();
                }    
            }
        }

        public void DeleteChore(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // What do you think this code will do if there is a roommate in the room we're deleting???
                    cmd.CommandText = @"DELETE FROM Chore
                                        WHERE chore.Id = @id";
                    cmd.CommandText = @"DELETE FROM RoommateChore
                                        WHERE ChoreId = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<RoommateChore> GetAssignedChore()
        {   
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id AS 'Chore Id', Name, FirstName, LastName, rc.Id AS 'Roommate Chore Id', r.Id AS 'Roommate Id' 
                                        FROM Chore c JOIN RoommateChore rc ON c.Id = rc.ChoreId 
                                        JOIN Roommate r ON rc.RoommateId = r.Id";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<RoommateChore> assignedChore = new List<RoommateChore>();

                    while (reader.Read())
                    {
                        int assignedColumnRcIdPosition = reader.GetOrdinal("Roommate Chore Id");
                        int assignedRcIdValue = reader.GetInt32(assignedColumnRcIdPosition);

                        int assignedColumnRIdPosition = reader.GetOrdinal("Roommate Id");
                        int assignedRIdValue = reader.GetInt32(assignedColumnRIdPosition);

                        int assignedColumnIdPosition = reader.GetOrdinal("Chore Id");
                        int assignedIdValue = reader.GetInt32(assignedColumnIdPosition);

                        int assignedColumnChorePosition = reader.GetOrdinal("Name");
                        string assignedChoreValue = reader.GetString(assignedColumnChorePosition);

                        int unassignedColumnFnPosition = reader.GetOrdinal("FirstName");
                        string assignedFnValue = reader.GetString(unassignedColumnFnPosition);

                        int unassignedColumnLnPosition = reader.GetOrdinal("LastName");
                        string assignedLnValue = reader.GetString(unassignedColumnLnPosition);

                        RoommateChore c = new RoommateChore
                        {
                            Id = assignedColumnRcIdPosition,
                            Chore = new Chore
                            {
                                Id = assignedIdValue,
                                Name = assignedChoreValue
                            },
                            Roommate = new Roommate
                            {
                                Id = assignedColumnRIdPosition,
                                FirstName = assignedFnValue,
                                LastName = assignedLnValue
                            }

                        };

                        assignedChore.Add(c);
                    }

                    reader.Close();

                    return assignedChore;
                }
            }
        }
        public void ReAssignChore(int choreId, int roommateId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE RoommateChore
                                        SET ChoreId = @choreId, RoommateId = @roommateId
                                        WHERE ChoreId = @choreId";
                    cmd.Parameters.AddWithValue("@choreId", choreId);
                    cmd.Parameters.AddWithValue("@roommateId", roommateId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
