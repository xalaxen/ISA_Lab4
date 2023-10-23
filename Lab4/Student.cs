using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class Student
    {
        public int Id { get; set; }

        private string surname;
        private string name;
        private string patronymic;
        private bool sex;
        private int age;

        public Student() { }

        [Required]
        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }
        
        [Required]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Patronymic
        {
            get { return patronymic; }
            set { patronymic = value; }
        }

        public bool Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        [Required]
        public int Age
        {
            get { return age; }
            set { age = value; }
        }
    }
}
