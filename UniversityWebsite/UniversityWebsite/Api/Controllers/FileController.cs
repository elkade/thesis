using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Helper.Files;

namespace UniversityWebsite.Api.Controllers
{
    [RoutePrefix("api/file")]
    public class FileController : ApiController
    {
        private readonly IFileManager _fileManager;

        public FileController()
        {
            _fileManager = new FileManager();
        }

        [Route("{id:guid}")]
        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            var info = _fileManager.GetPath(id, "");
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(info.Path, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = info.Name
            };
            return result;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetInfoBySubject(int subjectId, int? limit=null, int? offset=null)//limit offset ogarnąć dobrze
        {
            var results = await _fileManager.GetBySubject(subjectId, limit ?? 50, offset ?? 0);
            return Ok(results);
        }

        [Route("")]
        public async Task<IHttpActionResult> Post(int subjectId)
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                return BadRequest("Unsupported media type");

            var userId = User.Identity.GetUserId();

            try
            {
                var file = await _fileManager.Add(Request, subjectId, userId);
                return Ok(file);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetBaseException().Message);
            }

        }
        [Route("{fileId:guid}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(string fileId)
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                return BadRequest("Unsupported media type");

            var userId = User.Identity.GetUserId();

            try
            {
                var file = await _fileManager.Update(Request, userId, fileId);
                return Ok(file);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetBaseException().Message);
            }

        }
        [HttpDelete]
        [Route("{fileId:guid}")]
        public async Task<IHttpActionResult> Delete(string fileId)
        {
            await _fileManager.Delete(fileId);

            return Ok();
        }

        [Route("gallery")]
        public async Task<IHttpActionResult> GetGallery(int? limit = null, int? offset = null)
        {
            var results = await _fileManager.GetGallery(limit ?? 50, offset ?? 0);
            return Ok(results);
        }

        [Route("gallery")]
        public async Task<IHttpActionResult> PostGallery()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                return BadRequest("Unsupported media type");
            var userId = User.Identity.GetUserId();
            try
            {
                var file = await _fileManager.AddToGallery(Request, userId);
                return Ok(file);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetBaseException().Message);
            }
        }
        [Route("gallery/{fileId:guid}")]
        public async Task<IHttpActionResult> PutGallery(string fileId)
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                return BadRequest("Unsupported media type");
            var userId = User.Identity.GetUserId();
            try
            {
                var file = await _fileManager.UpdateInGallery(Request, userId, fileId);
                return Ok(file);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetBaseException().Message);
            }
        }
    }
}
