using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MiniTrello.Api.Models;
using MiniTrello.Domain.Entities;
using RestSharp;

namespace MiniTrello.Win8Phone
{
    public partial class lane : PhoneApplicationPage
    {
        public lane()
        {
            var client = new RestClient("http://minitrelloapis.apphb.com");
            var request = new RestRequest("/lines/" + App.BoardId + "/" + App.Token, Method.GET);
            request.RequestFormat = DataFormat.Json;

            //el model es el error
            var asyncHandler = client.ExecuteAsync<ReturnLinesModel>(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.Data != null)
                    {
                        App.ListLanes = (IList<Lines>)r.Data.Lines;
                        Dibujar();
                    }
                }
            });
            InitializeComponent();
        }

        private void Dibujar()
        {
            foreach (var lanes in App.ListLanes)
            {
                Button btn = new Button() { Content = lanes.Title };
                btn.Width = 210;
                btn.Height = 66;

                btn.Tag = lanes.Id;
                btn.Foreground = new SolidColorBrush(Colors.Brown);
                btn.Click += new RoutedEventHandler(btn_Click);
                lb1.Items.Add(btn);

            }
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                App.LineId= button.Tag.ToString();
                NavigationService.Navigate(new Uri("/card.xaml", UriKind.Relative));
            }

        }
    }
}