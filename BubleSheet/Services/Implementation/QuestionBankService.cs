using bubblesheet.Infrastracture.Dtos;
using bubblesheet.Infrastracture.Repos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using MiniShop.Application.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class QuestionBankService(IAccount account,IAccountService accountService,IStudentScore studentScore,IYear year,IStudentSubjectService StudentSubjectService,IStudentLessonService studentLessonService ,IJwtService jwtService, IQuestionExam questionExam, IUnitOfWork unitOfWork,IQuestion question,IStudentAnswer studentAnswer ,IStudentAttempts studentAttempts ,IQuestionService questionService , IQuestionBank questionBank , IStudentAttempts studentAttempt,IStudentAttemptService studentAttemptService) : IQuestionBankService
    {
        private readonly IStudentScore _studentScore = studentScore;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IQuestionBank _questionBank = questionBank;
        private readonly IStudentAttempts _studentAttempt = studentAttempt;
        private readonly IStudentAttemptService _studentAttemptService = studentAttemptService;
        private readonly IQuestionService _questionService = questionService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IStudentAnswer _studentAnswer = studentAnswer;
        private readonly IQuestionExam _questionExam = questionExam;
        private readonly IStudentLessonService _studentLessonService = studentLessonService;
        private readonly IStudentSubjectService _StudentSubjectService = StudentSubjectService;
        private readonly IYear _year = year;
        private readonly IAccount _account = account;
        private readonly IAccountService _accountService = accountService;
        public async Task<QuestionBankQuestionWithAttemptID> GetQuestionBankWithQuestions(int questionBankId)
        {
            int studentId = _jwtService.GetCurrentStudentId();

            var student = await _account.GetStudentByIDAsync(studentId);

            var questionBank = await _questionBank.GetQuestionById(questionBankId);

            if (questionBank == null)
                throw new Exception("Question Bank not found.");

            bool isAdmin = _accountService.checkIfHeIsAdmin(student.Email);
            if (!isAdmin) 
            {
                if (!questionBank.IsActive)
                    throw new Exception("Question Bank is Not Active.");

                bool studentIsPaid =
                    await _StudentSubjectService
                        .IsStudentSubscribedToSubject(
                            studentId,
                            questionBank.Lesson.SubjectId);

                if (!questionBank.IsFree && !studentIsPaid)
                    throw new Exception("Question Bank Is Not Access.");
            }

            int attemptId;

            var existAttempt = await _studentAttempt
                .GetAttemptHasNotSumbittedByIdForQuestionBank(
                    studentId,
                    questionBankId);

            if (existAttempt == null)
            {

                attemptId = await _studentAttemptService
                    .AddStudentAttemptAsync(
                        new StudentAttemptDTO
                        {
                            Id = questionBankId,
                            submitType = submitType.QuestionBank
                        });
            }
            else
            {
                attemptId = existAttempt.StudentAttemptId;
            }

            var questionIds = await _questionBank
                .GetQuestionIdsByQuestionBankId(questionBankId);

            var questions = await _questionService
                .GetQuestionsByIds(questionIds);

            return new QuestionBankQuestionWithAttemptID
            {
                BankId = questionBank.BankId,
                BankName = questionBank.BankName,
                Description = questionBank.Description,
                QCount = questionIds.Count,
                Questions = questions,
                AttemptId = attemptId
            };
        }
        public async Task<QuestionBankDTO> AddQuestionBank(AddQuestionBankDTO questionBankDTO)
        {
            var questionBank = new QuestionBank(
                questionBankDTO.BankName,
                questionBankDTO.IsFree,
                questionBankDTO.Description,
                questionBankDTO.LessonId
            );

            var result = await _questionBank.AddQuestionBank(questionBank);

            return new QuestionBankDTO
            {
                BankId = result.BankId,
                BankName = result.BankName,
                QCount = result.QCount,
                Description = result.Description,
                IsFree = result.IsFree,
            };
        }
        public async Task<QuestionBankDTO> EditQuestionBank(EditQuestionBankDTO questionBankDTO)
        {
            var QuestionBank = await _questionBank.GetQuestionById(questionBankDTO.QId);

            QuestionBank.Update(questionBankDTO.BankName,questionBankDTO.IsFree,questionBankDTO.Description);
            await _unitOfWork.SaveChangesAsync();

            return new QuestionBankDTO
            {
                BankId = questionBankDTO.QId,
                BankName = questionBankDTO.BankName ,
                QCount = QuestionBank.QCount,
                Description = questionBankDTO.Description,
                IsFree = questionBankDTO.IsFree,
            };
        }
        public async Task<QuestionBankDTO> GetQuestionBank(int QuestionBankId)
        {
            var QBank = await _questionBank.GetQuestionById(QuestionBankId);
            if (QBank == null)
                throw new Exception("Question Bank not found.");

            if (!QBank.IsActive)
                throw new Exception("Question Bank is Not Active.");
            int studentId = _jwtService.GetCurrentStudentId();

            bool StudentIsPaid = await _StudentSubjectService.IsStudentSubscribedToSubject(studentId, QBank.Lesson.SubjectId);
            if (!QBank.IsFree && !StudentIsPaid)
                throw new Exception("Question Bank Is Not Access.");

            return new QuestionBankDTO
            {
                QCount = QBank.QCount,
                BankId = QBank.BankId,
                BankName = QBank.BankName,
                IsFree = QBank.IsFree,
                Description = QBank.Description
            };
        }

        public async Task<SumbitResponseDto> SubmitQuestionBank(SubmitDto submitDto) 
        {
            int studentId = _jwtService.GetCurrentStudentId();

            var attempt = await _studentAttempt.GetById(submitDto.StudentAttemptId);

            if (attempt == null)
                throw new Exception("Attempt not found.");

            if (attempt.StudentId != studentId)
                throw new Exception("Unauthorized.");
            if (attempt.IsSubmitted)
                throw new Exception("Attempt already submitted."); 

            var questionIds = submitDto.Answers
                .Select(x => x.QuestionId)
                .ToList();


            var questions = await _questionService.GetQuestionsByIds(questionIds);

            var studentAnswers = new List<StudentAnswer>();

            int correctAnswers = 0;

            foreach (var answer in submitDto.Answers)
            {
                var question = questions.FirstOrDefault(q => q.QuestionId == answer.QuestionId);

                if (question == null)
                    throw new Exception($"Question {answer.QuestionId} not found.");

                bool isCorrect = question.Choices
                    .Any(c => c.ChoiseId == answer.ChoiceId && c.IsCorrect);

                if (isCorrect)
                    correctAnswers++;

                studentAnswers.Add(new StudentAnswer(
                    submitDto.StudentAttemptId,
                    answer.QuestionId,
                    answer.ChoiceId,
                    isCorrect));
            }

            await _studentAnswer.AddRangeAsync(studentAnswers);

            attempt.Submit(correctAnswers);
            await _unitOfWork.SaveChangesAsync();

            if (!attempt.QuestionBankId.HasValue)
                throw new Exception("Question Bank not found for this attempt.");

            var lessonId = await _questionBank
                .GetLessonIdByQuestionBankId(attempt.QuestionBankId.Value);

            if (!lessonId.HasValue)
                throw new Exception("Lesson not found for this question bank.");


            await _studentLessonService
                .DoUpdateLessonStatus(lessonId.Value);


            var AcYearId = await _year.GetAcademicYearIdByQBankId(attempt.QuestionBankId.Value);
            var studentScore = await _studentScore.GetByStudentAndYearAsync(studentId, AcYearId.Value);
            bool IsSolved = await _studentAttempt.IfStudentSolveQbank(studentId, attempt.QuestionBankId.Value);

            if (!IsSolved)
            {
                attempt.ToggleIsFirst(); // Isfirst is True
                if (((int)correctAnswers * 100 / questions.Count) >= 50)
                {
                    if (studentScore == null)
                    {
                        StudentScore newStudentScore = new StudentScore(AcYearId.Value, studentId);
                        await _studentScore.AddAsync(newStudentScore);
                        newStudentScore.AddScore(10);
                        await _studentScore.SaveChangesAsync();
                    }
                    else
                    {
                        studentScore.AddScore(10);
                    }
                    await _studentScore.SaveChangesAsync();
                }
            }


            await _unitOfWork.SaveChangesAsync();
            return new SumbitResponseDto
            {
                Score = correctAnswers,
                precentage = (int)correctAnswers * 100 / questions.Count
            };
        }
        public async Task DeleteQuestionBank(int Id)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var questionIds = await _questionBank
                    .GetQuestionIdsByQuestionBankId(Id);
                await _questionExam.DeleteQuestionsFromAllExams(questionIds);
                var StuAnswers = await _studentAnswer.GetAllAnswersByQuestionIDs(questionIds);
                await _studentAnswer.RemoveRangeAsync(StuAnswers);
                var AttemptIds = await _studentAttempt.GetAllAttemptIDs(Id, submitType.QuestionBank,true);
                await _studentAttempt.RemoveRangeStudentAttempt(AttemptIds);
                await _questionService.DeleteQuestions(questionIds);
                await _questionBank.DeleteQuestionBank(Id);

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task DeleteQuestionBanks(List<int> IDs)
        {
            foreach (var id in IDs)
            {
                await DeleteQuestionBank(id);
            }
        }
        public async Task MakeToggle(int QuestionBankId)
        {
            var QBank = await _questionBank.GetQuestionById(QuestionBankId);
            QBank.MakeToggle();
            await _unitOfWork.SaveChangesAsync(); 
        }
    }
}
