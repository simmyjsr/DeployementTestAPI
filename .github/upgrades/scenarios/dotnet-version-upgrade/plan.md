### 01-update-project-tfms: Update project target frameworks to net10.0

Update the TargetFramework property for all projects that will be migrated to net10.0.

Scope: InventoryAPI\InventoryAPI.csproj

Assessment context: InventoryAPI project currently targets net8.0 and requires package updates. Test project already targets net10.0.

Known risks: Binary/source incompatible APIs may require small code fixes and unit test adjustments. Some package APIs changed across major versions.

Research starting points: Review assessment.md for API incompatibilities and the list of NuGet packages recommended for update.

**Done when**: InventoryAPI.csproj TargetFramework is net10.0 and solution builds successfully.


### 02-update-packages: Update NuGet packages flagged as Upgrade Recommended or Deprecated

Update package versions to their latest compatible net10.0 releases, prioritizing security fixes and deprecated packages with clear replacements.

Scope: InventoryAPI\InventoryAPI.csproj

Assessment context: Microsoft.EntityFrameworkCore, Microsoft.AspNetCore.Authentication.JwtBearer, Microsoft.Extensions.Identity.Core flagged for updates.

Known risks: API changes in EF Core or authentication packages may require small code updates.

**Done when**: Packages updated, solution builds, and unit tests run successfully.


### 03-replace-deprecated-packages: Replace or remove deprecated packages

Replace deprecated packages with recommended alternatives where available. If no replacement exists, pin and flag for manual review.

Scope: InventoryAPI\InventoryAPI.csproj

**Done when**: Deprecated packages removed or replaced and build succeeds.


### 04-build-and-fix: Build solution and address compilation errors/warnings

Build entire solution with TreatWarningsAsErrors enabled for the migrated projects and fix any remaining compile or analyzer warnings introduced by the upgrade.

**Done when**: Solution builds without errors and no warnings in modified projects.


### 05-run-tests: Run unit tests and fix failures

Run unit test suite and resolve any failing tests. Integration tests that require external DBs remain skipped unless configured.

**Done when**: All unit tests pass.


### 06-commit: Commit and finalize

Create a single commit containing all upgrade changes and push the working branch.

**Done when**: Commit exists on working branch and user is informed.
