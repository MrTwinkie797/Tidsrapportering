using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TidsRapportering.Models;

namespace TidsRapportering.Controllers
{
    public class ReportsAPIController : ApiController
    {
        private ReportTimeEntities1 db = new ReportTimeEntities1();

        // GET: api/Reports
        public IQueryable<Report> GetReport()
        {
            return db.Report;
        }

        // GET: api/Reports/5
        [ResponseType(typeof(Report))]
        public async Task<IHttpActionResult> GetReport(int id)
        {
            Report report = await db.Report.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            return Ok(report);
        }

        // PUT: api/Reports/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutReport(int id, Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != report.Id)
            {
                return BadRequest();
            }

            db.Entry(report).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Reports
        [HttpPost]
        [ResponseType(typeof(Report))]
        public async Task<IHttpActionResult> AddReport(Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Report.Add(report);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ReportExists(report.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = report.Id }, report);
        }

        // DELETE: api/Reports/5
        [ResponseType(typeof(Report))]
        public async Task<IHttpActionResult> DeleteReport(int id)
        {
            Report report = await db.Report.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            db.Report.Remove(report);
            await db.SaveChangesAsync();

            return Ok(report);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReportExists(int id)
        {
            return db.Report.Count(e => e.Id == id) > 0;
        }
    }
}