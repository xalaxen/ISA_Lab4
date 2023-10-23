using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_Server
{
    public class Functions
    {
        public string PrintAllNotes(ref DbSet<Student> students)
        {
            string StudentsList = "";
            foreach (Student s in students)
            {
                StudentsList += "ID: " + s.Id.ToString() + " | " +
                    "ФИО: " + s.Surname.ToString() + " " + s.Name.ToString() + " " + s.Patronymic.ToString() +
                    " | Пол: " + ConvertSex(s.Sex).ToString() +
                    " | Возраст: " + s.Age.ToString() + "\n";
            }
            return StudentsList;
        }

        public string ConvertSex(bool s)
        {
            if (s == true) { return "М"; }
            else { return "Ж"; }
        }

        public string ConvertSex(string s)
        {
            if (s == "М" || s == "м") { return "true"; }
            if (s == "Ж" || s == "ж") { return "false"; }
            else { return ""; }
        }

        public string PrintNotesByNumber(int note_number, ref DbSet<Student> students)
        {
            try
            {
                Student nstudent = students.Find(note_number);
                string student = "ФИО: " + nstudent.Surname + " "
                                + nstudent.Name + " "
                                + nstudent.Patronymic + " | "
                                + "Пол: " + ConvertSex(nstudent.Sex) + " | "
                                + "Возраст: " + nstudent.Age + "\n";
                return student;
            }
            catch (Exception e) { return "Такой записи нет!\n"; }
        }

        public string RemoveNotesFromFile(int note_number)
        {
            using (StudentContext db = new StudentContext())
            {
                try
                {
                    db.Students.Remove(db.Students.Find(note_number));
                    db.SaveChanges();
                    return "";
                }
                catch (Exception e) { return "Записи с таким номром не существует!\n"; }
            }
        }

        public void AddNoteToFile(string surname, string name, string patronymic, string sex, int age)
        {
            using (StudentContext db = new StudentContext())
            {
                try
                {
                    db.Students.Add(new Student(surname, name, patronymic, Convert.ToBoolean(sex), age));
                    db.SaveChanges();
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }
    }
}
