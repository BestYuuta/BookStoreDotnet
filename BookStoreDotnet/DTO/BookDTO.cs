﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDotnet.DTO
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string BookCover { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
