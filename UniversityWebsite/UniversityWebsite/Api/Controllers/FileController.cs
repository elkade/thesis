using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Filters;
using UniversityWebsite.Services;

namespace UniversityWebsite.Api.Controllers
{
    [RoutePrefix("api/file")]
    public class FileController : ApiController
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [Route("{id:guid}")]
        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            var userId = User.Identity.GetUserId();
            var info = _fileService.GetPath(id, userId);
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

        [Route("gallery/{id:guid}")]
        [HttpGet]
        public HttpResponseMessage GetImage(string id)
        {
            var info = _fileService.GetPath(id, "");
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(info.Path, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = info.Name
            };
            var extension = Path.GetExtension(info.Name);
            if (extension == null) return result;
            var contenttype = "image/"+extension.Trim(new[] {'.'});
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(contenttype);
            return result;
        }

        [Route("")]
        [Limit(50), Offset]
        [HttpGet]
        public async Task<IHttpActionResult> GetInfoBySubject(int subjectId, int? limit=null, int? offset=null)
        {
            //FilterLimitOffset(ref limit, ref offset);
            var results = await _fileService.GetBySubject(subjectId, limit.Value, offset.Value);
            return Ok(results);
        }

        [Route("count")]
        [HttpGet]
        public IHttpActionResult GetFilesNumberBySubject(int subjectId)
        {
            var results = _fileService.GetFilesNumberBySubject(subjectId);
            return Ok(results);
        }

        [Route("gallery/count")]
        [HttpGet]
        public IHttpActionResult GetGalleryImagesNumber()
        {
            var results = _fileService.GetGalleryImagesNumber();
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
                var file = await _fileService.Add(Request, subjectId, userId);
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
                var file = await _fileService.Update(Request, userId, fileId);
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
            await _fileService.Delete(fileId);

            return Ok();
        }

        [Limit(50), Offset]
        [Route("gallery")]
        public async Task<IHttpActionResult> GetGallery(int? limit = null, int? offset = null)
        {
            var results = await _fileService.GetGallery(limit.Value, offset.Value);
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
                var file = await _fileService.AddToGallery(Request, userId);
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
                var file = await _fileService.UpdateInGallery(Request, userId, fileId);
                return Ok(file);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetBaseException().Message);
            }
        }
    }
}
