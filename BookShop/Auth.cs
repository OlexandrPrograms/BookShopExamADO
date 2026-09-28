using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop
{
    static internal class Auth
    {
        public static void Login()
        {
            using (var context = new ADOBookShopEntities())
            {
                Console.Write("Введіть логін: ");
                string login = Console.ReadLine() ?? "";
                Console.Write("Введіть пароль: ");
                string password = Console.ReadLine() ?? "";
                var user = context.Users.FirstOrDefault(u => u.Username == login && u.PasswordHash == password);
                if (user == null)
                {
                    Console.WriteLine("Невірний логін або пароль!");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine($"Вхід успішний! Ласкаво просимо, {user.Username}!");
                Console.ReadKey();
                Menu.MainMenu();
            }
        }

        public static void Register()
        {
            using (var context = new ADOBookShopEntities())
            {
                Console.Write("Введіть логін: ");
                string login = Console.ReadLine() ?? "";
                Console.Write("Введіть пароль: ");
                string password = Console.ReadLine() ?? "";
                var existingUser = context.Users.FirstOrDefault(u => u.Username == login);
                if (existingUser != null)
                {
                    Console.WriteLine("Користувач з таким логіном вже існує!");
                    Console.ReadKey();
                    return;
                }

                var newUser = new Users { Username = login, PasswordHash = password };
                context.Users.Add(newUser);
                context.SaveChanges();
                Console.WriteLine("Реєстрація успішна! Тепер ви можете увійти.");
            }
            Console.WriteLine("Натисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }
    }
}
