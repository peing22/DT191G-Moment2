using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkoutDiary.Models;

namespace WorkoutDiary.Controllers;

public class WorkoutController : Controller
{
    // Metod som bland annat returnerar en lista med träningspass
    private List<WorkoutModel> Workouts()
    {
        // Skapar en instans av WorkoutModel
        WorkoutModel workout = new();

        // Anropar metod för att hämta alla träningspass och lagra dem i en lista
        List<WorkoutModel> workouts = workout.GetWorkouts();

        // Sorterar listan efter datum
        workouts = [.. workouts.OrderBy(w => w.Date)];

        // Beräknar den totala träningstiden
        int totalMinutes = workouts.Sum(w => w.Minutes ?? 0);

        // Lägger till den totala träningstiden i ViewData
        ViewData["TotalWorkoutTime"] = totalMinutes;

        return workouts;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // Lagrar strängvärde i variabel
        string welcome = "Välkommen till min applikation...";

        // Sätter värde för ViewBag som används i vyn
        ViewBag.Welcome = welcome;

        // Anropar metod för att beräkna total träningstid baserat på listan med träningspass
        List<WorkoutModel> workouts = Workouts();

        // Returnerar vyn
        return View();
    }

    [HttpGet]
    [Route("/min-traningsdagbok")]
    public IActionResult Diary()
    {
        // Anropar metod för att lagra träningspass i en lista
        List<WorkoutModel> workouts = Workouts();

        // Returnerar vyn och skickar med listan
        return View(workouts);
    }

    [HttpGet]
    [Route("/lagg-till-traningspass")]
    public IActionResult Add()
    {
        // Returnerar vyn
        return View();
    }

    [HttpPost]
    [Route("/lagg-till-traningspass")]
    public IActionResult Add(WorkoutModel model)
    {
        // Om postad data för ett träningspass validerar korrekt
        if (ModelState.IsValid)
        {
            // Serialiserar objektet till JSON och lagrar det i sessionen
            string json = JsonSerializer.Serialize(model);
            HttpContext.Session.SetString("Workout", json);

            // Omdirigerar till annan vy
            return Redirect("kansla-vid-aktivitet");
        }
        // Returnerar vyn om validering misslyckas
        return View();
    }

    [HttpGet]
    [Route("/kansla-vid-aktivitet")]
    public IActionResult Feeling()
    {
        // Returnerar vyn och skickar med listan med olika känslor
        return View(Feelings);
    }

    [HttpPost]
    [Route("/summering")]
    public IActionResult Summary(IFormCollection collection)
    {
        // Skapar en instans av WorkoutModel
        WorkoutModel workout = new();

        // Hämtar det sparade träningspasset från sessionen
        string json = HttpContext.Session.GetString("Workout")!;
        workout = JsonSerializer.Deserialize<WorkoutModel>(json)!;

        // Lagrar värdet för känslan från formuläret i en variabel
        int value = Convert.ToInt32(collection["Feeling"]);

        // Uppdaterar träningspasset med vald känsla
        workout.Feeling = Feelings[value].Text;

        // Anropar metod för att lagra träningspasset till JSON-filen
        workout.Save();

        // Returnerar vyn och skickar med träningspasset
        return View(workout);
    }

    // Skapar en lista med SelectListItem för olika känslor
    private readonly List<SelectListItem> Feelings =
    [
        new SelectListItem {Text = "Underbar", Value = "0"},
        new SelectListItem {Text = "Skön", Value = "1"},
        new SelectListItem {Text = "Likgiltig", Value = "2"},
        new SelectListItem {Text = "Jobbig", Value = "3"},
        new SelectListItem {Text = "Fruktansvärd", Value = "4"}
    ];
}