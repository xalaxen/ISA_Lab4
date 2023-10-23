using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab4
{
    public partial class MainWindow : Window
    {
        private Client viewClient;
        public MainWindow()
        {
            InitializeComponent();
            viewClient = new Client();
            DataContext = viewClient;
        }

        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            viewClient.SendMessage("Load");
            studentsGrid.AutoGeneratingColumn += studentsGrid_AutoGeneratingColumn;
            viewClient.FillStudentsList();            
        }

        private void SaveData_Click(object sender, RoutedEventArgs e)
        {
            if(viewClient.StudentsList != null)
            {
                viewClient.SaveStudentListData();
            }
            else
            {
                MessageBox.Show("Для начала надо загрузить данные!");
            }
        }

        private void RemoveData_Click(object sender, RoutedEventArgs e)
        {
            // получаем строку в которой находится кнопка
            Button btn = (Button)sender;
            DataGridRow row = (DataGridRow)studentsGrid.ItemContainerGenerator.ContainerFromItem(btn.DataContext);
            Student studentData = (Student)row.Item;
            // удаляем
            viewClient.RemoveStudentNote(studentData.Id);
        }

        //CellEditEnding="studentsGrid_CellEditEnding" 
        // и надо потом доделать ограничения добавления записей по хорошему
        private void studentsGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var editedStudent = e.Row.Item as Student;

            foreach (var propertyInfo in typeof(Student).GetProperties())
            {
                if (propertyInfo.GetCustomAttribute<RequiredAttribute>() != null)
                {
                    var originalValue = propertyInfo.GetValue(editedStudent);

                    if (originalValue == null || string.IsNullOrEmpty(originalValue.ToString()))
                    {
                        MessageBox.Show($"Колонка {propertyInfo.Name} должна быть обязательно заполнена.");
                        e.Cancel = true;
                        break;
                    }
                }
            }
        }


        //скрытие айдишников в интерфейсе
        private void studentsGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Id")
            {
                e.Cancel = true;
            }
        }
    }
}
