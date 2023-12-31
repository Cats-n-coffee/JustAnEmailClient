﻿using MimeKit;
using MimeKit.Text;
using MimeKit.Tnef;

// Class from:
// https://stackoverflow.com/questions/35868434/how-to-use-openpop-to-read-e-mail-as-html
namespace JustAnEmailClient.Services;

public class HtmlPreviewVisitor : MimeVisitor
{
    List<MultipartRelated> stack = new List<MultipartRelated>();
    List<MimeEntity> attachments = new List<MimeEntity>();
    string body;

    public HtmlPreviewVisitor()
    {
    }

    // The list of attachments that were in the MimeMessage.
    public IList<MimeEntity> Attachments
    {
        get { return attachments; }
    }

    // The HTML string that can be set on the BrowserControl.
    public string HtmlBody
    {
        get { return body ?? string.Empty; }
    }

    protected override void VisitMultipartAlternative(MultipartAlternative alternative)
    {
        // walk the multipart/alternative children backwards from greatest level of faithfulness to the least faithful
        for (int i = alternative.Count - 1; i >= 0 && body == null; i--)
            alternative[i].Accept(this);
    }

    protected override void VisitMultipartRelated(MultipartRelated related)
    {
        var root = related.Root;

        // push this multipart/related onto our stack
        stack.Add(related);

        // visit the root document
        root.Accept(this);

        // pop this multipart/related off our stack
        stack.RemoveAt(stack.Count - 1);
    }

    // look up the image based on the img src url within our multipart/related stack
    bool TryGetImage(string url, out MimePart image)
    {
        UriKind kind;
        int index;
        Uri uri;

        if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            kind = UriKind.Absolute;
        else if (Uri.IsWellFormedUriString(url, UriKind.Relative))
            kind = UriKind.Relative;
        else
            kind = UriKind.RelativeOrAbsolute;

        try
        {
            uri = new Uri(url, kind);
        }
        catch
        {
            image = null;
            return false;
        }

        for (int i = stack.Count - 1; i >= 0; i--)
        {
            if ((index = stack[i].IndexOf(uri)) == -1)
                continue;

            image = stack[i][index] as MimePart;
            return image != null;
        }

        image = null;

        return false;
    }

    // Save the image to our temp directory and return a "data:" url suitable for
    // the browser control to load.
    string GetDataImageSrc(MimePart image)
    {
        using (var output = new MemoryStream())
        {
            image.Content.DecodeTo(output);
            return string.Format("data:{0};base64,{1}", image.ContentType.MimeType, Convert.ToBase64String(output.GetBuffer(), 0, (int)output.Length));
        }
    }

    // Replaces <img src=...> urls that refer to images embedded within the message with
    // "data:" urls that the browser control will actually be able to load.
    void HtmlTagCallback(HtmlTagContext ctx, HtmlWriter htmlWriter)
    {
        if (ctx.TagId == HtmlTagId.Image && !ctx.IsEndTag && stack.Count > 0)
        {
            ctx.WriteTag(htmlWriter, false);

            // replace the src attribute with a file:// URL
            foreach (var attribute in ctx.Attributes)
            {
                if (attribute.Id == HtmlAttributeId.Src)
                {
                    MimePart image;
                    string url;

                    if (!TryGetImage(attribute.Value, out image))
                    {
                        htmlWriter.WriteAttribute(attribute);
                        continue;
                    }

                    url = GetDataImageSrc(image);

                    htmlWriter.WriteAttributeName(attribute.Name);
                    htmlWriter.WriteAttributeValue(url);
                }
                else
                {
                    htmlWriter.WriteAttribute(attribute);
                }
            }
        }
        else if (ctx.TagId == HtmlTagId.Body && !ctx.IsEndTag)
        {
            ctx.WriteTag(htmlWriter, false);

            // add and/or replace oncontextmenu="return false;"
            foreach (var attribute in ctx.Attributes)
            {
                if (attribute.Name.ToLowerInvariant() == "oncontextmenu")
                    continue;

                htmlWriter.WriteAttribute(attribute);
            }

            htmlWriter.WriteAttribute("oncontextmenu", "return false;");
        }
        else
        {
            // pass the tag through to the output
            ctx.WriteTag(htmlWriter, true);
        }
    }

    protected override void VisitTextPart(TextPart entity)
    {
        TextConverter converter;

        if (body != null)
        {
            // since we've already found the body, treat this as an attachment
            attachments.Add(entity);
            return;
        }

        if (entity.IsHtml)
        {
            converter = new HtmlToHtml
            {
                HtmlTagCallback = HtmlTagCallback
            };
        }
        else if (entity.IsFlowed)
        {
            var flowed = new FlowedToHtml();
            string delsp;

            if (entity.ContentType.Parameters.TryGetValue("delsp", out delsp))
                flowed.DeleteSpace = delsp.ToLowerInvariant() == "yes";

            converter = flowed;
        }
        else
        {
            converter = new TextToHtml();
        }

        body = converter.Convert(entity.Text);
    }

    protected override void VisitTnefPart(TnefPart entity)
    {
        // extract any attachments in the MS-TNEF part
        attachments.AddRange(entity.ExtractAttachments());
    }
    /*
    protected override void VisitMessagePart(MessagePart entity)
    {

    } */
}
