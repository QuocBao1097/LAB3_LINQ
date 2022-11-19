using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LAB3_CAU2
{
    public class Program
    {
        //public static void cau1(DBNorthwindDataContext db)
        //{
        //    var query = db.Products.Where(p => p.UnitsInStock == 0);

        //    foreach (var item in query)
        //    {
        //        item.UnitsInStock = 20;
        //    }
        //    db.SubmitChanges();
        //}


        //ĐỀ BÀI YÊU CẦU CHỈ VIẾT CÂU TRUY VẤN LINQ (từ 3 đến 25)
        public static void cau3(DBNorthwindDataContext db)
        {
            //var query = from p in db.Products
            //              where p.ProductName.StartsWith("G")
            //              select p.ProductName;

            var query = db.Products.Where(p => p.ProductName.StartsWith("G")).Select(p=>p.ProductName);

            foreach (var item in query)
            {
                Console.WriteLine("ProductName: "+item);
            }
        }
        public static void cau4(DBNorthwindDataContext db)
        {
            var query =   from p in db.Products join s in db.Suppliers on p.SupplierID equals s.SupplierID
                          where s.CompanyName == "Tokyo Traders"
                          select p.ProductName;

            foreach (var item in query)
            {
                Console.WriteLine("ProductName: " + item);
            }
        }

        public static void cau5(DBNorthwindDataContext db)
        {
            db.Customers.Where(c => c.City == "Berlin").Select(c => c.ContactName).ToList().ForEach(info => Console.WriteLine("ContactName: " + info));
        }

        public static void cau6(DBNorthwindDataContext db)
        {

            var query = from p in db.Products
                        join c in db.Categories on p.CategoryID equals c.CategoryID
                        where c.CategoryName == "Meat/Poultry"
                        select new
                        {
                            ID = p.ProductID,
                            Name = p.ProductName
                        };

            foreach (var item in query)
            {
                Console.WriteLine($"ProductID: {item.ID}\t ProductName: {item.Name}");
            }
        }


        public static void cau7(DBNorthwindDataContext db)
        {
            var query = from o in db.Orders
                        where o.ShipCountry == "Germany"
                        select new
                        {
                            OrderDate = o.OrderDate,
                            ShipName = o.ShipName
                        };
            foreach(var item in query)
            {
                Console.WriteLine($"Ngày đặt hàng: {item.OrderDate}\t ShipName: {item.ShipName}");
            }
        }


        public static void cau8(DBNorthwindDataContext db)
        {
            var query =   from p in db.Products
                          where p.UnitsOnOrder == 0
                          select new
                          {
                              ProductID = p.ProductID,
                              ProductName = p.ProductName
                          };
            foreach(var item in query)
            {
                Console.WriteLine($"ProductID:{item.ProductID}\t ProductName: {item.ProductName} ");
            }
        }

        public static void cau9(DBNorthwindDataContext db)
        {
            var query = from s in db.Suppliers
                        join p in db.Products on s.SupplierID equals p.SupplierID
                        where p.UnitPrice >= 10 && p.UnitsInStock >= 10
                        group s by new { s.CompanyName, s.ContactName } into gr
                        select gr;
                               
            Console.WriteLine("Liệt kê Supplier cung cấp 10 sản phẩm trở lên");
            query.ToList().ForEach(gr =>
            {
                Console.WriteLine($"{gr.Key}");
            });
        }

        public static void cau10(DBNorthwindDataContext db)
        {
            
            var queryString1 = from p in db.Products
                               select new
                               {
                                   ProductName = p.ProductName,
                                   UnitPrice = p.UnitPrice,
                                   UnitsInStock = p.UnitsInStock
                               };

            var queryString2 = from p in db.Products
                               join o in db.Order_Details on p.ProductID equals o.ProductID
                               select new
                               {
                                   ProductName = p.ProductName,
                                   UnitPrice = p.UnitPrice,
                                   UnitsInStock = p.UnitsInStock
                               };

            var queryString3 = queryString1.Except(queryString2); 
            
            int count = 0;
            foreach (var item in queryString3)
            {
                count++;
                Console.WriteLine($"ProductName: {item.ProductName}\tUnitPrice:{item.UnitPrice}\tUnitsInStock:{item.UnitsInStock}");
            }

            if (count == 0)
            {
                Console.WriteLine("Chưa có product nào chưa được bán");
            }
        }

        //11. Liệt kê các Supplier (CompanyName) chưa cung cấp Product nào
        public static void cau11(DBNorthwindDataContext db)
        {
            var queryStirng1 = from c in db.Suppliers
                              select c.CompanyName;

            var queryStirng2 = from p in db.Products join c in db.Suppliers on p.SupplierID equals c.SupplierID
                               select c.CompanyName;

            var queryString3 = queryStirng1.Except(queryStirng2);

            int count = 0;
            foreach (var item in queryString3)
            {
                count++;
                Console.WriteLine($"CompanyName: {item}");
            }

            if (count == 0)
            {
                Console.WriteLine("Chưa có Supplier (CompanyName) nào chưa được cung cấp");
            }
        }


        //12. Tính doanh thu năm 1996
        public static void cau12(DBNorthwindDataContext db)
        {
            var queryString1 = from o in db.Order_Details
                                select new
                                {
                                    MaOrder = o.OrderID,
                                    Giatri = o.Quantity * o.UnitPrice * (Decimal)(1 - o.Discount)
                                };

            var queryString2 =  (from o2 in queryString1
                                 join o in db.Orders on o2.MaOrder equals o.OrderID
                                 where o.OrderDate.Value.Year == 1996
                                 select o2.Giatri).Sum();

            Console.WriteLine($"Doanh thu: {queryString2}");
        }

        //13.Đếm số product của từng category (CategoryName, Quantity Pro)
        public static void cau13(DBNorthwindDataContext db)
        {
            var query = from p in db.Products
                        join c in db.Categories on p.CategoryID equals c.CategoryID
                        group c by c.CategoryName into gr
                        let sl = "Số lượng SP: " + gr.Count()
                        select new
                        {
                            CategoryName = gr.Key,
                            SoLuong = sl
                        };

            query.ToList().ForEach(i => Console.WriteLine($"{i.CategoryName}--->{i.SoLuong}"));
        }

        //14. Đếm số Product của từng Supplier (CompanyName, QuantityPro).
        public static void cau14(DBNorthwindDataContext db)
        {
            var query = from p in db.Products
                        join c in db.Suppliers on p.SupplierID equals c.SupplierID
                        group c by c.CompanyName into gr
                        let sl = "Số lượng SP: " + gr.Count()
                        select new
                        {
                            CompanyName = gr.Key,
                            SoLuong = sl
                        };

            query.ToList().ForEach(i => Console.WriteLine($"{i.CompanyName}\t{i.SoLuong}"));

        }
        //15. Cho biết Order (CustomerID, OrderDate) có trị giá (UnitPrice * Quantity và Discount) lớn nhất.

        //16. Cho biết Customer(ContactName) có Order có trị giá cao nhất.
        public static void Cau16(DBNorthwindDataContext db)
        {
            var items = from o in db.Orders
                        join od in db.Order_Details on o.OrderID equals od.OrderID
                        group od by true into g
                        select new
                        {
                            Max = g.Max(i => ((double)((i.UnitPrice * i.Quantity) - (i.UnitPrice * i.Quantity * ((decimal)i.Discount)))))
                        };
            double max = 0;
            foreach (var item in items)
            {
                max = item.Max;
            }
            var orders = from o in db.Orders
                         join od in db.Order_Details on o.OrderID equals od.OrderID
                         join ct in db.Customers on o.CustomerID equals ct.CustomerID
                         where ((double)((od.UnitPrice * od.Quantity) - (od.UnitPrice * od.Quantity * ((decimal)od.Discount)))) == max
                         select new
                         {
                             ContactName = ct.ContactName,
                             Max = ((double)((od.UnitPrice * od.Quantity) - (od.UnitPrice * od.Quantity * ((int)od.Discount))))
                         };
            foreach (var order in orders)
            {
                Console.WriteLine($"{order.ContactName}-{order.Max}");
            }
        }


        //17. Đếm số lần mua hàng của Customer (ContactName, Quantity).
        public static void cau17(DBNorthwindDataContext db)
        {
            var query = from c in db.Customers
                        join o in db.Orders on c.CustomerID equals o.CustomerID
                        group c by c.ContactName into gr
                        let dem = gr.Count()
                        select new
                        {
                            ContactName = gr.Key,
                            sl = dem
                        };
            query.ToList().ForEach(i => Console.WriteLine($"ContactName: {i.ContactName} Quantity:{i.sl}"));
        }


        //18. Tìm Customer (ContactName) có số lần mua hàng nhiều nhất.
        public static void Cau18(DBNorthwindDataContext db)
        {
            var kq1 = from o in db.Orders
                      group o by o.CustomerID into gr
                      select new
                      {
                          key = gr.Key,
                          count = gr.Count()
                      };

            var kq2 = from c in db.Customers
                      join k in kq1 on c.CustomerID equals k.key
                      where k.count == kq1.Max(i => i.count)
                      select c.ContactName;

            foreach (var i in kq2)
            {
                Console.WriteLine(i);
            }
        }
        //19. Tìm Employee chưa lập Order nào.
        public static void cau19(DBNorthwindDataContext db)
        {
            var queryString1 = from e in db.Employees
                               select e.EmployeeID.ToString();

            var queryString2 = from o in db.Orders join e in db.Employees on o.EmployeeID equals e.EmployeeID
                               select o.EmployeeID.ToString();

            var queryString3 = queryString1.Except(queryString2);

            foreach(var item in queryString3)
            {
                Console.WriteLine(item);
            }
        }
        //20. Tính tổng số lượng bán ra của mỗi Product (ProductName, Quantity) trong năm 1996.
        public static void Cau20(DBNorthwindDataContext db)
        {
            var items = from o in db.Orders
                        join od in db.Order_Details on o.OrderID equals od.OrderID
                        join p in db.Products on od.ProductID equals p.ProductID
                        where o.OrderDate.Value.Year == 1996
                        group od by new { p.ProductName, p.ProductID } into g
                        select g;


            foreach (var item in items)
            {
                Console.WriteLine($"{item.Key.ProductName}-{item.Sum(g => g.Quantity)}");
            }
        }
        //21. Liệt kê Supplier theo từng City (CompanyName)
        public static void Cau21(DBNorthwindDataContext db)
        {
            var items = from sp in db.Suppliers
                        select new { City = sp.City, CompanyName = sp.CompanyName };

            items = items.OrderBy(x => x.City);
            foreach (var item in items)
            {
                Console.WriteLine($"{item.City}-{item.CompanyName}");
            }
        }
        //22. Cho biết 3 Customer có doanh số cao nhất(ContactName, Sales).
        public static void Cau22(DBNorthwindDataContext db)
        {
            var items = from o in db.Orders
                        join od in db.Order_Details on o.OrderID equals od.OrderID
                        join ct in db.Customers on o.CustomerID equals ct.CustomerID
                        group od by ct.ContactName into g
                        select g;
            items = items.OrderByDescending(item => item.Sum(i => ((i.UnitPrice * i.Quantity) - (i.UnitPrice * i.Quantity * ((decimal)i.Discount))))).Take(3);
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Key}-{item.Sum(i => ((i.UnitPrice * i.Quantity) - (i.UnitPrice * i.Quantity * ((decimal)i.Discount))))}");
            }
        }

        //23.ính doanh số bán hàng của từng tháng trong năm 1996.
        public static void Cau23(DBNorthwindDataContext db)
        {
            var items = from o in db.Orders
                        join od in db.Order_Details on o.OrderID equals od.OrderID
                        where o.OrderDate.Value.Year == 1996
                        group od by o.OrderDate.Value.Month into g
                        select g;
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Key}-{item.Sum(i => ((i.UnitPrice * i.Quantity) - (i.UnitPrice * i.Quantity * ((decimal)i.Discount))))}");
            }
        }
        //24. Tính doanh thu của từng Product trong năm 1996
        public static void Cau24(DBNorthwindDataContext db)
        {
            var items = from o in db.Orders
                        join od in db.Order_Details on o.OrderID equals od.OrderID
                        join p in db.Products on od.ProductID equals p.ProductID
                        where o.OrderDate.Value.Year == 1996
                        group od by p.ProductName into g
                        select g;

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Key}-{item.Sum(i => ((i.UnitPrice * i.Quantity) - (i.UnitPrice * i.Quantity * ((decimal)i.Discount))))}");
            }
        }
        //25. Tính tổng số tiền vận chuyển hàng (Freight - Order) đến từng nước
        public static void Cau25(DBNorthwindDataContext db)
        {
            var items = from o in db.Orders
                        group o by o.ShipCountry into g
                        select g;

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Key}-{item.Sum(i => i.Freight)}");
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            DBNorthwindDataContext db = new DBNorthwindDataContext();
            //cau1(db);
            //cau2(db);
            //cau3(db);
            //cau4(db);
            //cau5(db);
            //cau6(db);
            //cau7(db);
            //cau8(db);
            //cau9(db);
            //cau10(db);
            //cau11(db);
            //cau12(db);
            //cau13(db);
            //cau14(db);
            //cau15(db);
            //Cau16(db);
            //cau17(db);
            //Cau18(db);
            //cau19(db);
            //Cau20(db);
            //Cau21(db);
            //Cau22(db);
            //Cau23(db);
            //Cau24(db);
            Cau25(db);
            Console.ReadKey();
        }
    }
}
