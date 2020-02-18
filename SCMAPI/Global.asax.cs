using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SimpleInjector.Integration.WebApi;
using SimpleInjector;
using DALayer.Login;
using BALayer;
using DALayer.MPR;
using BALayer.MPR;
using DALayer.Emails;
using DALayer.RFQ;
using BALayer.RFQ;
using BALayer.PurchaseAuthorization;
using DALayer.PurchaseAuthorization;
namespace SCMAPI
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			var container = new Container();
            container.Register<ILoginBA, LoginBA>();
            container.Register<ILoginDA, LoginDA>();
            container.Register<IMPRBA, MPRBA>();
			container.Register<IMPRDA, MPRDA>();
			container.Register<IRFQBA, RFQBA>();
			container.Register<IRFQDA, RFQDA>();
			container.Register<IEmailTemplateDA, EmailTemplateDA>();
            container.Register<IConfigAccessInterfaceBA, ConfigAccessRepoBA>();
            container.Register<IConfigAccessInterfaceDA, ConfigAccessRepoDA>();
            container.Register<IPurchaseAuthorizationDA, PurchaseAuthorizationDA>();
            container.Register<IPurchaseAuthorizationBA, PurchaseAuthorizationBA>();
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
	}
}
