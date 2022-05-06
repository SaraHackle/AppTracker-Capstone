using AppTrackerMVC.Models;
using AppTrackerMVC.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppTrackerMVC.Repositories
{
    public class TagRepository : ITagRepository
    {

        private readonly IConfiguration _config;

        public TagRepository(IConfiguration config)
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

        public List<Tag> GetAllTags()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, TagName FROM Tag Order By TagName";
                    var reader = cmd.ExecuteReader();

                    var tags = new List<Tag>();

                    while (reader.Read())
                    {
                        tags.Add(new Tag()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            TagName = DbUtils.GetString(reader, "TagName"),
                        });
                    }

                    reader.Close();

                    return tags;
                }
            }
        }

        public List<Tag> GetTagsByApplicationId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT t.Id, t.TagName, at.Id AS AppTagId, at.TagId, at.ApplicationId 
                        FROM Tag t
                        Join ApplicationTag at ON t.Id = at.TagId
                        Join Application a ON at.ApplicationId = a.Id
                        Where a.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        List<Tag> tags = new List<Tag>();


                        while (reader.Read())
                        {
                            Tag tag = new Tag()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                TagName = DbUtils.GetString(reader, "TagName")
                            };

                            tags.Add(tag);

                        }

                        return tags;

                    }
                }
            }


        }

        public List<ApplicationTag> GetApplicationTagsByApplicationId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT t.Id, t.TagName, at.Id AS AppTagId, at.TagId, at.ApplicationId 
                        FROM ApplicationTag at
                        Join Tag t ON t.Id = at.TagId
                        Join Application a ON at.ApplicationId = a.Id
                        Where a.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {


                        List<ApplicationTag> appTags = new List<ApplicationTag>();

                        while (reader.Read())
                        {


                            ApplicationTag appTag = new ApplicationTag()
                            {
                                Id = DbUtils.GetInt(reader, "AppTagid"),
                                TagId = DbUtils.GetInt(reader, "TagId"),
                                ApplicationId = DbUtils.GetInt(reader, "ApplicationId"),

                                Tag = new Tag()
                                {
                                    Id = DbUtils.GetInt(reader, "Id"),
                                    TagName = DbUtils.GetString(reader, "TagName")
                                }
                            };
                            appTags.Add(appTag);
                        }

                        return appTags;

                    }
                }
            }


        }


        public ApplicationTag GetApplicationTagById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, TagId, ApplicationId 
                        FROM ApplicationTag  
                        
                        Where Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        ApplicationTag appTags = null;

                        if (reader.Read())
                        {
                            ApplicationTag appTag = new ApplicationTag()
                            {
                                TagId = DbUtils.GetInt(reader, "TagId"),
                                ApplicationId = DbUtils.GetInt(reader, "ApplicationId")
                            };


                        }


                        return appTags;
                    }
                }
            }
        }
        public void AddApplicationTag(ApplicationTag appTag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO
                                        ApplicationTag (TagId, ApplicationId) 
                                        OUTPUT INSERTED.ID
                                        VALUES(@tagId, @applicationId)";

                    DbUtils.AddParameter(cmd, "@tagId", appTag.TagId);
                    DbUtils.AddParameter(cmd, "@applicationId", appTag.ApplicationId);


                    appTag.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void DeleteApplicationTag(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                     DELETE from ApplicationTag
                     WHERE Id = @id ";
                    DbUtils.AddParameter(cmd, "@id", id);

                    cmd.ExecuteNonQuery();
                }

            }
        }
    }
}
