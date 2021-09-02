using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IDepartmentService
    {
        Task<List<int>> GetIdByDegree(string degreeCode);
        Task<List<int>> GetIdsByCollege(string collegeCode);
        Task<int> GetIdsByDepartment(string departmentCode);
    }
}
