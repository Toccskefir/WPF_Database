using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Database
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EmployeeService employeeService;

        public MainWindow()
        {
            InitializeComponent();
            employeeService = new EmployeeService();
            Read();
        }

        private void Read()
        {
            dataGridEmployees.ItemsSource = employeeService.GetAll();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            EmployeeForm form = new EmployeeForm(employeeService);
            form.Closed += (_, _) => Read();
            form.ShowDialog();
        }

        private void buttonModify_Click(object sender, RoutedEventArgs e)
        {
            Employee selected = dataGridEmployees.SelectedItem as Employee;
            if (selected == null)
            {
                MessageBox.Show("Módosításhoz válasszon ki egy dolgozót!");
                return;
            }

            EmployeeForm form = new EmployeeForm(employeeService, selected);
            form.Closed += (_, _) => Read();
            form.ShowDialog();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            Employee selected = dataGridEmployees.SelectedItem as Employee;
            if (selected == null)
            {
                MessageBox.Show("Törléshez válasszon ki egy dolgozót!");
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Biztosan törölni szeretné az alábbi dolgozót: {selected.Name}?",
                "Törlés", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                if (employeeService.Delete(selected.Id))
                {
                    MessageBox.Show("Sikeres törlés!");
                }
                else
                {
                    MessageBox.Show("Sikertelen törlés!");
                }
                Read();
            }
        }
    }
}