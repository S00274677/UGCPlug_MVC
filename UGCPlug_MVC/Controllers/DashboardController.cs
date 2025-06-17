using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using UGCPlug_MVC.Filters;
using UGCPlug_MVC.Models;

[NoCache]
// This controller handles business-side dashboard actions for viewing and managing submissions.
// It communicates with the UGCPlug_API to fetch and update submission data.
public class DashboardController : Controller
{
    private readonly string apiBaseUrl = "http://localhost:51175/";

    // Load all submissions for the logged-in business and show them in the dashboard view.
    public async Task<ActionResult> Index()
    {
        if (Session["BusinessId"] == null)
            return RedirectToAction("Login", "Auth");

        string businessId = Session["BusinessId"].ToString();
        ViewBag.BusinessName = Session["BusinessName"];

        List<Submission> submissions = new List<Submission>();

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiBaseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Call the Web API to get submissions for the current business
            HttpResponseMessage response = await client.GetAsync($"api/Submissions?businessId={businessId}");

            if (response.IsSuccessStatusCode)
            {
                submissions = await response.Content.ReadAsAsync<List<Submission>>();
                Debug.WriteLine("✅ Loaded " + submissions.Count + " submissions");
            }
            else
            {
                Debug.WriteLine("❌ API call failed with status: " + response.StatusCode);
            }
        }

        return View(submissions);
    }

    // Load a single submission from the API for editing
    public async Task<ActionResult> Edit(int id)
    {
        Submission submission;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiBaseUrl);
            HttpResponseMessage response = await client.GetAsync($"api/Submissions/{id}");

            if (!response.IsSuccessStatusCode)
                return HttpNotFound();

            submission = await response.Content.ReadAsAsync<Submission>();
        }

        return View(submission);
    }

    // Submit edited changes to the API and redirect back to the dashboard with a success message
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(Submission submission)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiBaseUrl);
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/Submissions/{submission.Id}", submission);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error saving changes.");
                return View(submission);
            }
        }

        TempData["EditSuccess"] = "Changes saved successfully.";
        return RedirectToAction("Index");
    }

    // Load a submission to confirm deletion
    public async Task<ActionResult> Delete(int id)
    {
        Submission submission;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiBaseUrl);
            var response = await client.GetAsync($"api/Submissions/{id}");

            if (!response.IsSuccessStatusCode)
                return HttpNotFound();

            submission = await response.Content.ReadAsAsync<Submission>();
        }

        return View(submission);
    }

    // Delete a submission via the API and return to the dashboard
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiBaseUrl);
            var response = client.DeleteAsync($"api/Submissions/{id}").Result;
            return RedirectToAction("Index");
        }
    }
}
