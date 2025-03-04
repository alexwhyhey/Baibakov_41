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

namespace Baibakov_41
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {

        List<OrderProduct> selectedOrderProducts = new List<OrderProduct>();
        List<Product> selectedProducts = new List<Product>();
        
        private Order currentOrder = new Order();
        private OrderProduct currentOrderProduct = new OrderProduct();


        public OrderWindow(List<OrderProduct> selectedOrderProducts, List<Product> selectedProducts, string ClientName)
        {
            InitializeComponent();
            var currentPickUps = Baibakov_41Entities.GetContext().PickUpPoint.ToList();

            PickUpPointComboBox.ItemsSource = currentPickUps;
            ClientNameTextBlock.Text = ClientName;
            OrderNumberTextBlock.Text = (Baibakov_41Entities.GetContext().Order.ToList().Count + 1).ToString();

            ProductListView.ItemsSource = selectedProducts;
            foreach (var p in selectedProducts)
            {
                p.ProductQuantityInStock = 1;

                foreach (var q in selectedOrderProducts)
                {
                    if (p.ProductArticleNumber == q.ProductArticleNumber)
                        p.ProductQuantityInStock = q.ProductCount;
                }
            }

            this.selectedOrderProducts = selectedOrderProducts;
            this.selectedProducts = selectedProducts;
            CreateDatePicker.Text = DateTime.Now.ToString();
            SetDeliveryDate();

        }

        private void SetDeliveryDate()
        {
            throw new NotImplementedException();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
