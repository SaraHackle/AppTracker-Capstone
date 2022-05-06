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

                        application = NewApplicationFromReader(reader);

                        
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

                    DbUtils.AddParameter(cmd, "@company", application.Company);
                    DbUtils.AddParameter(cmd, "@location", application.Location);
                    DbUtils.AddParameter(cmd, "@description", application.Description);
                    DbUtils.AddParameter(cmd, "@dateApplied", application.DateApplied);
                    DbUtils.AddParameter(cmd, "@salary", application.Salary);
                    DbUtils.AddParameter(cmd, "@userId", application.UserId);

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

                    DbUtils.AddParameter(cmd, "@company", application.Company);
                    DbUtils.AddParameter(cmd, "@location", application.Location);
                    DbUtils.AddParameter(cmd, "@description", application.Description);
                    DbUtils.AddParameter(cmd, "@dateApplied", application.DateApplied);
                    DbUtils.AddParameter(cmd, "@salary", application.Salary);
                    DbUtils.AddParameter(cmd, "@userId", application.UserId);
                    DbUtils.AddParameter(cmd, "@id", application.Id);


                    cmd.ExecuteNonQuery();
                }

            }

        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                     DELETE from Application
                     WHERE Id = @id ";
                    DbUtils.AddParameter(cmd, "@id", id);

                    cmd.ExecuteNonQuery();
                }

            }
        }

        private Application NewApplicationFromReader(SqlDataReader reader)
        {
            Application application = new Application()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                Company = DbUtils.GetString(reader, "Company"),
                Location = DbUtils.GetString(reader,"Location"),
                Description = DbUtils.GetString(reader, "Description"),
                DateApplied = (DateTime)DbUtils.GetNullableDateTime(reader, "DateApplied"),
                Salary= DbUtils.GetString(reader, "Salary"),
                UserId = DbUtils.GetInt(reader, "AppUserId"),
                User= new UserProfile()
                {
                    Id = DbUtils.GetInt(reader, "Id"),
                    UserName = DbUtils.GetString(reader, "UserName"),
                    FirstName = DbUtils.GetString(reader,"FirstName"),
                    LastName = DbUtils.GetString(reader,"LastName"),
                    Email = DbUtils.GetString(reader, "Email"),
                },
                
            };
            return application;
        }

    }
}

