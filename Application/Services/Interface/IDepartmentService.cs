using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IDepartmentService
    {
        Task<List<int>> GetIdsByDegree(string degreeCode);
        Task<List<int>> GetIdsByCollege(List<string> collegeCode);
        Task<List<int>> GetIdsByDepartment(List<string> departmentCode);
    }
}
