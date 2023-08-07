<?php
function determineCarreTailles($nombreCarres, $zoneLargeur = 1920, $zoneHauteur = 823)
{
  // Calcul de la taille d'un côté des carrés
  $cote = sqrt(($zoneLargeur * $zoneHauteur) / $nombreCarres);

  // Calcul du nombre de lignes et de colonnes
  $nombreLignes = floor($zoneHauteur / $cote);
  $nombreColonnes = ceil($nombreCarres / $nombreLignes);

  return array(
    'cote' => $cote,
    'nombreLignes' => $nombreLignes,
    'nombreColonnes' => $nombreColonnes
  );
}

function afficherTableauImages($nombreCarres, $images)
{
  $resultat = determineCarreTailles($nombreCarres);
  $cote = $resultat['cote'];
  $nombreLignes = $resultat['nombreLignes'];
  $nombreColonnes = $resultat['nombreColonnes'];

  echo "<table>";

  $z = 0;

  for ($i = 0; $i < $nombreLignes; $i++) {
    echo "<tr>";
    for ($j = 0; $j < $nombreColonnes; $j++) {
      echo "<td><img src='" . $images[$z] . "' style='width: " . $cote . "px; height: " . $cote . "px;' /></td>";
      $z++;
    }
    echo "</tr>";
  }
  echo "</table>";
} // Exemple d'utilisation : 


function getListeNomsImages($dossier)
{
  // Vérifier si le dossier existe
  if (!is_dir($dossier)) {
    return array(); // Retourne un tableau vide si le dossier n'existe pas
  }

  $nomsImages = array(); // Tableau pour stocker les noms d'images

  // Ouvrir le dossier
  if ($handle = opendir($dossier)) {
    while (false !== ($fichier = readdir($handle))) {
      // Exclure les dossiers '.' et '..'
      if ($fichier !== '.' && $fichier !== '..') {
        // Vérifier si le fichier est une image (vous pouvez ajouter d'autres extensions d'image si nécessaire)
        $extensionsValides = array('jpg', 'jpeg', 'png', 'gif');
        $extension = strtolower(pathinfo($fichier, PATHINFO_EXTENSION));
        if (in_array($extension, $extensionsValides)) {
          $nomsImages[] = $fichier;
        }
      }
    }
    closedir($handle); // Fermer le dossier
  }

  return $nomsImages;
}

// Exemple d'utilisation :
$dossierImages = "./"; // Remplacez "chemin_vers_votre_dossier" par le chemin du dossier contenant les images
$listeNomsImages = getListeNomsImages($dossierImages);


$nombreCarres = count($listeNomsImages);

afficherTableauImages($nombreCarres, $listeNomsImages); ?>


<style>
  * {
    margin: 0;
    padding: 0;
    border-collapse: collapse;
  }
</style>

<script>

  console.log(window.innerHeight);
  console.log(window.innerWidth); </script>