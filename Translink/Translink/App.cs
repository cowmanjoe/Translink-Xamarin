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
            SetupIOC();

            SetupTabbedNav(); 
        }

        private void SetupIOC()
        {
            FreshMvvm.FreshIOC.Container.Register<IRouteDataService, RouteDataService>();
            FreshMvvm.FreshIOC.Container.Register<IDepartureDataService, DepartureSearcher>(); 
        }
        
        private void SetupTabbedNav()
        {
            var tabbedNav = new FreshMvvm.FreshTabbedNavigationContainer();
            tabbedNav.AddTab<RouteListPageModel>("Routes", null);
            tabbedNav.AddTab<StopSearchPageModel>("Stop Search", null);
            MainPage = tabbedNav; 
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
