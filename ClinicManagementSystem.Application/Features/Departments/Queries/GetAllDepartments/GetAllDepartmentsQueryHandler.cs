using ClinicManagementSystem.Application.Common.Models;
using ClinicManagementSystem.Application.DTOs.Departments;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Departments.Queries.GetAllDepartments
{
    public class GetAllDepartmentsQueryHandler
        : IRequestHandler<GetAllDepartmentsQuery, PaginatedResult<DepartmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllDepartmentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<DepartmentDto>> Handle(
            GetAllDepartmentsQuery request,
            CancellationToken cancellationToken)
        {
            
            var (items, totalCount) = await _unitOfWork.DepartmentRepository
                .GetPagedWithEmployeeCountAsync(request.PageNumber, request.PageSize);

           
            var dtos = items.Select(x => new DepartmentDto
            {
                Id = x.Department.Id,
                Name = x.Department.Name,
                EmployeeCount = x.EmployeeCount
            }).ToList();

            return new PaginatedResult<DepartmentDto>
            {
                Items = dtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }
    }
}