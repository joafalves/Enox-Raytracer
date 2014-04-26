using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    [Serializable]
    public class Scene
    {
        #region fields

        private List<Material> materials = new List<Material>();
        private List<Solid> solids = new List<Solid>();
        private List<Light> lights = new List<Light>();
        private Camera camera = new Camera();
        private Image image = new Image();
        private int recursionDepth = 2;

        #endregion

        #region properties

        public int RecursionDepth
        {
            get { return recursionDepth; }
            set { recursionDepth = value; }
        }

        public List<Material> Materials
        {
            get { return materials; }
            set { materials = value; }
        }

        public List<Solid> Solids
        {
            get { return solids; }
            set { solids = value; }
        }

        public List<Light> Lights
        {
            get { return lights; }
            set { lights = value; }
        }

        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        public Image Image
        {
            get { return image; }
            set { image = value; }
        }

        #endregion

        #region methods

        public static Scene FromFile(string filename)
        {
            var fileContent = File.ReadAllText(filename, Encoding.UTF8);
            return FromString(fileContent);
        }

        public static Scene FromString(string fileContent)
        {
            Scene scene = new Scene();

            fileContent = fileContent.ToLower().Replace("\r", string.Empty).Replace("\t", string.Empty).Replace(".", ",");
            var splitted = fileContent.Split('}');
            foreach (var split in splitted)
            {
                var innerSplit = split.Split('{');
                var name = innerSplit[0].Trim();

                switch (name)
                {
                    case "camera":
                        Camera camera = Camera.FromString(innerSplit[1]);
                        scene.camera = camera;
                        break;
                    case "light": 
                        Light light = Light.FromString(innerSplit[1]);
                        scene.lights.Add(light);
                        break;
                    case "material":
                        Material material = Material.FromString(innerSplit[1]);
                        scene.materials.Add(material);
                        break;
                    case "solid":
                        Solid solid = Solid.FromString(innerSplit[1]);
                        scene.solids.Add(solid);
                        break;
                    case "image":
                        Enox.Framework.Image image = Enox.Framework.Image.FromString(innerSplit[1]);
                        scene.image = image;
                        break;
                }
            }

            return scene;
        }

        #endregion
    }
}
