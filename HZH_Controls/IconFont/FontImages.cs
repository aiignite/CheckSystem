// ***********************************************************************
// Assembly         : HZH_Controls
// Created          : 2019-09-11
//
// ***********************************************************************
// <copyright file="FontImages.cs">
//     Copyright by Huang Zhenghui(黄正辉) All, QQ group:568015492 QQ:623128629 Email:623128629@qq.com
// </copyright>
//
// Blog: https://www.cnblogs.com/bfyx
// GitHub：https://github.com/kwwwvagaa/NetWinformControl
// gitee：https://gitee.com/kwwwvagaa/net_winform_custom_control.git
//
// If you use this code, please keep this note.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using HZH_Controls.Helpers;

namespace HZH_Controls.IconFont
{
    /// <summary>
    /// 字体图标图片,awesome字体默认加载，elegant字体在使用时延迟加载
    /// 图标示例 http://www.fontawesome.com.cn/faicons/?tdsourcetag=s_pcqq_aiomsg
    /// 图标示例 https://www.elegantthemes.com/blog/resources/elegant-icon-font
    /// </summary>
    public static class FontImages
    {
        /// <summary>
        /// The m font collection
        /// </summary>
        private static readonly PrivateFontCollection m_fontCollection = new PrivateFontCollection();

        /// <summary>
        /// The m fonts awesome
        /// </summary>
        private static readonly Dictionary<string, Font> m_fontsAwesome = new Dictionary<string, Font>();

        /// <summary>
        /// The m fonts elegant
        /// </summary>
        private static readonly Dictionary<string, Font> m_fontsElegant = new Dictionary<string, Font>();

        /// <summary>
        /// The m cache maximum size
        /// </summary>
        private static Dictionary<int, float> m_cacheMaxSize = new Dictionary<int, float>();
        /// <summary>
        /// The minimum font size
        /// </summary>
        private const int MinFontSize = 8;
        /// <summary>
        /// The maximum font size
        /// </summary>
        private const int MaxFontSize = 43;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <exception cref="FileNotFoundException">Font file not found</exception>
        static FontImages()
        {
            var strPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.ToLower().Replace("file:///", "");
            var strDir = Path.GetDirectoryName(strPath);
            if (!Directory.Exists(Path.Combine(strDir, "IconFont")))
            {
                Directory.CreateDirectory(Path.Combine(strDir, "IconFont"));
            }
            var strFile = Path.Combine(strDir, "IconFont\\FontAwesome.ttf");
            if (!File.Exists(strFile))
            {
                var fs = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("HZH_Controls.IconFont.FontAwesome.ttf");
                var sw = new FileStream(strFile, FileMode.Create, FileAccess.Write);
                fs.CopyTo(sw);
                sw.Close();
                fs.Close();
            }

            m_fontCollection.AddFontFile(strFile);

            float size = MinFontSize;
            for (var i = 0; i <= (MaxFontSize - MinFontSize) * 2; i++)
            {
                m_fontsAwesome.Add(size.ToString("F2"), new Font(m_fontCollection.Families[0], size, FontStyle.Regular, GraphicsUnit.Point));
                size += 0.5f;
            }
        }

        /// <summary>
        /// Gets the font awesome.
        /// </summary>
        /// <value>The font awesome.</value>
        public static FontFamily FontAwesome
        {
            get
            {
                for (var i = 0; i < m_fontCollection.Families.Length; i++)
                {
                    if (m_fontCollection.Families[i].Name == "FontAwesome")
                    {
                        return m_fontCollection.Families[i];
                    }
                }
                return m_fontCollection.Families[0];
            }
        }

