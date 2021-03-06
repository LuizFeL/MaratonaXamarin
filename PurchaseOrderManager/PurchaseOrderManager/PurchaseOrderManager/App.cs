﻿using PurchaseOrderManager.Model;
using PurchaseOrderManager.Pages;
using PurchaseOrderManager.Storage;
using Xamarin.Forms;

namespace PurchaseOrderManager
{
    public class App : Application
    {
        public static Login CurrentUser { get; set; }

        public App()
        {
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
