using System.Security.Cryptography;
using System.Text;

namespace HashGuard {
    internal class Program {

        private static readonly string[] hashStrings = {
        "MD5",
        "SHA1",
        "SHA256",
        "SHA384",
        "SHA512",
        };

        private static void Main(string[] args) {
            while (true) {
                string modesString = null;
                for (int i = 1; i <= (int)Mode.CompareFiles; i++) {
                    modesString += i + ". " + (Mode)i + "\n";
                }
                modesString += "Choose what to hash:";
                int mode = InputToMode(modesString);
                Console.Clear();

                switch ((Mode)mode) {
                    case Mode.Text:
                        ModeText("Input text: ");
                        break;
                    case Mode.File:
                        ModeFile("File path: ");
                        break;
                    case Mode.CompareTexts:
                        ModeComTexts();
                        break;
                    case Mode.CompareFiles:
                        ModeComFiles();
                        break;
                }
            }
        }

        private static string[] ModeText(string message) {
            Console.Write(message);
            string textToHash = Console.ReadLine();
            byte[] textBytes = Encoding.UTF8.GetBytes(textToHash);

            string[] hashes = new string[hashStrings.Length];

            for (int i = 0; i < hashStrings.Length; i++) {
                using (var hasher = HashAlgorithm.Create(hashStrings[i])) {
                    byte[] hashBytes = hasher.ComputeHash(textBytes);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                    hashes[i] = hash;
                    Console.WriteLine($"Checksum {i} ({hashStrings[i]}): {hash.ToLower()}");
                }
            }
            return hashes;
        }

        private static string[] ModeFile(string message) {
            string filePath = InputToValidFilePath(message);
            Console.WriteLine("FilePath - " + filePath);

            if (filePath.EndsWith(".lnk")) {
                filePath = UtilityKit.ShortcutUtility.GetShortcutTarget(filePath);
            }

            string[] hashes = new string[hashStrings.Length];
            try {
                using (var fileStream = File.OpenRead(filePath)) {

                    for (int i = 0; i < hashStrings.Length; i++) {
                        using (var hasher = HashAlgorithm.Create(hashStrings[i])) {
                            fileStream.Position = 0;

                            byte[] hashBytes = hasher.ComputeHash(fileStream);
                            string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                            hashes[i] = hash;

                            Console.WriteLine($"Checksum ({hashStrings[i]}): {hash.ToLower()}");
                        }
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"An error occurred in {Mode.File}: {ex.Message}");
            }
            return hashes;
        }

        private static void ModeComTexts() {
            string[] firstTextHashes = ModeText("First Text: ");
            string[] secondTextHashes = ModeText("Second Text: ");

            while (!ComparingHashes(firstTextHashes, secondTextHashes)) {
                var inputString = Console.ReadKey(intercept: true);
                if (inputString.Key == ConsoleKey.Escape) {
                    Environment.Exit(0);
                }
            }
        }

        private static void ModeComFiles() {
            string[] firstTextHashes = ModeFile("First file path: ");
            string[] secondTextHashes = ModeFile("Second file path: ");

            while (!ComparingHashes(firstTextHashes, secondTextHashes)) {
                var inputString = Console.ReadKey(intercept: true);
                if (inputString.Key == ConsoleKey.Escape) {
                    Environment.Exit(0);
                }
            }
        }

        private static bool ComparingHashes(string[] firstSet, string[] secondSet) {
            if (firstSet.Length != secondSet.Length) {
                Console.WriteLine($"Not an equal number of hashes in both sets. First: {firstSet.Length}, Second: {secondSet.Length}.");
                return false;
            }
            Console.WriteLine("Comparing two sets of hashes.");
            for (int i = 0; i < firstSet.Length; i++) {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"({hashStrings[i]}) - ");
                Console.ForegroundColor = firstSet[i] == secondSet[i] ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"{(firstSet[i] == secondSet[i] ? "Match" : "Mismatch")}");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            return true;
        }

        private static string InputToValidFilePath(string message) {
            while (true) {
                Console.Write(message);
                string inputString = Console.ReadLine().Trim('\"');

                if (!File.Exists(inputString)) {
                    Console.Clear();
                    Console.WriteLine("File not found. Please enter a valid file path. ");
                    continue;
                }
                return inputString;
            }
        }

        private static int InputToMode(string message) {
            while (true) {
                Console.Write(message);
                var input = Console.ReadKey();

                if (char.IsDigit(input.KeyChar)) {
                    var number = int.Parse(input.KeyChar.ToString());
                    if (number < 1 && number > (int)Mode.CompareFiles) {
                        Console.WriteLine("Index out of bounds.");
                    }
                    else {
                        return number;
                    }
                }
                else if (input.Key == ConsoleKey.Escape) {
                    Environment.Exit(0);
                }
                else {
                    Console.WriteLine("Invalid input!");
                }
            }
        }
    }
}