        /// <summary>
        /// Gets the elegant icons.
        /// </summary>
        /// <value>The elegant icons.</value>
        /// <exception cref="FileNotFoundException">Font file not found</exception>
        public static FontFamily ElegantIcons
        {
            get
            {
                if (m_fontsElegant.Count <= 0)
                {
                    lock (m_fontsElegant)
                    {
                        if (m_fontsElegant.Count <= 0)
                        {
                            var strPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.ToLower().Replace("file:///", "");
                            var strDir = Path.GetDirectoryName(strPath);
                            if (!Directory.Exists(Path.Combine(strDir, "IconFont")))
                            {
                                Directory.CreateDirectory(Path.Combine(strDir, "IconFont"));
                            }
                            var strFile = Path.Combine(strDir, "IconFont\\ElegantIcons.ttf");
                            if (!File.Exists(strFile))
                            {
                                var fs = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("HZH_Controls.IconFont.ElegantIcons.ttf");
                                var sw = new FileStream(strFile, FileMode.Create, FileAccess.Write);
                                fs.CopyTo(sw);
                                sw.Close();
                                fs.Close();
                            }
                            m_fontCollection.AddFontFile(strFile);

                            float size = MinFontSize;
                            for (var i = 0; i <= (MaxFontSize - MinFontSize) * 2; i++)
                            {
                                m_fontsElegant.Add(size.ToString("F2"), new Font(m_fontCollection.Families[0], size, FontStyle.Regular, GraphicsUnit.Point));
                                size += 0.5f;
                            }
                        }
                    }
                }
                for (var i = 0; i < m_fontCollection.Families.Length; i++)
                {
                    if (m_fontCollection.Families[i].Name == "ElegantIcons")
                    {
                        return m_fontCollection.Families[i];
                    }
                }
                return m_fontCollection.Families[0];
            }
        }
        /// <summary>
        /// 获取图标
        /// </summary>
        /// <param name="iconText">图标名称</param>
        /// <param name="imageSize">图标大小</param>
        /// <param name="foreColor">前景色</param>
        /// <param name="backColor">背景色</param>
        /// <returns>图标</returns>
        public static Icon GetIcon(
            FontIcons iconText, int imageSize = 32, Color? foreColor = null, Color? backColor = null)
        {
            var image = GetImage(iconText, imageSize, foreColor, backColor);
            return image != null ? ConvertToIcon(image) : null;
        }

        /// <summary>
        /// 获取图标.
        /// </summary>
        /// <param name="iconText">图标名称.</param>
        /// <param name="imageSize">图标大小.</param>
        /// <param name="foreColor">前景色</param>
        /// <param name="backColor">背景色.</param>
        /// <returns>Bitmap.</returns>
        /// <exception cref="FileNotFoundException">Font file not found</exception>
        public static Bitmap GetImage(FontIcons iconText, int imageSize = 32, Color? foreColor = null, Color? backColor = null)
        {
            Dictionary<string, Font> _fs;
            if (iconText.ToString().StartsWith("A_"))
                _fs = m_fontsAwesome;
            else
            {
                if (m_fontsElegant.Count <= 0)
                {
                    lock (m_fontsElegant)
                    {
                        if (m_fontsElegant.Count <= 0)
                        {
                            var strPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.ToLower().Replace("file:///", "");
                            var strDir = Path.GetDirectoryName(strPath);
                            if (strDir != null && !Directory.Exists(Path.Combine(strDir, "IconFont")))
                            {
                                Directory.CreateDirectory(Path.Combine(strDir, "IconFont"));
                            }
                            if (strDir != null)
                            {
                                var strFile = Path.Combine(strDir, "IconFont\\ElegantIcons.ttf");
                                if (!File.Exists(strFile))
                                {
                                    var fs = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("HZH_Controls.IconFont.ElegantIcons.ttf");
                                    var sw = new FileStream(strFile, FileMode.Create, FileAccess.Write);
                                    if (fs != null)
                                    {
                                        fs.CopyTo(sw);
                                        sw.Close();
                                        fs.Close();
                                    }
                                }
                                m_fontCollection.AddFontFile(strFile);
                            }

                            float size = MinFontSize;
                            for (var i = 0; i <= (MaxFontSize - MinFontSize) * 2; i++)
                            {
                                m_fontsElegant.Add(size.ToString("F2"), new Font(m_fontCollection.Families[0], size, FontStyle.Regular, GraphicsUnit.Point));
                                size += 0.5f;
                            }
                        }
                    }
                }
                _fs = m_fontsElegant;
            }

            if (!foreColor.HasValue)
                foreColor = Color.Black;
            var imageFont = _fs[MinFontSize.ToString("F2")];
            var textSize = new SizeF(imageSize, imageSize);
            using (var bitmap = new Bitmap(48, 48))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                //float size = MaxFontSize;
                float fltMaxSize = MaxFontSize;
                if (m_cacheMaxSize.ContainsKey(imageSize))
                {
                    fltMaxSize = Math.Max(MaxFontSize, m_cacheMaxSize[imageSize] + 5);
                }
                while (fltMaxSize >= MinFontSize)
                {
                    var font = _fs[fltMaxSize.ToString("F2")];
                    SizeF sf = GetIconSize(iconText, graphics, font);
                    if (sf.Width < imageSize && sf.Height < imageSize)
                    {
                        imageFont = font;
                        textSize = sf;
                        break;
                    }

                    fltMaxSize -= 0.5f;
                }

                if (!m_cacheMaxSize.ContainsKey(imageSize) || (m_cacheMaxSize.ContainsKey(imageSize) && m_cacheMaxSize[imageSize] < fltMaxSize))
                {
                    m_cacheMaxSize[imageSize] = fltMaxSize;
                }
            }

