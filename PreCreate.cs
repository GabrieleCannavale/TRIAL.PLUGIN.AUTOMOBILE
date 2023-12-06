using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRIAL.PLUGIN.AUTOMOBILE
{
    public class PreCreate :IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService =
              (ITracingService)serviceProvider.GetService(typeof(ITracingService)); 

            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity target = (Entity)context.InputParameters["Target"];
                    
                    //fumag_dataimmatricolazione
                    if (target.Contains("fumag_dataimmatricolazione"))
                    {
                        DateTime dataImm = target.GetAttributeValue<DateTime>("fumag_dataimmatricolazione");
                        tracingService.Trace($"data immatricolazione: {dataImm.Date}");
                        if (dataImm != DateTime.MinValue)
                        {
                            if (dataImm.Date < DateTime.Now.Date)
                            {
                                throw new Exception("La Data di immatricolazione deve essere maggiore o uguale a quella di oggi");
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {

                throw new InvalidPluginExecutionException($"ERRORE PLUGIN PreCreate: {e.Message}");
            }
        }
    }
}
