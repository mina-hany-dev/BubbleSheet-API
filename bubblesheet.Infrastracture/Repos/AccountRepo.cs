using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bubblesheet.Infrastracture.Data;
using Domain.bublesheet;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace bubblesheet.Infrastracture.Repos
{
    public class AccountRepo : IAccount
    {
        private readonly bubblesheetDbContext _context;
        public AccountRepo(bubblesheetDbContext bubblesheetDb)
        {
            _context = bubblesheetDb;
        }
        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await Task.CompletedTask;
        }
        public async Task<Student?> GetStudentByEmailAsync(string email)
        {
            var student = await _context.Students.FirstOrDefaultAsync(e=>e.Email==email);
            return student;
        }
        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await Task.CompletedTask;
        }
        public async Task<Student?> GetStudentByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Students.FirstOrDefaultAsync(s=>s.RefreshToken==refreshToken);
        }
        public async Task<Student?> GetStudentByIDAsync(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(e => e.StudentId == id);
            return student;
        }
        public async Task<List<Student>> GetAllStudentsById(List<int> IDs)
        {
            return await _context.Students
                .Where(x => IDs.Contains(x.StudentId))
                .ToListAsync();
        }
        public async Task DeleteStudent(Student student) 
        {
            _context.Students.Remove(student);
            await Task.CompletedTask;
        }
        public async Task<int> CountOfStudent()
        {
            return await _context.Students.CountAsync();
        }
    }
}
