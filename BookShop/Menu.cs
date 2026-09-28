using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BookShop
{
    static internal class Menu
    {
        public static void LoginMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.Write(
                "==== Вхід ====\n" +
                " 1. Увійти\n" +
                " 2. Зареєструватися\n" +
                " 0. Вийти\n" +
                " > ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        Auth.Login();
                        break;
                    case "2":
                        Auth.Register();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.Write(
                    "=== КНИЖКОВИЙ МАГАЗИН ===\n" +
                    " 1. Перегляд та пошук книг\n" +
                    " 2. Керування книгами (Додати / Редагувати / Видалити)\n" +
                    " 3. Продаж або Списання книги\n" +
                    " 4. Встановити акцію (знижку)\n" +
                    " 5. Відкласти книгу для покупця\n" +
                    " 6. Додати нового клієнта\n" +
                    " 7. Звіти та Аналітика (Новинки / ТОП-и)\n" +
                    " 0. Вихід\n" +
                    " > ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        BookManager.ViewAndSearchBooks();
                        break;
                    case "2":
                        ManageMenu();
                        break;
                    case "3":
                        BookManager.SellOrWriteOffBook();
                        break;
                    case "4":
                        BookManager.SetDiscount();
                        break;
                    case "5":
                        BookManager.ReserveBook();
                        break;
                    case "6":
                        BookManager.AddCustomer();
                        break;
                    case "7":
                        ReportsMenu();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public static void ManageMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.Write(
                "=== Керування книгами ===\n" +
                " 1. Додати книгу\n" +
                " 2. Редагувати книгу\n" +
                " 3. Видалити книгу\n" +
                " 0. Назад\n" +
                " > ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        BookManager.AddBook();
                        break;
                    case "2":
                        BookManager.EditBook();
                        break;
                    case "3":
                        BookManager.DeleteBook();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static public void ReportsMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.Write(
                    "=== ЗВІТИ ТА АНАЛІТИКА ===\n" +
                    " 1. Новинки (найновіші надходження)\n" +
                    " 2. ТОП найпродаваніших книг\n" +
                    " 3. ТОП популярних авторів\n" +
                    " 4. ТОП популярних жанрів\n" +
                    " 5. Загальний фінансовий звіт за період\n" +
                    " 0. Назад\n" +
                    " > ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        ReportManager.ShowNewArrivals();
                        break;
                    case "2":
                        ReportManager.ShowTopSellingBooks();
                        break;
                    case "3":
                        ReportManager.ShowTopAuthors();
                        break;
                    case "4":
                        ReportManager.ShowTopGenres();
                        break;
                    case "5":
                        ReportManager.ShowFinancialSummary();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
