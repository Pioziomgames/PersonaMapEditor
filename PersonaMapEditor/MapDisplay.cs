using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PersonaMapEditor.MainForm;

namespace PersonaMapEditor
{
    public partial class MapDisplay : UserControl
    {
        public MapFile Map;
        public Color BackgroundColor;
        public Color GridBackgroundColor;
        public Color GridColor;
        public bool DisplayGrid = true;
        public bool DrawGridBehind = true;
        public bool DisplayCoordinates = true;
        public Dictionary<int,Image> TileImages;
        private new Font Font;
        private int BiggestX = 0;
        private int BiggestY = 0;
        private Point[]? SpritePositions;
        private int xOffset;
        private int yOffset;
        private int size;
        private int TileSize;
        private TileSet? TileSet;
        private int draggingTileX = -1;
        private int draggingTileY = -1;
        private int MouseX = 0;
        private int MouseY = 0;
        private int addingTile = -1;
        private int curX = -1;
        private int curY = -1;
        private int addRot = 0;
        public MapDisplay()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            Map = new();
            BackgroundColor = Color.FromArgb(1, 89, 128);
            GridBackgroundColor = Color.FromArgb(162, 162, 162);
            GridColor = Color.Black;
            TileImages = new();
            Font = new Font("Arial", Math.Min(ClientSize.Width, ClientSize.Height) / 16);
        }
        public void LoadImages()
        {
            TileImages.Clear();
            try
            {
                string? exePath = Path.GetDirectoryName(Application.ExecutablePath);
                exePath ??= string.Empty;
                string tilesPath = Path.Combine(exePath, "Data\\TileImages");
                if (Directory.Exists(tilesPath) && TileSet != null && TileSet.Tiles != null)
                {
                    for (int i = 0; i < TileSet.Tiles.Length; i++)
                    {
                        TileImages.Add(TileSet.Tiles[i].Id, Bitmap.FromFile(Path.Combine(tilesPath, TileSet.Tiles[i].FileName)));
                        if (TileSet.Tiles[i].SizeX > BiggestX)
                            BiggestX = TileSet.Tiles[i].SizeX;
                        if (TileSet.Tiles[i].SizeY > BiggestY)
                            BiggestY = TileSet.Tiles[i].SizeY;
                    }
                    SpritePositions = new Point[TileImages.Count];
                    return;
                }
            }
            catch { }
            SpritePositions = new Point[TileImages.Count];
            TileSet = null;
        }
        public void SetMap(MapFile map)
        {
            Map = map;
            Invalidate();
        }
        public void SaveMap(string path)
        {
            Map.Save(path);
        }
        public void SetTileSet(TileSet tileSet)
        {
            TileSet = tileSet;
            LoadImages();
            Invalidate();
        }
        protected Point FindXY(int x, int y)
        {
            int X = -1;
            int Y = -1;
            if (x < xOffset || x > xOffset + size || y < yOffset || y > yOffset + size)
            {
                return new Point(-2, -2);
            }

            for (int i = 0; i < 16; i++)
            {
                if (xOffset + TileSize * i <= x && xOffset + TileSize * (i + 1) > x)
                    X = i;
                if (yOffset + TileSize * i <= y && yOffset + TileSize * (i + 1) > y)
                    Y = i;
            }
            return new Point(X, Y);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            size = Math.Min(ClientSize.Width, ClientSize.Height);
            TileSize = (int)((double)size / 16.0);
            size = TileSize * 16;
            if (ClientSize.Width >= ClientSize.Height)
            {
                xOffset = (ClientSize.Width - size) / 4;
                yOffset = (ClientSize.Height - size) / 2;
            }
            else
            {
                xOffset = (ClientSize.Width - size) / 2;
                yOffset = (int)Math.Round((ClientSize.Height - size) / 1.5f);
            }
            using (SolidBrush brush = new(BackgroundColor))
                g.FillRectangle(brush, 0, 0, ClientSize.Width, ClientSize.Height);
            using (SolidBrush brush = new(GridBackgroundColor))
                g.FillRectangle(brush, xOffset, yOffset, size, size);

            using (Pen pen = new(GridColor))
            {
                if (DisplayGrid && DrawGridBehind)
                {
                    for (int i = 0; i < 16; i++)
                        for (int j = 0; j < 16; j++)
                            g.DrawRectangle(pen, xOffset + TileSize * j, yOffset + TileSize * i, TileSize, TileSize);
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        if (Map.Tiles[i][j].IsUsed != 0 && Map.Tiles[i][j].IsUsed2 != 0 &&
                            (j != draggingTileX || i != draggingTileY))
                        {
                            DrawTileImage(g, Map.Tiles[i][j], xOffset + TileSize * j, yOffset + TileSize * i);
                        }
                        if (DisplayGrid && !DrawGridBehind)
                            g.DrawRectangle(pen, xOffset + TileSize * j, yOffset + TileSize * i, TileSize, TileSize);
                    }
                }
            }
            if (draggingTileX != -1 && draggingTileY != -1)
            {
                MapTile tile = Map.Tiles[draggingTileY][draggingTileX];
                DrawTileImage(g, tile, MouseX - (TileSize * tile.TileSizeX) / 2, MouseY - (TileSize * tile.TileSizeY) / 2);
            }

