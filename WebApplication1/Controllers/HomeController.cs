using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using WebApplication1.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

namespace WebApplication1.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			string dossierImages = @"C:\Users\Alexandre\Documents\Developpement\Projets\background-wallpaper"; // Remplacez "chemin_vers_votre_dossier" par le chemin du dossier contenant les images
			List<string> listeNomsImages = SortImagesByColor(dossierImages);

            int nombreCarres = listeNomsImages.Count;
            Dictionary<string, double> resultat = DetermineCarreTailles(nombreCarres);


            double cote = resultat["cote"];
            int nombreLignes = (int)resultat["nombreLignes"];
            int nombreColonnes = (int)resultat["nombreColonnes"];

            ViewBag.NombreCarres = nombreCarres;
            ViewBag.Images = listeNomsImages;
            ViewBag.Resultat = resultat;

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}


		public static Dictionary<string, double> DetermineCarreTailles(int nombreCarres, int zoneLargeur = 2200, int zoneHauteur = 1293)
		{
			// Calcul de la taille d'un côté des carrés
			double cote = Math.Sqrt((zoneLargeur * zoneHauteur) / nombreCarres);

			// Calcul du nombre de lignes et de colonnes
			int nombreLignes = (int)(zoneHauteur / cote);
			int nombreColonnes = (int)Math.Ceiling((double)nombreCarres / nombreLignes);

			return new Dictionary<string, double>
		{
			{ "cote", cote },
			{ "nombreLignes", nombreLignes },
			{ "nombreColonnes", nombreColonnes }
		};
		}

		public static List<string> GetListeNomsImages(string dossier)
		{
			// Vérifier si le dossier existe
			if (!System.IO.Directory.Exists(dossier))
			{
				return new List<string>(); // Retourne une liste vide si le dossier n'existe pas
			}

			List<string> nomsImages = new List<string>(); // Liste pour stocker les noms d'images

			// Obtenir la liste des fichiers dans le dossier
			string[] fichiers = System.IO.Directory.GetFiles(dossier);

			foreach (string fichier in fichiers)
			{
				// Vérifier si le fichier est une image (vous pouvez ajouter d'autres extensions d'image si nécessaire)
				string extension = System.IO.Path.GetExtension(fichier).ToLower();
				string[] extensionsValides = { ".jpg", ".jpeg", ".png", ".gif" };

				if (Array.Exists(extensionsValides, ext => ext == extension))
				{
					nomsImages.Add(fichier);
				}
			}

			return nomsImages;
		}

        static List<string> SortImagesByColor(string folderPath)
        {
            string[] imageFiles = Directory.GetFiles(folderPath, "*.jpg");
            List<(string, Color)> imagesWithColors = new List<(string, Color)>();

            foreach (string imageFile in imageFiles)
            {
                Color avgColor = GetAverageColor(imageFile);
                imagesWithColors.Add((imageFile, avgColor));
            }

            List<string> sortedImages = imagesWithColors.OrderBy(image => ColorToInt(image.Item2)).Select(image => image.Item1).ToList();
            return sortedImages;
        }
        static int ColorToInt(Color color)
        {
            return color.ToArgb();
        }	
        static Color GetAverageColor(string imagePath)
        {
            using (Bitmap bmp = new Bitmap(imagePath))
            {
                int width = bmp.Width;
                int height = bmp.Height;
                int numPixels = width * height;
                long rTotal = 0;
                long gTotal = 0;
                long bTotal = 0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color pixelColor = bmp.GetPixel(x, y);
                        rTotal += pixelColor.R;
                        gTotal += pixelColor.G;
                        bTotal += pixelColor.B;
                    }
                }

                int avgR = (int)(rTotal / numPixels);
                int avgG = (int)(gTotal / numPixels);
                int avgB = (int)(bTotal / numPixels);

                return Color.FromArgb(avgR, avgG, avgB);
            }
        }
    }
}