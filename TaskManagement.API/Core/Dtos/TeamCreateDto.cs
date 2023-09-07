namespace TaskManagement.API.Core.Dtos
{
    public class TeamCreateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<int> MemberIds { get; set; }
    }
}
