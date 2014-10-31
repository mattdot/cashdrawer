using CashDrawers;
using CashDrawers.APG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CashDrawersApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        CashDrawer cashDrawer;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.cashDrawer = new UsbCashDrawer();
            this.cashDrawer.StatusChanged += cashDrawer_StatusChanged;
            await this.cashDrawer.InitAsync();
            this.Status.Text = (await this.cashDrawer.GetStatusAsync()).ToString();
        }

        async void cashDrawer_StatusChanged(object sender, CashDrawerStatusEventArgs args)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                this.Status.Text = args.Status.ToString();
            });
        }

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            await this.cashDrawer.OpenAsync();
        }
    }
}
