using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink;


namespace Translink.PageModels
{
    public class RoutePageModel : FreshMvvm.FreshBasePageModel
    {

        public Route Route { get; set; }

        public RoutePageModel ()
        {

        }

        public override void Init (object initData)
        {
            base.Init(initData);

            Route = initData as Route;
        }
    }
}
