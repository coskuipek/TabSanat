using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace TabSanat.Overloads
{
    public class ListMinimumAttribute : ValidationAttribute
    {
        private readonly int _minElements;
        public ListMinimumAttribute(int minElements)
        {
            _minElements = minElements;
        }
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count >= _minElements;
            }
            return false;
        }
    }

}
