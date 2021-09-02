using Application.Services.Interface;
using AutoMapper;
using AutoMapper.Configuration;
using Infrastructure.Infrastructure;
using Infrastructure.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(
            IUnitOfWork unitOfWork
        )
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<List<int>> GetIdByDegree(string degreeCode)
        {
            var departmentModel = this._unitOfWork.Department.Where(d => d.Degree == (DegreeType)Enum.Parse(typeof(DegreeType), degreeCode));
            var departmentIds = await departmentModel.Select(d => d.Id).ToListAsync();
            return departmentIds;
        }

        public async Task<List<int>> GetIdsByCollege(string collegeCode)
        {
            var departmentModel = this._unitOfWork.Department.Where(d => d.College.Equals(collegeCode));
            var departmentIds = await departmentModel.Select(d => d.Id).ToListAsync();
            return departmentIds;
        }

        public async Task<int> GetIdsByDepartment(string departmentCode)
        {
            var departmentModel = await this._unitOfWork.Department.FirstOrDefaultAsync(d => d.Prefix.Equals(departmentCode));
            return departmentModel.Id;
        }
    }
}
