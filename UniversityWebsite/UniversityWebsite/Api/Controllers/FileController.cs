using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Api.Model;
using UniversityWebsite.Filters;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Api.Controllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za zarządzanie materiałami dydaktycznymi oraz galerią obrazków przechowywanymi w systemie.
    /// </summary>
    [RoutePrefix("api/file")]
    public class FileController : ApiController
    {
        private readonly IFileService _fileService;

        /// <summary>
        /// Tworzy nową instancję kontrolera.
        /// </summary>
        /// <param name="fileService">Serwis zarządzający plikami systemu</param>
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Pobiera plik o podanym id.
        /// </summary>
        /// <param name="id">Guid pliku</param>
        /// <returns>Plik lub NotFound, jeżeli plik o podanym id nie istnieje w systemie.</returns>
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
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = info.Name;
            return result;
        }

        /// <summary>
        /// Pobiera obrazek o podanym id.
        /// </summary>
        /// <param name="id">Guid obrazka</param>
        /// <returns>Plik graficzny lub NotFound, jeżeli plik o podanym id nie istnieje w systemie.</returns>
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
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = info.Name;
            var extension = Path.GetExtension(info.Name);
            if (extension == null) return result;
            var contenttype = "image/" + extension.Trim(new[] { '.' });
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(contenttype);
            return result;
        }

        /// <summary>
        /// Zwraza listę obiektów zawierających informacje o plikach będących materiałami dydaktycznymi danego przedmiotu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <param name="limit">Maksymalna liczba zwrócowych obiektów</param>
        /// <param name="offset">Numer porządkowy pierwszego obiektu listy</param>
        /// <returns>Lista obiektów zawierających informacje o pliku</returns>
        [Route("")]
        [Limit(50), Offset]
        [HttpGet]
        public async Task<PaginationVm<FileDto>> GetInfoBySubject(int subjectId, int limit = 50, int offset = 0)
        {
            var files = await _fileService.GetBySubject(subjectId, limit, offset);
            var number = _fileService.GetFilesNumberBySubject(subjectId);
            return new PaginationVm<FileDto>(files, number, limit, offset);
        }

        /// <summary>
        /// Dodaje nowy plik zawarty w kontekście zapytania do pdanego przedmiotu.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <returns>Dane dodanego pliku.</returns>
        [Route("")]
        public async Task<IHttpActionResult> Post(int subjectId)
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                return BadRequest("Unsupported media type");

            var userId = User.Identity.GetUserId();

            var file = await _fileService.Add(Request, subjectId, userId);
            return Ok(file);
        }
        /// <summary>
        /// Nadpisuje istniejący plik nową wersją, zachowując dane starego pliku.
        /// </summary>
        /// <param name="fileId">Id aktualizowanrgo pliku</param>
        /// <returns>Dane zaktualizowanego pliku.</returns>
        [Route("{fileId:guid}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(string fileId)
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                return BadRequest("Unsupported media type");

            var userId = User.Identity.GetUserId();

            var file = await _fileService.Update(Request, userId, fileId);
            return Ok(file);
        }

        /// <summary>
        /// Usuwa plik o podanym id z systemu.
        /// </summary>
        /// <param name="fileId">Guid pliku</param>
        /// <returns>Status HTTP</returns>
        [HttpDelete]
        [Route("{fileId}")]
        public async Task<IHttpActionResult> Delete(string fileId)
        {
            await _fileService.Delete(fileId);

            return Ok();
        }

        /// <summary>
        /// Zwraza listę obiektów zawierających informacje o obrazkach znajdujących się w galerii obrazków systemu.
        /// </summary>
        /// <param name="limit">Maksymalna liczba zwrócowych obiektów</param>
        /// <param name="offset">Numer porządkowy pierwszego obiektu listy</param>
        /// <returns>Lista obiektów zawierających informacje o pliku graficznym</returns>
        [Limit(50), Offset]
        [Route("gallery")]
        public async Task<PaginationVm<FileDto>> GetGallery(int limit = 50, int offset = 0)
        {
            var images = await _fileService.GetGallery(limit, offset);
            var number = _fileService.GetGalleryImagesNumber();
            return new PaginationVm<FileDto>(images, number, limit, offset);
        }

        /// <summary>
        /// Dodaje nowy obrazek do galerii systemu.
        /// </summary>
        /// <returns>Informacje o obrazku</returns>
        [Route("gallery")]
        public async Task<IHttpActionResult> PostGallery()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                return BadRequest("Unsupported media type");
            var userId = User.Identity.GetUserId();
            var file = await _fileService.AddToGallery(Request, userId);
            return Ok(file);
        }
        /// <summary>
        /// Nadpisuje istniejący obrazek nową wersją, zachowując dane starego pliku graficznego.
        /// </summary>
        /// <param name="fileId">Id aktualizowanrgo obrazka</param>
        /// <returns>Dane zaktualizowanego obrazka.</returns>
        [Route("gallery/{fileId:guid}")]
        public async Task<IHttpActionResult> PutGallery(string fileId)
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                return BadRequest("Unsupported media type");
            var userId = User.Identity.GetUserId();
            var file = await _fileService.UpdateInGallery(Request, userId, fileId);
            return Ok(file);
        }
    }
}
