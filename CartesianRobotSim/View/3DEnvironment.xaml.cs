using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CartesianRobotSim.View
{
    /// <summary>
    /// Interaction logic for _3DEnvironment.xaml
    /// </summary>
    public partial class _3DEnvironment : Page
    {
        public _3DEnvironment()
        {
            InitializeComponent();
        }
    }
    ///How to move the pointer in code-behind
    ///In the _3DEnvironment.xaml.cs(code - behind) you can set the transform offsets, for example:
    ///PointerTransform.OffsetX = newX; PointerTransform.OffsetY = newY; PointerTransform.OffsetZ = newZ;
}

