﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace xxAMIDOxx.xxSTACKSxx.Provider.PactTests.Fixtures
{
    public class ProviderStateMiddleware
    {
        private const string ConsumerName = "GenericMenuConsumer";
        private readonly RequestDelegate _next;
        private readonly IDictionary<string, Action> _providerStates;

        public ProviderStateMiddleware(RequestDelegate next)
        {
            _next = next;
            _providerStates = new Dictionary<string, Action>
            {
                {
                    "There is no data",
                    RemoveAllData
                },
                {
                    "There is data",
                    AddData
                }
                ,
                {
                    "Existing menu",
                    AddData
                }
            };
        }

        private void RemoveAllData()
        {
            //string path = Path.Combine(Directory.GetCurrentDirectory(), @"../../../../data");
            //var deletePath = Path.Combine(path, "somedata.txt");

            //if (File.Exists(deletePath))
            //{
            //    File.Delete(deletePath);
            //}
        }

        private void AddData()
        {
            //string path = Path.Combine(Directory.GetCurrentDirectory(), @"../../../../data");
            //var writePath = Path.Combine(path, "somedata.txt");

            //if (!File.Exists(writePath))
            //{
            //    File.Create(writePath);
            //}
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value == "/provider-states")
            {
                this.HandleProviderStatesRequest(context);
                await context.Response.WriteAsync(String.Empty);
            }
            else
            {
                await this._next(context);
            }
        }

        private void HandleProviderStatesRequest(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            if (context.Request.Method.ToUpper() == HttpMethod.Post.ToString().ToUpper() &&
                context.Request.Body != null)
            {
                string jsonRequestBody = String.Empty;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    jsonRequestBody = reader.ReadToEnd();
                }

                var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

                //A null or empty provider state key must be handled
                if (providerState != null && !String.IsNullOrEmpty(providerState.State) &&
                    providerState.Consumer == ConsumerName)
                {
                    _providerStates[providerState.State].Invoke();
                }
            }
        }
    }
}
