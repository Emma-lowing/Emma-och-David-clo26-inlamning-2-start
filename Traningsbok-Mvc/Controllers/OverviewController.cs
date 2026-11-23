using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Traningsbok.Models;
using Traningsbok.Services;
using Traningsbok.ViewModels;

namespace Traningsbok.Controllers;

/// <summary>
/// Sidor för veckoöversikt och statistik för en övning. Logiken ligger i IStatisticsService.
/// </summary>
public class OverviewController : AppController
{
    private readonly IStatisticsService _statistics;
    private readonly IWorkoutRepository _repository;
    private readonly IWorkoutFormatter _formatter;

    public OverviewController(IStatisticsService statistics, IWorkoutRepository repository, IWorkoutFormatter formatter)
    {
        _statistics = statistics;
        _repository = repository;
        _formatter = formatter;
    }

    // Utan val visas innevarande vecka. Veckan börjar på måndag (ISO 8601).
    public IActionResult Index(int? year, int? week) => Run(() =>
    {
        DateTime today = DateTime.Today;
        int selectedYear = Math.Clamp(year ?? ISOWeek.GetYear(today), 2000, 2100);
        int selectedWeek = Math.Clamp(week ?? ISOWeek.GetWeekOfYear(today), 1, 53);

        WeekOverview overview = _statistics.GetWeekOverview(selectedYear, selectedWeek);
        return View(new WeekViewModel
        {
            Year = selectedYear,
            Week = selectedWeek,
            WorkoutCount = overview.WorkoutCount,
            TotalTime = _formatter.FormatDuration(overview.TotalMinutes),
            Rows = overview.MinutesPerType
                .Select(row => (
                    row.TrainingTypeName,
                    _formatter.FormatDuration(row.Minutes),
                    row.GoalMinutes is int goal ? _formatter.FormatDuration(goal) : (string?)null))
                .ToList(),
        });
    });

    public IActionResult Exercise(string? name) => Run(() =>
    {
        var model = new ExerciseStatsViewModel
        {
            Name = name?.Trim() ?? "",
            ExerciseNames = _repository.GetAllWorkouts()
                .SelectMany(w => w.Exercises)
                .Select(e => e.Name)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Order()
                .ToList(),
        };

        if (model.Name.Length > 0)
        {
            ExerciseStats stats = _statistics.GetExerciseStats(model.Name);
            model.MaxWeight = _formatter.FormatWeight(stats.MaxWeight);
            model.TotalSets = stats.TotalSets;
        }
        return View(model);
    });
}
