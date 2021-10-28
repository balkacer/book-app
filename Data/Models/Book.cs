using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public int EditorialId { get; set; }

        public virtual Author Author { get; set; }
        public virtual Category Category { get; set; }
        public virtual Editorial Editorial { get; set; }
    }
}
