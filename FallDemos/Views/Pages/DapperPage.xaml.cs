using Dapper;
using FallDemos.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FallDemos.Views.Pages {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DapperPage : Page {


        // Make sure you turn on the TCP Protocol for your SQL Server!!!
        // 'Trusted_Connection=True' requires 'Enterprise Authentication' capability in Package.appxmanifest
        //private const string ConnectionString = @"Server=.\sqlexpress;Initial Catalog=Northwind;Persist Security Info=False;User ID=NorthwindUser;Password=Northwind1@;MultipleActiveResultSets=True";
        private const string ConnectionString = @"Server=.\sqlexpress;Initial Catalog=Northwind;Trusted_Connection=True;MultipleActiveResultSets=True";

        public DapperPage() {
            this.InitializeComponent();
        }

        private async void ButtonLoad_Click(object sender, RoutedEventArgs e) {
            try {

                using (var con = new SqlConnection(ConnectionString)) {
                    await con.OpenAsync();

                    // Dapper - QueryAsync()
                    var t1 = con.QueryAsync<Supplier>("select * from dbo.Suppliers order by CompanyName");
                    var t2 = con.QueryAsync<Product>("select * from dbo.Products order by ProductName");

                    await Task.WhenAll(t1, t2); // Requires MARS=True in ConnectionString

                    listViewSuppliers.ItemsSource = t1.Result;
                    listViewProducts.ItemsSource = t2.Result;
                }
            } catch (System.Exception ex) {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }
    }
}
