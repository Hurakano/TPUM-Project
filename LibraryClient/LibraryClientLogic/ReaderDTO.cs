﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryClient.LibraryClientLogic
{
    public abstract class ReaderDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
