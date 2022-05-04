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

                        List<Tag> appTags = new List<Tag>();

                        while (reader.Read())
                        {
                            Tag tag = new Tag()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                TagName = DbUtils.GetString(reader, "TagName")
                            };

                            appTags.Add(tag);
                        }

                        return appTags;

                    }
                }
            }


        }
    }
}
