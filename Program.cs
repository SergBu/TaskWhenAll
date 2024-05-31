// See https://aka.ms/new-console-template for more information

using System;

var vehicleIds = new List<int>();
var i = 0;

var random = new Random();

do
{
    i++;
    vehicleIds.Add(random.Next());
}
while (i < 1100);

var vehicles = await SplitRequestVehiclesAsync(vehicleIds, 200);

foreach (var vehicle in vehicles)
{
    Console.WriteLine(vehicle);
}



async Task<List<bool>> SplitRequestVehiclesAsync(List<int> vehicleIds, int limit)
{
    var result = new List<bool>();
    var requestsCount = vehicleIds.Count / limit + 1;
    var lastRequestLimit = vehicleIds.Count % limit;
    var partialResults = new List<Response>(); 
    var parallelRequests = new List<Task>();

    for (int i = 0; i < requestsCount; i++)
    {
        var requestData = vehicleIds.GetRange(limit * i, (i == requestsCount - 1) ? lastRequestLimit : limit);
        parallelRequests.Add(Task.Run(async () => partialResults.Add(await GetVehicles(requestData))));  //apiClient
    }

    await Task.WhenAll(parallelRequests);
    foreach (var partialResult in partialResults)
    {
        result.AddRange(partialResult.result);
    }

    return result;
}

async Task<Response> GetVehicles(List<int> vehicleIds)
{
    var result = new List<bool>();
    var response = new Response();

    foreach (var id in vehicleIds)
    {
        if (id > 633616341)
            result.Add(false);
        else
            result.Add(true);
    }

    response.result = result;
    return response;
}

public record Response
{
    public List<bool> result { get; set; }
}