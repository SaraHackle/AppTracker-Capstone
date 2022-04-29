using Microsoft.Data.SqlClient;
using AppTrackerMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AppTrackerMVC.Utils;

namespace AppTrackerMVC.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {

        private readonly IConfiguration _config;

        public ApplicationRepository(IConfiguration config)
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

        public List<Application> GetAllApplicationsByUser(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT a.Id, a.Company, a.Location, 
                              a.Description,
                              a.DateApplied, a.Salary, a.UserId AS AppUserId,
                              u.Id, u.UserName, u.FirstName,
                              u.LastName, u.Email
                         FROM Application a
                              LEFT JOIN UserProfile u ON a.UserId = u.id
                              
                       WHERE a.UserId = @userId";
                    cmd.Parameters.AddWithValue("@userId", userId);
                    var reader = cmd.ExecuteReader();

                    var applications = new List<Application>();

                    while (reader.Read())
                    {
                   
                        applications.Add(NewApplicationFromReader(reader));
                    }

                    reader.Close();

                    return applications;
                }
            }
        }

        public Application GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                   SELECT a.Id, a.Company, a.Location, 
                              a.Description,
                              a.DateApplied, a.Salary, a.UserId AS AppUserId,
                              u.Id, u.UserName, u.FirstName,
                              u.LastName, u.Email
                         FROM Application a
                              LEFT JOIN UserProfile u ON a.UserId = u.id
                              
                       WHERE a.Id = @Id";

                    cmd.Parameters.AddWithValue("@id", id);

                    Application application = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        application = new Application
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Company = reader.GetString(reader.GetOrdinal("Company")),
                            Location = reader.GetString(reader.GetOrdinal("Location")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            DateApplied = (DateTime)DbUtils.GetNullableDateTime(reader, "DateApplied"),
                            Salary = reader.GetString(reader.GetOrdinal("Salary")),
                            UserId = reader.GetInt32(reader.GetOrdinal("AppUserId")),
                            User = new UserProfile()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                            },

                        };
                    }
                    reader.Close();

                    return application;
                }
            }
        }


        public void Add(Application application)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO
                                        Application (Company, Location, Description, DateApplied, Salary, UserId) 
                                        OUTPUT INSERTED.ID
                                        VALUES(@company, @location, @description, @dateApplied, @salary, @userId)";

                    cmd.Parameters.AddWithValue("@company", application.Company);
                    cmd.Parameters.AddWithValue("@location", application.Location);
                    cmd.Parameters.AddWithValue("@description", application.Description);
                    cmd.Parameters.AddWithValue("@dateApplied", application.DateApplied);
                    cmd.Parameters.AddWithValue("@salary", application.Salary);
                    cmd.Parameters.AddWithValue("@userId", application.UserId);

                    application.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Application application)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Application
                            SET 
                                Company = @company, 
                                Location = @location, 
                                Description = @description, 
                                DateApplied = @dateApplied,
                                Salary = @salary,
                                UserId = @userId
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue( "@company", application.Company);
                    cmd.Parameters.AddWithValue("@location", application.Location);
                    cmd.Parameters.AddWithValue("@description", application.Description);
                    cmd.Parameters.AddWithValue("@dateApplied", application.DateApplied);
                    cmd.Parameters.AddWithValue("@salary", application.Salary);
                    cmd.Parameters.AddWithValue("@userId", application.UserId);
                    cmd.Parameters.AddWithValue("@id", application.Id);


                    cmd.ExecuteNonQuery();
                }

            }

        }

        private Application NewApplicationFromReader(SqlDataReader reader)
        {
            Application application = new Application()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Company = reader.GetString(reader.GetOrdinal("Company")),
                Location = reader.GetString(reader.GetOrdinal("Location")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                DateApplied = (DateTime)DbUtils.GetNullableDateTime(reader, "DateApplied"),
                Salary= reader.GetString(reader.GetOrdinal("Salary")),
                UserId = reader.GetInt32(reader.GetOrdinal("AppUserId")),
                User= new UserProfile()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                },
                
            };
            return application;
        }

    }
}

