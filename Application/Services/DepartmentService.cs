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

        public async Task<List<int>> GetIdsByDegree(string degreeCode)
        {
            var departmentModel = this._unitOfWork.Department.Where(d => d.Degree == (DegreeType)Enum.Parse(typeof(DegreeType), degreeCode));
            var departmentIds = await departmentModel.Select(d => d.Id).ToListAsync();
            return departmentIds;
        }

        public async Task<List<int>> GetIdsByCollege(List<string> collegeCode)
        {
            var departmentModel = this._unitOfWork.Department.Where(d => collegeCode.Contains(d.College));
            var departmentIds = await departmentModel.Select(d => d.Id).ToListAsync();
            return departmentIds;
        }

        public async Task<List<int>> GetIdsByDepartment(List<string> departmentCode)
        {
            var departmentModel = this._unitOfWork.Department.Where(d => departmentCode.Contains(d.Prefix));
            var departmentIds = await departmentModel.Select(d => d.Id).ToListAsync();
            return departmentIds;
        }
    }
}
