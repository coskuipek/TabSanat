using System;

namespace TabSanat.Model.Base
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public bool IsDeleted { get; set; }
    }
}
