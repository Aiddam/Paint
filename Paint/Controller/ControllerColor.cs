using Paint.Model;

namespace Paint.Controller
{
    public class ControllerColor
    {
        public ColorRGB colorRGB = new ColorRGB();
        public ControllerColor(byte red = 0, byte green = 0, byte blue = 0)
        {
            colorRGB.Red = red;
            colorRGB.Green = green;
            colorRGB.Blue = blue;
        }
    }
}
