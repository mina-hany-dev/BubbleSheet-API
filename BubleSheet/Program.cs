using bubblesheet.Infrastracture.Data;
using bubblesheet.Infrastracture.Repos;
using BubleSheet.Services.Implementation;
using BubleSheet.Services.Interfaces;
using BubleSheet.Services.Models;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniShop.Application.Interfaces;
using System.Text;

namespace BubleSheet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Database
            builder.Services.AddDbContext<bubblesheetDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.Configure<BunnyOptions>(
    builder.Configuration.GetSection("BunnyStorage"));

            builder.Services.AddScoped<IBunnyStorageService,
                BunnyStorageService>();



            // Dependency Injection

            builder.Services.AddScoped<IAccount, AccountRepo>();
            builder.Services.AddScoped<IJwtService, JwtServiceImplementation>();

            builder.Services.AddScoped<IExam, ExamRepo>();
            builder.Services.AddScoped<IAdvertiser, AdvertiserRepo>();
            builder.Services.AddScoped<ICode, CodeRepo>();
            builder.Services.AddScoped<ITransaction, TranscationRepo>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISubject, SubjectRepo>();
            builder.Services.AddScoped<IstudentSubject, StudentSubjectRepo>();
            builder.Services.AddScoped<IQuestion, QuestionRepo>();
            builder.Services.AddScoped<IYear, YearRepo>();
            builder.Services.AddScoped<ILesson,LessonRepo>();
            builder.Services.AddScoped<IStudentLesson, StudentLessonRepo>();
            builder.Services.AddScoped<IStudentAttempts, StudentAttemptRepo>();
            builder.Services.AddScoped<IPdf,PDFRepo>();
            builder.Services.AddScoped<IQuestionBank,QuestionBankRepo>();
            builder.Services.AddScoped<IQuestionExam,QuestionExamRepo>();
            builder.Services.AddScoped<IStudentAnswer, StudentAnswerRepo>();
            builder.Services.AddScoped<IRandomExamTemplate, RandomExamTemplateRepo>();
            builder.Services.AddScoped<IImgAd, ImgAdRepo>();
            builder.Services.AddScoped<IChoices, ChoiceRepo>();
            builder.Services.AddScoped<IResetPassword, ResetPasswordRepo>();
            builder.Services.AddScoped<IStudentScore, StudentScoreRepo>();

            builder.Services.AddScoped<IRandomExamTempleteService, RandomExamTempleteService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IStudentLessonService, StudentLessonService>();
            builder.Services.AddScoped<IpdfService, PdfService>();
            builder.Services.AddScoped<IAdvertiserService, AdvertisersService>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IExamService, ExamService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<ICodeService, CodeService>();
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            builder.Services.AddScoped<ILessonService, LessonService>();
            builder.Services.AddScoped<IStudentSubjectService, StudentSubjectService>();
            builder.Services.AddScoped<IStudentAttemptService, StudentAttemptService>();
            builder.Services.AddScoped<IQuestionBankService, QuestionBankService>();
            builder.Services.AddScoped<IImgadService, AdImgService>();
            builder.Services.AddScoped<IYearService, YearService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();

            // JWT

            builder.Services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer =
                            builder.Configuration["Jwt:Issuer"],

                        ValidAudience =
                            builder.Configuration["Jwt:Audience"],

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    builder.Configuration["Jwt:Key"]!
                                ))
                    };
                });


            builder.Services.AddAuthorization();

            builder.Services.AddHttpClient();
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpContextAccessor();



            // CORS

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowFrontend", policy =>
            //    {
            //        policy
            //        .SetIsOriginAllowed(origin => true)
            //        .AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .AllowCredentials();
            //    });
            //});

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy
                        .WithOrigins(
                            "https://bubblesheet.com",
                            "https://www.bubblesheet.com"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });



            var app = builder.Build();



            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            // CORS لازم الأول
            app.UseCors("AllowFrontend");


            // مؤقتًا على Somee
            // app.UseHttpsRedirection();


            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();


            app.Run();
        }
    }
}