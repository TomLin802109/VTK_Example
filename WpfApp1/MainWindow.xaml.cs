using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kitware.VTK;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            vtk_render.Load += Vtk_render_Load;
        }

        private void Vtk_render_Load(object sender, EventArgs e)
        {
            //// Create a simple sphere. A pipeline is created.
            //vtkSphereSource sphere = vtkSphereSource.New();
            //sphere.SetThetaResolution(8);
            //sphere.SetPhiResolution(16);

            //vtkShrinkPolyData shrink = vtkShrinkPolyData.New();
            //shrink.SetInputConnection(sphere.GetOutputPort());
            //shrink.SetShrinkFactor(0.9);

            //vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            //mapper.SetInputConnection(shrink.GetOutputPort());

            //// The actor links the data pipeline to the rendering subsystem
            //vtkActor actor = vtkActor.New();
            //actor.SetMapper(mapper);
            //actor.GetProperty().SetColor(1,0,0);

            //// Create components of the rendering subsystem
            ////
            //vtkRenderer ren1 = vtk_render.RenderWindow.GetRenderers().GetFirstRenderer();
            //vtkRenderWindow renWin = vtk_render.RenderWindow;

            //// Add the actors to the renderer, set the window size
            ////
            //ren1.AddViewProp(actor);
            ////renWin.SetSize(250, 250);
            //renWin.Render();
            vtkCamera camera = vtk_render.RenderWindow.GetRenderers().GetFirstRenderer().GetActiveCamera();
            camera.Zoom(1.5);
            SetBackground();
            Axes();
        }

        private void SetBackground()
        {
            var render = vtk_render.RenderWindow.GetRenderers().GetFirstRenderer();
            render.SetBackground2(0.6, 0.6, 0.6);
            render.SetBackground(0.3, 0.3, 0.3);
            render.SetGradientBackground(true);
            //render.SetBackgroundTexture(new vtkTexture());
            vtk_render.RenderWindow.Render();
        }
        private void Axes()
        {
            vtkSphereSource sphereSource = vtkSphereSource.New();
            sphereSource.SetCenter(0.0, 0.0, 0.0);
            sphereSource.SetRadius(0.5);

            //create a mapper
            vtkPolyDataMapper sphereMapper = vtkPolyDataMapper.New();
            sphereMapper.SetInputConnection(sphereSource.GetOutputPort());

            // create an actor
            vtkActor sphereActor = vtkActor.New();
            sphereActor.SetMapper(sphereMapper);

            // a renderer and render window
            vtkRenderWindow renderWindow = vtk_render.RenderWindow;
            vtkRenderer renderer = renderWindow.GetRenderers().GetFirstRenderer();
            // add the actors to the scene
            renderer.AddActor(sphereActor);

            vtkAxesActor axes = vtkAxesActor.New();
            // The axes are positioned with a user transform
            //vtkTransform transform = vtkTransform.New();
            //transform.Translate(0.75, 0.0, 0.0);
            //axes.SetUserTransform(transform);
            axes.SetTotalLength(10, 10, 10);
            axes.AxisLabelsOff();
            
            // properties of the axes labels can be set as follows
            // this sets the x axis label to red
            // axes.GetXAxisCaptionActor2D().GetCaptionTextProperty().SetColor(1,0,0);

            // the actual text of the axis label can be changed:
            // axes.SetXAxisLabelText("test");

            renderer.AddActor(axes);
            // we need to call Render() for the whole renderWindow, 
            // because vtkAxesActor uses an overlayed renderer for the axes label
            // in total we have now two renderer
            renderWindow.Render();
        }
        private void Arrow()
        {
            // Create arrows.  
            var render = vtk_render.RenderWindow.GetRenderers().GetFirstRenderer();
            //vtkArrowSource[] arrowSource = new vtkArrowSource[]
            //{
            //    vtkArrowSource.New(),vtkArrowSource.New(),vtkArrowSource.New()
            //};
            var arrowSource = vtkArrowSource.New();
            arrowSource.SetShaftRadius(0.01);
            arrowSource.SetShaftResolution(36);
            arrowSource.SetTipRadius(0.05);
            arrowSource.SetTipLength(0.5);
            arrowSource.SetTipResolution(36);
            
            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInputConnection(arrowSource.GetOutputPort());
            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);
            actor.SetPosition(0.0, 0.0, 0.0);
            render.AddActor(actor);
            render.Render();
            render.ResetCamera();
            //vtkArrowSource arrowSource02 = vtkArrowSource.New();
            //arrowSource02.SetShaftResolution(24);   // default = 6
            //arrowSource02.SetTipResolution(10);     // default = 6

            //// Visualize
            //vtkPolyDataMapper mapper02 = vtkPolyDataMapper.New();

            //mapper02.SetInputConnection(arrowSource02.GetOutputPort());

            //vtkActor actor02 = vtkActor.New();

            //actor02.SetMapper(mapper02);

            //actor02.SetPosition(0.0, -0.25, 0.0);
            //vtkRenderWindow renderWindow = vtk_render.RenderWindow;
            //vtkRenderer renderer = renderWindow.GetRenderers().GetFirstRenderer();
            ////renderer.SetBackground(0.2, 0.3, 0.4);

            //renderer.AddActor(actor02);
            //renderer.ResetCamera();
        }

        private void DrawCloud(IEnumerable<OpenTK.Vector3> cloud, float size=1)
        {
            var pts = vtkPoints.New();
            foreach (var i in cloud)
                pts.InsertNextPoint(i.X, i.Y, i.Z);
            var polydata = vtkPolyData.New();
            polydata.SetPoints(pts);
            var filter = vtkVertexGlyphFilter.New();
            filter.SetInput(polydata);
            filter.Update();
            var mapper = vtkPolyDataMapper.New();
            mapper.SetInput(filter.GetOutput());
            var actor = vtkActor.New();
            actor.SetMapper(mapper);
            actor.GetProperty().SetPointSize(size);
            actor.GetProperty().SetColor(1, 0, 0);
            var render = vtk_render.RenderWindow.GetRenderers().GetFirstRenderer();
            render.AddActor(actor);
            render.ResetCamera();
            render.Render();
        }

        private void BtnReadCloud_Click(object sender, RoutedEventArgs e)
        {
            var op = new Microsoft.Win32.OpenFileDialog();
            if (op.ShowDialog() == true)
            {
                var io = new Quadrep.PointCloudIO();
                var cloud = io.ReadBinary("TS2153D_raw.cloud");
                DrawCloud(cloud);
            }

        }
    }
}
