using System;

namespace UniversityWebsite.Services.Models
{
    public class Tile
    {
        public string Href;
        public DateTime Date;
        public string Header;
        public string Paragraph;

        //public string ToHtml()
        //{
        //    using(StringWriter stringWriter = new StringWriter())
        //    using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
        //    {
        //        writer.AddAttribute(HtmlTextWriterAttribute.Class, "col-sm-4 tile");
        //        writer.RenderBeginTag("article"); // Begin #1

        //        writer.AddAttribute(HtmlTextWriterAttribute.Href, Href);
        //        writer.RenderBeginTag(HtmlTextWriterTag.A); // Begin #2

        //        writer.RenderBeginTag(HtmlTextWriterTag.Div); // Begin #3

        //        writer.RenderBeginTag("time"); // Begin #4
        //        writer.Write(Date);
        //        writer.RenderEndTag();

        //        writer.RenderBeginTag("header"); // Begin #5
        //        writer.Write(Header);
        //        writer.RenderEndTag();

        //        writer.RenderBeginTag(HtmlTextWriterTag.P); // Begin #6
        //        writer.Write(Paragraph);
        //        writer.RenderEndTag();

        //        writer.RenderEndTag(); // End #3

        //        writer.RenderEndTag(); // End #2

        //        writer.RenderEndTag(); // End #1
        //        return stringWriter.ToString();
        //    }
        //}
    }
}