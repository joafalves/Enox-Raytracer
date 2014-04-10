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
using OpenTK;

using Vector3 = Enox.Framework.Vector3;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace Enox.WinForms
{
    public partial class MainWindow : Form
    {
        private const int MAX_DISPLAY_LISTS = 1; // we only need 1 
        private int[] displayLists = new int[MAX_DISPLAY_LISTS];
        private Scene myScene;
        private object mutex = new object();
        private BackgroundWorker bgWorker = new BackgroundWorker();
        private Stopwatch stopwatch = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();

            Application.Idle += Application_Idle;
        }

        void Application_Idle(object sender, EventArgs e)
        {
            while (sceneViewGLControl.IsIdle && myScene != null && !bgWorker.IsBusy)
            {
                SetViewport();
                Render();
            }
        }

        void Render()
        {
            sceneViewGLControl.MakeCurrent();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.PushMatrix();
            {  
                if (myScene != null)
                {
                    GL.Translate(myScene.Camera.Position.X, myScene.Camera.Position.Y, myScene.Camera.Position.Z);
                    GL.Translate(0, 0, myScene.Camera.Distance * -1);
                    GL.CallLists(displayLists[0], ListNameType.Int, displayLists); // faster
                }

                sceneViewGLControl.SwapBuffers();
            }
            GL.PopMatrix();
        }

        void SetDisplayList()
        {
            int firstList = GL.GenLists(MAX_DISPLAY_LISTS);
            displayLists[0] = firstList;

            GL.NewList(firstList, ListMode.Compile);

            RenderDisplayList();

            GL.EndList();
        }

        void RenderDisplayList()
        {
            GL.PushMatrix();
            {
                GL.Begin(PrimitiveType.Triangles);
                {
                    foreach (Solid s in myScene.Solids)
                    {
                        foreach (Triangle t in s.Triangles)
                        {
                            Enox.Framework.Color c = myScene.Materials[t.MaterialIndex].Color;
                            GL.Color3(c.R, c.G, c.B);
                            GL.Normal3(t.Normal.X, t.Normal.Y, t.Normal.Z);
                            foreach (Vector3 p in t.Points)
                            {
                                GL.Vertex3(p.X, p.Y, p.Z);
                            }
                        }
                    }
                }
                GL.End();
            }
            GL.PopMatrix();
        }

        void SetViewport()
        {
            if (sceneViewGLControl.ClientSize.Height == 0)
                sceneViewGLControl.ClientSize = new System.Drawing.Size(sceneViewGLControl.ClientSize.Width, 1);

            GL.Viewport(0, 0, sceneViewGLControl.ClientSize.Width, sceneViewGLControl.ClientSize.Height);

            float aspectRatio = (float)sceneViewGLControl.ClientSize.Width / (float)sceneViewGLControl.ClientSize.Height;
            Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(myScene.Camera.FieldOfView * (float)(Math.PI / 180.0f), aspectRatio, 1, 500); //
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perpective);

            // MathHelper.PiOver4

            //GL.Ortho(-myScene.Cameras[0].Vertical, myScene.Cameras[0].Horizontal, myScene.Cameras[0].Vertical, -myScene.Cameras[0].Horizontal, 0, 100);
            //GL.Ortho(-1, 1, 1, -1, 0f, 100.0f);

            //Matrix4 perspective = 

            //OpenTK.Vector3 eye = new OpenTK.Vector3(myScene.Cameras[0].Position.X, myScene.Cameras[0].Position.Y,
            //    myScene.Cameras[0].Position.Z);

            //Matrix4 lookat = Matrix4.LookAt(eye, OpenTK.Vector3.Zero, OpenTK.Vector3.UnitY);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadMatrix(ref lookat);

            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();

            //GL.MatrixMode(MatrixMode.Projection);
            //Matrix4 p = Matrix4.CreatePerspectiveFieldOfView(OpenTK.MathHelper.PiOver4,
            //    myScene.Cameras[0].Horizontal / myScene.Cameras[0].Vertical,
            //    0.1f, 100.0f); 
            //GL.LoadMatrix(ref p);

            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();
        }

        void SetScene()
        {
            if (myScene == null) return;

            float[] light_position = { myScene.Lights[0].Position.X, myScene.Lights[0].Position.Y, myScene.Lights[0].Position.Z, 1.0f };
            float[] light_ambient = { myScene.Image.Color.R, myScene.Image.Color.G, myScene.Image.Color.B, 1.0f };
            float[] mat_specular = { 1.0f, 1.0f, 1.0f, 1.0f };

            GL.ShadeModel(ShadingModel.Smooth);

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.CullFace);

            GL.Light(LightName.Light0, LightParameter.Ambient, light_ambient);
            GL.Light(LightName.Light0, LightParameter.Position, light_position);
            GL.Light(LightName.Light0, LightParameter.Diffuse, mat_specular);

            GL.ClearColor(myScene.Image.Color.R, myScene.Image.Color.G, myScene.Image.Color.B, 1.0f);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            sceneViewGLControl.Paint += sceneViewGLControl_Paint;
            sceneViewGLControl.Resize += sceneViewGLControl_Resize;

            GL.ClearColor(System.Drawing.Color.CornflowerBlue);

            bgWorker.DoWork += bgWorker_DoWork;
            bgWorker.RunWorkerCompleted += bgWorker_RunWorkerCompleted;
            bgWorker.ProgressChanged += bgWorker_ProgressChanged;
            bgWorker.WorkerReportsProgress = true;
        }

        void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            windowProgressBar.Value = e.ProgressPercentage;
        }

        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            windowProgressBar.Value = 0;

            stopwatch.Stop();
            Console.WriteLine("elapsed: " + stopwatch.Elapsed.TotalSeconds);

            MessageBox.Show("Rendered with success in " + stopwatch.Elapsed.Seconds + " seconds", "Done");

            this.Enabled = true;
        }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (myScene != null)
            {
                stopwatch.Reset();
                stopwatch.Start();

                try
                {
                    Bitmap bmp = new Bitmap(myScene.Image.Horizontal, myScene.Image.Vertical);
                    System.Drawing.Color[,] bmData = new System.Drawing.Color[myScene.Image.Horizontal, myScene.Image.Vertical];
                    //byte[] colorArray = new byte[myScene.Images[0].Horizontal * myScene.Images[0].Vertical];

                    float height = 2 * myScene.Camera.Distance *
                               (float)Math.Tan((myScene.Camera.FieldOfView * (Math.PI / 180.0f)) / 2);
                    float width = myScene.Image.Horizontal / myScene.Image.Vertical * height;

                    float pixelSize = width / myScene.Image.Horizontal;

                    Vector3 origin = new Vector3(myScene.Camera.Position.X, myScene.Camera.Position.Y, myScene.Camera.Distance);

                    int c = 0;
                    Parallel.ForEach(Partitioner.Create(0, myScene.Image.Horizontal), range =>
                    {
                        //Console.WriteLine("BLOCK PARALLEL");
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            for (int y = 0; y < myScene.Image.Vertical; y++)
                            //Parallel.For(0, myScene.Images[0].Vertical, y =>
                            {
                                float px = pixelSize * (i + 0.5f) - (width / 2);
                                float py = pixelSize * (y + 0.5f) - (height / 2);
                                Vector3 df = new Vector3(px, py, 0) - origin;
                                Vector3 direction = Vector3.Normalize(df);

                                Ray r = new Ray()
                                {
                                    Direction = direction,
                                    Origin = origin
                                };

                                Enox.Framework.Color color = Ray.Trace(myScene, r, 2);

                                float red = (color.R > 1 ? 1 : color.R);
                                float green = (color.G > 1 ? 1 : color.G);
                                float blue = (color.B > 1 ? 1 : color.B);

                                //lock (mutex)
                                //{
                                //    bmp.SetPixel(i, y,
                                //        System.Drawing.Color.FromArgb((int)(255), (int)(red * 255),
                                //        (int)(green * 255), (int)(blue * 255)));
                                //}

                                bmData[i, y] = System.Drawing.Color.FromArgb((int)(255), (int)(red * 255),
                                                (int)(green * 255), (int)(blue * 255));

                                c++;
                                bgWorker.ReportProgress(c * 100 / (myScene.Image.Horizontal * myScene.Image.Vertical));

                            }
                        }
                        // });
                    });

                    // store pixel data on bitmap:
                    for (int i = 0; i < myScene.Image.Horizontal; i++)
                    {
                        for (int y = 0; y < myScene.Image.Vertical; y++)
                        {
                            bmp.SetPixel(i, y, bmData[i, y]);
                        }
                    }

                    // old way (without parallelism)
                    //for (int i = 0; i < myScene.Images[0].Horizontal; i++)
                    //{
                    //    for (int y = 0; y < myScene.Images[0].Vertical; y++)
                    //    {
                    //        float px = pixelSize * (i + 0.5f) - (width / 2);
                    //        float py = pixelSize * (y + 0.5f) - (height / 2);
                    //        Vector3 df = new Vector3(px, py, 0) - origin;
                    //        Vector3 direction = Vector3.Normalize(df);

                    //        Ray r = new Ray()
                    //        {
                    //            Direction = direction,
                    //            Origin = origin
                    //        };

                    //        Enox.Framework.Color color = Ray.Trace(myScene, r, 2);

                    //        float red = (color.R > 1 ? 1 : color.R);
                    //        float green = (color.G > 1 ? 1 : color.G);
                    //        float blue = (color.B > 1 ? 1 : color.B);

                    //        bmp.SetPixel(i, y,
                    //            System.Drawing.Color.FromArgb((int)(255), (int)(red * 255),
                    //            (int)(green * 255), (int)(blue * 255)));
                    //    }
                    //}

                    //for (int i = 0; i < 3; i++)
                    //{
                    //    for (int y = 0; y < 3; y++)
                    //    {
                    //        Task t = new Task(() =>
                    //        {
                    //            int myi = i;
                    //            int myy = y;
                    //            Console.WriteLine(myi + "**" + myy);
                    //            float rangeX = divx * myi + divx;
                    //            for (int l = divx * myi; l < rangeX; l++)
                    //            {
                    //                float rangeY = divy * myy + divy;
                    //                for (int c = divy * myy; c < rangeY; c++)
                    //                {


                    //                    float px = pixelSize * (myi + 0.5f) - (width / 2);
                    //                    float py = pixelSize * (myy + 0.5f) - (height / 2);
                    //                    Vector3 df = new Vector3(px, py, 0) - origin;
                    //                    Vector3 direction = Vector3.Normalize(df);

                    //                    Ray r = new Ray()
                    //                    {
                    //                        Direction = direction,
                    //                        Origin = origin
                    //                    };

                    //                    Enox.Framework.Color color = Ray.Trace(myScene, r, 2);

                    //                    float red = (color.R > 1 ? 1 : color.R);
                    //                    float green = (color.G > 1 ? 1 : color.G);
                    //                    float blue = (color.B > 1 ? 1 : color.B);

                    //                    lock (mutex)
                    //                    {
                    //                        //try
                    //                        //{
                    //                        bmp.SetPixel(l, c,
                    //                            System.Drawing.Color.FromArgb((int)(255), (int)(red * 255),
                    //                            (int)(green * 255), (int)(blue * 255)));
                    //                        //}
                    //                        //catch (Exception ex)
                    //                        //{
                    //                        //    Console.WriteLine(ex.ToString());
                    //                        //}
                    //                    }
                    //                }
                    //            }
                    //        });
                    //        t.Start();
                    //        tasks.Add(t);
                    //    }
                    //}

                    //for (int i = 0; i < myScene.Images[0].Horizontal; i++)
                    //{
                    //    for (int y = 0; y < myScene.Images[0].Vertical; y++)
                    //    {
                    // construir raio(i, j)
                    // Color c = RayTrace(r);
                    // pixel[i, j] = c;

                    // origin camera  = (0, 0, d); d = 3
                    // direçao = normalize((px, py, pz) - (origin))
                    // px = pixelSize * (i + 0.5f) - (width/2)
                    // py = pixelSize * (y + 0.5f) - (height/2)
                    // pz = 0


                    //#if DEBUG
                    //                            if (i == 0 && y == 100)
                    //                            {
                    //                                //Console.WriteLine(direction);
                    //                                Console.WriteLine("color: " + c);
                    //                            }
                    //#endif
                    //    }
                    //}

                    //Task.WaitAll(tasks.ToArray());

                    bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                    pictureBox1.Image = bmp;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!");
                }
            }
        }

        void sceneViewGLControl_Resize(object sender, EventArgs e)
        {
            if (myScene != null)
                SetViewport();
        }

        void sceneViewGLControl_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (*.txt)|*.txt|SceneX Files (.scx)|*.scx";
            ofd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK && ofd.FileName != string.Empty)
            {
                try
                {
                    if (ofd.FileName.ToLower().EndsWith(".scx"))
                    {
                        myScene = (Scene)Serializer.DeserializeObject(ofd.FileName);
                    }
                    else
                    {
                        myScene = Scene.FromFile(ofd.FileName);
                    }

                    SetScene();
                    SetViewport();
                    SetDisplayList();

                    sceneViewGLControl.Invalidate();

                    propertyGrid1.SelectedObject = myScene;

                    //MessageBox.Show("Scene loaded with success!", "Sucess!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!");
                }
            }
        }

        private void renderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            windowProgressBar.Maximum = 100;
            windowProgressBar.Value = 0;

            this.Enabled = false;
            bgWorker.RunWorkerAsync();
        }

        private void toolsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            renderToolStripMenuItem.Enabled = (myScene != null ? true : false);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myScene == null) return;

            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "SceneX Files (.scx)|*.scx";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Serializer.SerializeObject(ofd.FileName, myScene);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
