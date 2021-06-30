using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.IntensityTransform;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ApoCore
{
    /// <summary>
    /// Static methods allowing performing different operations on Bitmap using EmguCV library
    /// </summary>
    public static class EmguOperations
    {
        /// <summary>
        /// Blurs image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="border"></param>
        /// <returns></returns>
        public static Bitmap Blur(Bitmap image, BorderType border = BorderType.Default)
        {
            Image<Bgr, Byte> myImage = image.ToImage<Bgr, Byte>();
            CvInvoke.Blur(myImage, myImage, new Size(5, 5), new Point(-1, -1), border);
            image = myImage.ToBitmap<Bgr, byte>();
            return image;
        }

        /// <summary>
        /// Blurs image (gaussian)
        /// </summary>
        /// <param name="image"></param>
        /// <param name="borderType"></param>
        /// <returns></returns>
        public static Bitmap GaussianBlur(Bitmap image, BorderType borderType = BorderType.Default)
        {
            Image<Bgr, Byte> myImage = image.ToImage<Bgr, Byte>();
            // sigmaX = 0 - calculates based on kernel size
            CvInvoke.GaussianBlur(myImage, myImage, new Size(5, 5), 0, borderType: borderType);
            image = myImage.ToBitmap<Bgr, byte>();
            return image;
        }

        /// <summary>
        /// Edge detection using Sobel
        /// </summary>
        /// <param name="image"></param>
        /// <param name="borderType"></param>
        /// <returns></returns>
        public static Bitmap Sobel(Bitmap image, BorderType borderType = BorderType.Isolated)
        {
            Image<Bgr, Byte> imageX = image.ToImage<Bgr, Byte>();
            Image<Bgr, Byte> imageY = image.ToImage<Bgr, Byte>();
            // TODO wyświetlić wybrany przez użytkownika
            CvInvoke.Sobel(imageX, imageX, DepthType.Cv8U, 1, 0, borderType: borderType);
            CvInvoke.Sobel(imageY, imageY, DepthType.Cv8U, 0, 1, borderType: borderType);
            //Image<Bgr, Byte> imageResult = new Image<Bgr, byte>(image.Size);
            //CvInvoke.HConcat(imageX, imageY, imageResult);
            image = imageX.ToBitmap<Bgr, byte>();
            return image;
        }

        /// <summary>
        /// Edge detection using Laplacian
        /// </summary>
        /// <param name="image"></param>
        /// <param name="borderType"></param>
        /// <returns></returns>
        public static Bitmap Laplacian(Bitmap image, BorderType borderType = BorderType.Isolated)
        {
            Image<Gray, Byte> _image = image.ToImage<Gray, Byte>();
            CvInvoke.Laplacian(_image, _image, DepthType.Cv8U, 1, 1, borderType: borderType);
            image = _image.ToBitmap<Gray, byte>();
            return image;
        }

        /// <summary>
        /// Edge detection using Canny
        /// </summary>
        /// <param name="image"></param>
        /// <param name="threshold1"></param>
        /// <param name="threshold2"></param>
        /// <param name="borderType"></param>
        /// <returns></returns>
        public static Bitmap Canny(Bitmap image, int threshold1 = 100, int threshold2 = 200, BorderType borderType = BorderType.Isolated)
        {
            Image<Gray, Byte> _image = image.ToImage<Gray, Byte>();
            CvInvoke.Canny(_image, _image, threshold1, threshold2);
            image = _image.ToBitmap<Gray, byte>();
            return image;
        }

        /// <summary>
        /// Sharpens image (linear)
        /// </summary>
        /// <param name="image"></param>
        /// <param name="mask"></param>
        /// <param name="borderType"></param>
        /// <returns></returns>
        public static Bitmap LinearSharpening(Bitmap image, float[,] mask, BorderType borderType = BorderType.Isolated)
        {
            Image<Bgr, Byte> _image = image.ToImage<Bgr, Byte>();
            var m = new ConvolutionKernelF(mask);
            CvInvoke.Filter2D(_image, _image, m, new Point(-1, -1), borderType: borderType);
            image = _image.ToBitmap<Bgr, byte>();
            return image;
        }

        /// <summary>
        /// Blurs image (median blur)
        /// </summary>
        /// <param name="image"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Bitmap MedianBlur(Bitmap image, int size = 3)
        {
            if (size < 1) throw new ArgumentOutOfRangeException("Size must be equal or greater than 1.");
            Image<Bgr, Byte> myImage = image.ToImage<Bgr, Byte>();
            CvInvoke.MedianBlur(myImage, myImage, size);
            image = myImage.ToBitmap<Bgr, byte>();
            return image;
        }

        /// <summary>
        /// Enum of supported arithmetic operations
        /// </summary>
        public enum ArithmeticOperationType
        {
            Add,
            Subtract,
            And,
            Or,
            Not,
            Xor,
            Blend,
        }

        /// <summary>
        /// Performs all arithmetic operations from <see cref="ArithmeticOperationType"/> 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="operationType"></param>
        /// <param name="secondimage"></param>
        /// <returns></returns>
        public static Bitmap ArithmeticOperations(Bitmap image, ArithmeticOperationType operationType, Bitmap secondimage = null)
        {
            Image<Bgr, Byte> mainImage = image.ToImage<Bgr, Byte>();
            Image<Bgr, Byte> secondImage = null;
            if (secondimage != null)
            {
                secondImage = secondimage.ToImage<Bgr, Byte>();
                if (mainImage.Size != secondImage.Size)
                    CvInvoke.Resize(secondImage, secondImage, mainImage.Size);
            }
            if (secondImage != null)
            {
                switch (operationType)
                {
                    case ArithmeticOperationType.Add:
                        CvInvoke.Add(mainImage, secondImage, mainImage);
                        break;
                    case ArithmeticOperationType.Subtract:
                        CvInvoke.Subtract(mainImage, secondImage, mainImage);
                        break;
                    case ArithmeticOperationType.Blend:
                        CvInvoke.AddWeighted(mainImage, 0.7, secondImage, 0.5, -100, mainImage);
                        break;
                    case ArithmeticOperationType.And:
                        CvInvoke.BitwiseAnd(mainImage, secondImage, mainImage);
                        break;
                    case ArithmeticOperationType.Or:
                        CvInvoke.BitwiseOr(mainImage, secondImage, mainImage);
                        break;
                    case ArithmeticOperationType.Xor:
                        CvInvoke.BitwiseXor(mainImage, secondImage, mainImage);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (operationType == ArithmeticOperationType.Not)
                    CvInvoke.BitwiseNot(mainImage, mainImage);
                else throw new Exception("Operation not supported");
            }
            //CvInvoke.Resize(mainImage, mainImage, secondImage.Size);
            image = mainImage.ToBitmap<Bgr, byte>();
            return image;
        }

        /// <summary>
        /// Enum of supported kernel shapes
        /// </summary>
        public enum KernelShape
        {
            Cross,
            Rectangle,
            Square,
            Rhombus,
        }

        /// <summary>
        /// Performs different morphology operations
        /// </summary>
        /// <param name="image"></param>
        /// <param name="operationtype"></param>
        /// <param name="kernelShape"></param>
        /// <param name="iterations"></param>
        /// <param name="borderType"></param>
        /// <returns></returns>
        //public static Bitmap MorphologyOperations(Bitmap image, MorphOp operationtype = MorphOp.Erode, KernelShape kernelShape =
        //    KernelShape.Square, int iterations = 2, BorderType borderType = BorderType.Isolated)
        //{
        //    Image<Bgr, Byte> myImage = image.ToImage<Bgr, Byte>();
        //    Point point = new Point(-1, -1);
        //    Size size = new Size(3, 3);
        //    Mat kernel = new Mat();
        //    switch (kernelShape)
        //    {
        //        case KernelShape.Cross:
        //            kernel = CvInvoke.GetStructuringElement(ElementShape.Cross, size, point);
        //            break;
        //        case KernelShape.Rectangle:
        //            kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, size, point);
        //            break;
        //        case KernelShape.Square:
        //            kernel = new Mat(3, 3, DepthType.Cv8U, 1);
        //            break;
        //        case KernelShape.Rhombus:
        //            break;
        //        default:
        //            break;
        //    }
        //    CvInvoke.MorphologyEx(myImage, myImage, operationtype, kernel, point, iterations, borderType, new MCvScalar());
        //    image = myImage.ToBitmap<Bgr, byte>();
        //    return image;
        //}

        /// <summary>
        /// Performs different morphology operations
        /// </summary>
        /// <param name="image"></param>
        /// <param name="operationtype"></param>
        /// <param name="kernelShape"></param>
        /// <param name="iterations"></param>
        /// <param name="borderType"></param>
        /// <returns></returns>
        public static Bitmap MorphologyOperations(Bitmap image, MorphOp operationtype = MorphOp.Erode, ElementShape elementShape =
            ElementShape.Cross, int iterations = 2, BorderType borderType = BorderType.Isolated)
        {
            Image<Bgr, Byte> myImage = image.ToImage<Bgr, Byte>();
            Point point = new Point(-1, -1);
            Size size = new Size(3, 3);
            Mat kernel = CvInvoke.GetStructuringElement(elementShape, size, point);
            CvInvoke.MorphologyEx(myImage, myImage, operationtype, kernel, point, iterations, borderType, new MCvScalar());
            image = myImage.ToBitmap<Bgr, byte>();
            return image;
        }

        /// <summary>
        /// Filtration of an image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="mask"></param>
        /// <param name="borderType"></param>
        /// <returns></returns>
        public static Bitmap FiltrationOneStep(Bitmap image, float[,] mask, BorderType borderType = BorderType.Replicate)
        {
            Image<Bgr, Byte> myImage = image.ToImage<Bgr, Byte>();
            var m1 = new ConvolutionKernelF(mask);
            CvInvoke.Filter2D(myImage, myImage, m1, new Point(-1, -1), borderType: borderType);
            image = myImage.ToBitmap<Bgr, byte>();
            return image;
        }

        /// <summary>
        /// Two step filtration of an image (1st step - smoothing, 2nd - sharpening)
        /// </summary>
        /// <param name="image"></param>
        /// <param name="mask1"></param>
        /// <param name="mask2"></param>
        /// <param name="borderType"></param>
        /// <returns></returns>
        public static Bitmap FiltrationTwoStep(Bitmap image, float[,] mask1, float[,] mask2, BorderType borderType = BorderType.Replicate)
        {
            Image<Bgr, Byte> myImage = image.ToImage<Bgr, Byte>();
            var m1 = new ConvolutionKernelF(mask1);
            CvInvoke.Filter2D(myImage, myImage, m1, new Point(-1, -1), borderType: borderType);
            var m2 = new ConvolutionKernelF(mask2);
            CvInvoke.Filter2D(myImage, myImage, m2, new Point(-1, -1), borderType: borderType);
            image = myImage.ToBitmap<Bgr, byte>();
            return image;
        }

        /// <summary>
        /// Performs skeleton operation on image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="elementShape"></param>
        /// <param name="iterations"></param>
        /// <param name="borderType"></param>
        /// <returns></returns>
        public static Bitmap Skeleton(Bitmap image, ElementShape elementShape = ElementShape.Cross, int iterations = 2,
            BorderType borderType = BorderType.Isolated)
        {
            Bitmap im_copy = image;
            Bitmap skel = new Bitmap(image.Width, image.Height);
            while (true)
            {
                Bitmap im_open = MorphologyOperations(im_copy, MorphOp.Open, elementShape, iterations, borderType);
                Bitmap im_temp = ArithmeticOperations(im_copy, ArithmeticOperationType.Subtract, im_open);
                Bitmap im_eroded = MorphologyOperations(im_copy, MorphOp.Erode, elementShape, iterations, borderType);
                skel = ArithmeticOperations(skel, ArithmeticOperationType.Or, im_temp);
                im_copy = im_eroded;
                Image<Gray, Byte> emguimg = im_copy.ToImage<Gray, Byte>();
                if (CvInvoke.CountNonZero(emguimg) == 0) break;
            }
            return skel;
        }

        /// <summary>
        /// Performs adaptive thresholding operation on image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="maxValue"></param>
        /// <param name="blockSize"></param>
        /// <param name="adaptiveType"></param>
        /// <param name="thresholdType"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Bitmap AdaptiveThresholding(Bitmap image, double maxValue = 255, int blockSize = 11,
            AdaptiveThresholdType adaptiveType = AdaptiveThresholdType.MeanC, ThresholdType thresholdType = ThresholdType.Binary, double param = 5)
        {
            Image<Gray, Byte> myImage = image.ToImage<Gray, Byte>();
            CvInvoke.AdaptiveThreshold(myImage, myImage, maxValue, adaptiveType, thresholdType, blockSize, param);
            image = myImage.ToBitmap<Gray, byte>();
            return image;
        }

        /// <summary>
        /// Performs otsu thresholding operation on image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="threshold"></param>
        /// <param name="useGaussianBlur"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static Bitmap OtsuThresholding(Bitmap image, double threshold, bool useGaussianBlur = false, double maxValue = 255)
        {
            if (useGaussianBlur) image = GaussianBlur(image);
            Image<Gray, Byte> myImage = image.ToImage<Gray, Byte>();
            CvInvoke.Threshold(myImage, myImage, threshold, maxValue, ThresholdType.Otsu);
            image = myImage.ToBitmap<Gray, byte>();
            return image;
        }

        /// <summary>
        /// Performs watershed operation
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Bitmap Watershed(Bitmap image)
        {
            Mat myImage_mat = image.ToMat();
            // remove alpha channel
            if(myImage_mat.NumberOfChannels == 4) CvInvoke.CvtColor(myImage_mat, myImage_mat, ColorConversion.Bgra2Bgr);
            // if image is one channel convert to 3 channel image
            if (myImage_mat.NumberOfChannels == 1) CvInvoke.CvtColor(myImage_mat, myImage_mat, ColorConversion.Gray2Bgr);
            Mat originalImage = myImage_mat.Clone();
                       
            originalImage.ConvertTo(originalImage, DepthType.Cv8U);
            Mat grayscaleImage_mat = image.ToMat();
            CvInvoke.CvtColor(myImage_mat, grayscaleImage_mat, ColorConversion.Bgr2Gray);

            Mat thresh_mat = new Mat();
            CvInvoke.Threshold(grayscaleImage_mat, thresh_mat, 0, 255, ThresholdType.BinaryInv | ThresholdType.Otsu);

            Mat opening_mat = new Mat();
            Point point = new Point(-1, -1);
            Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), point);
            CvInvoke.MorphologyEx(thresh_mat, opening_mat, MorphOp.Open, kernel, point, 1, BorderType.Default, new MCvScalar());

            Mat sure_bg_mat = new Mat();
            CvInvoke.Dilate(opening_mat, sure_bg_mat, kernel, point, 1, BorderType.Default, new MCvScalar());

            Mat dist_transform_mat = new Mat();
            CvInvoke.DistanceTransform(opening_mat, dist_transform_mat, null, DistType.L2, 5);

            Mat sure_fg_mat = new Mat();
            double[] min, max;
            Point[] minLoc, maxLoc;
            dist_transform_mat.MinMax(out min, out max, out minLoc, out maxLoc);
            CvInvoke.Threshold(dist_transform_mat, sure_fg_mat, 0.5 * max[0], 255, ThresholdType.Binary);

            Mat unknown = new Mat();
            sure_fg_mat.ConvertTo(sure_fg_mat, DepthType.Cv8U);
            CvInvoke.Subtract(sure_bg_mat, sure_fg_mat, unknown);

            Mat markers = new Mat();
            CvInvoke.ConnectedComponents(sure_fg_mat, markers);

            //double[] minMarkers, maxMarkers;
            //Point[] minLocMarkers, maxLocMarkers;
            // maks = objects found
            //markers.MinMax(out minMarkers, out maxMarkers, out minLocMarkers, out maxLocMarkers);
            originalImage.ConvertTo(originalImage, DepthType.Cv8U);
            markers.ConvertTo(markers, DepthType.Cv32S);
            CvInvoke.Watershed(originalImage, markers);
            Image<Gray, byte> boundaries = markers.ToImage<Gray, Int32>().Convert<byte>(delegate (Int32 x)
            {
                return (byte)(x == -1 ? 255 : 0);
            });
            boundaries._Dilate(1);
            Image<Bgr, byte> img = myImage_mat.ToImage<Bgr, byte>();
            img.SetValue(new Bgr(0, 0, 255), boundaries);
            return img.ToBitmap();
        }

        /// <summary>
        /// Returns moments of image and all contours
        /// </summary>
        /// <param name="image"></param>
        /// <param name="retrievalMode"></param>
        /// <param name="approxMethod"></param>
        /// <returns></returns>
        public static (Moments moments, VectorOfVectorOfPoint contours) FeatureVector(Bitmap image, RetrType retrievalMode = RetrType.Ccomp,
            ChainApproxMethod approxMethod = ChainApproxMethod.ChainApproxNone)
        {
            Image<Gray, Byte> myImage = image.ToImage<Gray, Byte>();
            CvInvoke.Threshold(myImage, myImage, 127, 255, ThresholdType.Binary);

            // find countours
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            CvInvoke.FindContours(myImage, contours, hierarchy, retrievalMode, approxMethod);
            // calculate moments
            Moments moments = CvInvoke.Moments(myImage, true);
            return (moments, contours);
        }
    }
 }
