using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace panes
{
    public partial class Page1 : PhoneApplicationPage
    {
        private Color WHITE = Color.FromArgb(255, 255, 255, 255);
        private Color RED = Color.FromArgb(255, 255, 0, 0);
        private Color ORANGE = Color.FromArgb(255, 255, 165, 0);
        private Color YELLOW = Color.FromArgb(255, 255, 255, 0);
        private Color GREEN = Color.FromArgb(255, 0, 128, 0);
        private Color BLUE = Color.FromArgb(255, 0, 0, 255);
        private Color PURPLE = Color.FromArgb(255, 128, 0, 128);

        Color[] colorCircle;

        private Rectangle source;
        private Color sourceColor;
        private bool sourcePicked = false;

        public Page1()
        {
            colorCircle = new Color[6]{
                RED,
                ORANGE,
                YELLOW,
                GREEN,
                BLUE,
                PURPLE
            };
            InitializeComponent();
            Grid playingfield = ((System.Windows.Controls.Grid)(this.FindName("PlayingField")));
            UIElementCollection rd = playingfield.Children;
            UIElement u = new Rectangle();
            rd.Add(u);
        }

        private void GestureListener_Tap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            if (!sourcePicked) //first tap (select the source color0
            {
                sourcePicked = true;
                source = (Rectangle)e.OriginalSource;
                sourceColor = ((SolidColorBrush)source.Fill).Color;
                source.Fill = new RadialGradientBrush(sourceColor, Color.FromArgb((byte)(sourceColor.A - 50), sourceColor.R, sourceColor.G, sourceColor.B));
            }
            else// second tap (change the destination rectangle)
            {
                Rectangle dest = (Rectangle)e.OriginalSource;
                sourcePicked = false;
                source.Fill = new SolidColorBrush(sourceColor);
                Color destColor = ((SolidColorBrush)dest.Fill).Color;
                dest.Fill = new SolidColorBrush(ChangeColor(sourceColor, destColor));
            }
        }

        private Color ChangeColor(Color from, Color to)
        {
            int fromIndex = 6, toIndex = 6;
            for (int i = 0; i < colorCircle.Length; i++)
            {
                if (colorCircle[i] == from)
                    fromIndex = i;
                else if (colorCircle[i] == to)
                    toIndex = i;
            }

            // WHITE case or within one 
            if (toIndex == 6 || Math.Abs(fromIndex - toIndex) <= 1 || (6 - Math.Abs(fromIndex - toIndex)) <= 1)
                return from;

            // Opposite color case
            else if (Math.Abs(fromIndex - toIndex) == 3)
                return WHITE;

            //gosh darn edge case
            else if ((from == RED && to == BLUE) || (from == BLUE && to == RED))
                return PURPLE;

            //second darn edge case
            else if ((from == PURPLE && to == ORANGE) || (from == ORANGE && to == PURPLE))
                return RED;
            // Adjacent color case
            else
                return colorCircle[(fromIndex + toIndex) / 2];
        }

        public class MyColor
        {
            public Color c;
            public Color blendLeft;
            public Color left;
            public Color opposite;
            public Color right;
            public Color blendRight;

            public MyColor(Color[] v)
            { 
                c = v[0]; 
                blendLeft = v[1]; 
                left = v[2]; 
                opposite = v[3]; 
                right = v[4]; 
                blendRight = v[5]; 
            }
        }

    }
}