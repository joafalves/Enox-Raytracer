using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enox.Framework
{
    public class Scene
    {
        #region fields

        private List<SceneObject> sceneObjects = new List<SceneObject>();
        
        #endregion

        #region constructors

        /// <summary>
        /// 
        /// </summary>
        public List<SceneObject> SceneObjects
        {
            get { return sceneObjects; }
            set { sceneObjects = value; }
        }

        #endregion

        #region methods



        #endregion
    }
}
