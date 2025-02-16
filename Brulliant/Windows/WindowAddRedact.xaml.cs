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

    public partial class WindowAddRedact : Window
    {
        private BrullEntities db = BrullEntities.GetContext();
        private Product _product;

        public WindowAddRedact(Product product = null)
        {
            InitializeComponent();
            _product = product;


            ManufacturerCB.ItemsSource = db.Manufacturers.ToList();
            SupplierCB.ItemsSource = db.Suppliers.ToList();
            CategoryCB.ItemsSource = db.Categories.ToList();

            if (_product != null)
            {
                ArticleTB.Text = _product.ProductArticleNumber;
                NameTB.Text = _product.ProductName;
                PriceTB.Text = _product.ProductCost.ToString();
                MaxDiscountTB.Text = _product.ProductMaxDiscount.ToString();
                ManufacturerCB.SelectedItem = _product.Manufacturers;
                SupplierCB.SelectedItem = _product.Suppliers;
                CategoryCB.SelectedItem = _product.Categories;
                DiscountTB.Text = _product.ProductCurrentDiscount.ToString();
                QuantityTB.Text = _product.QuantityWarehouse.ToString();
                DescriptionTB.Text = _product.Description;
            }
        }

        private void AddRedactButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_product == null)
                {
                    _product = new Product
                    {
                        ProductArticleNumber = ArticleTB.Text,
                        ProductName = NameTB.Text,
                        ProductCost = int.Parse(PriceTB.Text),
                        ProductMaxDiscount = int.Parse(MaxDiscountTB.Text),
                        Manufacturers = (Manufacturers)ManufacturerCB.SelectedItem,
                        Suppliers = (Suppliers)SupplierCB.SelectedItem,
                        Categories = (Categories)CategoryCB.SelectedItem,
                        ProductCurrentDiscount = int.Parse(DiscountTB.Text),
                        QuantityWarehouse = int.Parse(QuantityTB.Text),
                        Description = DescriptionTB.Text,
                        ProductUnit = "шт.",
                        Image = "/images/picture.png"
                    };
                    db.Product.Add(_product);
                }
                else
                {
                    _product.ProductArticleNumber = ArticleTB.Text;
                    _product.ProductName = NameTB.Text;
                    _product.ProductCost = int.Parse(PriceTB.Text);
                    _product.ProductMaxDiscount = int.Parse(MaxDiscountTB.Text);
                    _product.Manufacturers = (Manufacturers)ManufacturerCB.SelectedItem;
                    _product.Suppliers = (Suppliers)SupplierCB.SelectedItem;
                    _product.Categories = (Categories)CategoryCB.SelectedItem;
                    _product.ProductCurrentDiscount = int.Parse(DiscountTB.Text);
                    _product.QuantityWarehouse = int.Parse(QuantityTB.Text);
                    _product.Description = DescriptionTB.Text;
                }

                db.SaveChanges();
                MessageBox.Show("Данные сохранены успешно!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
