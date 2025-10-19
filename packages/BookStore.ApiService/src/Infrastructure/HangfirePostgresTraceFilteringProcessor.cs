using System.Diagnostics;
using OpenTelemetry;

internal sealed class HangfirePostgresTraceFilteringProcessor: BaseProcessor<Activity>
{
	public override void OnEnd(Activity activity)
	{
		var statement = activity.Tags.FirstOrDefault(kv => kv.Key == "db.statement");
		if (statement.Value != null && statement.Value.Contains("hangfire"))
		{
			activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
		}
	}
}
