using System.Threading.Tasks;
using Core;
using Dashboard.Models;
using Dashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard
{
    public class TreatmentPlanController : Controller
    {
        private readonly TreatmentApiService _treatmentApiService;

        public TreatmentPlanController(TreatmentApiService treatmentApiService)
        {
            _treatmentApiService = treatmentApiService;
        }

        // GET: TreatmentPlan/Create
        public IActionResult Create(int id)
        {
            TempData["dossierId"] = id;
            return View();
        }

        // POST: TreatmentPlan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DossierId,SessionsPerWeek,SessionDuration")]
            TreatmentPlanModel treatmentPlanModel)
        {
            if (ModelState.IsValid)
            {
                var treatmentPlan = new TreatmentPlan
                {
                    SessionDuration = treatmentPlanModel.SessionDuration,
                    SessionsPerWeek = treatmentPlanModel.SessionsPerWeek
                };
                var result = await _treatmentApiService.AddTreatmentPlan(treatmentPlan, treatmentPlanModel.DossierId);
                if (result.Success) return RedirectToAction("Index", "Home");
                ModelState.AddModelError("", result.Message);
            }

            ModelState.AddModelError("", "invalid treatmentPlan data");

            TempData["dossierId"] = treatmentPlanModel.DossierId;
            return View(treatmentPlanModel);
        }

        // // GET: TreatmentPlan/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var treatmentPlan = await _context.TreatmentPlans.FindAsync(id);
        //     if (treatmentPlan == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(treatmentPlan);
        // }

        // POST: TreatmentPlan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("TreatmentPlanId,SessionsPerWeek,SessionDuration,Active")] TreatmentPlan treatmentPlan)
        // {
        //     if (id != treatmentPlan.TreatmentPlanId)
        //     {
        //         return NotFound();
        //     }
        //
        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(treatmentPlan);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!TreatmentPlanExists(treatmentPlan.TreatmentPlanId))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(treatmentPlan);
        // }
        //
        // private bool TreatmentPlanExists(int id)
        // {
        //     return _context.TreatmentPlans.Any(e => e.TreatmentPlanId == id);
        // }
    }
}