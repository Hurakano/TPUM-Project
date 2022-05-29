﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryClient.LibraryClientData
{
    public class Loan
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid ReaderId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
