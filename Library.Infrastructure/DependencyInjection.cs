using FluentValidation;
using Library.Application.Abstractions;
using Library.Application.DTOs;
using Library.Application.Validation;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ✅ AutoMapper (DI extension)
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<Library.Application.Mapping.MappingProfile>();
            }, AppDomain.CurrentDomain.GetAssemblies());

            // FluentValidation
            services.AddScoped<IValidator<AuthorCreateDto>, AuthorCreateValidator>();
            services.AddScoped<IValidator<AuthorUpdateDto>, AuthorUpdateValidator>();
            services.AddScoped<IValidator<BookCreateDto>, BookCreateValidator>();
            services.AddScoped<IValidator<BookUpdateDto>, BookUpdateValidator>();
            services.AddScoped<IValidator<MemberCreateDto>, MemberCreateValidator>();
            services.AddScoped<IValidator<MemberUpdateDto>, MemberUpdateValidator>();
            services.AddScoped<IValidator<LoanCreateDto>, LoanCreateValidator>();
            services.AddScoped<IValidator<LoanReturnDto>, LoanReturnValidator>();

            return services;
        }
    }
}
