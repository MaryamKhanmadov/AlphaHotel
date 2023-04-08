namespace Alpha_Hotel_Project.Helpers
{
    public static class FileManager
    {
        public static string SaveFile(this IFormFile file, string rootPath, string foldername)
        {
            string filename = file.FileName;
            filename = filename.Length > 64 ? filename.Substring(filename.Length - 64, 64) : filename;
            filename = Guid.NewGuid().ToString() + filename;
            string path = Path.Combine(rootPath, foldername, filename);
            using (FileStream fileStream = new(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return filename;
        }
        public static string SaveFileSetting(this IFormFile file, string rootPath, string foldername)
        {
            string filename = file.FileName;
            filename = filename.Length > 64 ? filename.Substring(filename.Length - 64, 64) : filename;
            string path = Path.Combine(rootPath, foldername, filename);
            using (FileStream fileStream = new(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return filename;
        }
    }
}
