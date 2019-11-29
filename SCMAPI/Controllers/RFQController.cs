using BALayer.RFQ;
using System.Web.Http;

namespace SCMAPI.Controllers
{
	[RoutePrefix("Api/RFQ")]
	public class RFQController : ApiController
    {
		private readonly IRFQBA _rfqBusenessAcess;
		public RFQController(IRFQBA rfqBA)
		{
			this._rfqBusenessAcess = rfqBA;
		}
		[HttpGet]
		[Route("getRFQItems/{RevisionId}")]
		public IHttpActionResult getRFQItems(int RevisionId)
		{
			return Ok(this._rfqBusenessAcess.getRFQItems(RevisionId));
		}
	}
}
