using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestFinal.Model
{
    public class User
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
    }
}
