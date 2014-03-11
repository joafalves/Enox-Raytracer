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

            GL.LineWidth(4);
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
                GL.Vertex2(0, 100);
                GL.Vertex2(600, 100);
            }
            GL.End();

            sceneViewGLControl.SwapBuffers();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            
            if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK && ofd.FileName != string.Empty) {
                var fileContent = File.ReadAllText(ofd.FileName, Encoding.UTF8);
                fileContent = fileContent.ToLower().Replace("\r", string.Empty).Replace("\t", string.Empty);
                var splitted = fileContent.Split('}');
                foreach (var split in splitted)
                {
                    var innerSplit = split.Split('{');
                    var name = innerSplit[0].Trim();
                  
                    switch(name)
                    {
                        case "camera":
                            Camera camera = Camera.FromString(innerSplit[1]);
                            myScene.Cameras.Add(camera);
                            break;
                    }
                }
            }            
        }


    }
}
