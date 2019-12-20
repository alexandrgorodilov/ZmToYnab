using System;

namespace ZmToYnab.Models
{
    public struct ZmToYnabCategoriesMapping
    {
        public Guid ZmCategoriesId { get; set; }
        public Guid YnabCategoriesId { get; set; }
        public string CategoryName { get; set; }
    }
}
