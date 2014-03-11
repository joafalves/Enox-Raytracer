using System;
using System.Collections.Generic;
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



        #endregion
    }
}
