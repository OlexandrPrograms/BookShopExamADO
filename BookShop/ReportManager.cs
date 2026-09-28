using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop
{
    internal class ReportManager
    {
        public static void ShowNewArrivals()
        {
            using (var context = new ADOBookShopEntities())
            {
                var newBooks = context.Books
                    .OrderByDescending(b => b.PublishYear)
                    .ThenByDescending(b => b.Id)
                    .Take(10)
                    .ToList();

                Console.WriteLine("\n=== ТОП-10 НОВИНОК ===\n");
                foreach (var b in newBooks)
                {
                    Console.WriteLine($"ID: {b.Id,-4} | \"{b.Title}\" ({b.PublishYear} р.) — {b.Author} | Ціна: {b.SalePrice} грн");
                }
            }
            Pause();
        }

        public static void ShowTopSellingBooks()
        {
            using (var context = new ADOBookShopEntities())
            {
                var topBooks = context.Sales
                    .Where(s => s.OperationType == "Sale")
                    .GroupBy(s => s.BookId)
                    .Select(g => new
                    {
                        BookId = g.Key,
                        TotalQuantity = g.Sum(s => s.Quantity)
                    })
                    .OrderByDescending(x => x.TotalQuantity)
                    .Take(5)
                    .ToList();

                Console.WriteLine("\n=== ТОП-5 НАЙПРОДАВАНІШИХ КНИГ ===\n");
                foreach (var item in topBooks)
                {
                    var book = context.Books.FirstOrDefault(b => b.Id == item.BookId);
                    string title = book != null ? book.Title : "Книгу видалено";
                    Console.WriteLine($"Продано: {item.TotalQuantity,4} шт. | ID: {item.BookId,-4} | \"{title}\"");
                }
            }
            Pause();
        }

        public static void ShowTopAuthors()
        {
            using (var context = new ADOBookShopEntities())
            {
                var topAuthors = context.Sales
                    .Where(s => s.OperationType == "Sale")
                    .Join(context.Books, s => s.BookId, b => b.Id, (s, b) => new { s.Quantity, b.Author })
                    .GroupBy(x => x.Author)
                    .Select(g => new
                    {
                        Author = g.Key,
                        TotalSold = g.Sum(x => x.Quantity)
                    })
                    .OrderByDescending(a => a.TotalSold)
                    .Take(5)
                    .ToList();

                Console.WriteLine("\n=== ТОП-5 ПОПУЛЯРНИХ АВТОРІВ ===\n");
                foreach (var a in topAuthors)
                {
                    Console.WriteLine($"Продано примірників: {a.TotalSold,4} | Автор: {a.Author}");
                }
            }
            Pause();
        }

        public static void ShowTopGenres()
        {
            using (var context = new ADOBookShopEntities())
            {
                var topGenres = context.Sales
                    .Where(s => s.OperationType == "Sale")
                    .Join(context.Books, s => s.BookId, b => b.Id, (s, b) => new { s.Quantity, b.Genre })
                    .GroupBy(x => x.Genre)
                    .Select(g => new
                    {
                        Genre = g.Key,
                        TotalSold = g.Sum(x => x.Quantity)
                    })
                    .OrderByDescending(g => g.TotalSold)
                    .Take(5)
                    .ToList();

                Console.WriteLine("\n=== ТОП-5 ПОПУЛЯРНИХ ЖАНРІВ ===\n");
                foreach (var g in topGenres)
                {
                    Console.WriteLine($"Продано примірників: {g.TotalSold,4} | Жанр: {g.Genre}");
                }
            }
            Pause();
        }

        public static void ShowFinancialSummary()
        {
            Console.Write("Введіть період у днях для аналізу (наприклад, 30): ");
            if (!int.TryParse(Console.ReadLine(), out int days) || days <= 0)
            {
                Console.WriteLine("Некоректна кількість днів!");
                Pause();
                return;
            }

            DateTime startDate = DateTime.Now.AddDays(-days);

            using (var context = new ADOBookShopEntities())
            {
                var salesPeriod = context.Sales
                    .Where(s => s.CreatedAt >= startDate)
                    .ToList();

                int totalSalesCount = salesPeriod.Where(s => s.OperationType == "Sale").Sum(s => s.Quantity);
                int totalWriteOffs = salesPeriod.Where(s => s.OperationType == "WriteOff").Sum(s => s.Quantity);
                decimal totalRevenue = salesPeriod.Where(s => s.OperationType == "Sale").Sum(s => s.Quantity * s.PricePerItem);

                Console.WriteLine($"\n=== ФІНАНСОВИЙ ЗВІТ ЗА ОСТАННІ {days} ДНІВ ===");
                Console.WriteLine($"Усього продано книг:  {totalSalesCount} шт.");
                Console.WriteLine($"Усього списано книг:  {totalWriteOffs} шт.");
                Console.WriteLine($"Загальна виручка:     {totalRevenue:F2} грн");
            }
            Pause();
        }

        private static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб повернутися...");
            Console.ReadKey();
        }
    }
}
