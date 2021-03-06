﻿using System;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;

namespace HighFlowStorage
{
    class HighFlowStorageConfig
    {
        private const string fileName = "HighFlowStorageConfig.json";

        // variables, which can be changed in the config file
        // The numbers are what will be used if the variable isn't present in the config file

        public float liquidStorageCapacity = 5000f;
        public float liquidStorageConstructionTime = 240f;
        public float liquidStorageMetalCost = 800f;

        public float Gas3StorageCapacity = 300f;
        public float Gas3StorageConstructionTime = 240f;
        public float Gas3StorageMetalCost = 800f;

        public float Gas5StorageCapacity = 300f;
        public float Gas5StorageConstructionTime = 240f;
        public float Gas5StorageMetalCost = 800f;

        // building colors
        // Each number can be 0 to 255 and no decimal numbers
        // Setting alpha to 0 will disable the customm color

        public byte ColorLiquidHorizontalRed = 0;
        public byte ColorLiquidHorizontalGreen = 255;
        public byte ColorLiquidHorizontalBlue = 104;
        public byte ColorLiquidHorizontalAlpha = 255;

        public byte ColorLiquidVerticalRed = 50;
        public byte ColorLiquidVerticalGreen = 50;
        public byte ColorLiquidVerticalBlue = 0;
        public byte ColorLiquidVerticalAlpha = 255;

        public byte ColorGasHorizontalRed = 0;
        public byte ColorGasHorizontalGreen = 255;
        public byte ColorGasHorizontalBlue = 104;
        public byte ColorGasHorizontalAlpha = 255;

        public byte ColorGasVerticalRed = 200;
        public byte ColorGasVerticalGreen = 0;
        public byte ColorGasVerticalBlue = 104;
        public byte ColorGasVerticalAlpha = 255;

        // end of config file variables

        private static HighFlowStorageConfig internalConfig = null;

        public static HighFlowStorageConfig Config
        {
            get
            {
                if (internalConfig == null)
                {
                    // load the config file, but only the first time the method is called.
                    string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    internalConfig = LoadConfig<HighFlowStorageConfig>(Path.Combine(assemblyFolder, fileName));
                }
                return internalConfig;
            }
        }

        protected static T LoadConfig<T>(string path) where T : class
        {
            try
            {
                JsonSerializer serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings { Formatting = Formatting.Indented, ObjectCreationHandling = ObjectCreationHandling.Replace });
                
                // assign default values in case something isn't mentioned in the file
                T result = (T)Activator.CreateInstance(typeof(T));
                using (StreamReader streamReader = new StreamReader(path))
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
                    {
                        result = serializer.Deserialize<T>(jsonReader);
                        jsonReader.Close();
                    }
                    streamReader.Close();
                }
                return result;
            }
            catch
            {
                // something went wrong while loading the file, possibly missing or malformated file
                // return the default values
                return (T)Activator.CreateInstance(typeof(T));
            }
            
        }
    }
}
