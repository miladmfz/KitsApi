public class FileManager
{
    public void SavePdfToStorage1(MemoryStream pdfStream, string filePath1, string filePath2)
    {
        try
        {
            // تبدیل MemoryStream به آرایه بایت
            byte[] pdfData = pdfStream.ToArray();

            // ذخیره فایل در مسیر مشخص
            File.WriteAllBytes(filePath1, pdfData);
            File.Copy(filePath1, filePath2);



            // نمایش پیام در کنسول
            Console.WriteLine("فایل PDF با موفقیت ذخیره شد.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("خطا در ذخیره فایل PDF: " + ex.Message);
        }
    }
    public void SavePdfToStorage(MemoryStream pdfStream, string filePath)
    {
        try
        {
            // تبدیل MemoryStream به آرایه بایت
            byte[] pdfData = pdfStream.ToArray();

            // ایجاد فایل با حالت انحصاری
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fileStream.Write(pdfData, 0, pdfData.Length);
                fileStream.Close();
                fileStream.Dispose();   
            }

            // نمایش پیام در کنسول
            Console.WriteLine("فایل PDF با موفقیت ذخیره شد.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("خطا در ذخیره فایل PDF: " + ex.Message);
        }
    }



    public void DeletePdfFromStorage(string filePath)
    {
        try
        {
            File.Delete(filePath);
            Console.WriteLine("فایل با موفقیت حذف شد.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("خطا در حذف فایل PDF: " + ex.Message);
        }
    }




    public byte[] GetFile(string filePath)
    {
        try
        {
            if (System.IO.File.Exists(filePath))
            {
                // برگرداندن فایل از مسیر مشخص
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                string fileName = System.IO.Path.GetFileName(filePath);

                return fileBytes;
            }
            else
            {
                Console.WriteLine("فایل مورد نظر وجود ندارد.");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("خطا در بازیابی فایل: " + ex.Message);
            return null;
        }
    }






}

