using System.Net.Http.Headers;
using Swipeclock;

// SEE: https://twpapi.payrollservers.us/swagger/ui/index

using var client = new HttpClient();

// Step 1 - Parse Site IDs
var csvPath = Path.Combine( AppContext.BaseDirectory, "Swipeclock-Sites.csv" );

var siteIds = CsvHelper.GetSiteIdsFromCsv(csvPath);

if (siteIds.Count == 0)
{
    throw new InvalidOperationException($"No site IDs were found in '{csvPath}'.");
}

// string siteId = "105495"; // Customer's Swipeclock site ID
string siteId = siteIds[0];

// Step 2: Generate signed JWT
string jwt = JwtSignature.CreateJwt(siteId);

// Step 3: Exchange JWT for authentication token
string token = await JwtSignature.GetUserTokenAsync(client, jwt);

// Step 4 - get Swipeclock data

// Swipeclock - Get Employees
string url = $"https://twpapi.payrollservers.us/api/{siteId}/employees";
using var request = new HttpRequestMessage(HttpMethod.Get, url);
request.Headers.Authorization =
    new AuthenticationHeaderValue("Bearer", token);
using var response = await client.SendAsync(request);
string result = await response.Content.ReadAsStringAsync();
response.EnsureSuccessStatusCode();
Console.WriteLine(result);

// TODO: store employee data to push to Big Query

// TODO: get timecards

// TODO: get timecard punch status

// TODO: get timecard summary

// TODO: get time off requests

// TODO: get Payroll Activities

// TODO: get Payroll Activities Schemas

// TODO: get rules

// TODO: get integrated scheduling rule

// TODO: Get WFH Accrual Rules

// TODO: get logins

// TODO: get schedules

// TODO: get groups

// TODO: get accruals

// TODO: get accrual activity

// TODO: get accrual schema

// TODO: ensure I got timecard approval info

// TODO: anything else ?

// TODO: repeat for each employee

// TODO: push data to Big Query
