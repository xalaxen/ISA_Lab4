using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_Server
{
    internal class StudentContext:DbContext
    {
        public StudentContext() : base("DBConnection") { }
        public DbSet<Student> Students { get; set; }
    }
}
