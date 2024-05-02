using K8055Velleman.Game;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace K8055Velleman.Lib.ClassExtension;

public class BButton : Button
{
    public Color highlight = Color.Gray;
    public Color lowlight = Color.Black;

    public BButton() 
    {
        GotFocus += (object sender, EventArgs e) => { Highlight(); AudioManager.PlaySound(AudioFile.MouseOver); };
        LostFocus += (object sender, EventArgs e) => { Lowlight(); };
        MouseEnter += (object sender, EventArgs e) => { Focus(); };
        //MouseLeave += (object sender, EventArgs e) => { Lowlight(); };
    }

    private void Highlight()
    {
        BackColor = highlight;
    }

    private void Lowlight()
    {
        BackColor = lowlight;
    }
}
