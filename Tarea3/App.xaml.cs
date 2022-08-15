﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Tarea3.Views;

namespace Tarea3
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new HomePage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
