using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace WorkoutDiary.Models;

public class WorkoutModel
{
    // Egenskap för datum med validering, get- och setmetoder
    [Display(Name = "Datum")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Ange datum!")]
    public DateTime? Date { get; set; }

    // Egenskap för aktivitet med validering, get- och setmetoder
    [Display(Name = "Aktivitet")]
    [Required(ErrorMessage = "Ange aktivitet!")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Aktivitet måste anges med 2-20 tecken!")]
    public string? Activity { get; set; }

    // Egenskap för träningstid med validering, get- och setmetoder
    [Display(Name = "Träningstid (minuter)")]
    [Required(ErrorMessage = "Ange träningstid i antal minuter!")]
    [Range(10, int.MaxValue, ErrorMessage = "Träningstid måste anges med minst 10 minuter!")]
    public int? Minutes { get; set; }

    // Egenskap för känsla
    [Display(Name = "Känsla")]
    public string? Feeling { get; set; }

    // Sökväg till en JSON-fil där träningspassen lagras
    private static readonly string FilePath = "wwwroot/workouts.json";

    // Metod för att spara ett träningspass till JSON-filen
    public void Save()
    {
        // Anropar metod för att läsa in befintliga träningspass som lagras i en lista
        List<WorkoutModel> workouts = LoadWorkouts();

        // Adderar det aktuella träningspasset till listan med träningspass
        workouts.Add(this);

        // Sparar den uppdaterade lista till JSON-filen
        string json = JsonSerializer.Serialize(workouts);
        File.WriteAllText(FilePath, json);
    }

    // Metod för att hämta alla träningspass från JSON-filen
    public List<WorkoutModel> GetWorkouts()
    {
        return LoadWorkouts();
    }

    // Metod för att läsa in träningspass från JSON-filen
    private static List<WorkoutModel> LoadWorkouts()
    {
        // Om JSON-filen existerar
        if (File.Exists(FilePath))
        {
            // Läser innehållet från JSON-filen
            string json = File.ReadAllText(FilePath);

            // Returnerar en lista av deserialiserade WorkoutModel-objekt eller en tom lista
            return JsonSerializer.Deserialize<List<WorkoutModel>>(json) ?? [];
        }
        // Returnerar en tom lista om JSON-filen inte existerar
        return [];
    }
}