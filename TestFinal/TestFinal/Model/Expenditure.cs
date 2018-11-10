using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestFinal.Model
{
    public class Expenditure
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string TitleExpenditure { get; set; }
        public string Kind { get; set; }
        public string Category { get; set; }
        public double AmountOfMoney { get; set; }
        public DateTime DateOfExpenditure { get; set; }
        public string Note { get; set; }
    }
}
