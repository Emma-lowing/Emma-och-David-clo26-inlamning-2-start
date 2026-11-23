# Services

Interface som sidorna anropar, och klasser som ni bygger.

- `IHouseholdRepository` och `InMemoryHouseholdRepository`: låtsasdatabasen. Ersätt den med er egen databasklass.
- `CategoryService`, `BudgetService`, `BudgetWarningService` och `MonthlyOverviewService`: visar demodata eller kastar `NotImplementedException` tills ni byggt dem.
- `MoneyFormatter`: fungerar, men formaterar beloppen fel med flit (önskemål 1).
