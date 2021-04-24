//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ApoCore
//{
//    public static class ImageOperations
//    {
//        /// <summary>
//        /// Performs negation operation on image
//        /// </summary>
//        /// <param name="image"></param>
//        /// <returns></returns>
//        //public static ImageModel Negation(this ImageModel image)
//        //{
//        //    int maxR = 0;
//        //    int maxG = 0;
//        //    int maxB = 0;
//        //    foreach (var item in image.HistogramData)
//        //    {
//        //        switch (item.Channel)
//        //        {
//        //            case ChannelModel.Red:
//        //                maxR = item.Max;
//        //                break;
//        //            case ChannelModel.Green:
//        //                maxG = item.Max;
//        //                break;
//        //            case ChannelModel.Blue:
//        //                maxB = item.Max;
//        //                break;
//        //            default:
//        //                break;
//        //        }
//        //    }
//        //    for (int x = 0; x < image.Image.Width; x++)
//        //    {
//        //        for (int y = 0; y < image.Image.Height; y++)
//        //        {
//        //            image.LutR[x, y] = maxR - image.LutR[x, y];
//        //            image.LutG[x, y] = maxG - image.LutG[x, y];
//        //            image.LutB[x, y] = maxB - image.LutB[x, y];
//        //            Color newcolor = Color.FromArgb(image.LutR[x, y], image.LutG[x, y], image.LutB[x, y]);
//        //            image.Image.SetPixel(x, y, newcolor);
//        //        }
//        //    }
//        //    //return image.Image;
//        //    return image;
//        //}
//        public static Bitmap Negation3(ImageModel image)
//        {
//            int maxR = 0;
//            int maxG = 0;
//            int maxB = 0;
//            foreach (var item in image.HistogramData)
//            {
//                switch (item.Channel)
//                {
//                    case ChannelModel.Red:
//                        maxR = item.Max;
//                        break;
//                    case ChannelModel.Green:
//                        maxG = item.Max;
//                        break;
//                    case ChannelModel.Blue:
//                        maxB = item.Max;
//                        break;
//                    default:
//                        break;
//                }
//            }
//            for (int x = 0; x < image.Image.Width; x++)
//            {
//                for (int y = 0; y < image.Image.Height; y++)
//                {
//                    image.LutR[x, y] = maxR - image.LutR[x, y];
//                    image.LutG[x, y] = maxG - image.LutG[x, y];
//                    image.LutB[x, y] = maxB - image.LutB[x, y];
//                    Color newcolor = Color.FromArgb(image.LutR[x, y], image.LutG[x, y], image.LutB[x, y]);
//                    image.Image.SetPixel(x, y, newcolor);
//                }
//            }
//            //return image.Image;
//            return image.Image;
//        }
//    }
//}
