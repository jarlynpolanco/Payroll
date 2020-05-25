using Payroll.Data;
using Payroll.Data.Models;
using Payroll.Implementations;
using System.Collections.Generic;

namespace Payroll.Services
{
    public class EmployeeService
    {
        private readonly UnitOfWork<AppDbContext> _unitOfWork;

        public EmployeeService(UnitOfWork<AppDbContext> unitOfWork) =>
            _unitOfWork = unitOfWork;

        public IEnumerable<Employee> GetAll() => _unitOfWork.GetRepository<Employee>().GetAll();

        public IEnumerable<Employee> GetAll(string include) => 
            _unitOfWork.GetRepository<Employee>().GetAllWithInclude(include);

        public Employee FindByID(long id) => _unitOfWork.GetRepository<Employee>().Get(t => t.ID == id);

        public bool Exists(long id) => _unitOfWork.GetRepository<Employee>().Get(t => t.ID == id) != null;

        public void Add(Employee entity)
        {
            _unitOfWork.GetRepository<Employee>().Add(entity);
            _unitOfWork.Save();
        }

        public void Update(Employee entity)
        {
            _unitOfWork.GetRepository<Employee>().Update(entity);
            _unitOfWork.Save();
        }

        public void Delete(Employee employee) => _unitOfWork.GetRepository<Employee>().Delete(employee);

    }
}
