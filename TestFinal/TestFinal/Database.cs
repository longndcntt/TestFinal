using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TestFinal.Model;

namespace TestFinal
{
    public class Database
    {
        //lấy thư mục lưu trữ csdl trên hệ thống
        string folder = System.Environment.GetFolderPath
            (System.Environment.SpecialFolder.Personal);
        public bool createDatabase()
        {
            try
            {
                //tạo csdl
                using (var connection = new
                    SQLiteConnection(System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    //tạo 2 bang
                    connection.CreateTable<Expenditure>();
                    connection.CreateTable<Receipt>();
                    connection.CreateTable<User>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                //Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        #region Receipt
        //Sort Receipt by Money
        public List<Receipt> SortReceiptByMoney()
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    return connection.Table<Receipt>().OrderBy(p => p.AmountOfMoney).ToList<Receipt>();
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                //Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        //Sort Receipt by date
        public List<Receipt> SortReceiptByDate()
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    return connection.Table<Receipt>().OrderBy(p => p.DateOfReceipt).ToList<Receipt>();
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                //Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        //Xóa Receipt
        public bool DeleteReceipt(Receipt receipt)
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    connection.Delete(receipt);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                //Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        //Lọc Receipt theo ngày tháng năm
        public List<Receipt> SelectReceiptByDate(DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    return connection.Table<Receipt>().
                        Where(p=> p.DateOfReceipt >= fromDate && p.DateOfReceipt <= toDate).ToList<Receipt>();
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                //Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        //Thêm thu
        public bool InsertReceipt(Receipt loai)
        {
            try
            {
                using (var connection = new
                    SQLiteConnection(System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    connection.Insert(loai);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                //   Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        //Xuất danh sác thu
        public List<Receipt> SelectReceipt()
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    return connection.Table<Receipt>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                //Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        //Tim theo category va note
        public List<Receipt> SearchReceiptByContent(string content)
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    var list = connection.Table<Receipt>().Where(p => p.Category.Contains(content) ||
                                                                        p.Note.Contains(content)).ToList();
                    return list;
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }
        //Tim theo Id
        public Receipt SearchReceiptById(int id)
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    var receipt = connection.Table<Receipt>().Where(p => p.Id == id).SingleOrDefault();
                    return receipt;
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }
        //Edit Receipt
        public bool UpdateReceipt(Receipt re)
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    connection.Update(re);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }

        #endregion

        #region Expenditure
        //Lọc Receipt theo ngày tháng năm
        public List<Expenditure> SelectExpenditureByDate(DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    return connection.Table<Expenditure>().
                        Where(p => p.DateOfExpenditure >= fromDate && p.DateOfExpenditure <= toDate).ToList<Expenditure>();
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                //Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        //Xóa Receipt
        public bool DeleteExpenditure(Expenditure expenditure)
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    connection.Delete(expenditure);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                //Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        //Tim theo Id
        public Expenditure SearchExpenditureById(int id)
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    var expenditure = connection.Table<Expenditure>().Where(p => p.Id == id).SingleOrDefault();
                    return expenditure;
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }
        //Sort Expenditure by Money
        public List<Expenditure> SortExpenditureByMoney()
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    return connection.Table<Expenditure>().OrderBy(p => p.AmountOfMoney).ToList<Expenditure>();
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                //Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        //Sort Expenditure by date
        public List<Expenditure> SortExpenditureByDate()
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    return connection.Table<Expenditure>().OrderBy(p => p.DateOfExpenditure).ToList<Expenditure>();
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                //Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        //Tim theo category va note
        public List<Expenditure> SearchExpenditureByContent(string content)
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    var list = connection.Table<Expenditure>().Where(p => p.Category.Contains(content) ||
                                                                        p.Note.Contains(content)).ToList<Expenditure>();
                    return list;
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }
        //Lọc Receipt theo ngày tháng năm
        public List<Expenditure> SelectExpendituretByDate(DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    return connection.Table<Expenditure>().
                        Where(p => p.DateOfExpenditure >= fromDate && p.DateOfExpenditure <= toDate).ToList<Expenditure>();
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                //Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        //Edit chi
        public bool UpdateExpenditure(Expenditure expenditure)
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    connection.Update(expenditure);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }
        //Thêm chi
        public bool InsertExpenditure(Expenditure h)
        {
            try
            {
                using (var connection = new
                    SQLiteConnection(System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    connection.Insert(h);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }
        //Xuất danh sách chi
        public List<Expenditure> SelectExpenditure()
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    return connection.Table<Expenditure>().ToList<Expenditure>();
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }
        //Xoa danh sach chi
        public bool DeleteExpenditure(List<Expenditure> lst)
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    foreach (var item in lst)
                    {
                        connection.Delete<Expenditure>(item.Id);
                    }
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }
        #endregion

        #region User
        //Thêm User (sign up)
        public bool InsertUser(User us)
        {
            try
            {
                using (var connection = new
                    SQLiteConnection(System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    connection.Insert(us);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                //   Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        //Đếm số User hợp lệ (login)
        public int CountUsers(string username, string password)
        {
            try
            {
                using (var connection = new SQLiteConnection
                    (System.IO.Path.Combine(folder, "Accountant.db")))
                {
                    return connection.Table<User>().Where(p => p.UserName == username && p.Password == password).Count();
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.Message);
                return -1;
            }
        }
        #endregion
    }
}
