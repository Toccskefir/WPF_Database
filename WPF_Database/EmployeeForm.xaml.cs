using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_Database
{
    /// <summary>
    /// Interaction logic for EmployeeForm.xaml
    /// </summary>
    public partial class EmployeeForm : Window
    {
        private EmployeeService employeeService;
        private Employee employeeUpdate;

        public EmployeeForm(EmployeeService employeeService)
        {
            InitializeComponent();
            this.employeeService = employeeService;
        }

        public EmployeeForm(EmployeeService employeeService, Employee employeeUpdate)
        {
            InitializeComponent();
            this.employeeService = employeeService;
            this.employeeUpdate = employeeUpdate;
            this.buttonAdd.Visibility = Visibility.Collapsed;
            this.buttonUpdate.Visibility = Visibility.Visible;

            textBoxName.Text = employeeUpdate.Name;
            textBoxAge.Text = employeeUpdate.Age.ToString();
            textBoxSalary.Text = employeeUpdate.Salary.ToString();
            radioButtonMale.IsChecked = employeeUpdate.Gender == "férfi";
            radioButtonFemale.IsChecked = employeeUpdate.Gender == "nő";
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Employee employee = createEmployeeFromInputData();
                if (employeeService.Create(employee))
                {
                    MessageBox.Show("Sikeres hozzáadás!");
                    textBoxName.Text = "";
                    textBoxAge.Text = "";
                    textBoxSalary.Text = "";
                    radioButtonMale.IsChecked = false;
                    radioButtonFemale.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("Hiba történt hozzáadás során!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Employee employee = createEmployeeFromInputData();
                if (employeeService.Update(this.employeeUpdate.Id, employee))
                {
                    MessageBox.Show("Sikeres módosítás!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Hiba történt a módosítás során! Javasoljuk az ablak bezárását!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Employee createEmployeeFromInputData()
        {
            string name = textBoxName.Text.Trim();
            string gender = radioButtonMale.IsChecked == true ? "férfi" : "nő";
            string ageText = textBoxAge.Text.Trim();
            string salaryText = textBoxSalary.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Név megadása kötelező!");
            }
            if (!(bool)radioButtonMale.IsChecked && !(bool)radioButtonFemale.IsChecked)
            {
                throw new Exception("Nem kiválasztása kötelező!");
            }
            if (string.IsNullOrEmpty(ageText))
            {
                throw new Exception("Életkor megadása kötelező!");
            }
            if (!int.TryParse(ageText, out int age))
            {
                throw new Exception("Életkor csak szám lehet!");
            }
            if (string.IsNullOrEmpty(salaryText))
            {
                throw new Exception("Fizetés megadása kötelező!");
            }
            if (!int.TryParse(salaryText, out int salary))
            {
                throw new Exception("Fizetés csak szám lehet!");
            }
            if (age < 18 || age > 150)
            {
                throw new Exception("Életkor 18 és 150 közötti lehet!");
            }
            if (salary < 0)
            {
                throw new Exception("Fizetés nem lehet negatív!");
            }

            Employee employee = new Employee();
            employee.Name = name;
            employee.Gender = gender;
            employee.Age = age;
            employee.Salary = salary;
            return employee;
        }
    }
}
