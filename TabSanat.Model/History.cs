using System;
using TabSanat.Model.Base;

namespace TabSanat.Model
{
    public class History : BaseEntity
    {

        public DateTime DateTime { get; set; }

        public string Description { get; set; }

    }
}
