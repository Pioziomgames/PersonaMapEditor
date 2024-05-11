using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace PersonaMapEditor
{
    public partial class MainForm : Form
    {
        public struct Tile
        {
            public int Id { get; set; }
            public string FileName { get; set; }
            public int SizeX { get; set; }
            public int SizeY { get; set; }
        }
        public class TileSet
        {
            public string? Name { get; set; }
            public Tile[]? Tiles { get; set; }
        }
        public struct Config
        {
            public bool DisplayGrid { get; set; }
            public bool DrawGridBehind { get; set; }
            public bool DisplayCoordinates { get; set; }
            [JsonConverter(typeof(ColorConverter))]
            public Color BackgroundColor { get; set; }
            [JsonConverter(typeof(ColorConverter))]
            public Color GridBackgroundColor { get; set; }
            [JsonConverter(typeof(ColorConverter))]
            public Color GridColor { get; set; }
            public Config()
            {
                DisplayGrid = true;
                DrawGridBehind = true;
                DisplayCoordinates = true;
                BackgroundColor = Color.FromArgb(1, 89, 128);
                GridBackgroundColor = Color.FromArgb(162, 162, 162);
                GridColor = Color.Black;
            }
        }
        Config CurrentConfig;
        public List<TileSet> TileSets = new();
        private readonly string ConfigPath;
        private string CurrentFile = string.Empty;
        public MainForm()
        {
            InitializeComponent();
            string? exePath = Path.GetDirectoryName(Application.ExecutablePath) ?? string.Empty;
            string dataPath = Path.Combine(exePath, "Data");
            bool configExists = false;
            bool loadedATileSet = false;
            ConfigPath = Path.Combine(dataPath, "config.json");
            if (Directory.Exists(dataPath))
            {
                string[] jsonPaths = Directory.GetFiles(dataPath, "*.json", SearchOption.TopDirectoryOnly);
                foreach (string path in jsonPaths)
                {
                    try
                    {
                        if (Path.GetFileName(path).ToLower() == "config.json")
                        {
                            CurrentConfig = JsonSerializer.Deserialize<Config>(File.ReadAllText(path));
                            configExists = true;
                            LoadConfig();
                        }
                        else
                        {

                            TileSet? tileSet = JsonSerializer.Deserialize<TileSet>(File.ReadAllText(path));
                            if (tileSet != null)
                                TileSets.Add(tileSet);
                            loadedATileSet = true;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(dataPath);
                }
                catch { }
            }
            if (!loadedATileSet)
            {
                MessageBox.Show("No tileset were able to be loaded.\nThis program will not work without a tileset.",
                    "Loading error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            if (!configExists)
            {
                CurrentConfig = new();
                SaveConfig();
            }
            Display.SetTileSet(TileSets[0]);
        }
        private bool seenConfigError = false;
        private void LoadConfig()
        {
            Display.BackgroundColor = CurrentConfig.BackgroundColor;
            Display.GridColor = CurrentConfig.GridColor;
            Display.GridBackgroundColor = CurrentConfig.GridBackgroundColor;

            Display.DisplayCoordinates = CurrentConfig.DisplayCoordinates;
            displayCoordinatesToolStripMenuItem.Checked = CurrentConfig.DisplayCoordinates;

            drawGridBehindTilesToolStripMenuItem.Checked = CurrentConfig.DrawGridBehind;
            Display.DrawGridBehind = CurrentConfig.DrawGridBehind;

            showGridToolStripMenuItem.Checked = CurrentConfig.DisplayGrid;
            Display.DisplayGrid = CurrentConfig.DisplayGrid;
        }
        private void SaveConfig()
        {
            try
            {
                File.WriteAllText(ConfigPath, JsonSerializer.Serialize<Config>(CurrentConfig, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch
            {
                if (!seenConfigError)
                {
                    MessageBox.Show("Unable to read/write to the config file,\nyour settings will not be saved.",
                        "Config error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    seenConfigError = true;
                }
            }
        }
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Map Files (*.map)|*.map|All Files (*.*)|*.*",
                CheckFileExists = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Display.SetMap(new MapFile(dialog.FileName));
                SetCurrentFile(dialog.FileName);
            }
        }

        private void BackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new();
            //dialog.Color = Display.BackgroundColor;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Display.BackgroundColor = dialog.Color;
                CurrentConfig.BackgroundColor = Display.BackgroundColor;
                SaveConfig();
                Display.Invalidate();
            }
        }

        private void GridBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new();
            //dialog.Color = Display.GridBackgroundColor;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Display.GridBackgroundColor = dialog.Color;
                CurrentConfig.GridBackgroundColor = Display.GridBackgroundColor;
                SaveConfig();
                Display.Invalidate();
            }
        }

        private void GridColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new();
            //dialog.Color = Display.GridColor;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Display.GridColor = dialog.Color;
                CurrentConfig.GridColor = Display.GridColor;
                SaveConfig();
                Display.Invalidate();
            }
        }

        private void ShowGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showGridToolStripMenuItem.Checked = !showGridToolStripMenuItem.Checked;
            Display.DisplayGrid = showGridToolStripMenuItem.Checked;
            CurrentConfig.DisplayGrid = Display.DisplayGrid;
            SaveConfig();
            Display.Invalidate();
        }

        private void DrawGridBehindTilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawGridBehindTilesToolStripMenuItem.Checked = !drawGridBehindTilesToolStripMenuItem.Checked;
            Display.DrawGridBehind = drawGridBehindTilesToolStripMenuItem.Checked;
            CurrentConfig.DrawGridBehind = Display.DrawGridBehind;
            SaveConfig();
            Display.Invalidate();
        }

        private void DisplayCoordinatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            displayCoordinatesToolStripMenuItem.Checked = !displayCoordinatesToolStripMenuItem.Checked;
            Display.DisplayCoordinates = displayCoordinatesToolStripMenuItem.Checked;
            CurrentConfig.DisplayCoordinates = Display.DisplayCoordinates;
            SaveConfig();
            Display.Invalidate();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (string.IsNullOrEmpty(CurrentFile))
            {
                SaveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                Display.SaveMap(CurrentFile);
            }
            Cursor.Current = Cursors.Default;
        }
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new()
            {
                Title = "Save your map file",
                Filter = "Map Files (*.map)|*.map|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Display.SaveMap(dialog.FileName);
                SetCurrentFile(dialog.FileName);
            }
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Display.SetMap(new MapFile());
            UnsetCurrentFile();
        }
        private void SetCurrentFile(string fileName)
        {
            CurrentFile = fileName;
            Text = "Persona Map Editor - " + fileName;
        }
        private void UnsetCurrentFile()
        {
            CurrentFile = string.Empty;
            Text = "Persona Map Editor";
        }
        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    try
                    {
                        Display.SetMap(new MapFile(files[0]));
                    }
                    catch
                    {
                        MessageBox.Show("Error while reading map file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentConfig = new();
            LoadConfig();
            SaveConfig();
            Invalidate();
        }

    }
    public class ColorConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected StartObject token");

            int r = 0, g = 0, b = 0, a = 255; // Defaults
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("Expected PropertyName token");

                string? propertyName = reader.GetString() ?? throw new JsonException("Expected PropertyName token got null");
                reader.Read();
                switch (propertyName)
                {
                    case "R":
                        r = reader.GetInt32();
                        break;
                    case "G":
                        g = reader.GetInt32();
                        break;
                    case "B":
                        b = reader.GetInt32();
                        break;
                    case "A":
                        a = reader.GetInt32();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            return Color.FromArgb(a, r, g, b);
        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("R", value.R);
            writer.WriteNumber("G", value.G);
            writer.WriteNumber("B", value.B);
            writer.WriteNumber("A", value.A);
            writer.WriteEndObject();
        }
    }
}