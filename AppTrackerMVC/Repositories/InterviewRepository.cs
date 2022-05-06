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
    public class InterviewRepository : IInterviewRepository
    {

        private readonly IConfiguration _config;

        public InterviewRepository(IConfiguration config)
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

        public List<Interview> GetAllInterviewsByUser(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                      SELECT DISTINCT i.Id AS InterviewId, i.InterviewDate, i.AdditionalInfo, i.ApplicationId, 
                              a.Id, a.Company, a.Location, a.Description, a.DateApplied, a.Salary, a.UserId AS AppUserId          
                         FROM  Interview i
                              LEFT JOIN Application a ON a.Id = i.ApplicationId
     
                       WHERE a.UserId = @UserId";
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    var reader = cmd.ExecuteReader();

                    var interviews = new List<Interview>();

                    while (reader.Read())
                    {

                        interviews.Add(NewInterviewFromReader(reader));
                    }

                    reader.Close();

                    return interviews;
                }
            }
        }

        public Interview GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                     SELECT i.Id AS InterviewId, i.InterviewDate, i.AdditionalInfo, i.ApplicationId, 
                              a.Id, a.Company, a.Location, a.Description, a.DateApplied, a.Salary, a.UserId AS AppUserId
                             
                         FROM Interview i
                              LEFT JOIN Application a ON a.Id = i.ApplicationId
                              
                              
                       WHERE i.Id = @Id";

                    cmd.Parameters.AddWithValue("@id", id);

                    Interview interview = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        interview = NewInterviewFromReader(reader);
                       
                    }
                    reader.Close();

                    return interview;
                }
            }
        }

        public List<Interview> GetInterviewsByApplicationId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT i.Id AS InterviewId, i.InterviewDate, i.AdditionalInfo, i.ApplicationId, 
                               a.Id, a.Company, a.Location, a.Description, a.DateApplied, a.Salary, a.userId as AppUserId                
                         FROM Interview i
                              LEFT JOIN Application a ON a.Id = i.ApplicationId
                                
         
                       WHERE a.Id = @Id";

                    cmd.Parameters.AddWithValue("@id", id);

                    var reader = cmd.ExecuteReader();

                    var interviews = new List<Interview>();

                    while (reader.Read())
                    {

                        interviews.Add(NewInterviewFromReader(reader));
                    }

                    reader.Close();

                    return interviews;
                }
            }
        }

        public void Add(Interview interview)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO
                                        Interview (InterviewDate, AdditionalInfo, ApplicationId) 
                                        OUTPUT INSERTED.ID
                                        VALUES( @interviewDate, @additionalInfo, @applicationId)";

                    DbUtils.AddParameter(cmd, "@interviewDate", interview.InterviewDate);
                    DbUtils.AddParameter(cmd, "@additionalInfo", interview.AdditionalInfo);
                    DbUtils.AddParameter(cmd, "@applicationId", interview.ApplicationId);


                    interview.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Interview interview)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Interview
                            SET 
                                InterviewDate = @interviewDate, 
                                AdditionalInfo = @additionalInfo, 
                                ApplicationId = @applicationId
                              
                            WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@interviewDate", interview.InterviewDate);
                    DbUtils.AddParameter(cmd, "@additionalInfo", interview.AdditionalInfo);
                    DbUtils.AddParameter(cmd, "@applicationId", interview.ApplicationId);
                    DbUtils.AddParameter(cmd, "@id", interview.Id);


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
                     DELETE from Interview
                     WHERE Id = @id ";
                    DbUtils.AddParameter(cmd, "@id", id);

                    cmd.ExecuteNonQuery();
                }

            }
        }

        private Interview NewInterviewFromReader(SqlDataReader reader)
        {
            Interview interview = new Interview()
            {
                Id = DbUtils.GetInt(reader, "InterviewId"),
                InterviewDate = (DateTime)DbUtils.GetNullableDateTime(reader, "InterviewDate"),
                AdditionalInfo = DbUtils.GetString(reader, "AdditionalInfo"),
                ApplicationId = DbUtils.GetInt(reader, "ApplicationId"),
                Application = new Application()
                {
                    Id = DbUtils.GetInt(reader, "Id"),
                    Company = DbUtils.GetString(reader, "Company"),
                    Location = DbUtils.GetString(reader, "Location"),
                    Description = DbUtils.GetString(reader, "Description"),
                    DateApplied = (DateTime)DbUtils.GetNullableDateTime(reader, "DateApplied"),
                    Salary = DbUtils.GetString(reader, "Salary"),
                    UserId = DbUtils.GetInt(reader, "AppUserId"),

                },
            };
            return interview;
        }

    }
}

