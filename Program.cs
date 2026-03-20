

using Auth.Data;
//using AutoMapper;
using CS_APIServerProject.Data;
//using CS_APIServerProject.Mapping;
using CS_APIServerProject.Repository;
using CS_APIServerProject.Seed;
using CS_APIServerProject.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CS_APIServerProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.




            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Deactivating blocks of program for fixing mistakes of fundament and then returning to advandced autorizationg and other
            builder.Services.AddScoped<IFileStorage, FileStorage>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            //-----
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddOpenApi();
     
            //builder.Services.AddSingleton

            //DB
            builder.Services.AddDbContext<DataBaseContext>(opt =>
                opt.UseSqlServer(builder.Configuration.
                GetConnectionString("DefaultConnection")));

            // AutoMapper removed; using ManualMapper utility instead
                
            builder.Services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = ctx =>
                {
                    var problem = new ValidationProblemDetails(ctx.ModelState)
                    {
                        Title = "Validation failed",
                        Status = StatusCodes.Status400BadRequest,
                        Type = "https:statuses.com/400",
                        Instance = ctx.HttpContext.Request.Path
                    };
                    problem.Extensions["traceId"] = ctx.HttpContext.TraceIdentifier;
                    return new BadRequestObjectResult(problem); 
                };
            });

            // Exception handler will be configured after the app is built

            //JWT
            var jwtKey = builder.Configuration["Jwt:Key"];
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(jwtKey))
                    };
                });
            // Do not call AddAuthentication() a second time - already configured above
            builder.Services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

           builder.Services.AddIdentityCore<AppUser>()
                       .AddRoles<AppRole>()
                       .AddEntityFrameworkStores<DataBaseContext>()
                       .AddSignInManager();


            var app = builder.Build();

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    var status = ex switch
                    {
                        ArgumentNullException => StatusCodes.Status400BadRequest,
                        ArgumentException => StatusCodes.Status400BadRequest,
                        InvalidOperationException => StatusCodes.Status400BadRequest,
                        KeyNotFoundException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    context.Response.StatusCode = status;
                    var problem = new ProblemDetails
                    {
                        Title = status switch
                        {
                            400 => "Bad request",
                            404 => "NotFound",
                            409 => "Conflict",
                            _ => "Server error"

                        },
                        Status = status,
                        Type = $"https:statuses.com/{status}",
                        Instance = context.Request.Path,
                        Detail = app.Environment.IsDevelopment() ? ex.Message : null
                    };
                    problem.Extensions["traceId"] = context.TraceIdentifier;

                    await Results.Problem(problem).ExecuteAsync(context);
                });
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseAuthentication(); // MUST come before UseAuthorization
            app.UseAuthorization();

            app.UseStaticFiles();

            // seed, map controllers, run
            //await IdentitySeed.SeedAsync(app.Services);
            app.MapControllers();

            await IdentitySeed.SeedAsync(app.Services);

            app.Run();
        }
    }
}
