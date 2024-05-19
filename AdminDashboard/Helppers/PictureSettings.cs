namespace AdminDashboard.Helppers
{
	public class PictureSettings
	{
		public static string UploadFile(IFormFile file , string folderName)
		{
			// 1. Get Folder Path
			var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName);
			// 2. Set FileName UNIQUE
			var fileName = Guid.NewGuid() + file.FileName;
			// 3. Get File Path
			var filePath = Path.Combine(folderPath, fileName);
			// 4. Save Files As Streams
			var fileStream = new FileStream(filePath , FileMode.Create);
			// 5. Copy File Into Stream 
			file.CopyTo(fileStream);
			// 6. Return FileName
			return Path.Combine("images\\products", fileName);
		}

		public static void DeleteFile(string folderName , string fileName)
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName , fileName);
			if (File.Exists(filePath))
				File.Delete(filePath);
		}
	}
}
