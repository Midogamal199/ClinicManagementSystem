using System;
using ClinicManagementSystem.Domain.Enums;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.VerifyRegistration
{
    public class VerifyRegistrationCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}