            if (TileSet != null && TileSet.Tiles != null && SpritePositions != null)
            {
                float Xprogress = 0.5f;
                float Yprogress = 0.5f;
                for (int i = 0; i < TileSet.Tiles.Length; i++)
                {
                    Point pos;
                    if (ClientSize.Width >= ClientSize.Height)
                    {
                        pos = new Point(
                        (int)(((double)ClientSize.Width - (0.5 + (double)BiggestX - ((double)BiggestX - (double)TileSet.Tiles[i].SizeX) / 2.0) * TileSize) - Xprogress * TileSize),
                        (int)((0.5 + Yprogress + ((double)BiggestY - (double)TileSet.Tiles[i].SizeY / 2.0) - 1) * TileSize));
                        Yprogress += BiggestY + 0.5f;
                        if ((Yprogress + BiggestY + 0.5f) * TileSize >= ClientSize.Height)
                        {
                            Yprogress = 0.5f;
                            Xprogress = BiggestX + 1f;
                        }
                    }
                    else
                    {
                        pos = new Point(
                        (int)((Xprogress + 0.5 + ((double)BiggestX - (double)TileSet.Tiles[i].SizeX) / 2.0) * TileSize),
                        (int)((0.5 + Yprogress + ((double)BiggestY - (double)TileSet.Tiles[i].SizeY / 2.0) - 1) * TileSize));
                        Xprogress += BiggestY + 0.5f;
                        if ((Xprogress + BiggestX + 0.5f) * TileSize >= ClientSize.Width)
                        {
                            Xprogress = 0.5f;
                            Yprogress = BiggestX + 1f;
                        }
                    }
                    SpritePositions[i] = pos;
                    g.DrawImage(TileImages[TileSet.Tiles[i].Id],
                        pos.X, pos.Y, TileSize * TileSet.Tiles[i].SizeX, TileSize * TileSet.Tiles[i].SizeY);
                }

                if (addingTile > -1)
                {
                    int x = MouseX - (TileSize * TileSet.Tiles[addingTile].SizeX) / 2;
                    int y = MouseY - (TileSize * TileSet.Tiles[addingTile].SizeY) / 2;
                    int width = TileSize * TileSet.Tiles[addingTile].SizeX;
                    int height = TileSize * TileSet.Tiles[addingTile].SizeY;
                    if (addRot == 1)
                        g.DrawImage(TileImages[TileSet.Tiles[addingTile].Id], new Point[] {
                            new Point(x, y + height),
                            new Point(x, y),
                            new Point(x + width, y + height)});
                    else if (addRot == 2)
                        g.DrawImage(TileImages[TileSet.Tiles[addingTile].Id], new Point[] {
                            new Point(x + width, y + height),
                            new Point(x, y + height),
                            new Point(x + width, y)});
                    else if (addRot == 3)
                        g.DrawImage(TileImages[TileSet.Tiles[addingTile].Id], new Point[] {
                            new Point(x + width, y),
                            new Point(x + width, y + height),
                            new Point(x, y)});
                    else
                        g.DrawImage(TileImages[TileSet.Tiles[addingTile].Id], x, y, width, height);
                }
            }

