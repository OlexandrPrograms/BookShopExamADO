using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop
{
    static internal class BookManager
    {
        public static void AddBook()
        {
            using (var context = new ADOBookShopEntities())
            {
                Console.WriteLine("\n=== ДОДАВАННЯ НОВОЇ КНИГИ ===");

                Console.Write("Назва книги: ");
                string title = Console.ReadLine() ?? "";

                Console.Write("ПІБ Автора: ");
                string author = Console.ReadLine() ?? "";

                Console.Write("Видавництво: ");
                string publisher = Console.ReadLine() ?? "";

                Console.Write("Жанр: ");
                string genre = Console.ReadLine() ?? "";

                Console.Write("Кількість сторінок: ");
                int.TryParse(Console.ReadLine(), out int pages);

                Console.Write("Рік видання: ");
                int.TryParse(Console.ReadLine(), out int year);

                Console.Write("Себевартість: ");
                decimal.TryParse(Console.ReadLine(), out decimal costPrice);

                Console.Write("Ціна для продажу: ");
                decimal.TryParse(Console.ReadLine(), out decimal salePrice);

                Console.Write("Кількість на складі: ");
                int.TryParse(Console.ReadLine(), out int quantity);

                Console.Write("Це продовження іншої книги? (Введіть ID першої частини або Enter якщо ні): ");
                string parentInput = Console.ReadLine();
                int? parentBookId = int.TryParse(parentInput, out int pId) ? pId : (int?)null;

                var newBook = new Books
                {
                    Title = title,
                    Author = author,
                    Publisher = publisher,
                    Genre = genre,
                    PagesCount = pages,
                    PublishYear = year,
                    CostPrice = costPrice,
                    SalePrice = salePrice,
                    Quantity = quantity,
                    ParentBookId = parentBookId
                };

                context.Books.Add(newBook);
                context.SaveChanges();
                Console.WriteLine("Книгу успішно додано!");
            }
            Console.WriteLine("Натисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        public static void EditBook()
        {
            using (var context = new ADOBookShopEntities())
            {
                PrintBooks();
                Console.Write("Введіть ID книги для редагування: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Некоректний ID!");
                    Console.ReadKey();
                    return;
                }

                var book = context.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    Console.WriteLine("Книгу не знайдено!");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine($"\nРедагування книги \"{book.Title}\" (залиште поле порожнім, щоб не змінювати значення):");

                Console.Write($"Назва [{book.Title}]: ");
                string title = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(title)) book.Title = title;

                Console.Write($"Автор [{book.Author}]: ");
                string author = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(author)) book.Author = author;

                Console.Write($"Видавництво [{book.Publisher}]: ");
                string publisher = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(publisher)) book.Publisher = publisher;

                Console.Write($"Жанр [{book.Genre}]: ");
                string genre = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(genre)) book.Genre = genre;

                Console.Write($"Кількість сторінок [{book.PagesCount}]: ");
                string pagesStr = Console.ReadLine();
                if (int.TryParse(pagesStr, out int pages)) book.PagesCount = pages;

                Console.Write($"Рік видання [{book.PublishYear}]: ");
                string yearStr = Console.ReadLine();
                if (int.TryParse(yearStr, out int year)) book.PublishYear = year;

                Console.Write($"Себевартість [{book.CostPrice}]: ");
                string costStr = Console.ReadLine();
                if (decimal.TryParse(costStr, out decimal cost)) book.CostPrice = cost;

                Console.Write($"Ціна продажу [{book.SalePrice}]: ");
                string saleStr = Console.ReadLine();
                if (decimal.TryParse(saleStr, out decimal sale)) book.SalePrice = sale;

                Console.Write($"Кількість [{book.Quantity}]: ");
                string qtyStr = Console.ReadLine();
                if (int.TryParse(qtyStr, out int qty)) book.Quantity = qty;

                context.SaveChanges();
                Console.WriteLine("Дані книги успішно оновлено!");
            }
            Console.WriteLine("Натисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        public static void DeleteBook()
        {
            using (var context = new ADOBookShopEntities())
            {
                PrintBooks();
                Console.Write("Введіть ID книги для видалення: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Некоректний ID!");
                    Console.ReadKey();
                    return;
                }

                var book = context.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    Console.WriteLine("Книгу не знайдено!");
                    Console.ReadKey();
                    return;
                }

                context.Books.Remove(book);
                context.SaveChanges();
                Console.WriteLine("Книгу успішно видалено!");
            }
            Console.WriteLine("Натисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        public static void ViewAndSearchBooks()
        {
            using (var context = new ADOBookShopEntities())
            {
                Console.Write("Введіть назву, автора або жанр для пошуку (або Enter для перегляду всіх): ");
                string query = (Console.ReadLine() ?? "").Trim().ToLower();

                var books = context.Books
                    .Where(b => string.IsNullOrEmpty(query) ||
                                (b.Title != null && b.Title.ToLower().Contains(query)) ||
                                (b.Author != null && b.Author.ToLower().Contains(query)) ||
                                (b.Genre != null && b.Genre.ToLower().Contains(query)))
                    .ToList();

                if (!books.Any())
                {
                    Console.WriteLine("Книг не знайдено!");
                    Console.ReadKey();
                    return;
                }

                PrintBooks(books);
            }
            Console.WriteLine("Натисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        public static void SellOrWriteOffBook()
        {
            using (var context = new ADOBookShopEntities())
            {
                PrintBooks();
                Console.Write("Введіть ID книги: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Некоректний ID!");
                    Console.ReadKey();
                    return;
                }

                var book = context.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    Console.WriteLine("Книгу не знайдено!");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Введіть кількість: ");
                if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
                {
                    Console.WriteLine("Некоректна кількість!");
                    Console.ReadKey();
                    return;
                }

                if (book.Quantity < qty)
                {
                    Console.WriteLine($"Недостатньо книг на складі! Доступно: {book.Quantity}");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Оберіть операцію (1 - Sale, 2 - WriteOff): ");
                string typeChoice = Console.ReadLine() ?? "";

                string opType = typeChoice == "2" ? "WriteOff" : "Sale";

                decimal discount = book.DiscountPercent ?? 0m;
                decimal finalPricePerItem = opType == "Sale"
                    ? book.SalePrice * (1m - (discount / 100m))
                    : 0m;

                book.Quantity -= qty;

                var sale = new Sales
                {
                    BookId = book.Id,
                    OperationType = opType,
                    Quantity = qty,
                    PricePerItem = finalPricePerItem,
                    CreatedAt = DateTime.Now
                };

                context.Sales.Add(sale);
                context.SaveChanges();

                Console.WriteLine($"Операцію '{opType}' успішно проведено! Залишок на складі: {book.Quantity}");
            }
            Console.WriteLine("Натисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        public static void SetDiscount()
        {
            using (var context = new ADOBookShopEntities())
            {
                PrintBooks();
                Console.Write("Введіть ID книги: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Некоректний ID!");
                    Console.ReadKey();
                    return;
                }

                var book = context.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    Console.WriteLine("Книгу не знайдено!");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Введіть відсоток знижки (наприклад, 10) або натисніть Enter щоб видалити акцію: ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    book.DiscountPercent = null;
                    Console.WriteLine("Акцію для цієї книги скасовано!");
                }
                else if (decimal.TryParse(input, out decimal discount) && discount >= 0 && discount <= 100)
                {
                    book.DiscountPercent = discount;
                    Console.WriteLine($"Встановлено знижку {discount}% для книги \"{book.Title}\"!");
                }
                else
                {
                    Console.WriteLine("Некоректний відсоток знижки!");
                    Console.ReadKey();
                    return;
                }

                context.SaveChanges();
            }
            Console.WriteLine("Натисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        public static void ReserveBook()
        {
            using (var context = new ADOBookShopEntities())
            {
                PrintBooks();
                Console.Write("Введіть ID книги: ");
                if (!int.TryParse(Console.ReadLine(), out int bookId))
                {
                    Console.WriteLine("Некоректний ID книги!");
                    Console.ReadKey();
                    return;
                }

                var book = context.Books.FirstOrDefault(b => b.Id == bookId);
                if (book == null)
                {
                    Console.WriteLine("Книгу не знайдено!");
                    Console.ReadKey();
                    return;
                }

                ViewCustomers();
                Console.Write("Введіть ID клієнта: ");
                if (!int.TryParse(Console.ReadLine(), out int customerId))
                {
                    Console.WriteLine("Некоректний ID клієнта!");
                    Console.ReadKey();
                    return;
                }

                var customer = context.Customers.FirstOrDefault(c => c.Id == customerId);
                if (customer == null)
                {
                    Console.WriteLine("Клієнта з таким ID не знайдено!");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Введіть кількість для резерву: ");
                if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
                {
                    Console.WriteLine("Некоректна кількість!");
                    Console.ReadKey();
                    return;
                }

                if (book.Quantity < qty)
                {
                    Console.WriteLine($"Недостатньо книг на складі для відкладення! Доступно: {book.Quantity}");
                    Console.ReadKey();
                    return;
                }

                var reservation = new Reservations
                {
                    BookId = book.Id,
                    CustomerId = customer.Id,
                    Quantity = qty,
                    ReservedAt = DateTime.Now,
                    Status = "Active"
                };
                book.Quantity -= qty;

                context.Reservations.Add(reservation);
                context.SaveChanges();

                Console.WriteLine($"Книгу \"{book.Title}\" успішно відкладено для клієнта {customer.FullName}!");
            }
            Console.WriteLine("Натисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        public static void PrintBooks(List<Books> books = null)
        {
            using (var context = new ADOBookShopEntities())
            {
                books = books ?? context.Books.ToList();

                Console.WriteLine("\n=== СПИСОК КНИГ ===");

                if (!books.Any())
                {
                    Console.WriteLine("Список книг порожній.");
                    return;
                }

                foreach (var b in books)
                {
                    string promo = b.DiscountPercent.HasValue && b.DiscountPercent > 0
                        ? $" [АКЦІЯ -{b.DiscountPercent}%]"
                        : "";

                    Console.WriteLine($"ID: {b.Id} | \"{b.Title}\" ({b.PublishYear} р.) — {b.Author}");
                    Console.WriteLine($"   Жанр: {b.Genre} | Видавництво: {b.Publisher} | Стор: {b.PagesCount}");
                    Console.WriteLine($"   Ціна: {b.SalePrice} грн{promo} | На складі: {b.Quantity} шт.");
                    if (b.ParentBookId.HasValue)
                    {
                        Console.WriteLine($"   * Продовження книги з ID: {b.ParentBookId}");
                    }
                    Console.WriteLine(new string('-', 50));
                }
            }
        }

        public static void AddCustomer()
        {
            using (var context = new ADOBookShopEntities())
            {
                Console.WriteLine("\n=== РЕЄСТРАЦІЯ НОВОГО КЛІЄНТА ===");

                Console.Write("ПІБ Клієнта: ");
                string fullName = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(fullName))
                {
                    Console.WriteLine("ПІБ не може бути порожнім!");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Телефон: ");
                string phone = Console.ReadLine() ?? "";

                Console.Write("Email: ");
                string email = Console.ReadLine() ?? "";

                var newCustomer = new Customers
                {
                    FullName = fullName,
                    Phone = phone,
                    Email = email
                };

                context.Customers.Add(newCustomer);
                context.SaveChanges();

                Console.WriteLine($"Клієнта успішно додано! Присвоєно ID: {newCustomer.Id}");
            }
            Console.WriteLine("Натисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        public static void ViewCustomers()
        {
            using (var context = new ADOBookShopEntities())
            {
                var customers = context.Customers.ToList();

                if (!customers.Any())
                {
                    Console.WriteLine("Клієнтів не знайдено");
                    return;
                }

                Console.WriteLine("\n=== СПИСОК КЛІЄНТІВ ===");
                foreach (var c in customers)
                {
                    Console.WriteLine($"ID: {c.Id,-4} | ПІБ: {c.FullName,-25} | Тел: {c.Phone,-13} | Email: {c.Email}");
                }
                Console.WriteLine(new string('-', 70));
            }
        }
    }
}
