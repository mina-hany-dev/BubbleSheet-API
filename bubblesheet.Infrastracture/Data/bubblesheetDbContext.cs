using Domain.bublesheet.Entities;
using Microsoft.EntityFrameworkCore;

namespace bubblesheet.Infrastracture.Data
{
    public class bubblesheetDbContext : DbContext
    {
        public bubblesheetDbContext(
            DbContextOptions<bubblesheetDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionBank> QuestionBanks { get; set; }
        public DbSet<Subject> subjects { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<AcademicYear> academicYears { get; set; }
        public DbSet<Code> Codes { get; set; }
        public DbSet<Advertisers> Advertisers { get; set; }
        public DbSet<WalletTransactions> WalletTransactions { get; set; }
        public DbSet<StudentSubjects> StudentSubjects { get; set; }
        public DbSet<StudentAttempt> studentAttempts { get; set; }
        public DbSet<StudentLesson> StudentLessons { get; set; }
        public DbSet<PdfFile> Pdfs { get; set; }
        public DbSet<ExamQuestion> examQuestions { get; set; }
        public DbSet<StudentAnswer> studentAnswers { get; set; }
        public DbSet<RandomExamTemplate> randomExamTemplates { get; set; }
        public DbSet<ImgAd> ImgAds { get; set; }
        public DbSet<ResetPassword> resetPasswords { get; set; }
        public DbSet<StudentScore> StudentScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================================================
            // EXAM
            // =========================================================

            modelBuilder.Entity<Exam>(builder =>
            {
                builder.HasKey(x => x.ExamID);

                builder.Property(x => x.ExamName)
                    .IsRequired()
                    .HasMaxLength(200);

                builder.HasOne(x => x.Lesson)
                    .WithMany(x => x.Exams)
                    .HasForeignKey(x => x.LessonID)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // =========================================================
            // QUESTION BANK
            // =========================================================

            modelBuilder.Entity<QuestionBank>(builder =>
            {
                builder.HasOne(x => x.Lesson)
                    .WithMany(x => x.QuestionBanks)
                    .HasForeignKey(x => x.LessonID)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // =========================================================
            // PDF FILE
            // =========================================================

            modelBuilder.Entity<PdfFile>(builder =>
            {
                builder.HasOne(x => x.Lesson)
                    .WithMany(x => x.pdfFiles)
                    .HasForeignKey(x => x.LessonID)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // =========================================================
            // STUDENT SUBJECT
            // =========================================================

            modelBuilder.Entity<StudentSubjects>(builder =>
            {
                builder.HasOne(x => x.Student)
                    .WithMany()
                    .HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // =========================================================
            // STUDENT LESSON
            // =========================================================

            modelBuilder.Entity<StudentLesson>(builder =>
            {
                builder.HasKey(x => x.Id);

                builder.HasOne(x => x.Student)
                    .WithMany()
                    .HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(x => x.Lesson)
                    .WithMany(x => x.StudentLessons)
                    .HasForeignKey(x => x.LessonId)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            // =========================================================
            // STUDENT ATTEMPT
            // =========================================================

            modelBuilder.Entity<StudentAttempt>(builder =>
            {
                builder.HasOne(x => x.Student)
                    .WithMany()
                    .HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            // =========================================================
            // STUDENT ANSWER
            // =========================================================

            modelBuilder.Entity<StudentAnswer>(builder =>
            {
                builder.HasOne(x => x.StudentAttempt)
                    .WithMany()
                    .HasForeignKey(x => x.StudentAttemptId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(x => x.Question)
                    .WithMany()
                    .HasForeignKey(x => x.QuestionId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.HasOne(x => x.Choice)
                    .WithMany()
                    .HasForeignKey(x => x.ChoiceId)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            // =========================================================
            // EXAM QUESTION
            // =========================================================

            modelBuilder.Entity<ExamQuestion>(builder =>
            {
                builder.HasKey(x => x.Id);

                builder.HasOne(x => x.Exam)
                    .WithMany(x => x.ExamQuestions)
                    .HasForeignKey(x => x.ExamId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(x => x.Question)
                    .WithMany(x => x.ExamQuestions)
                    .HasForeignKey(x => x.QuestionId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.HasIndex(x => new
                {
                    x.ExamId,
                    x.QuestionId
                })
                .IsUnique();
            });
        }
    }
}