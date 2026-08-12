using System;
using BCrypt.Net;

namespace KursovyiProject
{
    public static class PasswordHasher
    {
        private const int WarFactor = 13;
        private const string Pepper = "82Gfm?#!>85H";

        public static string Hash(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Пароль не введено!");
            }

            string pepper = password + Pepper;
            return BCrypt.Net.BCrypt.HashPassword(pepper, WarFactor);
        }

        public static bool Verify(string password, string hash)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            string pepper = password + Pepper;
            return BCrypt.Net.BCrypt.Verify(pepper, hash);
        }
    }
}
