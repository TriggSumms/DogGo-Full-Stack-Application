using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
//using System.configuration.dll

namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walk> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, [Date], Duration, WalkerId, DogId FROM Walks";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walk> walks = new List<Walk>();

                    while (reader.Read())
                    {
                        Walk walk = new Walk()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            //Date = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId"))


                        };
                        walks.Add(walk);
                    }

                    reader.Close();

                    return walks;
                }
            }
        }
        public List<Walk> GetALLWalksandIds(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
						SELECT w.Id, w.[Date], w.Duration, w.WalkerId, w.DogId,
						d.[Name] AS NameofDog, d.OwnerId, o.[Name] AS NameofOwner,
                         Walker.[Name] AS NameofWalker, walker.ImageUrl, walker.NeighborhoodId,
						 n.Name AS NeighborhoodName
                        FROM Walks w
                        JOIN Dog d ON w.DogId = d.Id
                        Join Walker ON w.WalkerId = Walker.Id
                        JOIN [Owner] o ON d.OwnerId = o.Id
						JOIN Neighborhood n on Walker.NeighborhoodId = n.Id    
                        WHERE Walker.Id = @walkerId
            ";

                    cmd.Parameters.AddWithValue("@walkerId", walkerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walk> walks = new List<Walk>();
                    while (reader.Read())
                    {
                        Walk walk = new Walk
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Dog = new Dog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("DogId")),
                                Name = reader.GetString(reader.GetOrdinal("NameofDog")),
                            },
                            Owner = new Owner()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Name = reader.GetString(reader.GetOrdinal("NameofOwner")),
                            }
                            //NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            //Neighborhood = new Neighborhood()
                            //{
                            //    Id = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            //    Name = reader.GetString(reader.GetOrdinal("NeighborhoodName")),
                            //}

                        };

                        walks.Add(walk);
                    }

                    reader.Close();

                    return walks;
                }
            }
        }
   
    }
}
