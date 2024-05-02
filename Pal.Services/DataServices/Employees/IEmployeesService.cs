using Pal.Core.Domains.Empolyees;
using Pal.Core.Enums;
using Pal.Data.DTOs;
using Pal.Data.DTOs.Employees;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services.DataServices.Employees
{
    public interface IEmployeesService
    {
        Task<SyncPaginatedListModel<EmployeesList>> GetAllAsListAsync(DataManagerRequest dm);

        Task<List<Employee>> getEmployees();

        Task<EmployeeDTO> GetEmployeeByIdAsync(int id);

        Task<int> GetEmployeeIdByUerIdAsync(string id);

        Task<int> AddEmployeeAsync(EmployeeDTO model);

        Task<int> UpdateEmployeeAsync(EmployeeDTO model);

        Task<ResponseType> DeleteEmployeeAsync(int id);
    }
}
