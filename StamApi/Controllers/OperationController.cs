using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.stam;
using DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace StamApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : Controller
    {
        private readonly IOperationRepository _operationRepository;

        public OperationController(IOperationRepository operationRepository)
        {
            _operationRepository = operationRepository;
        }

        // GET: api/operation/:value
        [HttpGet("{value}")]
        public async Task<ActionResult<Operation>> GetOperation(string value)
        {
            var operation = await _operationRepository.GetOperation(value);
            return operation == null ? NotFound() : operation;
        }

        // GET: api/operation/
        [HttpGet]
        public async Task<ActionResult<ICollection<Operation>>> GetDetails()
        {
            var operations = await _operationRepository.GetOperations();
            return operations == null ? NotFound() : operations.ToList();
        }
    }
}