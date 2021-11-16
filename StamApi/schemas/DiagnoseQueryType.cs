using System.Linq;
using System.Threading.Tasks;
using Core.stam;
using EF_Datastore;
using HotChocolate;
using HotChocolate.Data;

namespace StamApi.schemas
{
    public class DiagnoseQueryType
    {
        [UseProjection]
        [UseFiltering]
        public async Task<Diagnose> GetDiagnose([Service] StamDbContext context, int code)
        {
            return context.Diagnoses.FirstOrDefault(x => x.Code == code);
        }

        public IQueryable<Diagnose> GetDiagnoses([Service] StamDbContext context)
        {
            return context.Diagnoses.AsQueryable();
        }

        [UseProjection]
        [UseFiltering]
        public async Task<Operation> GetOperation([Service] StamDbContext context, string value)
        {
            return context.Operations.FirstOrDefault(x => x.Value == value);
        }

        public IQueryable<Operation> GetOperations([Service] StamDbContext context)
        {
            return context.Operations.AsQueryable();
        }
    }
}