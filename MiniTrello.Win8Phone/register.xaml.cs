using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MiniTrello.Api.Models;
using RestSharp;


namespace MiniTrello.Win8Phone
{
    public partial class Register : PhoneApplicationPage
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var model = new AccountRegisterModel();
            model.FirstName = FirstName.Text;
            model.LastName = LastName.Text;
            model.Email = Email.Text;
            model.Password = Password.Password;
            model.ConfirmPassword = ConfirmPassword.Password;
            var client = new RestClient("http://minitrelloapis.apphb.com");
            var request = new RestRequest("/register", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(model);
            var asyncHandler = client.ExecuteAsync<ReturnModel>(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.Data != null)
                    {
                        NavigationService.Navigate(new Uri("/login.xaml", UriKind.Relative));
                    }
                }
            });
                


        }


    }
}