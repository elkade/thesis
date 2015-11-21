using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Core;
using UniversityWebsite.Services;
using System.Collections.Generic;
using UniversityWebsite.Validation;

namespace UniversityWebsite.ApiControllers
{
    public class PageController : ApiController
    {
        private readonly IPageService _pageService;
        private DomainContext db = new DomainContext();

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        // GET api/Page
        public IEnumerable<Page> GetPages()
        {
            return _pageService.GetAll();
        }

        // GET api/Page/5
        [ValidateCustomAntiForgeryToken]
        [ResponseType(typeof(Page))]
        public IHttpActionResult GetPage(string urlName)
        {
            Page page = _pageService.FindPage(urlName);
            if (page == null)
            {
                return NotFound();
            }

            return Ok(page);
        }

        // PUT api/Page/5
        public IHttpActionResult PutPage(int id, Page page)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != page.Id)
            {
                return BadRequest();
            }
            _pageService.UpdatePage(page);
            return StatusCode(HttpStatusCode.NoContent);
            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!PageExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

        }

        // POST api/Page
        [ResponseType(typeof(Page))]
        public IHttpActionResult PostPage(Page page)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pages.Add(page);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = page.Id }, page);
        }

        // DELETE api/Page/5
        [ResponseType(typeof(Page))]
        public IHttpActionResult DeletePage(int id)
        {
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return NotFound();
            }

            db.Pages.Remove(page);
            db.SaveChanges();

            return Ok(page);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PageExists(int id)
        {
            return db.Pages.Count(e => e.Id == id) > 0;
        }
    }
}