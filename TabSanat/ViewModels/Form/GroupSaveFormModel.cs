using System;
using System.Collections.Generic;

namespace TabSanat.ViewModels.Form
{
    public class GroupSaveFormModel
    {
        public string NewName { get; set; }
        public Guid CourseId { get; set; }

        public List<GroupItem> GroupItems { get; set; }

    }
    public class GroupItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool DeleteThis { get; set; }
    }

}
