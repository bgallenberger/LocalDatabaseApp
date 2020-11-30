using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace LocalDatabaseApp
{
    public class Fact
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string TheFact { get; set; }
    }
}

