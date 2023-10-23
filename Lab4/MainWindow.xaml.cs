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
        private bool isErrorDisplayed = false;
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
            if (viewClient.StudentsList != null)
            {
                if (viewClient.CheckStudents())
                {
                    viewClient.SaveStudentListData();
                }
                else
                {
                    MessageBox.Show("Данные небыли сохранены!\nЗаполните или удалите красные строчки!\n\nПоля: фамили и имя обязательны!\nВозраст должен быть больше 0!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Для начала надо загрузить данные!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
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
