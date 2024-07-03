namespace NetBootcamp.Repositories.Entities.Users
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
