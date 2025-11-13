using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainingEventSchedulerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Training Event Scheduler - EnumerateOccurrences Test ===\n");

            // Test all recurrence patterns
            TestDailyRecurrence();
            TestWeeklyRecurrence();
            TestWeeklyRecurrenceMultipleDays();
            TestMonthlyRecurrenceDayOfMonth();
            TestMonthlyRecurrenceDayOfWeek();
            TestYearlyRecurrence();

            Console.WriteLine("\n=== All Tests Completed ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void TestDailyRecurrence()
        {
            Console.WriteLine("--- TEST 1: Daily Recurrence (Every 3 Days) ---");
            var repeatData = new RepeatDataObject
            {
                RepeatType = RepeatType.Daily,
                RepeatInterval = 3
            };

            var startDate = new DateTime(2025, 1, 1);
            var endDate = new DateTime(2025, 1, 20);

            Console.WriteLine($"Start Date: {startDate:yyyy-MM-dd}");
            Console.WriteLine($"End Date: {endDate:yyyy-MM-dd}");
            Console.WriteLine($"Repeat Interval: {repeatData.RepeatInterval}");

            var occurrences = EnumerateOccurrences(repeatData, startDate, endDate).ToList();
            PrintOccurrences(occurrences);
            Console.WriteLine($"Total occurrences: {occurrences.Count}\n");
        }

        static void TestWeeklyRecurrence()
        {
            Console.WriteLine("--- TEST 2: Weekly Recurrence (Every 2 Weeks, Same Day) ---");
            var repeatData = new RepeatDataObject
            {
                RepeatType = RepeatType.Weekly,
                RepeatInterval = 2,
                WeeklyRepeatDays = new List<DayOfWeek>() // Empty list = use start date's day
            };

            var startDate = new DateTime(2025, 1, 1); // Monday
            var endDate = new DateTime(2025, 2, 28);

            Console.WriteLine($"Start Date: {startDate:yyyy-MM-dd}");
            Console.WriteLine($"End Date: {endDate:yyyy-MM-dd}");
            Console.WriteLine($"Repeat Interval: {repeatData.RepeatInterval}");

            var occurrences = EnumerateOccurrences(repeatData, startDate, endDate).ToList();
            PrintOccurrences(occurrences);
            Console.WriteLine($"Total occurrences: {occurrences.Count}\n");
        }

        static void TestWeeklyRecurrenceMultipleDays()
        {
            Console.WriteLine("--- TEST 3: Weekly Recurrence (Every 2 Weeks, Mon/Wed/Fri) ---");
            var repeatData = new RepeatDataObject
            {
                RepeatType = RepeatType.Weekly,
                RepeatInterval = 2,
                WeeklyRepeatDays = new List<DayOfWeek>
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Friday
                }
            };

            var startDate = new DateTime(2025, 1, 1); // Monday
            var endDate = new DateTime(2025, 1, 31);

            Console.WriteLine($"Start Date: {startDate:yyyy-MM-dd}");
            Console.WriteLine($"End Date: {endDate:yyyy-MM-dd}");
            Console.WriteLine($"Repeat Interval: {repeatData.RepeatInterval}");

            var occurrences = EnumerateOccurrences(repeatData, startDate, endDate).ToList();
            PrintOccurrences(occurrences);
            Console.WriteLine($"Total occurrences: {occurrences.Count}\n");
        }

        static void TestMonthlyRecurrenceDayOfMonth()
        {
            Console.WriteLine("--- TEST 4: Monthly Recurrence (15th of Every Month) ---");
            var repeatData = new RepeatDataObject
            {
                RepeatType = RepeatType.Monthly,
                RepeatInterval = 1,
                MonthlyRepeatType = MonthlyRepeatType.DayOfTheMonth
            };

            var startDate = new DateTime(2025, 1, 15);
            var endDate = new DateTime(2025, 6, 30);

            Console.WriteLine($"Start Date: {startDate:yyyy-MM-dd}");
            Console.WriteLine($"End Date: {endDate:yyyy-MM-dd}");
            Console.WriteLine($"Repeat Interval: {repeatData.RepeatInterval}");

            var occurrences = EnumerateOccurrences(repeatData, startDate, endDate).ToList();
            PrintOccurrences(occurrences);
            Console.WriteLine($"Total occurrences: {occurrences.Count}\n");
        }

        static void TestMonthlyRecurrenceDayOfWeek()
        {
            Console.WriteLine("--- TEST 5: Monthly Recurrence (2nd Tuesday of Every Month) ---");
            var repeatData = new RepeatDataObject
            {
                RepeatType = RepeatType.Monthly,
                RepeatInterval = 1,
                MonthlyRepeatType = MonthlyRepeatType.DayOfTheWeek
            };

            var startDate = new DateTime(2025, 1, 9); // 2nd Tuesday of January
            var endDate = new DateTime(2025, 6, 30);

            Console.WriteLine($"Start Date: {startDate:yyyy-MM-dd}");
            Console.WriteLine($"End Date: {endDate:yyyy-MM-dd}");
            Console.WriteLine($"Repeat Interval: {repeatData.RepeatInterval}");

            var occurrences = EnumerateOccurrences(repeatData, startDate, endDate).ToList();
            PrintOccurrences(occurrences);
            Console.WriteLine($"Total occurrences: {occurrences.Count}\n");
        }

        static void TestYearlyRecurrence()
        {
            Console.WriteLine("--- TEST 6: Yearly Recurrence (Every 2 Years) ---");
            var repeatData = new RepeatDataObject
            {
                RepeatType = RepeatType.Yearly,
                RepeatInterval = 2
            };

            var startDate = new DateTime(2025, 1, 15);
            var endDate = new DateTime(2035, 12, 31);

            Console.WriteLine($"Start Date: {startDate:yyyy-MM-dd}");
            Console.WriteLine($"End Date: {endDate:yyyy-MM-dd}");
            Console.WriteLine($"Repeat Interval: {repeatData.RepeatInterval}");

            var occurrences = EnumerateOccurrences(repeatData, startDate, endDate).ToList();
            PrintOccurrences(occurrences);
            Console.WriteLine($"Total occurrences: {occurrences.Count}\n");
        }

        static void PrintOccurrences(List<DateTime> occurrences)
        {
            foreach (var occurrence in occurrences)
            {
                Console.WriteLine($"  {occurrence:yyyy-MM-dd (dddd)}");
            }
        }

        #region EnumerateOccurrences Method and Helpers (Copied from TrainingEventScheduler)

        /// <summary>
        /// Enumerates recurrence dates (actual training dates) between startDate and endDate inclusive.
        /// This ignores DaysBefore because DaysBefore is only used for early creation in scheduled runs.
        /// </summary>
        private static IEnumerable<DateTime> EnumerateOccurrences(RepeatDataObject repeatData, DateTime startDate, DateTime endDate)
        {
            switch (repeatData.RepeatType)
            {
                case RepeatType.Daily:
                    {
                        var current = startDate;
                        while (current <= endDate)
                        {
                            yield return current;
                            current = current.AddDays(repeatData.RepeatInterval);
                        }
                        break;
                    }
                case RepeatType.Weekly:
                    {
                        // If no specific days, use the start date's day.
                        if (repeatData.WeeklyRepeatDays == null || repeatData.WeeklyRepeatDays.Count == 0)
                        {
                            var current = startDate;
                            while (current <= endDate)
                            {
                                yield return current;
                                current = current.AddDays(7 * repeatData.RepeatInterval);
                            }
                        }
                        else
                        {
                            // Sort for deterministic ordering
                            var orderedDays = repeatData.WeeklyRepeatDays.OrderBy(d => d).ToList();
                            var firstWeekStart = startDate.AddDays(-(int)startDate.DayOfWeek);
                            var weekIndex = 0;
                            var currentWeekStart = firstWeekStart;
                            while (true)
                            {
                                // Only include weeks matching interval
                                if ((weekIndex % repeatData.RepeatInterval) == 0)
                                {
                                    foreach (var dow in orderedDays)
                                    {
                                        var occurrence = currentWeekStart.AddDays((int)dow);
                                        if (occurrence < startDate) continue;
                                        if (occurrence > endDate) yield break;
                                        yield return occurrence;
                                    }
                                }
                                currentWeekStart = currentWeekStart.AddDays(7);
                                weekIndex++;
                                if (currentWeekStart > endDate) break;
                            }
                        }
                        break;
                    }
                case RepeatType.Monthly:
                    {
                        var current = startDate;
                        if (repeatData.MonthlyRepeatType == MonthlyRepeatType.DayOfTheMonth)
                        {
                            var day = startDate.Day;
                            while (current <= endDate)
                            {
                                var daysInMonth = DateTime.DaysInMonth(current.Year, current.Month);
                                if (day <= daysInMonth)
                                {
                                    var occurrence = new DateTime(current.Year, current.Month, day);
                                    if (occurrence >= startDate && occurrence <= endDate)
                                    {
                                        yield return occurrence;
                                    }
                                }
                                current = current.AddMonths(repeatData.RepeatInterval);
                            }
                        }
                        else
                        {
                            // DayOfTheWeek pattern: capture ordinal and weekday from startDate
                            var targetDow = startDate.DayOfWeek;
                            var ordinal = ((startDate.Day - 1) / 7) + 1; // 1..5
                            while (current <= endDate)
                            {
                                var occurrence = NthWeekdayOfMonth(current.Year, current.Month, targetDow, ordinal);
                                if (occurrence.HasValue && occurrence.Value >= startDate && occurrence.Value <= endDate)
                                {
                                    yield return occurrence.Value;
                                }
                                current = current.AddMonths(repeatData.RepeatInterval);
                            }
                        }
                        break;
                    }
                case RepeatType.Yearly:
                    {
                        var current = startDate;
                        while (current <= endDate)
                        {
                            yield return current;
                            current = current.AddYears(repeatData.RepeatInterval);
                        }
                        break;
                    }
            }
        }

        // Returns the Nth weekday of a month (e.g., 2nd Tuesday). If ordinal exceeds actual occurrences returns null.
        private static DateTime? NthWeekdayOfMonth(int year, int month, DayOfWeek dow, int ordinal)
        {
            if (ordinal < 1 || ordinal > 5) return null;
            var first = new DateTime(year, month, 1);
            var offset = ((int)dow - (int)first.DayOfWeek + 7) % 7;
            var day = 1 + offset + (ordinal - 1) * 7;
            if (day > DateTime.DaysInMonth(year, month)) return null;
            return new DateTime(year, month, day);
        }

        #endregion
    }

    #region Supporting Classes

    /// <summary>
    /// RepeatDataObject
    /// </summary>
    [Serializable]
    public class RepeatDataObject
    {
        /// <summary>
        /// Gets or sets the type of the repeat.
        /// </summary>
        public RepeatType RepeatType { get; set; }

        /// <summary>
        /// Gets or sets the repeat interval.
        /// </summary>
        public int RepeatInterval { get; set; }

        /// <summary>
        /// Gets or sets the weekly repeat days.
        /// </summary>
        public List<DayOfWeek> WeeklyRepeatDays { get; set; }

        /// <summary>
        /// Gets or sets the type of the monthly repeat.
        /// </summary>
        public MonthlyRepeatType MonthlyRepeatType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatDataObject"/> class.
        /// </summary>
        public RepeatDataObject()
        {
            WeeklyRepeatDays = new List<DayOfWeek>();
        }
    }

    /// <summary>
    /// Repeat Type Enumeration
    /// </summary>
    public enum RepeatType
    {
        Daily = 0,
        Weekly = 1,
        Monthly = 2,
        Yearly = 3
    }

    /// <summary>
    /// Monthly Repeat Type Enumeration
    /// </summary>
    public enum MonthlyRepeatType
    {
        DayOfTheMonth = 0,
        DayOfTheWeek = 1
    }

    #endregion
}
