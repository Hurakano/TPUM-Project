﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryClient.LibraryClientData
{
    public abstract class Reader
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
