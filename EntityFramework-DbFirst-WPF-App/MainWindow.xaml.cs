using DbFirst_Logic_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EntityFramework_DbFirst_WPF_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            var employee = new Employee
            {
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Salary = int.Parse(txtSalary.Text.Trim()),
                Address = txtAddress.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Password.Trim(),
            };

            SaveUser(employee);
            MessageBox.Show("Data saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            ResetControls();
            LoadData();
        }

        EFCodeFirstEntities db = new EFCodeFirstEntities();
        private void SaveUser(Employee user)
        {

            //using (var context = new EmployeeDataContext())
            //{
            // Check if the user already exists in the database

            var existingUser = db.Employees.Find(user.Id);
            if (existingUser != null)
            {
                // Update existing user
                db.Entry(existingUser).CurrentValues.SetValues(user);
            }
            else
            {
                // Add new user
                db.Employees.Add(user);
            }

            // Save changes
            db.SaveChanges();
            //}



        }


        private void LoadData()
        {
            var data = db.Employees.ToList();

            dgData.ItemsSource = data;
        }


        private Employee _selectedUser;
        private void dgData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (sender is DataGrid grid && grid.SelectedItem is Employee employee)
            {
                _selectedUser = employee;
                txtFirstName.Text = employee.FirstName;
                txtLastName.Text = employee.LastName;
                txtSalary.Text = Convert.ToString(employee.Salary);
                txtAddress.Text = employee.Address;
                txtEmail.Text = employee.Email;
                txtPassword.Password = employee.Password;

            }
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser != null)
            {
                _selectedUser.FirstName = txtFirstName.Text;
                _selectedUser.LastName = txtLastName.Text;
                _selectedUser.Salary = int.Parse(txtSalary.Text);
                _selectedUser.Address = txtAddress.Text;
                _selectedUser.Email = txtEmail.Text;
                _selectedUser.Password = txtPassword.Password;

                SaveUser(_selectedUser);
                MessageBox.Show("Data Updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshDataGrid();
                ResetControls();
            }
        }

        private void btnDeleteData_Click(object sender, RoutedEventArgs e)
        {
            // Get the button that was clicked
            Button btn = sender as Button;
            if (btn != null && btn.DataContext is Employee employeeToDelete)
            {
                // Ask for confirmation
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Delete the user from the database
                    //using (var context = new EmployeeDataContext())
                    //{
                    db.Employees.Remove(employeeToDelete);
                    db.SaveChanges();
                    //}

                    // Update the DataGrid
                    RefreshDataGrid();
                }
            }
        }

        private void RefreshDataGrid()
        {
            dgData.ItemsSource = null;
            LoadData();

        }

        private void ResetControls()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtSalary.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
        }
    }
}
