using System.Collections.Generic;

namespace TabSanat.ViewModels.Identity
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
            Claims = new List<RoleClaim>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public List<RoleClaim> Claims { get; set; }
    }
    public class RoleClaim
    {
        public string ClaimType { get; set; }
        public bool IsSelected { get; set; }
    }

}
