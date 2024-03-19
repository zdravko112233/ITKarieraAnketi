using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Threading.Tasks;

namespace ITKarieraAnketi
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=sql11.freesqldatabase.com;database=sql11692795;user=sql11692795;password=j9ky13rSlb;port=3306;", new MySqlServerVersion(new Version(5, 5, 62)));
        }
    }
}
