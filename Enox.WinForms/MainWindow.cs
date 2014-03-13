using OpenTK.Graphics.OpenGL;
using Enox.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Enox.WinForms
{
    public partial class MainWindow : Form
    {
        private Scene myScene = new Scene();

        public MainWindow()
        {
            InitializeComponent();
        }

        void SetViewport()
        {
            if (sceneViewGLControl.ClientSize.Height == 0)
                sceneViewGLControl.ClientSize = new System.Drawing.Size(sceneViewGLControl.ClientSize.Width, 1);

            GL.Viewport(0, 0, sceneViewGLControl.ClientSize.Width, sceneViewGLControl.ClientSize.Height);
            GL.Ortho(0, 640, 480, 0, 0, 100);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            sceneViewGLControl.Paint += sceneViewGLControl_Paint;
            sceneViewGLControl.Resize += sceneViewGLControl_Resize;

            GL.ClearColor(Color.CornflowerBlue);

            SetViewport();
        }

        void sceneViewGLControl_Resize(object sender, EventArgs e)
        {
            SetViewport();
        }

        void sceneViewGLControl_Paint(object sender, PaintEventArgs e)
        {
            sceneViewGLControl.MakeCurrent();

            GL.Clear(ClearBufferMask.ColorBufferBit);

            //GL.LineWidth(4);
            //GL.Begin(PrimitiveType.Lines);
            //{
            //    GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            //    GL.Vertex2(0, 100);
            //    GL.Vertex2(600, 100);
            //}
            //GL.End();

            sceneViewGLControl.SwapBuffers();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK && ofd.FileName != string.Empty)
            {
                var fileContent = File.ReadAllText(ofd.FileName, Encoding.UTF8);
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
                            myScene.Cameras.Add(camera);
                            break;
                        case "light":
                            Light light = Light.FromString(innerSplit[1]);
                            myScene.Lights.Add(light);
                            break;
                        case "material":
                            Material material = Material.FromString(innerSplit[1]);
                            myScene.Materials.Add(material);
                            break;
                        case "solid":
                            Solid solid = Solid.FromString(innerSplit[1]);
                            myScene.Solids.Add(solid);
                            break;
                        case "image":
                            Enox.Framework.Image image = Enox.Framework.Image.FromString(innerSplit[1]);
                            myScene.Images.Add(image);
                            break;
                    }
                }
            }
        }

        private void renderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float height = 2 * myScene.Cameras[0].Distance *
                       (float)Math.Tan((myScene.Cameras[0].FieldOfView * (Math.PI / 180.0f)) / 2);
            float width = myScene.Images[0].Horizontal / myScene.Images[0].Vertical * height;

            float pixelSize = width / myScene.Images[0].Horizontal;

            Vector3 origin = new Vector3(0, 0, myScene.Cameras[0].Distance);
          

            MessageBox.Show(height.ToString() + "::" + width.ToString() + "::" + pixelSize);

            for(int i = 0; i < myScene.Images[0].Horizontal; i++){
                for(int y = 0; y < myScene.Images[0].Vertical; y++) {
                    // construir raio(i, j)
                    // Color c = RayTrace(r);
                    // pixel[i, j] = c;

                    // origin camera  = (0, 0, d); d = 3
                    // direçao = normalize((px, py, pz) - (origin))
                    // px = pixelSize * (i + 0.5f) - (width/2)
                    // py = pixelSize * (y + 0.5f) - (height/2)
                    // pz = 0

                    float px = pixelSize * (i + 0.5f) - (width / 2);
                    float py = pixelSize * (y + 0.5f) - (height/2);
                    Vector3 df = new Vector3(px, py, 0) - origin;
                    Vector3 direction = Vector3.Normalize(df);


                    Console.WriteLine(direction);
                 

                }
            }
        }
    }
}
