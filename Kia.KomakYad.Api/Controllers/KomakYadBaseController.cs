using Kia.KomakYad.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Kia.KomakYad.Api.Controllers
{
    public abstract class KomakYadBaseController : ControllerBase
    {
        public async Task<OkObjectResult> Ok<T>(IQueryable<T> query, SearchBaseParams filters) where T : class
        {
            var pageList = await PagedList<T>.CreateAsync(query, filters);

            Response.AddPagination(pageList);
            return base.Ok(pageList);
        }
    }
}
