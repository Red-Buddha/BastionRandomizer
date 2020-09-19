using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BastionRandomiztion
{
    class Randomizer
    {
        private List<string> scripts = new List<string>();
        private byte[] mapDataBlock1;
        private byte[] mapDataBlock2;
        private int dataBlock2Length = 0;
        private int[] randomLevelOrder;

        public bool randomizeLevels;
        public bool randomizeLoot;
        public bool randomizeEnemies;
        public bool noCutscenes;
        public bool noHub;
        public bool weapons;
        public bool abilities;
        public bool loot;
        public bool upgrades;
        public bool cores;
        public bool guaranteeWeapon;
        public bool randomizeHopscotch;

        public Random rand;

        GameData data = new GameData();


        private int[] Shuffle(int[] array)
        {
            int[] randomOrder = new int[array.Length];
            array.CopyTo(randomOrder, 0);

            int n = randomOrder.Count();
            while (n > 1)
            {
                --n;
                int i = rand.Next(n + 1);
                int value = randomOrder[i];
                randomOrder[i] = randomOrder[n];
                randomOrder[n] = value;
            }

            return randomOrder;
        }

        ///////////////////////////
        /// LEVEL RANDOMIZATION ///
        ///////////////////////////
        
        public void RandomizeLevelOrder(string path)
        {
            // create array all levels and for levels that will be randomized
            int[] newOrder = Enumerable.Range(0, 32).ToArray();
            int[] tempOrder = new int[17] { 2, 4, 5, 8, 9, 10, 14, 15, 16, 17, 18, 21, 23, 25, 27, 28, 29 };

            randomLevelOrder = Shuffle(tempOrder);

            for (int j = 0; j < tempOrder.Count(); ++j)
            {
                newOrder[tempOrder[j]] = randomLevelOrder[j];
            }

            SetMapValues(newOrder, path);
        }

        private void SetMapValues(int[] order, string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path + "/Content/Game/MapLayouts.original.xml");

            var levels = doc.GetElementsByTagName("MapNode");

            for (int i = 0; i < order.Length; ++i)
            {
                LevelInfo temp = data.levelInfos[order[i]];

                levels[i].Attributes["Name"].InnerXml = temp.name;
                levels[i].Attributes["Graphic"].InnerXml = temp.graphic;
                levels[i].Attributes["AudioCue"].InnerXml = temp.audio;

                if (i >= 3)
                    levels[i].Attributes["GoalPiecesRequired"].InnerXml = temp.cores.ToString();

                if (levels[i].Attributes["CompletedTextId"] != null)
                    levels[i].Attributes["CompletedTextId"].InnerXml = temp.completedtext;
            }

            doc.Save(path + "/Content/Game/MapLayouts.xml");
        }

        ////////////////////////////
        ///  ITEM RANDOMIZATION  ///
        ////////////////////////////
        
        public void RandomizeLoot()
        {
            if (!randomizeHopscotch)
                data.loot.Remove(data.loot.Find(x => x.name == "Jump_Kit"));

            if(weapons)
                data.randomizedLoot.AddRange(data.loot.FindAll(x => x.type == LootType.Weapon));
            if (abilities)
                data.randomizedLoot.AddRange(data.loot.FindAll(x => x.type == LootType.Ability));
            if (upgrades)
                data.randomizedLoot.AddRange(data.loot.FindAll(x => x.type == LootType.Upgrade));
            if (loot)
                data.randomizedLoot.AddRange(data.loot.FindAll(x => x.type == LootType.Generic));
            if (cores)
                data.randomizedLoot.AddRange(data.loot.FindAll(x => x.type == LootType.Core));

            int[] newOrder = Enumerable.Range(0, data.randomizedLoot.Count).ToArray();
                        
            newOrder = Shuffle(newOrder);

            SetLootValues(newOrder);
        }

        private void SetLootValues(int[] order)
        {
            //FileStream file = new FileStream("test.txt", FileMode.Create);
            //StreamWriter stream = new StreamWriter(file);

            List<Loot> tempLoot = new List<Loot>(data.randomizedLoot.Count);
            foreach (Loot loot in data.randomizedLoot)
            {
                tempLoot.Add(new Loot(loot));
            }

            for (int i = 0; i < order.Length; ++i)
            {
                Loot temp = tempLoot[order[i]];

                // for now this sets hopscotch to be in its original location if it gets randomized to the ram_kit after it is required
                if(data.randomizedLoot[i].name == "Ram_Kit" && temp.name == "Jump_Kit")
                {
                    data.randomizedLoot[i].name = data.randomizedLoot[i - 1].name;
                    data.randomizedLoot[i].type = data.randomizedLoot[i - 1].type;

                    data.randomizedLoot[i - 1].name = temp.name;
                    data.randomizedLoot[i - 1].type = temp.type;

                    continue;
                }

                data.randomizedLoot[i].name = temp.name;
                data.randomizedLoot[i].type = temp.type;
            }

            if(guaranteeWeapon && data.randomizedLoot[0].type != LootType.Weapon)
            {
                for(int i = 0; i < data.randomizedLoot.Count; ++i)
                {
                    if(data.randomizedLoot[i].type == LootType.Weapon)
                    {
                        Loot temp = new Loot(data.randomizedLoot[i]);

                        data.randomizedLoot[i].name = data.randomizedLoot[0].name;
                        data.randomizedLoot[i].type = data.randomizedLoot[0].type;

                        data.randomizedLoot[0].name = temp.name;
                        data.randomizedLoot[0].type = temp.type;

                        break;
                    }
                }
            }

            //for(int j = 0; j < data.randomizedLoot.Count; ++j)
            //{
            //    stream.WriteLine(data.randomizedLoot[j].name + " " + data.randomizedLoot[j].levelIndex);
            //}

            //stream.Close();
            //file.Close();
        }

        /////////////////////////////
        ///  ENEMY RANDOMIZATION  ///
        /////////////////////////////

        public void SetEnemyPackages(string path)
        {
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path + "/Content/Game/Units/Mudgatororiginal.xml");

                XmlNodeList NPCs = doc.GetElementsByTagName("Npc");
                XmlAttribute package = doc.CreateAttribute("CustomPackage");
                package.InnerXml = "Hunt01";
                NPCs[0].Attributes.Append(package);

                doc.Save(path + "/Content/Game/Units/Mudgator.xml");
            }

            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path + "/Content/Game/Units/Turretoriginal.xml");

                XmlNodeList NPCs = doc.GetElementsByTagName("Npc");
                XmlAttribute package = doc.CreateAttribute("CustomPackage");
                package.InnerXml = "Falling01";
                NPCs[0].Attributes.Append(package);
                XmlAttribute package2 = doc.CreateAttribute("CustomPackage");
                package2.InnerXml = "Rescue01";
                NPCs[2].Attributes.Append(package2);
                XmlAttribute package3 = doc.CreateAttribute("CustomPackage");
                package3.InnerXml = "ProtoIntro01b";
                NPCs[3].Attributes.Append(package3);
                XmlAttribute package4 = doc.CreateAttribute("CustomPackage");
                package4.InnerXml = "ProtoIntro01b";
                NPCs[6].Attributes.Append(package4);

                doc.Save(path + "/Content/Game/Units/Turret.xml");
            }

            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path + "/Content/Game/Units/Turtleoriginal.xml");

                XmlNodeList NPCs = doc.GetElementsByTagName("Npc");
                XmlAttribute package = doc.CreateAttribute("CustomPackage");
                package.InnerXml = "Gauntlet01";
                NPCs[0].Attributes.Append(package);

                doc.Save(path + "/Content/Game/Units/Turtle.xml");
            }

            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path + "/Content/Game/Units/Stinkweedoriginal.xml");

                XmlNodeList NPCs = doc.GetElementsByTagName("Npc");
                XmlAttribute package = doc.CreateAttribute("CustomPackage");
                package.InnerXml = "Fortress01";
                NPCs[0].Attributes.Append(package);

                doc.Save(path + "/Content/Game/Units/Stinkweed.xml");
            }

            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path + "/Content/Game/Units/Spineweedoriginal.xml");

                XmlNodeList NPCs = doc.GetElementsByTagName("Npc");
                XmlAttribute package = doc.CreateAttribute("CustomPackage");
                package.InnerXml = "Gauntlet01";
                NPCs[0].Attributes.Append(package);

                doc.Save(path + "/Content/Game/Units/Spineweed.xml");
            }

            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path + "/Content/Game/Units/Lassooriginal.xml");

                XmlNodeList NPCs = doc.GetElementsByTagName("Npc");
                XmlAttribute package = doc.CreateAttribute("CustomPackage");
                package.InnerXml = "Gauntlet01";
                NPCs[0].Attributes.Append(package);

                doc.Save(path + "/Content/Game/Units/Lasso.xml");
            }

            RandomizeEnemies();
        }

        public void RandomizeEnemies()
        {
            int[] newOrder = Enumerable.Range(0, data.enemies.Count).ToArray();

            newOrder = Shuffle(newOrder);

            SetEnemyValues(newOrder);
        }

        public void SetEnemyValues(int[] order)
        {
            List<Enemy> tempEnemies = new List<Enemy>(data.enemies.Count);
            foreach (Enemy enemy in data.enemies)
            {
                tempEnemies.Add(new Enemy(enemy));
            }

            for (int i = 0; i < order.Length; ++i)
            {
                data.enemies[i].name = tempEnemies[order[i]].name;
            }

            FileStream file = new FileStream("test.txt", FileMode.Create);
            StreamWriter stream = new StreamWriter(file);

            for (int j = 0; j < data.enemies.Count; ++j)
            {
                stream.WriteLine(data.enemies[j].name + " " + data.enemies[j].levelIndex);
            }

            stream.Close();
            file.Close();
        }

        ///////////////////////
        /// FILE MANAGEMENT ///
        ///////////////////////

        public void ReadScript(string path, MapData map)
        {
            using (FileStream fileStream = new FileStream(path + "\\Content\\Maps\\" + map.levelName + ".original.map", FileMode.Open))
            {
                BinaryReader reader = new BinaryReader(fileStream);

                mapDataBlock1 = new byte[map.scriptStart];
                reader.Read(mapDataBlock1, 0, map.scriptStart);
                int capacity = reader.ReadInt32();

                scripts.Clear();
                for (int index = 0; index < capacity; ++index)
                    scripts.Add(reader.ReadString());

                dataBlock2Length = (int)fileStream.Length - map.scriptEnd;
                mapDataBlock2 = new byte[(int)fileStream.Length - map.scriptEnd];
                reader.Read(mapDataBlock2, 0, (int)fileStream.Length - map.scriptEnd);

                //if(map.levelName == "Shrine01")
                //{
                //    string name = "LightMeleeScared";

                //    List<int> locations = new List<int>();
                //    for(int i = 0; i < mapDataBlock2.Length; ++i)
                //    {
                //        if (mapDataBlock2[i] == name[0])
                //            if (mapDataBlock2[i + 1] == name[1])
                //                if (mapDataBlock2[i + 2] == name[2])
                //                    if (mapDataBlock2[i + 3] == name[3])
                //                        if(mapDataBlock2[i - 1] == name.Length)
                //                            locations.Add(i - 1);
                //    }

                //    int trew = 3546345;
                //}

                reader.Close();
            }
        }

        public void WriteScript(string path, MapData map)
        {
            using (FileStream fileStream = new FileStream(path + "\\Content\\Maps\\" + map.levelName + ".map", FileMode.Create))
            {
                BinaryWriter writer = new BinaryWriter(fileStream);

                writer.Write(mapDataBlock1, 0, mapDataBlock1.Length);

                writer.Write(scripts.Count);
                foreach (string str in scripts)
                    writer.Write(str);

                writer.Write(mapDataBlock2, 0, mapDataBlock2.Length);

                writer.Close();
            }
        }

        public void BackupFiles(string folderPath)
        {
            if (File.Exists(folderPath + "/Content/Game/MapLayouts.original.xml") == false)
            {
                File.Copy(folderPath + "/Content/Game/MapLayouts.xml", folderPath + "/Content/Game/MapLayouts.original.xml");
            }

            if (File.Exists(folderPath + "/Content/Game/Units/Mudgatororiginal.xml") == false)
            {
                File.Copy(folderPath + "/Content/Game/Units/Mudgator.xml", folderPath + "/Content/Game/Units/Mudgatororiginal.xml");
            }
            if (File.Exists(folderPath + "/Content/Game/Units/Turretoriginal.xml") == false)
            {
                File.Copy(folderPath + "/Content/Game/Units/Turret.xml", folderPath + "/Content/Game/Units/Turretoriginal.xml");
            }
            if (File.Exists(folderPath + "/Content/Game/Units/Turtleoriginal.xml") == false)
            {
                File.Copy(folderPath + "/Content/Game/Units/Turtle.xml", folderPath + "/Content/Game/Units/Turtleoriginal.xml");
            }
            if (File.Exists(folderPath + "/Content/Game/Units/Stinkweedoriginal.xml") == false)
            {
                File.Copy(folderPath + "/Content/Game/Units/Stinkweed.xml", folderPath + "/Content/Game/Units/Stinkweedoriginal.xml");
            }
            if (File.Exists(folderPath + "/Content/Game/Units/Spineweedoriginal.xml") == false)
            {
                File.Copy(folderPath + "/Content/Game/Units/Spineweed.xml", folderPath + "/Content/Game/Units/Spineweedoriginal.xml");
            }
            if (File.Exists(folderPath + "/Content/Game/Units/Lassooriginal.xml") == false)
            {
                File.Copy(folderPath + "/Content/Game/Units/Lasso.xml", folderPath + "/Content/Game/Units/Lassooriginal.xml");
            }

            DirectoryInfo dir = new DirectoryInfo(folderPath + "/Content/Maps/");
            FileInfo[] files = Array.FindAll(dir.GetFiles(), f => f.Extension.Equals(".map"));

            foreach (FileInfo file in files)
            {
                string filename = file.Name.Substring(0, file.Name.IndexOf("."));

                if (File.Exists(file.DirectoryName + "/" + filename + ".original.map"))
                    continue;

                file.CopyTo(file.DirectoryName + "/" + filename + ".original.map");
            }
        }

        public void RestoreFiles(string folderPath)
        {
            if (File.Exists(folderPath + "/Content/Game/MapLayouts.original.xml"))
            {
                File.Delete(folderPath + "/Content/Game/MapLayouts.xml");
                File.Move(folderPath + "/Content/Game/MapLayouts.original.xml", folderPath + "/Content/Game/MapLayouts.xml");
            }

            if (File.Exists(folderPath + "/Content/Game/Units/Mudgatororiginal.xml"))
            {
                File.Delete(folderPath + "/Content/Game/Units/Mudgator.xml");
                File.Move(folderPath + "/Content/Game/Units/Mudgatororiginal.xml", folderPath + "/Content/Game/Units/Mudgator.xml");
            }
            if (File.Exists(folderPath + "/Content/Game/Units/Turretoriginal.xml"))
            {
                File.Delete(folderPath + "/Content/Game/Units/Turret.xml");
                File.Move(folderPath + "/Content/Game/Units/Turretoriginal.xml", folderPath + "/Content/Game/Units/Turret.xml");
            }
            if (File.Exists(folderPath + "/Content/Game/Units/Turtleoriginal.xml"))
            {
                File.Delete(folderPath + "/Content/Game/Units/Turtle.xml");
                File.Move(folderPath + "/Content/Game/Units/Turtleoriginal.xml", folderPath + "/Content/Game/Units/Turtle.xml");
            }
            if (File.Exists(folderPath + "/Content/Game/Units/Stinkweedoriginal.xml"))
            {
                File.Delete(folderPath + "/Content/Game/Units/Stinkweed.xml");
                File.Move(folderPath + "/Content/Game/Units/Stinkweedoriginal.xml", folderPath + "/Content/Game/Units/Stinkweed.xml");
            }
            if (File.Exists(folderPath + "/Content/Game/Units/Spineweedoriginal.xml"))
            {
                File.Delete(folderPath + "/Content/Game/Units/Spineweed.xml");
                File.Move(folderPath + "/Content/Game/Units/Spineweedoriginal.xml", folderPath + "/Content/Game/Units/Spineweed.xml");
            }
            if (File.Exists(folderPath + "/Content/Game/Units/Lassooriginal.xml"))
            {
                File.Delete(folderPath + "/Content/Game/Units/Lasso.xml");
                File.Move(folderPath + "/Content/Game/Units/Lassooriginal.xml", folderPath + "/Content/Game/Units/Lasso.xml");
            }

            DirectoryInfo dir = new DirectoryInfo(folderPath + "/Content/Maps/");
            FileInfo[] files = Array.FindAll(dir.GetFiles(), f => f.FullName.Contains("original"));

            foreach (FileInfo file in files)
            {
                string filename = file.Name.Substring(0, file.Name.IndexOf("."));

                File.Delete(file.DirectoryName + "/" + filename + ".map");
                File.Move(file.FullName, file.DirectoryName + "/" + filename + ".map");
            }
        }

        ////////////////////
        /// SCRIPT EDITS ///
        ////////////////////

        public void EditLevelScripts(string path)
        {
            // Rippling Walls
            if (randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[0]);
                SetLevelData(0);
                WriteScript(path, data.Maps[0]);
            }

            // Sole Regret
            if (randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[1]);
                SetLevelData(1);
                if(randomizeEnemies)
                {
                    SoleModifyFightTriggers();
                }
                WriteScript(path, data.Maps[1]);
            }

            // Wharf District
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[2]);
                if (noHub)
                {
                    WharfSkipHub();
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(2);
                }
                WriteScript(path, data.Maps[2]);
            }

            // Bastion
            if (randomizeLevels || noCutscenes || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[3]);
                if (randomizeLevels)
                {
                    SetLevelOrder();
                    UnlockBastion();
                }
                if (noCutscenes)
                {
                    RemoveBastionCutscenes();
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(3);
                }
                WriteScript(path, data.Maps[3]);
            }

            // Workmen Ward
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[4]);
                if (noHub)
                {
                    WorkmenSkipHub(2);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(4);
                }
                WriteScript(path, data.Maps[4]);
            }

            // Sundown Path
            if (noCutscenes || noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[5]);
                if (noCutscenes)
                {
                    RemoveSundownCutscene();
                }
                if (noHub)
                {
                    SundownSkipHub(5);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(5);
                }
                WriteScript(path, data.Maps[5]);
            }

            // Melting Pot
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[6]);
                if (noHub)
                {
                    MeltingSkipHub(4);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(6);
                }
                WriteScript(path, data.Maps[6]);
            }

            // Hanging Gardens
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[7]);
                if (noHub)
                {
                    GardensSkipHub(8);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(7);
                }
                WriteScript(path, data.Maps[7]);
            }

            // Cinderbrick Fort
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[8]);
                if (noHub)
                {
                    CinderbrickSkipHub(9);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(8);
                }
                WriteScript(path, data.Maps[8]);
            }

            // Pyth Orchard
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[9]);
                if (noHub)
                {
                    PythSkipHub(10);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(9);
                }
                WriteScript(path, data.Maps[9]);
            }

            // Langston
            if (randomizeLevels || noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[10]);
                if (randomizeLevels)
                {
                    UncoupleLangston();
                }
                if (noHub)
                {
                    LangstonSkipHub(14);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(10);
                }
                WriteScript(path, data.Maps[10]);
            }

            // Prosper Bluff
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[11]);
                if (noHub)
                {
                    ProsperSkipHub(15);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(11);
                }
                WriteScript(path, data.Maps[11]);
            }

            // Wild Outskirts
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[12]);
                if (noHub)
                {
                    OutskirtsSkipHub(16);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(12);
                }
                WriteScript(path, data.Maps[12]);
            }

            // Jawson Bog
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[13]);
                if (noHub)
                {
                    JawsonSkipHub(17);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(13);
                }
                WriteScript(path, data.Maps[13]);
            }

            // Jawson Dream
            if (randomizeLevels || noCutscenes || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[14]);
                if(randomizeLevels)
                {
                    //RemoveDreamMonument();
                }
                if (noCutscenes)
                {
                    RemoveDreamCutscene();
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(14);
                }
                
                WriteScript(path, data.Maps[14]);
            }

            // Roathus Lagoon
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[15]);
                if (noHub)
                {
                    RoathusSkipHub(18);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(15);
                }
                WriteScript(path, data.Maps[15]);
            }

            // Point Lemaign
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[16]);
                if (noHub)
                {
                    LemaignSkipHub(21);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(16);
                }
                WriteScript(path, data.Maps[16]);
            }

            // Colford Cauldron
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[17]);
                if (noHub)
                {
                    ColfordSkipHub(23);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(17);
                }
                WriteScript(path, data.Maps[17]);
            }

            // Mount Zand
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[18]);
                if (noHub)
                {
                    ZandSkipHub(25);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(18);
                }
                WriteScript(path, data.Maps[18]);
            }

            // Burstone Quarry
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[19]);
                if (noHub)
                {
                    // Invasion skip makes skipping hub not possible so its removed from the mode for now
                    BurstoneSkipHub(27);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(19);
                }
                WriteScript(path, data.Maps[19]);
            }

            // Ura Invasion
            if (randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[20]);
                SetLevelData(20);
                WriteScript(path, data.Maps[20]);
            }

            // Urzendra Gate
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[21]);
                if (noHub)
                {
                    UrzendraSkipHub(28);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(21);
                }
                WriteScript(path, data.Maps[21]);
            }

            // Zulten's Hollow
            if (noHub || randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[22]);
                if (noHub)
                {
                    ZultenSkipHub(29);
                }
                if (randomizeLoot || randomizeEnemies)
                {
                    SetLevelData(22);
                }
                WriteScript(path, data.Maps[22]);
            }

            // Tazal 1
            if (randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[23]);
                SetLevelData(23);
                WriteScript(path, data.Maps[23]);
            }

            // Tazal 2
            if (randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[24]);
                SetLevelData(24);
                WriteScript(path, data.Maps[24]);
            }

            // Tazal 3
            if (randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[25]);
                SetLevelData(25);
                WriteScript(path, data.Maps[25]);
            }

            // Tazal 4
            if (randomizeLoot || randomizeEnemies)
            {
                ReadScript(path, data.Maps[26]);
                SetLevelData(26);
                WriteScript(path, data.Maps[26]);
            }
        }

        #region Level Functions
        
        private void SetLevelData(int index)
        {
            List<Loot> levelLoot = data.randomizedLoot.FindAll(x => x.levelIndex == index);
            List<Enemy> levelEnemies = data.enemies.FindAll(x => x.levelIndex == index);

            // add current level enemies and loot into the same list
            List<Tuple<string, int, int>> objects = new List<Tuple<string, int, int>>();
            if(randomizeLoot)
                objects.AddRange(levelLoot.Select(x => Tuple.Create(x.name, x.start, x.length)));
            if(randomizeEnemies)
                objects.AddRange(levelEnemies.Select(x => Tuple.Create(x.name, x.start, x.length)));

            // sort the list in reverse order
            objects.Sort((x, y) => y.Item2.CompareTo(x.Item2));
            
            List<byte> tempdata = mapDataBlock2.ToList();

            for (int i = 0; i < objects.Count; ++i)
            {
                tempdata.RemoveRange(objects[i].Item2, objects[i].Item3 + 1);
                
                List<byte> lootName = new List<byte>();
                lootName.Add((byte)objects[i].Item1.Length);
                lootName.AddRange(Encoding.UTF8.GetBytes(objects[i].Item1));

                tempdata.InsertRange(objects[i].Item2, lootName);
            }

            mapDataBlock2 = tempdata.ToArray();
        }

        #region Sole Regret
        private void SoleModifyFightTriggers()
        {
            // find a way to progress fight beyond checking for box spawns at line ~130 and ~270 and ~300
            // trigger at fight in wharf district needs looked at
            // crash in wild outskirts
            // crash in gauntlet
        }
        #endregion

        #region Wharf District
        private void WharfSkipHub()
        {
            scripts[18] = "OnFlagTrue HeFell LoadMap " + data.levelInfos[randomLevelOrder[0]].name + " ; DelaySeconds = 1.25\r\n";
            scripts[965] = "OnUsed 964 LoadMap " + data.levelInfos[randomLevelOrder[0]].name + "\r\n";
        }
        #endregion

        #region Bastion
        private void UnlockBastion()
        {
            scripts[2862] = "OnCounter GENERAL GOAL_PIECES_ON_PLAYER >= 1 UseableOn VarOvermap ; \r\n";
            scripts[2863] = "OnCounter GENERAL GOAL_PIECES_ON_PLAYER >= 1 PlayAnimation PlotWorld_Active VarOvermap\r\n";

            scripts[2865] = "OnFlagTrue FlagGlobalPlayerMustBuild UseableOn VarOvermap ; \r\n";
            scripts[2866] = "OnFlagTrue FlagGlobalPlayerMustBuild PlayAnimation PlotWorld_Active VarOvermap ;\r\n";
        }

        private void SetLevelOrder()
        {
            // clearing out original unlocks
            for (int j = 0; j < 53; ++j)
                scripts[3543 + j] = "\r\n";

            // adding randomized level order
            for (int i = 0; i < randomLevelOrder.Length - 1; ++i)
            {
                scripts[3544 + i] = "OnLoad SetFlagTrue MAPS_UNLOCKED " + data.levelInfos[randomLevelOrder[i + 1]].name + " ; RequiredFlag = FlagGlobalComplete" + data.levelInfos[randomLevelOrder[i]].name + " ; SaveStatus = true\r\n";
            }

            scripts[3544 + randomLevelOrder.Length - 1] = "OnLoad SetFlagTrue MAPS_UNLOCKED FinalArena01 ; RequiredFlag = FlagGlobalComplete" + data.levelInfos[randomLevelOrder[randomLevelOrder.Length - 1]].name + " ; SaveStatus = true\r\n";
        }

        private void RemoveBastionCutscenes()
        {
            scripts[50] = "\r\n";
            scripts[51] = "\r\n";
            scripts[52] = "\r\n";
            scripts[53] = "\r\n";
            scripts[54] = "\r\n";
        }
        #endregion

        #region Workmen Ward
        private void WorkmenSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[591] = "OnUsed 9066 LoadMap " + levelname + "\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Sundown Path
        private void RemoveSundownCutscene()
        {
            scripts[605] = "OnFlagTrue SetupEndingPan LoadMap ProtoTown03 ; DelaySeconds = 1.5\r\n";
        }

        private void SundownSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    if (noCutscenes)
                        scripts[605] = "OnFlagTrue SetupEndingPan LoadMap " + levelname + " ; DelaySeconds = 1.5\r\n";
                    else
                        scripts[619] = "OnFlagTrue CommenceEndingPan LoadMap " + levelname + " ; DelaySeconds = 13.5\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Melting Pot
        private void MeltingSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[587] = "\tLoadMap " + levelname + " ; DelaySeconds = 4.5\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Hanging Gardens
        private void GardensSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[513] = "OnFlagTrue SmashCut LoadMap " + levelname + " ; DelaySeconds = 0.5\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Cinderbrick Fort
        private void CinderbrickSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[821] = "\tLoadMap " + levelname + " ; DelaySeconds = 4.0\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Pyth Orchard
        private void PythSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[421] = "OnUsed VarSkyway LoadMap " + levelname + "\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Langston
        private void UncoupleLangston()
        {
            scripts[1213] = "\tLoadMap ProtoTown03 ; DelaySeconds = 2.0 ;\r\n";
        }

        private void LangstonSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[1213] = "\tLoadMap " + levelname + " ; DelaySeconds = 2.0 ;\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Prosper Bluff
        private void ProsperSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[510] = "\tLoadMap " + levelname + " ; DelaySeconds = 4.35 ;\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Wild Outskirts
        private void OutskirtsSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[854] = "\tLoadMap " + levelname + " ; DelaySeconds = 10.5\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Jawson Bog
        private void JawsonSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[487] = "\tLoadMap " + levelname + " ; DelaySeconds = 7.0\r\n";

                    return;
                }
            }
        }
        #endregion
        
        #region Jawson Dream

        // GoalStructure 151874 151887
        private void RemoveDreamMonument()
        {
            List<byte> tempdata = mapDataBlock2.ToList();
            int range = 14;
            tempdata.RemoveRange(151874, range);

            List<byte> replacement = new List<byte>();
            replacement.Add((byte)"".Length);
            replacement.AddRange(Encoding.UTF8.GetBytes(""));

            tempdata.InsertRange(151874, replacement);

            mapDataBlock2 = tempdata.ToArray();
        }

        private void RemoveDreamCutscene()
        {
            scripts[765] = "\tLoadMap Scenes02 ; DelaySeconds = 3.1\r\n";
            scripts[766] = "\tSetGlobalFlagTrue FlagGlobalCompleteScenes01 ;";
        }
        #endregion

        #region Roathus Lagoon
        private void RoathusSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[959] = "\tLoadMap " + levelname + " ; DelaySeconds = 3.0\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Point Lemaign
        private void LemaignSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[711] = "\tLoadMap " + levelname + " ; DelaySeconds = 4.0\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Colford Cauldron
        private void ColfordSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[552] = "\tLoadMap " + levelname + " ; DelaySeconds = 4.5\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Mount Zand
        private void ZandSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[489] = "\tLoadMap " + levelname + " ; DelaySeconds = 5.0\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Burstone Quarry
        private void BurstoneSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[770] = "\tLoadMap " + levelname + " ; DelaySeconds = 4.0\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Ura Invasion
        #endregion

        #region Urzendra Gate
        private void UrzendraSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[974] = "\tLoadMap " + levelname + " ; DelaySeconds = 7.0\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Zulten's Hollow
        private void ZultenSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[941] = "\tLoadMap " + levelname + " ; DelaySeconds = 9.0\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Tazal
        #endregion Tazal

        #endregion
    }
}
