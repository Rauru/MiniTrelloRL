using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MiniTrello.Api.Models;
using MiniTrello.Domain.Entities;
using RestSharp;
using System.Windows.Media;

namespace MiniTrello.Win8Phone
{
    public partial class organization : PhoneApplicationPage
    {
        public organization()
        {
            var client = new RestClient("http://minitrelloapis.apphb.com");
            var request = new RestRequest("/organizations/" + App.Token, Method.GET);
            request.RequestFormat = DataFormat.Json;

            //el model es el error
            var asyncHandler = client.ExecuteAsync<ReturnOrganizationsModel>(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.Data != null)
                    {
                        App.ListOrganizations = (IList<Organization>) r.Data.Organizations;
                        Dibujar();
                    }
                }
            });
            InitializeComponent();
        }

        private void Dibujar()
        {
            foreach (var organizations in App.ListOrganizations)
            {
                Button btn = new Button() { Content = organizations.Title };
                btn.Width = 210;
                btn.Height = 66;
              
                btn.Tag = organizations.Id;
                btn.Foreground = new SolidColorBrush(Colors.Green);
                btn.Click += new RoutedEventHandler(btn_Click);
                lb1.Items.Add(btn);
               
            }

        }
        void btn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                App.OrganizationId =button.Tag.ToString();
                NavigationService.Navigate(new Uri("/board.xaml", UriKind.Relative));
            }
                
        }
    }
}