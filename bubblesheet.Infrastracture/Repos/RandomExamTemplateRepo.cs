using bubblesheet.Infrastracture.Data;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Repos
{
    public class RandomExamTemplateRepo : IRandomExamTemplate
    {
        private readonly bubblesheetDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public RandomExamTemplateRepo(bubblesheetDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<RandomExamTemplate?> GetByLessonIdAsync(int lessonId)
        {
            return await _context.randomExamTemplates
                .FirstOrDefaultAsync(x => x.LessonId == lessonId);
        }

        public async Task AddAsync(RandomExamTemplate template)
        {
            await _context.randomExamTemplates.AddAsync(template);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<RandomExamTemplate?> UpdateAsync(RandomExamTemplate template)
        {
            _context.randomExamTemplates.Update(template);
            await _unitOfWork.SaveChangesAsync();
            return template;
        }
        public async Task DeleteRandExam(int Id)//By Lesson Id
        {
            var rand = await _context.randomExamTemplates.FirstOrDefaultAsync(x=>x.LessonId == Id);
            _context.randomExamTemplates.Remove(rand);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
