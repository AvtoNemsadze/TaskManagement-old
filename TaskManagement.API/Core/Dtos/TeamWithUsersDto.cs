using System.Text.Json.Serialization;

namespace TaskManagement.API.Core.Dtos
{
    public class TeamWithUsersDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Members { get; set; }
    }
}
