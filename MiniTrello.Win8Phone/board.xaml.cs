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
    public partial class board : PhoneApplicationPage
    {
        public board()
        {
            var client = new RestClient("http://minitrelloapis.apphb.com");
            var request = new RestRequest("/boards/"+App.OrganizationId+"/"+ App.Token, Method.GET);
            request.RequestFormat = DataFormat.Json;

            //el model es el error
            var asyncHandler = client.ExecuteAsync<ReturnBoardsModel>(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.Data != null)
                    {
                        App.ListBoards = (IList<Board>)r.Data.Boards;
                        Dibujar();
                    }
                }
            });

            InitializeComponent();
        }

        private void Dibujar()
        {
            foreach (var boards in App.ListBoards)
            {
                Button btn = new Button() { Content = boards.Title };
                btn.Width = 210;
                btn.Height = 66;

                btn.Tag = boards.Id;
                btn.Foreground = new SolidColorBrush(Colors.Blue);
                btn.Click += new RoutedEventHandler(btn_Click);
                lb1.Items.Add(btn);

            }
        }
        void btn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                App.BoardId = button.Tag.ToString();
                NavigationService.Navigate(new Uri("/lane.xaml", UriKind.Relative));
            }

        }
    }
}