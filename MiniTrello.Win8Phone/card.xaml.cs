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
    public partial class card : PhoneApplicationPage
    {
        public card()
        {
            var client = new RestClient("http://minitrelloapis.apphb.com");
            var request = new RestRequest("/cards/" + App.LineId + "/" + App.Token, Method.GET);
            request.RequestFormat = DataFormat.Json;

            //el model es el error
            var asyncHandler = client.ExecuteAsync<ReturnCardsModel>(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.Data != null)
                    {
                        App.ListCards = (IList<Cards>)r.Data.Cards;
                        Dibujar();
                    }
                }
            });
            InitializeComponent();
        }
        private void Dibujar()
        {
            foreach (var cards in App.ListCards)
            {
                Button btn = new Button() { Content = cards.Text };
                btn.Width = 400;
                btn.Height = 70;
                btn.Foreground = new SolidColorBrush(Colors.Orange);
                lb1.Items.Add(btn);
            }
        }

    }
}