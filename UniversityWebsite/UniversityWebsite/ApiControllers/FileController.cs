using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Helper.Files;

namespace UniversityWebsite.ApiControllers
{
    [RoutePrefix("api/file")]
    public class FileController : ApiController
    {
        private IFilesManager fileManager;

        public FileController()
        {
            this.fileManager = new FilesManager();
        }

        [Route("{id}")]
        public HttpResponseMessage Get(string id)
        {
            var info = fileManager.GetPath(id, "");
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
        public async Task<IHttpActionResult> GetInfoBySubject(int subjectId)
        {
            var results = await fileManager.GetBySubject(subjectId);
            return Ok(results);
        }

        [Route("{fileId}")]
        public async Task<IHttpActionResult> Post(string fileId = null, [FromUri]int? subjectId = null)
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return BadRequest("Unsupported media type");
            }

            var userId = User.Identity.GetUserId();

            try
            {
                if (string.IsNullOrEmpty(fileId))
                {

                    var file = await fileManager.Add(Request, subjectId, userId);
                    return Ok(file);
                }
                else
                {
                    var file = await fileManager.Update(Request, subjectId, userId, fileId);
                    return Ok(file);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetBaseException().Message);
            }

        }

        [HttpDelete]
        [Route("{fileId}")]
        public async Task<IHttpActionResult> Delete(string fileId)
        {
            await fileManager.Delete(fileId);

            return Ok();
        }
    }
}
