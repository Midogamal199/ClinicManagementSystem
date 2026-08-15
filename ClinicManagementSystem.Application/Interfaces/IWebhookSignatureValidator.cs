using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Application.Interfaces
{
    public interface IWebhookSignatureValidator
    {
        bool IsValid(string payload, string receivedSignature);
    }
}
