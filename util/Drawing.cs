using System;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Configuration;

namespace AoC2022.util
{
    public static class Drawing
    {
        public static void Save(Bitmap img, string Subpath)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;
            string path = ConfigurationManager.AppSettings.Get("OutFolder") + "\\" + Subpath;
            img.Save(path, ImageFormat.Png);
        }
        /// <summary>
        /// Converting text to image (png).
        /// </summary>
        /// <param name="text">text to convert</param>
        /// <param name="font">Font to use</param>
        /// <param name="textColor">text color</param>
        /// <param name="maxWidth">max width of the image</param>
        /// <param name="path">path to save the image</param>
        public static void DrawText(String text, Font font, Color textColor, int maxWidth, String path)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;
            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);
            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font, maxWidth);

            //set the stringformat flags to rtl
            StringFormat sf = new StringFormat();
            //uncomment the next line for right to left languages
            //sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            sf.Trimming = StringTrimming.Word;
            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);
            //Adjust for high quality
            drawing.CompositingQuality = CompositingQuality.HighQuality;
            drawing.InterpolationMode = InterpolationMode.HighQualityBilinear;
            drawing.PixelOffsetMode = PixelOffsetMode.HighQuality;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            //paint the background
            drawing.Clear(Color.Transparent);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, new RectangleF(0, 0, textSize.Width, textSize.Height), sf);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();
            img.Save(path, ImageFormat.Png);
            img.Dispose();

        }
            
        public static Color Inferno(float percentile)
        {
            percentile = Math.Min(Math.Max(percentile, 0),1); 
            var Inferno = new string[] {
                "#140B36", "#160B39", "#170B3B", "#190B3E", "#1A0B40", "#1C0C43", "#1D0C45", "#1F0C47", "#200C4A", "#220B4C", "#240B4E", "#260B50",
                "#270B52", "#290B54", "#2B0A56", "#2D0A58", "#2E0A5A", "#300A5C", "#32095D", "#34095F", "#350960", "#370961", "#390962", "#3B0964",
                "#3C0965", "#3E0966", "#400966", "#410967", "#430A68", "#450A69", "#460A69", "#480B6A", "#4A0B6A", "#4B0C6B", "#4D0C6B", "#4F0D6C",
                "#500D6C", "#520E6C", "#530E6D", "#550F6D", "#570F6D", "#58106D", "#5A116D", "#5B116E", "#5D126E", "#5F126E", "#60136E", "#62146E",
                "#63146E", "#65156E", "#66156E", "#68166E", "#6A176E", "#6B176E", "#6D186E", "#6E186E", "#70196E", "#72196D", "#731A6D", "#751B6D",
                "#761B6D", "#781C6D", "#7A1C6D", "#7B1D6C", "#7D1D6C", "#7E1E6C", "#801F6B", "#811F6B", "#83206B", "#85206A", "#86216A", "#88216A",
                "#892269", "#8B2269", "#8D2369", "#8E2468", "#902468", "#912567", "#932567", "#952666", "#962666", "#982765", "#992864", "#9B2864",
                "#9C2963", "#9E2963", "#A02A62", "#A12B61", "#A32B61", "#A42C60", "#A62C5F", "#A72D5F", "#A92E5E", "#AB2E5D", "#AC2F5C", "#AE305B",
                "#AF315B", "#B1315A", "#B23259", "#B43358", "#B53357", "#B73456", "#B83556", "#BA3655", "#BB3754", "#BD3753", "#BE3852", "#BF3951",
                "#C13A50", "#C23B4F", "#C43C4E", "#C53D4D", "#C73E4C", "#C83E4B", "#C93F4A", "#CB4049", "#CC4148", "#CD4247", "#CF4446", "#D04544",
                "#D14643", "#D24742", "#D44841", "#D54940", "#D64A3F", "#D74B3E", "#D94D3D", "#DA4E3B", "#DB4F3A", "#DC5039", "#DD5238", "#DE5337",
                "#DF5436", "#E05634", "#E25733", "#E35832", "#E45A31", "#E55B30", "#E65C2E", "#E65E2D", "#E75F2C", "#E8612B", "#E9622A", "#EA6428",
                "#EB6527", "#EC6726", "#ED6825", "#ED6A23", "#EE6C22", "#EF6D21", "#F06F1F", "#F0701E", "#F1721D", "#F2741C", "#F2751A", "#F37719",
                "#F37918", "#F47A16", "#F57C15", "#F57E14", "#F68012", "#F68111", "#F78310", "#F7850E", "#F8870D", "#F8880C", "#F88A0B", "#F98C09",
                "#F98E08", "#F99008", "#FA9107", "#FA9306", "#FA9506", "#FA9706", "#FB9906", "#FB9B06", "#FB9D06", "#FB9E07", "#FBA007", "#FBA208",
                "#FBA40A", "#FBA60B", "#FBA80D", "#FBAA0E", "#FBAC10", "#FBAE12", "#FBB014", "#FBB116", "#FBB318", "#FBB51A", "#FBB71C", "#FBB91E",
                "#FABB21", "#FABD23", "#FABF25", "#FAC128", "#F9C32A", "#F9C52C", "#F9C72F", "#F8C931", "#F8CB34", "#F8CD37", "#F7CF3A", "#F7D13C",
                "#F6D33F", "#F6D542", "#F5D745", "#F5D948", "#F4DB4B", "#F4DC4F", "#F3DE52", "#F3E056", "#F3E259", "#F2E45D", "#F2E660", "#F1E864",
                "#F1E968", "#F1EB6C", "#F1ED70", "#F1EE74", "#F1F079", "#F1F27D", "#F2F381", "#F2F485", "#F3F689", "#F4F78D", "#F5F891", "#F6FA95",
                "#F7FB99", "#F9FC9D", "#FAFDA0", "#FCFEA4"
            };
            int colorCount = Inferno.Length;
            return ColorTranslator.FromHtml(Inferno[Math.Min((int) percentile * colorCount, colorCount-1)]);
        }
    }
}
