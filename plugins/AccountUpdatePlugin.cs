using System;
using Microsoft.Xrm.Sdk;

namespace D365Plugins
{
    public class PostAccountUpdate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                tracingService.Trace("PostAccountUpdate: Plugin execution started.");

                // Validate input parameters
                if (!context.InputParameters.Contains("Target"))
                {
                    tracingService.Trace("PostAccountUpdate: Target parameter not found in InputParameters.");
                    throw new InvalidPluginExecutionException("Target parameter is required for account updates.");
                }

                Entity account = (Entity)context.InputParameters["Target"];
                tracingService.Trace($"PostAccountUpdate: Account ID = {account.Id}");

                // Retrieve the updated account to access all fields
                Entity updatedAccount = service.Retrieve("account", account.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                tracingService.Trace($"PostAccountUpdate: Retrieved account with name = {updatedAccount.GetAttributeValue<string>("name")}");

                // Log relevant account attributes
                string accountName = updatedAccount.GetAttributeValue<string>("name") ?? "N/A";
                string accountNumber = updatedAccount.GetAttributeValue<string>("accountnumber") ?? "N/A";
                string revenue = updatedAccount.GetAttributeValue<decimal?>("revenue")?.ToString() ?? "N/A";

                tracingService.Trace($"PostAccountUpdate: Account updated - Name: {accountName}, Number: {accountNumber}, Revenue: {revenue}");

                // Audit logging - store update details in account description
                string updateLog = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Account updated by user {context.UserId}. Updated fields recorded.\n";
                string currentDescription = updatedAccount.GetAttributeValue<string>("description") ?? "";
                
                if (!string.IsNullOrWhiteSpace(currentDescription))
                {
                    updateLog = currentDescription + updateLog;
                }

                Entity updateEntity = new Entity("account", account.Id);
                updateEntity["description"] = updateLog;
                service.Update(updateEntity);

                tracingService.Trace("PostAccountUpdate: Account update log written successfully.");
                tracingService.Trace("PostAccountUpdate: Plugin execution completed successfully.");
            }
            catch (Exception ex)
            {
                tracingService.Trace($"PostAccountUpdate: Exception occurred - {ex.Message}");
                tracingService.Trace($"PostAccountUpdate: Stack trace - {ex.StackTrace}");
                throw new InvalidPluginExecutionException($"An error occurred in PostAccountUpdate: {ex.Message}", ex);
            }
        }
    }
}
