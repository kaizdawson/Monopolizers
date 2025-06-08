using Microsoft.AspNetCore.Http;
using Monopolizers.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Monopolizers.Service.Contract
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model, string userId);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