            var srcImage = new Bitmap(imageSize, imageSize);
            using (var graphics = Graphics.FromImage(srcImage))
            {
                if (backColor.HasValue && backColor.Value != Color.Empty && backColor.Value != Color.Transparent)
                    graphics.Clear(backColor.Value);
                var s = char.ConvertFromUtf32((int)iconText);
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                graphics.SetGDIHigh();
                using (Brush brush2 = new SolidBrush(foreColor.Value))
                {
                    graphics.DrawString(s, imageFont, brush2, new PointF((imageSize - textSize.Width) / 2.0f + 1, (imageSize - textSize.Height) / 2.0f + 1));
                }
            }

            return srcImage;
        }

        /// <summary>
        /// Gets the size of the icon.
        /// </summary>
        /// <param name="iconText">The icon text.</param>
        /// <param name="graphics">The graphics.</param>
        /// <param name="font">The font.</param>
        /// <returns>Size.</returns>
        private static Size GetIconSize(FontIcons iconText, Graphics graphics, Font font)
        {
            var text = char.ConvertFromUtf32((int)iconText);
            return graphics.MeasureString(text, font).ToSize();
        }

        /// <summary>
        /// Converts to icon.
        /// </summary>
        /// <param name="srcBitmap">The source bitmap.</param>
        /// <param name="size">The size.</param>
        /// <returns>Icon.</returns>
        /// <exception cref="ArgumentNullException">srcBitmap</exception>
        private static Icon ToIcon(Bitmap srcBitmap, int size)
        {
            if (srcBitmap == null)
            {
                throw new ArgumentNullException("srcBitmap");
            }

            Icon icon;
            using (var memoryStream = new MemoryStream())
            {
                new Bitmap(srcBitmap, new Size(size, size)).Save(memoryStream, ImageFormat.Png);
                Stream stream = new MemoryStream();
                var binaryWriter = new BinaryWriter(stream);
                if (stream.Length <= 0L)
                {
                    return null;
                }

                binaryWriter.Write((byte)0);
                binaryWriter.Write((byte)0);
                binaryWriter.Write((short)1);
                binaryWriter.Write((short)1);
                binaryWriter.Write((byte)size);
                binaryWriter.Write((byte)size);
                binaryWriter.Write((byte)0);
                binaryWriter.Write((byte)0);
                binaryWriter.Write((short)0);
                binaryWriter.Write((short)32);
                binaryWriter.Write((int)memoryStream.Length);
                binaryWriter.Write(22);
                binaryWriter.Write(memoryStream.ToArray());
                binaryWriter.Flush();
                binaryWriter.Seek(0, SeekOrigin.Begin);
                icon = new Icon(stream);
                stream.Dispose();
            }

            return icon;
        }

        /// <summary>
        /// 转换Image为Icon
        /// </summary>
        /// <param name="image">要转换为图标的Image对象</param>
        /// <param name="nullTonull">当image为null时是否返回null。false则抛空引用异常</param>
        /// <exception cref="ArgumentNullException" />
        /// <returns></returns>
        public static Icon ConvertToIcon(Image image, bool nullTonull = false)
        {
            if (image == null)
            {
                if (nullTonull)
                {
                    return null;
                }
                throw new ArgumentNullException("Image is null");
            }

            using (MemoryStream msImg = new MemoryStream(), msIco = new MemoryStream())
            {
                image.Save(msImg, ImageFormat.Png);

                using (var bin = new BinaryWriter(msIco))
                {
                    //写图标头部
                    bin.Write((short)0);           //0-1保留
                    bin.Write((short)1);           //2-3文件类型。1=图标, 2=光标
                    bin.Write((short)1);           //4-5图像数量（图标可以包含多个图像）

                    bin.Write((byte)image.Width);  //6图标宽度
                    bin.Write((byte)image.Height); //7图标高度
                    bin.Write((byte)0);            //8颜色数（若像素位深>=8，填0。这是显然的，达到8bpp的颜色数最少是256，byte不够表示）
                    bin.Write((byte)0);            //9保留。必须为0
                    bin.Write((short)0);           //10-11调色板
                    bin.Write((short)32);          //12-13位深
                    bin.Write((int)msImg.Length);  //14-17位图数据大小
                    bin.Write(22);                 //18-21位图数据起始字节

                    //写图像数据
                    bin.Write(msImg.ToArray());

                    bin.Flush();
                    bin.Seek(0, SeekOrigin.Begin);
                    return new Icon(msIco);
                }
            }
        }
    }
}