            if (DisplayCoordinates && curX > -1 && curY > -1)
            {
                using (SolidBrush brush = new(GridColor))
                {
                    g.DrawString($"{curX + 1:D2}:{curY + 1:D2}", Font, brush, ClientSize.Width - Font.SizeInPoints * 4, 0);
                }
            }
        }
        void DrawTileImage(Graphics g, MapTile tile, int x, int y)
        {
            int width = TileSize * tile.TileSizeX;
            int height = TileSize * tile.TileSizeY;
            if (tile.Rotation == 1)
                g.DrawImage(TileImages[tile.TileType], new Point[] {
                    new Point(x, y + height),
                    new Point(x, y),
                    new Point(x + width, y + height)});
            else if (tile.Rotation == 2)
                g.DrawImage(TileImages[tile.TileType], new Point[] {
                    new Point(x + width, y + height),
                    new Point(x, y + height),
                    new Point(x + width, y)});
            else if (tile.Rotation == 3)
                g.DrawImage(TileImages[tile.TileType], new Point[] {
                    new Point(x + width, y),
                    new Point(x + width, y + height),
                    new Point(x, y)});
            else
                g.DrawImage(TileImages[tile.TileType], x, y, width, height);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
            Font = new Font("Arial", Math.Max(Math.Min(ClientSize.Width, ClientSize.Height) / 16, 1));
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (curX == -1 || curY == -1)
                    return;

                if (curX == -2 || curY == -2)
                {
                    if (TileSet != null && TileSet.Tiles != null && SpritePositions != null)
                    {
                        for (int i = 0; i < TileSet.Tiles.Length; i++)
                        {
                            if (e.X >= SpritePositions[i].X && e.X <= SpritePositions[i].X + BiggestX * TileSize &&
                                e.Y >= SpritePositions[i].Y && e.Y <= SpritePositions[i].Y + BiggestY * TileSize)
                            {
                                addingTile = i;
                                break;
                            }
                        }
                    }
                    return;
                }

                MapTile tile = Map.Tiles[curY][curX];
                if (tile.IsUsed != 1 && tile.IsUsed2 != 1)
                    return;
                draggingTileX = curX;
                draggingTileY = curY;
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (draggingTileX > -1 && draggingTileY > -1)
                {
                    if (Map.Tiles[draggingTileY][draggingTileX].Rotation >= 3)
                        Map.Tiles[draggingTileY][draggingTileX].Rotation = 0;
                    else
                        Map.Tiles[draggingTileY][draggingTileX].Rotation++;
                    Invalidate();
                }
                else if (addingTile > -1)
                {
                    if (addRot >= 3)
                        addRot = 0;
                    else
                        addRot++;
                    Invalidate();
                }
                else if (curY > -1 && curX > -1 && Map.Tiles[curY][curX].IsUsed != 0 && Map.Tiles[curY][curX].IsUsed2 != 0)
                {
                    if (Map.Tiles[curY][curX].Rotation >= 3)
                        Map.Tiles[curY][curX].Rotation = 0;
                    else
                        Map.Tiles[curY][curX].Rotation++;
                    Invalidate();
                }
            }
            
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (addingTile > -1 && TileSet != null && TileSet.Tiles != null)
                {
                    if (curY > -1 && curX > -1)
                    {
                        Map.Tiles[curY][curX] = new MapTile(
                            (byte)TileSet.Tiles[addingTile].Id, (byte)TileSet.Tiles[addingTile].SizeX,
                            (byte)TileSet.Tiles[addingTile].SizeY, (byte)addRot);
                    }
                    addingTile = -1;
                    addRot = 0;
                    Invalidate();
                }
                else if (draggingTileX != -1 || draggingTileY != -1)
                {
                    if (curX == -2 && curY == -2)
                        Map.Tiles[draggingTileY][draggingTileX] = new MapTile();
                    else if (curX != -1 && curY != -1)
                    {
                        (Map.Tiles[curY][curX], Map.Tiles[draggingTileY][draggingTileX]) =
                            (Map.Tiles[draggingTileY][draggingTileX], Map.Tiles[curY][curX]);
                    }
                    draggingTileX = -1;
                    draggingTileY = -1;
                    Invalidate();
                }
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;
            bool redraw = false;
            if (draggingTileX > -1 && draggingTileY > -1)
            {
                if (Map.Tiles[draggingTileY][draggingTileX].TileSizeX > 1)
                    x -= (TileSize * Map.Tiles[draggingTileY][draggingTileX].TileSizeX) / 4;
                if (Map.Tiles[draggingTileY][draggingTileX].TileSizeY > 1)
                    y -= (TileSize * Map.Tiles[draggingTileY][draggingTileX].TileSizeY) / 4;
                redraw = true;
            }
            else if (addingTile > -1 && TileSet?.Tiles != null)
            {
                if (TileSet.Tiles[addingTile].SizeX > 1)
                    x -= (TileSize * TileSet.Tiles[addingTile].SizeX) / 4;
                if (TileSet.Tiles[addingTile].SizeY > 1)
                    y -= (TileSize * TileSet.Tiles[addingTile].SizeY) / 4;
                redraw = true;
            }
            Point xy = FindXY(x, y);
            curX = xy.X;
            curY = xy.Y;
            MouseX = e.X;
            MouseY = e.Y;
            if (DisplayCoordinates || redraw)
                Invalidate();
        }
    }
}
