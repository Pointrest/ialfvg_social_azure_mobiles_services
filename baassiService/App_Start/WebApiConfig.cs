﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using baassiService.DataObjects;
using baassiService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security.Providers;

namespace baassiService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();
            options.LoginProviders.Remove(typeof(AzureActiveDirectoryLoginProvider));
            options.LoginProviders.Add(typeof(AzureActiveDirectoryExtendedLoginProvider));

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));
            //config.SetIsHosted(true); // jeust if we want to debug locally

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;


            options.PushAuthorization =
                Microsoft.WindowsAzure.Mobile.Service.Security.AuthorizationLevel.User;
            
            Database.SetInitializer(new baassiInitializer());
        }
    }

    public class baassiInitializer : ClearDatabaseSchemaIfModelChanges<baassiContext>
    {
        protected override void Seed(baassiContext context)
        {
            /*List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }*/

            base.Seed(context);
        }
    }
}

