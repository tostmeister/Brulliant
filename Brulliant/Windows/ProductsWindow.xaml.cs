using Brulliant.BD;
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
using System.Windows.Shapes;

namespace Brulliant.Windows
{

    public partial class ProductsWindow : Window
    {
        private User _loggedUser;
        private BrullEntities db = BrullEntities.GetContext();
        List<Product> Products;
        List<Product> FilteredProducts;

        public ProductsWindow(User user)
        {
            InitializeComponent();
            _loggedUser = user;
            Username.Text = user.UserFullName;
            db = BrullEntities.GetContext();
            Products = db.Product.ToList();
            FilteredProducts = Products?.ToList() ?? new List<Product>();
            ProductsListView.ItemsSource = Products;

            LoadManufacturers();
            UpdateQuantityText();

            if (_loggedUser.UserRole == 1)
            {
                AdminButtonsPanel.Visibility = Visibility.Visible;
            }
            else
            {
                AdminButtonsPanel.Visibility = Visibility.Collapsed;
            }
        }

        public ProductsWindow()
        {
            InitializeComponent();
            _loggedUser = null;
            Username.Text = "Гость";
            db = BrullEntities.GetContext();
            Products = db.Product.ToList();
            FilteredProducts = Products?.ToList() ?? new List<Product>();
            ProductsListView.ItemsSource = Products;

            LoadManufacturers();
            UpdateQuantityText();

            AdminButtonsPanel.Visibility = Visibility.Collapsed;
        }


        private void LoadManufacturers()
        {
            var manufacturers = db.Manufacturers.ToList();
            manufacturers.Insert(0, new Manufacturers { Name = "Все производители" });
            ManufacturerFilterCB.ItemsSource = manufacturers;
            ManufacturerFilterCB.DisplayMemberPath = "Name";
            ManufacturerFilterCB.SelectedIndex = 0;
        }

        private void UpdateQuantityText()
        {
            int totalProducts = Products.Count;
            int displayedProducts = ProductsListView.Items.Count;
            QuantityTB.Text = $"{displayedProducts} из {totalProducts}";
        }


        private void UserSearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ManufacturerFilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string searchText = UserSearchTB.Text.ToLower();
            var selectedManufacturer = ManufacturerFilterCB.SelectedItem as Manufacturers;
            if (selectedManufacturer == null) return;

            FilteredProducts = db.Product
               .Where(p =>
                   (string.IsNullOrEmpty(searchText) ||
                   p.ProductName.ToLower().Contains(searchText) ||
                   p.Description.ToLower().Contains(searchText) ||
                   p.Manufacturers.Name.ToLower().Contains(searchText) ||
                   p.ProductCost.ToString().Contains(searchText) ||
                   p.QuantityWarehouse.ToString().Contains(searchText))
               )
               .Where(p =>
                   selectedManufacturer.Name == "Все производители" ||
                   p.Manufacturers.Name == selectedManufacturer.Name
               )
               .ToList();

            ProductsListView.ItemsSource = FilteredProducts;
            UpdateQuantityText();
        }

        private void SortAscendingClick(object sender, RoutedEventArgs e)
        {
            FilteredProducts = FilteredProducts.OrderBy(p => p.ProductCost).ToList();
            ProductsListView.ItemsSource = FilteredProducts;
            UpdateQuantityText();
        }

        private void SortDescendingClick(object sender, RoutedEventArgs e)
        {
            FilteredProducts = FilteredProducts.OrderByDescending(p => p.ProductCost).ToList();
            ProductsListView.ItemsSource = FilteredProducts;
            UpdateQuantityText();
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы вышли из системы");
            this.Close();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            WindowAddRedact window = new WindowAddRedact();
            window.ShowDialog();
            Products = db.Product.ToList();
            ProductsListView.ItemsSource = Products;
            UpdateQuantityText();
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsListView.SelectedItem as Product;
            if (selectedProduct != null)
            {
                db.Product.Remove(selectedProduct);
                db.SaveChanges();
                Products = db.Product.ToList();
                ProductsListView.ItemsSource = Products;
                UpdateQuantityText();

            }
            else
            {
                MessageBox.Show("Выберите товар для удаления.");
            }
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsListView.SelectedItem as Product;
            if (selectedProduct != null)
            {
                WindowAddRedact window = new WindowAddRedact(selectedProduct);
                window.ShowDialog();
                Products = db.Product.ToList();
                ProductsListView.ItemsSource = Products;
                UpdateQuantityText();
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования.");
            }
        }
        private List<Product> _basket = new List<Product>();

        private void ProductsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedProduct = ProductsListView.SelectedItem as Product;
            if (selectedProduct != null)
            {
                _basket.Add(selectedProduct);
                MessageBox.Show($"Товар \"{selectedProduct.ProductName}\" добавлен в корзину.");
            }
        }

        private void BasketButton_Click(object sender, RoutedEventArgs e)
        {
            WindowBasket basketWindow = new WindowBasket(_basket);
            basketWindow.Owner = this;
            basketWindow.ShowDialog();
        }

    }
}
