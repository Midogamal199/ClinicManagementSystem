using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ClinicManagementSystem.Application.Features.Auth.Commands.RequestRegistrationOtp
{
    public class RequestRegistrationOtpCommand: IRequest<Unit>
    {
        public string Email { get; set; }

    }
}
