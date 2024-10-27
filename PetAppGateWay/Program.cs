using Mediator_BankAccount.Commands;
using Mediator_BankAccount.Services.IRepository;
using MediatoR_CurrencyRate.Queries;
using Mediator_Email.Commands;
using Mediator_Email.Services.SMTP;
using MediatR;
using Micro_Account.Commands;
using Micro_Person.Services.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using PetAppGateWay.Enams;
using PetAppGateWay.Services.Gwt;
using PetAppGateWay.Services.Middleware;
using PetAppGateWay.Services.Workers;
using System.IO.Compression;

namespace PetAppGateWay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
          
            builder.Services.AddCors();

            builder.Services.AddOutputCache(options =>
            {
                options.AddPolicy(nameof(CashPolicy.GetUser), builder =>
                {
                    builder.Expire(TimeSpan.FromSeconds(20));
                    builder.Tag(nameof(CashTag.GetUser));
                });
            });

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "local";
            });

            builder.Services.AddAuthorization(opts => { });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AddJWT.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AddJWT.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AddJWT.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            //////////////////////////////////////////////////////////////////////////////
            builder.Services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.MimeTypes = new[] { "application/json", "text/json" }; 

                options.Providers.Add<BrotliCompressionProvider>();  
                options.Providers.Add<GzipCompressionProvider>();
            });
            builder.Services.Configure<BrotliCompressionProviderOptions>(opt => opt.Level = CompressionLevel.Fastest);
            ////////////////////////////////////////////////////////////          

            builder.Services.AddScoped<IRepository_Me_User, SQLStoreDbRepository>();
            builder.Services.AddScoped<IRepository_Bank_Account, SQLStoreDbBank_Account>();

            builder.Services.AddTransient<ISMTP, SMTP>();

            builder.Services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(AddOrUpdateCommand).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(SMPTCommand).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(BankAccountCommand).Assembly);  
                configuration.RegisterServicesFromAssembly(typeof(CurrencyRateQuery).Assembly);
            });

            // Add services to the container.
            builder.Services.AddHostedService<CurrencyWorker>();
            builder.Services.AddControllers();



            var app = builder.Build();
            
            app.UseExceptionHandlerMiddleware(); //Exceptions handler

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("*"));
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
          
            
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseOutputCache();

            app.MapControllers();

            app.Run();
        }
    }    
}
