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

namespace Baibakov_41
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage(User user)
        {
            InitializeComponent();
            if (user != null)
            {
                UserName.Text = UserName.Text + user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
                UserRole.Text = UserRole.Text + user.Role.RoleName;
            } else
            {
                UserName.Text = UserName.Text + "гость";
                UserRole.Text = UserRole.Text + "отсутствует";
            }

            var currentProducts = Baibakov_41Entities.GetContext().Product.ToList();
            CBoxType.SelectedIndex = 0;
            ascRadioBtn.IsChecked = true;

            ProductListView.ItemsSource = currentProducts;
            SomethingHasChanged();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        public void SomethingHasChanged()
        {
            var currentProducts = Baibakov_41Entities.GetContext().Product.ToList();

            if (ascRadioBtn.IsChecked == true)
            {
                currentProducts = currentProducts.OrderByDescending(p => p.ProductCost).ToList();
            } else if (descRadioBtn.IsChecked == true)
            {
                currentProducts = currentProducts.OrderBy(p => p.ProductCost).ToList();
            }

            switch (CBoxType.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    currentProducts = currentProducts.Where(p => (Convert.ToDecimal(p.ProductDiscountAmount) >= 0 && Convert.ToDecimal(p.ProductDiscountAmount) < 10)).ToList();
                    break;
                case 2:
                    currentProducts = currentProducts.Where(p => (Convert.ToDecimal(p.ProductDiscountAmount) >= 10 && Convert.ToDecimal(p.ProductDiscountAmount) < 15)).ToList();
                    break;
                case 3:
                    currentProducts = currentProducts.Where(p => (Convert.ToDecimal(p.ProductDiscountAmount) >= 15)).ToList();
                    break;
            }

            if (!string.IsNullOrWhiteSpace(TBoxSearch.Text))
            {
                currentProducts = currentProducts.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            }

            ProductListView.ItemsSource = currentProducts;

            ProductCountTextBlock.Text = currentProducts.Count.ToString() + " из " + Baibakov_41Entities.GetContext().Product.ToList().Count.ToString();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SomethingHasChanged();
        }

        private void CBoxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SomethingHasChanged();
        }


        private void ascRadioBtn_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Не работает
            SomethingHasChanged();
        }

        private void descRadioBtn_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Не работает
            SomethingHasChanged();
        }

        private void descRadioBtn_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Не работает
            SomethingHasChanged();
        }

        private void ascRadioBtn_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Не работает
            SomethingHasChanged();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (ProductListView.SelectedItems.Count >= 0)
            {
                var prod = ProductListView.SelectedItems as Product;
                selectedProducts.Add(prod);
            }
        }
    }
}
