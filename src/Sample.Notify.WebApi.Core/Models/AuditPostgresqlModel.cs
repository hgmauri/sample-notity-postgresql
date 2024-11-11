namespace Sample.Notify.WebApi.Core.Models;
public class AuditPostgresqlModel
{
	public string Table { get; set; }
	public string Action { get; set; }
	public string Json { get; set; }
}
