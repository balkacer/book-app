using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
