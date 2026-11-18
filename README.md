# MetaheuristicOptimizationNTP

## Database migrations

The application applies EF Core migrations on startup instead of using `EnsureCreated`.
Existing towns and accounts are preserved. Before upgrading a database created by an older
version, the application makes a one-time SQLite backup beside it: `tsp.db.before-migrations.bak`.
The initial migration accepts the existing `Towns` and `Users` tables without recreating them.

To add a future schema change from the repository folder:

```powershell
dotnet tool restore
dotnet ef migrations add DescribeYourChange --project MetaheuristicOptimizationNTP.csproj --output-dir Database/Migrations
```

Restart the application to apply the migration. Do not use `EnsureCreated` alongside migrations.

## Saving experiment runs

Create a population, evolve it, and stop evolution. Log in and choose **Runs → Save current run…**.
Enter a name and optional notes. The saved summary includes the owner, population creation time,
initial best distance, and current best distance. Creating a new population starts a new experiment;
stopping and resuming evolution continues the same experiment. Changing town coordinates or
adding/removing towns clears the current population and experiment.

Saved runs are summaries, not replayable routes. Deleting an account also deletes its saved runs.
Run-history viewing, editing and deletion will be added in the next step.
