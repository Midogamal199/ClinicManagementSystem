using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Domain.Entities;
using ClinicManagementSystem.Domain.Interfaces;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Prescriptions.Commands.CreatePrescription
{
    public class CreatePrescriptionCommandHandler : IRequestHandler<CreatePrescriptionCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePrescriptionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
        {
            var visit = await _unitOfWork.Repository<Visit>().GetByIdAsync(request.VisitId);

            if (visit is null)
            {
                throw new KeyNotFoundException($"Visit with Id '{request.VisitId}' was not found.");
            }
            var prescription = new Prescription
            {
                VisitId = request.VisitId,
                IssuedAt = DateTime.Now
            };
            foreach(var item in request.Items)
            {
                prescription.Items.Add(new PrescriptionItem
                {
                    MedicineName = item.MedicineName,
                    Dosage = item.Dosage
                });
            }
            await _unitOfWork.Repository<Prescription>().AddAsync(prescription);
            await _unitOfWork.SaveChangesAsync();

            return prescription.Id;
        }
    }
}
