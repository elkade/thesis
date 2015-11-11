using System;
using System.Web;
using System.Web.Mvc;
using UniversityWebsite.Core;
using UniversityWebsite.Domain;
using UniversityWebsite.Domain.Model;

namespace UniversityWebsite.Controllers
{
    public class FileController : Controller
    {
        public FileController(IDomainContext db)
        {
            _db = db;
        }
        private readonly IDomainContext _db;
        
        [HttpGet]
        public ActionResult Index(int id)
        {
            var file = _db.Files.Find(id);
            return File(file.Content, file.ContentType);
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (upload == null || upload.ContentLength <= 0) return Json(new {success = false});
            var newFile = new File
            {
                FileName = System.IO.Path.GetFileName(upload.FileName),
                ContentType = upload.ContentType
            };
            using (var reader = new System.IO.BinaryReader(upload.InputStream))
            {
                newFile.Content = reader.ReadBytes(upload.ContentLength);
            }
            newFile = _db.Files.Add(newFile);
            _db.SaveChanges();
            return Json(new { id = newFile.Id });
        }
        //private IFileService _fileService;

        //public FileController(IFileService fileService)
        //{
        //    fileService = _fileService;
        //}
        //[Authorize]
        //public ActionResult UploadImage(int id, string name, string alt)
        //{
        //    ProductImageDto newImage = new ProductImageDto();
        //    HttpPostedFileBase upload = Request.Files["upPhoto"];
        //    newImage.Name = name;
        //    newImage.Alt = alt;

        //    if (upload != null)
        //    {
        //        newImage.ContentType = upload.ContentType;

        //        Int32 length = upload.ContentLength;
        //        byte[] tempImage = new byte[length];
        //        upload.InputStream.Read(tempImage, 0, length);
        //        newImage.Image = tempImage;
        //    }

        //    _fileService.UpdateImage(id, newImage);
        //    return GetPhoto(id);
        //}
        //public ActionResult GetPhoto(int productId, int photoId = 0)
        //{
        //    _productService.GetDto(productId, out p);
        //    ProductImageDto image = p.ProductImage;
        //    if (image == null)
        //    {
        //        var dir = Server.MapPath("/Content");
        //        var path = Path.Combine(dir, "defaultPhoto.jpg");
        //        return File(path, "image/jpeg");
        //    }
        //    return new ImageResult(image.Image, image.ContentType);
        //}
        //public ActionResult GetPhotoMin(int productId, int photoId = 0)
        //{
        //    ProductDto p;
        //    _productService.GetDto(productId, out p);
        //    ProductImageDto image = p.ProductImage;
        //    if (image == null)
        //    {
        //        var dir = Server.MapPath("/Content");
        //        var path = Path.Combine(dir, "defaultPhotoMin.jpg");
        //        return File(path, "image/jpeg");
        //    }
        //    return new ImageResult(image.Image, image.ContentType);
        //}
        //public class ImageResult : ActionResult
        //{
        //    public String ContentType { get; set; }
        //    public byte[] ImageBytes { get; set; }
        //    public String SourceFilename { get; set; }

        //    public ImageResult(String sourceFilename, String contentType)
        //    {
        //        SourceFilename = sourceFilename;
        //        ContentType = contentType;
        //    }

        //    public ImageResult(byte[] sourceStream, String contentType)
        //    {
        //        ImageBytes = sourceStream;
        //        ContentType = contentType;
        //    }

        //    public override void ExecuteResult(ControllerContext context)
        //    {
        //        var response = context.HttpContext.Response;
        //        response.Clear();
        //        response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        response.ContentType = ContentType;

        //        if (ImageBytes != null)
        //        {
        //            var stream = new MemoryStream(ImageBytes);
        //            stream.WriteTo(response.OutputStream);
        //            stream.Dispose();
        //        }
        //        else
        //        {
        //            response.TransmitFile(SourceFilename);
        //        }
        //    }
        //}
	}

    //public class ProductImageDto
    //{
    //    public string Name { get; set; }
    //    public string Alt { get; set; }
    //    public string ContentType { get; set; }
    //    public byte[] Image { get; set; }
    //}
}