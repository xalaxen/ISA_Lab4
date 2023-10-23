using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class Student : IDataErrorInfo
    {
        private bool inEdit = false;
        private Student tmpStudent;
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

        public string this[string columnName]
        {
            get
            {
                switch(columnName)
                {
                    case "Surname":
                        if (string.IsNullOrEmpty(Surname))
                        {
                            return "Cell Surname must be filled!";
                        }
                        break;
                    case "Name":
                        if(string.IsNullOrEmpty(Name))
                        {
                            return "Cell Name must be filled!";
                        }
                        break;
                    case "Age":
                        if(Age == 0)
                        {
                            return "Age can not be 0!";
                        }
                        break;
                }
                return "";
            }
        }

        public string Error => null;
        /*{
            get
            {
                string error;
                StringBuilder sb = new StringBuilder();

                error = this["Surname"];
                if(error != string.Empty)
                {
                    sb.AppendLine(error);
                }

                error = this["Name"];
                if (error != string.Empty)
                {
                    sb.AppendLine(error);
                }

                error = this["Age"];
                if (error != string.Empty)
                {
                    sb.AppendLine(error);
                }
                return "";
            }
        }*/
    }
}
