using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Program
{
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> getTotalScoredGoals(string team, int year)
    {
        int totalGols = 0;
        int currentPage = 1;
        int totalPages = 1;

        using (HttpClient client = new HttpClient())
        {
            while (currentPage <= totalPages)
            {
                // Request para obter os dados como team1
                string urlTeam1 = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={currentPage}";
                string responseTeam1 = await client.GetStringAsync(urlTeam1);
                var jsonTeam1 = JObject.Parse(responseTeam1);

                if (jsonTeam1 != null)
                {
                    totalPages = (int?)(jsonTeam1["total_pages"]) ?? 1;

                    // Verifica se jsonTeam1["data"] não é nulo e tenta convertê-lo para uma lista de JToken
                    var dataTeam1 = jsonTeam1["data"] as JArray;
                    if (dataTeam1 != null)
                    {
                        foreach (var match in dataTeam1)
                        {
                            int team1Gols = int.Parse((string?)match["team1goals"] ?? "0");
                            totalGols += team1Gols;
                        }
                    }
                }
                // Request para obter os dados como team2
                string urlTeam2 = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={currentPage}";
                string responseTeam2 = await client.GetStringAsync(urlTeam2);
                var jsonTeam2 = JObject.Parse(responseTeam2);

                if (jsonTeam2 != null)
            {
                // Verifica se jsonTeam2["data"] não é nulo e tenta convertê-lo para uma lista de JToken
                var dataTeam2 = jsonTeam2["data"] as JArray;
                if (dataTeam2 != null)
                {
                    foreach (var match in dataTeam2)
                    {
                        int team2Goals = int.Parse((string?)match["team2goals"] ?? "0");
                        totalGols += team2Goals;
                    }
                }
            }

                currentPage++;
            }
        }

        return totalGols;
    }
}