using System;

namespace Cloud_Lab.Entities.DTO
{
    public class Portfolio
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}