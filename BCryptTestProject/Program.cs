using System;
using System.Linq;

namespace BCryptTestProject
{
    public class Program
    {
        static void Main(string[] args)
        {
            string _username = "username";
            string _password = "password";


            using(var context = new BCryptTestEntities())
            {
                //add a new account to the db
                //string salt = BCrypt.Net.BCrypt.GenerateSalt();
                //string saltedpw = BCrypt.Net.BCrypt.HashPassword(_password, salt);
                //context.Accounts.Add(new Account() {Id = 0, username = _username, password = saltedpw, salt = salt });
                //context.SaveChanges();

                //verifying a user in the database

                var result = context.Accounts.First(e => e.username == _username);

                if (result == null)
                {
                    Console.WriteLine("Invalid username/password combination");
                    return;
                }

                var salt = result.salt;
                var password = result.password;
                var saltedpw = BCrypt.Net.BCrypt.HashPassword(_password, salt);

                if (saltedpw.Equals(password))
                {
                    Console.WriteLine("Login succesful");
                }else
                {
                    Console.WriteLine("Login failed");
                }
               
            }
            Console.WriteLine("Success?");
        }
    }
}
