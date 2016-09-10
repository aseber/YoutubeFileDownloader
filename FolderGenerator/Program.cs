using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FolderGenerator
{
    class Program
    {

        // lavenshein of authors and songs
        // move featured to author side
        // move remix to song side
        // create folders and move files to folders
        // add authors to extended data
        // parallelize all functions with default function taking a Func<>() and parallelization parameter
        // levenshtein on different values

        static void Main(string[] args)
        {

            var files = Directory.GetFiles(@"S:\OneDrive\Personal Data\Audio\Unsorted Music");

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var mainAuthors = findMainAuthors(file);

                var locationBase = @"S:\OneDrive\Personal Data\Audio\Unsorted Music\";
                var oldLocation = $"{locationBase}{fileName}";
                var newLocation = $"{locationBase}{mainAuthors}\\{fileName}";
                Console.WriteLine($"\"{oldLocation}\" => \"{newLocation}\"");
                File.Move(oldLocation, newLocation);
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static IEnumerable<string> getFileInformation(string[] files, Func<string, string> function)
        {
            var caughtChanges = new List<string>();

            foreach (var file in files)
            {
                try
                {
                    var caughtChange = function(file);

                    if (caughtChange != null)
                    {
                        caughtChanges.Add(caughtChange);
                    }
                }
                catch (Exception e)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    Console.WriteLine($"Error occured: {e.GetType()} on file {fileName}");
                }
            }

            return caughtChanges;
        }

        private static void changeFiles(string[] files, Func<string, Tuple<string, string, string, string>> function)
        {
            var caughtChanges = new List<Tuple<string, string, string, string>>();

            foreach (var file in files)
            {
                try
                {
                    var caughtChange = function(file);

                    if (caughtChange != null)
                    {
                        caughtChanges.Add(caughtChange);
                    }
                } catch (Exception e)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    Console.WriteLine($"Error occured: {e.GetType()} on file {fileName}");
                }
            }

            printCaughtFiles(caughtChanges);

            if (caughtChanges.Count > 0)
            {
                bool commit = false;

                do { Console.WriteLine("Commit changes? (True/False)"); }
                while (!bool.TryParse(Console.ReadLine(), out commit));

                if (commit)
                {
                    foreach (var change in caughtChanges)
                    {
                        renameFile(change.Item2, change.Item3, change.Item4);
                    }

                    Console.WriteLine("Finished renaming files");
                }
            }
        }

        private static void renameFile(string file, string originalFileName, string newFileName)
        {
            var filePath = Path.GetDirectoryName(file);
            var fileName = Path.GetFileNameWithoutExtension(file);
            var extension = Path.GetExtension(file);

            File.Move($"{filePath}//{originalFileName + extension}", $"{filePath}//{newFileName + extension}");
        }

        private static void printCaughtFiles(IEnumerable<Tuple<string, string, string, string>> caughtChanges)
        {
            Console.WriteLine($"Caught {caughtChanges.Count()} files:");

            foreach (var change in caughtChanges)
            {
                Console.WriteLine($"\t{change.Item1}");
            }
        }

        private static string fixSpaces(string str)
        {
            var returnString = str;
            returnString = returnString.Trim();
            returnString = returnString.Replace("    ", " ");
            returnString = returnString.Replace("   ", " ");
            returnString = returnString.Replace("  ", " ");
            returnString = returnString.Trim();

            return returnString;
        }

        private static string findFeatured(string file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);

            if (fileName.Contains("feat."))
            {
                return fileName;
            }

            return null;
        }

        private static Tuple<string, string, string, string> changeFeatured(string file)
        {
            var filePath = Path.GetDirectoryName(file);
            var fileName = Path.GetFileNameWithoutExtension(file);
            var extension = Path.GetExtension(file);

            var originalFileName = fileName;

            if (fileName.Contains("feat."))
            {
                var featureIndex = fileName.IndexOf("feat.");
                var leadingCharacterIndex = featureIndex - 1;
             
                if (fileName[leadingCharacterIndex] == '(')
                {
                    var trailingCharacterIndex = fileName.IndexOf(')', featureIndex);
                    var distance = trailingCharacterIndex - leadingCharacterIndex + 1;
                    var textToMove = fileName.Substring(leadingCharacterIndex, distance);
                    fileName = fileName.Remove(leadingCharacterIndex, distance);
                    fileName = fixSpaces(fileName);

                    var splitData = fileName.Split('-');

                    var authors = splitData[0].Trim();
                    var song = splitData[1].Trim();
                    authors = $"{authors} {textToMove}";

                    fileName = $"{authors} - {song}";
                }
            }

            if (fileName != originalFileName)
            {
                return Tuple.Create($"\"{originalFileName}\" => \"{fileName}\"", file, originalFileName, fileName);
            }

            return null;
        }

        private static string findAuthorNames(string file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);

            var data = fileName.Split('-');

            if (data.Length == 2)
            {
                return data[0].Trim();
            }

            return null;
        }

        private static string findFeaturedAuthors(string file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);

            var data = fileName.Split('-');

            if (data.Length == 2)
            {
                var author = data[0].Trim();
                if (author.Contains("feat."))
                {
                    var featureIndex = author.IndexOf("feat.");

                    var leadingCharacterIndex = featureIndex - 1;
                    var leadingCharacter = author.ToCharArray()[leadingCharacterIndex];

                    if (leadingCharacter == '(')
                    {
                        var trailingParenthesisIndex = author.IndexOf(')', featureIndex);

                        author = author.Remove(0, leadingCharacterIndex - 1);
                        author = fixSpaces(author);

                        return author;
                    }
                }
                else
                {
                    return "";
                }
            }

            return null;
        }

        private static string findMainAuthors(string file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);

            var data = fileName.Split('-');

            if (data.Length == 2)
            {
                var author = data[0].Trim();
                if (author.Contains("feat."))
                {
                    var featureIndex = author.IndexOf("feat.");

                    var leadingCharacterIndex = featureIndex - 1;
                    var leadingCharacter = author.ToCharArray()[leadingCharacterIndex];

                    if (leadingCharacter == '(')
                    {
                        var trailingParenthesisIndex = author.IndexOf(')', featureIndex);
                        var distance = trailingParenthesisIndex - leadingCharacterIndex + 1;

                        author = author.Remove(leadingCharacterIndex, distance);
                        author = fixSpaces(author);

                        return author;
                    }
                } else
                {
                    return author;
                }
            }

            return null;
        }

        private static string findSongName(string file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);

            var data = fileName.Split('-');

            if (data.Length == 2)
            {
                return data[1].Trim();
            }

            return null;
        }

        private static string findInvalidFileNames(string file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);

            var data = fileName.Split('-');

            if (data.Length != 2)
            {
                return fileName;
            }

            return null;
        }

        private static Tuple<string, string, string, string> wrapFeatured(string file)
        {
            var feature = "feat.";

            var filePath = Path.GetDirectoryName(file);
            var fileName = Path.GetFileNameWithoutExtension(file);
            var extension = Path.GetExtension(file);

            var originalFileName = fileName;

            if (fileName.Contains(feature))
            {
                var featureIndex = fileName.IndexOf(feature);
                var featureLength = feature.Length;
                var leadingCharacterIndex = featureIndex - 1;
                var leadingCharacter = fileName.ToCharArray()[leadingCharacterIndex];
                var trailingCharacter = fileName.ToCharArray()[featureIndex + featureLength];

                if (leadingCharacter != '(')
                {
                    fileName = fileName.Insert(leadingCharacterIndex + 1, "(");
                    fileName = fileName.Insert(fileName.Length, ")");
                }
            }

            if (fileName != originalFileName)
            {
                return Tuple.Create($"\"{originalFileName}\" => \"{fileName}\"", file, originalFileName, fileName);
            }

            return null;
        }

        private static Tuple<string, string, string, string> fixFeatured(string file)
        {
            var featuredType = new[]
            {
                "featuring",
                "featured",
                "feat",
                "ft"
            };
            var filePath = Path.GetDirectoryName(file);
            var fileName = Path.GetFileNameWithoutExtension(file);
            var extension = Path.GetExtension(file);

            var originalFileName = fileName;

            foreach (var feature in featuredType)
            {
                var lowerCaseFileName = fileName.ToLower();

                if (lowerCaseFileName.Contains(feature))
                {
                    var featureIndex = lowerCaseFileName.IndexOf(feature);
                    var featureLength = feature.Length;
                    var leadingCharacter = fileName.ToCharArray()[featureIndex - 1];
                    var trailingCharacter = fileName.ToCharArray()[featureIndex + featureLength];

                    if (leadingCharacter == ' ' || leadingCharacter == '(')
                    {
                        switch (trailingCharacter)
                        {
                            case '.':
                                if (feature != "feat")
                                {
                                    fileName = fileName.Remove(featureIndex, featureLength);
                                    fileName = fileName.Insert(featureIndex, "feat");
                                }

                                break;
                            default:
                                fileName = fileName.Remove(featureIndex, featureLength);
                                fileName = fileName.Insert(featureIndex, "feat.");
                                break;
                        }
                    }
                }
            }

            if (fileName != originalFileName)
            {
                return Tuple.Create($"\"{originalFileName}\" => \"{fileName}\"", file, originalFileName, fileName);
            }

            return null;
        }

        private static Tuple<string, string, string, string> fixFileName(string file)
        {
            var trash = new[] {
                "lyric",
                "exclusive",
                "premiere",
                "video",
                "mix",
                "radio",
                "audio",
                "cover",
                "hd",
                "official",
                "edm",
                "free",
                "lyrics",
                "lyrics",
                "with",
                "cover art",
                "out now",
                "ncs",
                "original",
            };

            var filePath = Path.GetDirectoryName(file);
            var fileName = Path.GetFileNameWithoutExtension(file);
            var extension = Path.GetExtension(file);

            var originalFileName = fileName;

            foreach (var trashPiece in trash)
            {
                var lowerCaseFileName = fileName.ToLower();

                if (lowerCaseFileName.Contains(trashPiece))
                {
                    var index = lowerCaseFileName.IndexOf(trashPiece);
                    var length = trashPiece.Length;

                    int leadingCharIndex = index - 1;

                    if (leadingCharIndex < 0)
                    {
                        Console.WriteLine($"Error: below zero leadingCharIndex {trashPiece} | {fileName}");
                        continue;
                    }

                    var leadingChar = fileName.ToCharArray()[leadingCharIndex];
                    int trailingCharIndex = -1;

                    switch (leadingChar)
                    {
                        case '[':
                            trailingCharIndex = fileName.IndexOf(']', index);
                            break;
                        case '(':
                            trailingCharIndex = fileName.IndexOf(')', index);
                            break;
                        default:
                            break;
                    }

                    if (trailingCharIndex != -1)
                    {

                        var distance = trailingCharIndex - leadingCharIndex + 1;

                        Console.WriteLine($"\"{fileName.Substring(leadingCharIndex, distance)}\"");
                        fileName = fileName.Remove(leadingCharIndex, distance);

                    }
                }
            }

            fileName = fixSpaces(fileName);

            if (fileName != originalFileName)
            {
                return Tuple.Create($"\"{originalFileName}\" => \"{fileName}\"", file, originalFileName, fileName);
            }

            return null;
        }

        private static int levenshtein(string a, string b)
        {
            if (string.IsNullOrEmpty(a))
            {
                if (!string.IsNullOrEmpty(b))
                {
                    return b.Length;
                }
                return 0;
            }

            if (string.IsNullOrEmpty(b))
            {
                if (!string.IsNullOrEmpty(a))
                {
                    return a.Length;
                }
                return 0;
            }

            int cost;
            int[,] d = new int[a.Length + 1, b.Length + 1];
            int min1;
            int min2;
            int min3;

            for (var i = 0; i <= d.GetUpperBound(0); i += 1)
            {
                d[i, 0] = i;
            }

            for (var i = 0; i <= d.GetUpperBound(1); i += 1)
            {
                d[0, i] = i;
            }

            for (var i = 1; i <= d.GetUpperBound(0); i += 1)
            {
                for (var j = 1; j <= d.GetUpperBound(1); j += 1)
                {
                    cost = Convert.ToInt32(!(a[i - 1] == b[j - 1]));

                    min1 = d[i - 1, j] + 1;
                    min2 = d[i, j - 1] + 1;
                    min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];
        }

        private static List<Tuple<string, string>> findFileSimilaritiesParallel(IEnumerable<string> files, int maximumDistance)
        {
            var count = new ConcurrentBag<Tuple<string, string>>();

            Parallel.For(0, files.Count(), new ParallelOptions { MaxDegreeOfParallelism = 8 },
            (i) => {
                var file = files.ElementAt(i);

                Parallel.For(i + 1, files.Count(), new ParallelOptions { MaxDegreeOfParallelism = 2 },
                (j) => {
                    var file2 = files.ElementAt(j);

                    var fileName = Path.GetFileNameWithoutExtension(file);
                    var fileName2 = Path.GetFileNameWithoutExtension(file2);

                    var filePath = Path.GetFullPath(file);
                    var filePath2 = Path.GetFullPath(file2);

                    if (filePath != filePath2)
                    {
                        var distance = levenshtein(fileName, fileName2);

                        if (distance <= maximumDistance)
                        {
                            count.Add(Tuple.Create(fileName, fileName2));
                            Console.WriteLine($"Files \"{fileName}\" and \"{fileName2}\" are similar in name (distance: {distance})");
                        }
                    }
                });
            });

            var list = new List<Tuple<string, string>>();
            list.AddRange(count);

            return list;
        }
    }
}