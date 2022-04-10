using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationDataLayer
{
    public class Reader
    {
        public string Name { get; set; }
        public uint Age { get; set; }
        public string Address { get; set; }

        public Reader(string _name, uint _age, string _address)
        {
            Name = _name;
            Age = _age;
            Address = _address;
        }
    }
}
