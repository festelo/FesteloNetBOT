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
        public string Cookie { get; set; }
        public string Time { get; set; }
        public bool Withdraw { get; set; }
    }

}
