namespace Application.Features.Users.Queries.GetAllUsersQuery
{
    public class GetAllUsersResponse
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }



    }
}
