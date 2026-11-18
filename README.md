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

Each saved run includes all completed evolution steps since population creation, including steps
performed manually and during continuous evolution. Steps remain in memory until **Save current run**
saves the run and its complete log together. Stop/resume keeps the log; creating a new population or
changing the problem clears it. Saving again creates an independent snapshot.

Each step records both parent IDs, permutations and fitness; the crossover name, resulting permutation
and fitness; the mutation name, resulting permutation and fitness; the population's best fitness before
and after the step; and whether the offspring survived replacement. Permutations contain comma-separated,
zero-based town indices. A JSON text column on the run preserves those indices' town IDs, names and
coordinates, so later town edits/deletions do not change historical data. This is database storage,
not a separate JSON file.

For reports, smaller fitness (route distance) is better:
- Crossover improvement versus the better parent: `min(ParentAFitness, ParentBFitness) - CrossoverFitness`.
- Mutation improvement: `CrossoverFitness - MutationFitness`.
- Actual best-route improvement for the crossover/mutation pair: `BestFitnessBefore - BestFitnessAfter`.

Keep negative crossover/mutation differences: these represent worse results. Include unsuccessful
steps when calculating attempt counts and success rates. These measures describe observed outcomes;
they do not prove that an algorithm is inherently superior across different problems or run conditions.
Previously saved runs have no step log or town snapshot; their history cannot be reconstructed.

Deleting a run deletes its steps. Deleting an account also deletes its saved runs and their steps.
Run-history viewing, editing and deletion will be added in the next step.
