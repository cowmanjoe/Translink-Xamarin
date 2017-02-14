using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Translink.PageModels;
using Translink.Services;
using Xamarin.Forms;

namespace Translink
{
    public class App : Application
    {
        

        public App()
        {
            //MainPage = new MainPage();

            //MainPage = new RoutePage(); 

            SetupIOC();

            var routeList = FreshMvvm.FreshPageModelResolver.ResolvePageModel<RouteListPageModel>();
            var navContainer = new FreshMvvm.FreshNavigationContainer(routeList);
            MainPage = navContainer; 
        }

        private void SetupIOC()
        {
            FreshMvvm.FreshIOC.Container.Register<IRouteDataService, RouteDataService>(); 
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
