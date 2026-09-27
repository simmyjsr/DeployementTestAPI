# Upgrade Options

This file lists the upgrade options evaluated for upgrading the solution to .NET 10 and the recommended defaults for confirmation.

## Summary
- Target Framework: net10.0 (confirmed)
- Recommended upgrade strategy: All-at-Once
- Recommended commit strategy: Single Commit at End (aligns with All-at-Once)
- Package update policy: Update all packages flagged as "Upgrade Recommended" or "Deprecated" to the latest compatible stable versions for net10.0. Address security vulnerabilities by default.
- Deprecated package handling: Replace or remove deprecated packages where a modern alternative exists; flag for manual review when no replacement exists.
- Testing policy: Run full test suite and integration tests after upgrade; treat warnings as errors and fix before completing the task.

## Options (detailed)

### Upgrade Strategy
- id: upgradeStrategy
- choices:
  - id: all-at-once
	label: All-at-Once — upgrade every affected project to net10.0 in a single pass (recommended for single-project/low-complexity solutions)
  - id: bottom-up
	label: Bottom-Up — upgrade libraries first, apps last
  - id: top-down
	label: Top-Down — upgrade applications first, then libraries
- selected: all-at-once
- rationale: Single primary application project requires upgrade; small code change surface and test project already targeting net10.0.

### Package Update Policy
- id: packagePolicy
- choices:
  - id: update-all
	label: Update all recommended and incompatible packages to latest compatible stable versions for net10.0 (recommended)
  - id: minimal
	label: Update only packages that block compilation or have critical vulnerabilities
- selected: update-all
- rationale: Assessment flagged several packages as deprecated or recommended for upgrade; security fixes included by default.

### Deprecated Package Handling
- id: deprecatedHandling
- choices:
  - id: replace-when-available
	label: Replace deprecated packages with modern alternatives when a clear replacement exists
  - id: keep-and-pin
	label: Keep deprecated package but pin to safest supported version (not recommended)
- selected: replace-when-available
- rationale: Prefer modern supported libraries to reduce future maintenance and security risk.

### Commit Strategy Suggestion (informational)
- id: commitStrategy
- choices:
  - id: single
	label: Single Commit at End
  - id: after-each-task
	label: After Each Task
- selected: single
- rationale: Aligns with All-at-Once strategy; easier to roll back if needed.

### Test & Validation
- Run unit tests and integration tests on net10.0 after package updates
- Build with warnings-as-errors and resolve all warnings

## Generated-by
- processor: upgrade-options-processor (inline)
- assessment: .github/upgrades/scenarios/dotnet-version-upgrade/assessment.md

## Next Step
Confirm these options. After confirmation, the orchestrator will write the compact Upgrade Options block to scenario-instructions.md and proceed to generate the plan.
