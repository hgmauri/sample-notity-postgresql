using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Notify.WebApi.Controllers;

[Route("api/[controller]")]
public class SampleController : Controller
{
	[HttpGet]
	public Task<IActionResult> GetSampleNotify()
	{
		return Task.FromResult<IActionResult>(Ok());
	}
	
}