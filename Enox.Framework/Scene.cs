using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    public class Scene
    {
        #region fields

        private List<Material> materials = new List<Material>();
        private List<Solid> solids = new List<Solid>();
        private List<Light> lights = new List<Light>();
        private List<Camera> cameras = new List<Camera>();
        private List<Image> images = new List<Image>();

        #endregion

        #region properties

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

        public List<Camera> Cameras
        {
            get { return cameras; }
            set { cameras = value; }
        }

        public List<Image> Images
        {
            get { return images; }
            set { images = value; }
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
                        scene.cameras.Add(camera);
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
                        scene.images.Add(image);
                        break;
                }
            }

            return scene;
        }

        #endregion
    }
}
