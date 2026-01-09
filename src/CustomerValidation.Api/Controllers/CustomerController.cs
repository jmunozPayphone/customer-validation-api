using CustomerValidation.Api.DTOs;
using CustomerValidation.Api.Extensions;
using CustomerValidation.ApplicationCore.Features.Customers.Commands;
using CustomerValidation.SharedKernel.Guards;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerValidation.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController(ISender sender) : ControllerBase
{
    [HttpPost("risk-assessment")]
    public async Task<IActionResult> AssessRisk([FromBody] AssessCustomerRiskRequestDTO requestDTO)
    {
        Guard.ThrowIfNull(requestDTO);
        var result = await sender.Send(requestDTO.ToCommand());
        return this.OkFromResult(result);
    }
}
