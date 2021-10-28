using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class Editorial
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
