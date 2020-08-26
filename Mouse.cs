using DxLibDLL;
using MyLib;

namespace Shooting
{
    public class Mouse
    {
        int mouseX;
        int mouseY;

        public Mouse()
        {
        }

        public void Update()
        {
            DX.GetMousePoint(out mouseX, out mouseY);
        }

        public float AngleToMouse(float x, float y)
        {//find angle to mouse
            return MyMath.PointToPointAngle(x, y, mouseX, mouseY);
        }

        public void Draw()
        {
            DX.DrawRotaGraphF(mouseX, mouseY, 1, 0, Image.sight);
            //DX.DrawString(700, 0, mouseX + "   " + mouseY, DX.GetColor(255, 255, 255)); //shows mouse X and Y
        }
    }
}
