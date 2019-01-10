using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestFinal.Model
{
    public class Receipt
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string TitleReceipt { get; set; }
        public string Kind { get; set; }
        public string Category { get; set; }
        public double AmountOfMoney { get; set; }
        public DateTime DateOfReceipt { get; set; }
        public string Note { get; set; }
        public byte[] Image { get; set; }
    }
}
