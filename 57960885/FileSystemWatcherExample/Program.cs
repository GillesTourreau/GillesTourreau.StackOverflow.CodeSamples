namespace FileSystemWatcherExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            program.WatchDirectory("C:\\Temp", "Test.txt");

            Console.WriteLine("Watching changes...");
            Console.ReadLine();
        }

        private void WatchDirectory(string directoryPath, string filter)
        {
            var fileWatcher = new FileSystemWatcher(directoryPath, filter);
            fileWatcher.Changed += this.OnFileChanged;
            fileWatcher.Created += this.OnFileChanged;
            fileWatcher.Deleted += this.OnFileChanged;
            fileWatcher.Renamed += this.OnFileChanged;

            fileWatcher.EnableRaisingEvents = true;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File updated: {e.Name} (ChangeType: {e.ChangeType})");
        }
    }
}
