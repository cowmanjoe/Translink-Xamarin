﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.PageModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Translink.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouriteRoutePage : BasePage
    {
        public FavouriteRoutePage()
        {
            InitializeComponent();
            DepartureList.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "OnAppearing"); 
        }
    }
}
