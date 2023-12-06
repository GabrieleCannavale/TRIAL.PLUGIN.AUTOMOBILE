using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRIAL.PLUGIN.AUTOMOBILE
{
    public class PreUpdate : IPlugin
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
                    Entity preImage = context.PreEntityImages["PreImage"];



                    //fumag_dataimmatricolazione
                    if (target.Contains("fumag_dataimmatricolazione"))
                    {
                        DateTime dataImm = target.GetAttributeValue<DateTime>("fumag_dataimmatricolazione");
                        tracingService.Trace($"data immatricolazione: {dataImm.Date}");
                        if (dataImm != DateTime.MinValue)
                        {
                            if (dataImm.Date > DateTime.Now.Date)
                            {
                                throw new Exception("La Data di immatricolazione deve essere MINORE a quella di oggi");
                            }
                            else 
                            {
                      
                                int cilindrata = target.Contains("fumag_cilindrata") ? 
                                    target.GetAttributeValue<int>("fumag_cilindrata") : preImage.Contains("fumag_cilindrata") ? 
                                    preImage.GetAttributeValue<int>("fumag_cilindrata") : 0;
                                if (cilindrata < 1000) 
                                {
                                    throw new Exception("la cilindrata deve essere maggiore di 999");
                                }
                            }
                        } 
                    }
                }
            }
            catch (Exception e)
            {

                throw new InvalidPluginExecutionException($"ERRORE PLUGIN PreUpdate: {e.Message}");
            }
        }
    }
}
