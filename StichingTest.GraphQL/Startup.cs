using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Stitching;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace StichingTest.GraphQL
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("remote", (sp, client) =>
            {
                client.BaseAddress = new Uri("http://localhost:2000");
            });

            services.AddStitchedSchema(b => b
                .AddSchemaFromFile("remote", "./schema.graphql")
                .RenameType("remote", "Person", "User"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphQL(new QueryMiddlewareOptions
            {
                Path = "/graphql"
            });

            // force evaulation of schema for fast testing
            app.ApplicationServices.GetRequiredService<ISchema>();
        }
    }
}
