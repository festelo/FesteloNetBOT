using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FesteloNetBOT
{
    public class DataBase
    {
        public static DataBaseContext dataBase { get; private set; } = new DataBaseContext();

        public class DataBaseContext : DbContext
        {
            public DbSet<User> Users { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Filename=./data.db");
            }
        }
    }


    public class User
    {
        public int Id { get; set; }
        public int Balance { get; set; }
        public string Name { get; set; }
        public string Cookie { get; set; }
        public DateTime Time { get; set; }
        public bool Withdraw { get; set; } = false;

        public void Update(User user)
        {
            Balance = user.Balance;
            Time = user.Time;
        }
        public override string ToString()
        {
            return ToString(false);
        }
        public string ToString(bool full = false)
        {
            string ret = $"{Id}: Name: {Name} | Balance: {Balance} | Reward Date: {Time} UTC";
            if (full) ret += $"\nWithdraw: {Withdraw} | Cookie: {Cookie}\n";
            return ret;
        }
    }

}
