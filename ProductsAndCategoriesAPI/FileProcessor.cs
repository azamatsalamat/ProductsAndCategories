using ProductsAndCategoriesLibrary;

namespace ProductsAndCategoriesAPI
{
    public static class FileProcessor
    {
        public static void SaveFile(IFormFile file, string storageName)
        {
            string filepath = Path.Combine("wwwroot", storageName);

            if (file.Length > 0)
            {
                using (Stream fileStream = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }
            }
        }

        public static void DeleteFile(ProductFile file)
        {
            File.Delete(Path.Combine("wwwroot", file.StorageName));
        }
    }
}
