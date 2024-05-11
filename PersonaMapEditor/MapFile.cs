using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaMapEditor
{
    public class MapFile
    {
        public ushort MajorId;
        public ushort MinorId;
        public MapTile[][] Tiles;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MapFile()
        {
            Tiles = new MapTile[16][];
            for (int i = 0; i < 16; i++)
            {
                MapTile[] row = new MapTile[16];
                for (int j = 0; j < 16; j++)
                    row[j] = new MapTile();
                Tiles[i] = row;
            }
        }
        public MapFile(string path)
        {
            using (BinaryReader reader = new(File.OpenRead(path)))
                Read(reader);
        }
        public MapFile(BinaryReader reader)
        {
            Read(reader);
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal void Read(BinaryReader reader)
        {
            MajorId = reader.ReadUInt16();
            MinorId = reader.ReadUInt16();
            reader.BaseStream.Position -= 4;
            Tiles = new MapTile[16][];
            for (int i = 0; i < 16; i++)
            {
                MapTile[] row = new MapTile[16];
                for (int j = 0;  j < 16; j++)
                    row[j] = new MapTile(reader);
                Tiles[i] = row;
            }
        }
        internal void Write(BinaryWriter writer)
        {
            writer.Write(MajorId);
            writer.Write(MinorId);
            writer.BaseStream.Position -= 4;
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                    Tiles[i][j].Write(writer);
        }
        public void Save(string path)
        {
            using (BinaryWriter writer = new(File.OpenWrite(path)))
            {
                Write(writer);
                writer.Flush();
            }
        }
        public void Save(BinaryWriter writer)
        {
            Write(writer);
        }
    }
    public class MapTile
    {
        public byte IsUsed;
        public byte IsUsed2;
        public byte TileType;
        public byte Unk2;
        public byte Unk3;
        public byte Unk4;
        public byte Rotation;
        public byte TileSizeX;
        public byte TileSizeY;
        public byte Unk5;
        public byte Unk6;
        public byte Unk7;
        public MapTile()
        {
        }
        public MapTile(byte type, byte sizeX, byte sizeY)
        {
            IsUsed = 1;
            IsUsed2 = 1;
            TileType = type;
            TileSizeX = sizeX;
            TileSizeY = sizeY;
        }
        public MapTile(byte type, byte sizeX, byte sizeY, byte rotation)
        {
            IsUsed = 1;
            IsUsed2 = 1;
            TileType = type;
            TileSizeX = sizeX;
            TileSizeY = sizeY;
            Rotation = rotation;
        }
        public MapTile(BinaryReader reader)
        {
            reader.BaseStream.Position += 4;
            IsUsed = reader.ReadByte();
            IsUsed2 = reader.ReadByte();
            TileType = reader.ReadByte();
            Unk2 = reader.ReadByte();
            Unk3 = reader.ReadByte();
            Unk4 = reader.ReadByte();
            Rotation = reader.ReadByte();
            TileSizeX = reader.ReadByte();
            TileSizeY = reader.ReadByte();
            Unk5 = reader.ReadByte();
            Unk6 = reader.ReadByte();
            Unk7 = reader.ReadByte();
        }
        public void Write(BinaryWriter writer)
        {
            writer.BaseStream.Position += 4;
            writer.Write(IsUsed);
            writer.Write(IsUsed2);
            writer.Write(TileType);
            writer.Write(Unk2);
            writer.Write(Unk3);
            writer.Write(Unk4);
            writer.Write(Rotation);
            writer.Write(TileSizeX);
            writer.Write(TileSizeY);
            writer.Write(Unk5);
            writer.Write(Unk6);
            writer.Write(Unk7);
        }
    }
}
