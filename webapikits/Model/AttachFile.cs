using System;
using System.Data.Entity.Core.Objects;

namespace webapikits.Model
{
    public class AttachFile
    {
        public string Title { get; set; } = "";
        public string FileName { get; set; } = "";
        public string ClassName { get; set; } = "";
        public string Type { get; set; } = "";
        public string FilePath { get; set; } = "";
        public string FileType { get; set; } = "";
        public string Data { get; set; } = "";
        public string ObjectRef { get; set; } = "";


    }


    public class ConversationAttachDto
    {
        public string? Title { get; set; } = "";
        public string? PixelScale { get; set; } = "0";
        public string? ClassName { get; set; } = "";
        public string? ObjectRef { get; set; } = "";
        public string? ConversationRef { get; set; } = "";


    }



}
