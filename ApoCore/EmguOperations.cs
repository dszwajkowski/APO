using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System;
using System.Drawing;
using System.Linq;

namespace ApoCore
{
    public static class EmguOperations
    {
        public static Bitmap Blur(Bitmap image, BorderType border = BorderType.Default)
        {
            Image<Bgr, Byte> myImage = image.ToImage<Bgr, Byte>();
            CvInvoke.Blur(myImage, myImage, new Size(5, 5), new Point(-1, -1), border);
            image = myImage.ToBitmap<Bgr, byte>();
            return image;
        }

        public static Bitmap GaussianBlur(Bitmap image, BorderType borderType = BorderType.Default)
        {
            Image<Bgr, Byte> myImage = image.ToImage<Bgr, Byte>();
            CvInvoke.GaussianBlur(myImage, myImage, new Size(5, 5), 0, borderType: borderType);
            image = myImage.ToBitmap<Bgr, byte>();
            return image;
        }

        public static Bitmap Sobel(Bitmap image, BorderType borderType = BorderType.Isolated)
        {
            Image<Bgr, Byte> imageX = image.ToImage<Bgr, Byte>();
            Image<Bgr, Byte> imageY = image.ToImage<Bgr, Byte>();
            CvInvoke.Sobel(imageX, imageX, DepthType.Cv8U, 1, 0, borderType: borderType);
            CvInvoke.Sobel(imageY, imageY, DepthType.Cv8U, 0, 1, borderType: borderType);
            //Image<Bgr, Byte> imageResult = new Image<Bgr, byte>(image.Size);
            //CvInvoke.HConcat(imageX, imageY, imageResult);
            image = imageX.ToBitmap<Bgr, byte>();
            return image;
        }

        public static Bitmap Laplacian(Bitmap image, BorderType borderType = BorderType.Isolated)
        {
            Image<Gray, Byte> _image = image.ToImage<Gray, Byte>();
            CvInvoke.Laplacian(_image, _image, DepthType.Cv8U, 1, 1, borderType: borderType);
            image = _image.ToBitmap<Gray, byte>();
            return image;
        }

        public static Bitmap Canny(Bitmap image, int threshold1 = 100, int threshold2 = 200, BorderType borderType = BorderType.Isolated)
        {
            // TODO detect if grayscale or not
            Image<Gray, Byte> _image = image.ToImage<Gray, Byte>();
            //Image<Gray, Byte> imageResult = new Image<Gray, byte>(image.Size);
            CvInvoke.Canny(_image, _image, threshold1, threshold2);
            image = _image.ToBitmap<Gray, byte>();
            return image;
        }
        // TODO: Lab3 borderType - isolated, reflect, repliacte
        public static Bitmap LinearSharpening(Bitmap image, BorderType borderType = BorderType.Isolated)
        {
            Image<Bgr, Byte> _image = image.ToImage<Bgr, Byte>();
            //Image<Bgr, Byte> imageResult = new Image<Bgr, byte>(image.Size);
            float[,] m = { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
            var mask = new ConvolutionKernelF(m);
            CvInvoke.Filter2D(_image, _image, mask, new Point(-1, -1), borderType: borderType);
            image = _image.ToBitmap<Bgr, byte>();
            return image;
        }
        // TODO: Lab3 borderType - isolated, reflect, repliacte
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
                        //CvInvoke.BlendLinear(mainImage, secondImage, mainImage); 
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

        // TODO: Lab3 ArithmeticOperationType

        public static Bitmap MorphologyOperations(Bitmap image, MorphOp operationtype = MorphOp.Erode, ElementShape elementShape =
            ElementShape.Rectangle, int iterations = 2, BorderType borderType = BorderType.Isolated)
        {
            Image<Bgr, Byte> myImage = image.ToImage<Bgr, Byte>();
            Point point = new Point(-1, -1);
            Mat kernel = CvInvoke.GetStructuringElement(elementShape, new Size(3, 3), point);
            CvInvoke.MorphologyEx(myImage, myImage, operationtype, kernel, point, iterations, borderType, new MCvScalar());
            image = myImage.ToBitmap<Bgr, byte>();
            return image;
        }

        // TODO: Lab4 ArithmeticOperationType - erozja, dylatacja, otwarcia, zamknięcia, ElementShape - romb, kwadrat, iterations - 1-x, borderType - isolated, reflect, repliacte

        public static Bitmap FiltrationOneStep(Bitmap image, BorderType borderType = BorderType.Replicate)
        {
            throw new NotImplementedException();
            Image<Bgr, Byte> myImage = image.ToImage<Bgr, Byte>();
            Point point = new Point(-1, -1);
            float[,] m1 = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            float[,] m2 = { { 1, -2, 1 }, { -2, 4, -2 }, { 1, -2, 1 } };
            // TODO konvolution
            //Convolution 
            var mask = new ConvolutionKernelF(m1);
            CvInvoke.Filter2D(myImage, myImage, mask, point, borderType: borderType);
            image = myImage.ToBitmap<Bgr, byte>();
            return image;
            //Matrix.Create()
            //Accord.Math.
        }

        // TODO: Lab4 borderType - isolated, reflect, repliacte

        public static Bitmap FiltrationTwoStep(Bitmap image, BorderType borderType = BorderType.Replicate)
        {
            Image<Bgr, Byte> myImage = image.ToImage<Bgr, Byte>();
            Point point = new Point(-1, -1);
            float[,] m1 = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            float[,] m2 = { { 1, -2, 1 }, { -2, 4, -2 }, { 1, -2, 1 } };                    
            var mask1 = new ConvolutionKernelF(m1);
            CvInvoke.Filter2D(myImage, myImage, mask1, point, borderType: borderType);
            var mask2 = new ConvolutionKernelF(m2);
            CvInvoke.Filter2D(myImage, myImage, mask2, point, borderType: borderType);
            image = myImage.ToBitmap<Bgr, byte>();
            return image;
            //Matrix.Create()
            //Accord.Math.
        }

        // TODO: Lab4 borderType - isolated, reflect, repliacte

        public static Bitmap Skeleton(Bitmap image, ElementShape elementShape = ElementShape.Cross, int iterations = 2,
            BorderType borderType = BorderType.Isolated)
        {
            // TODO check if image is binary
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
        // TODO: Lab4 ElementShape - romb, kwadrat, borderType - isolated, reflect, repliacte


        public static Bitmap AdaptiveThresholding(Bitmap image, double maxValue = 255, int blockSize = 11, 
            AdaptiveThresholdType adaptiveType = AdaptiveThresholdType.MeanC, ThresholdType thresholdType = ThresholdType.Binary, double param = 5)
        {
            Image<Gray, Byte> myImage = image.ToImage<Gray, Byte>();
            CvInvoke.AdaptiveThreshold(myImage, myImage, maxValue, adaptiveType, thresholdType, blockSize, param);
            image = myImage.ToBitmap<Gray, byte>();
            return image;
        }

        // TODO: Lab5 maxValue?, blockSize!, AdaptiveThresholdType - MeanC, Gauss,

        public static Bitmap OtsuThresholding(Bitmap image, double threshold, double maxValue = 255)
        {
            Image<Gray, Byte> myImage = image.ToImage<Gray, Byte>();
            CvInvoke.Threshold(myImage, myImage, threshold, maxValue, ThresholdType.Otsu);
            image = myImage.ToBitmap<Gray, byte>();
            return image;
        }
        
        public static Bitmap Test(Bitmap image)
        {
            Image<Bgr, Byte> myImage = image.ToImage<Bgr, Byte>();
            Image<Gray, Byte> grayscaleImage = image.ToImage<Gray, Byte>();
            CvInvoke.CvtColor(myImage, grayscaleImage, ColorConversion.Bgr2Gray);

            Image<Gray, Byte> thresh = new Image<Gray, byte>(grayscaleImage.Size);
            CvInvoke.Threshold(grayscaleImage, thresh, 0, 255, ThresholdType.Otsu);

            Image<Gray, Byte> opening = new Image<Gray, byte>(grayscaleImage.Size);
            Point point = new Point(-1, -1);
            Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), point);
            CvInvoke.MorphologyEx(thresh, opening, MorphOp.Open, kernel, point, 1,
                BorderType.Default, new MCvScalar());
            
            Image<Gray, Byte> sure_bg = new Image<Gray, byte>(grayscaleImage.Size);
            CvInvoke.Dilate(opening, sure_bg, kernel, point, 1, BorderType.Default,
                new MCvScalar());

            Image<Gray, Byte> dist_transform = new Image<Gray, byte>(grayscaleImage.Size);
            //Image<Gray, Byte> labels = new Image<Gray, byte>(grayscaleImage.Size);
            CvInvoke.DistanceTransform(opening, dist_transform, null, DistType.L1, 5);

            image = dist_transform.ToBitmap<Gray, byte>();
            return image;
        }
        // TODO: Lab5 maxValue?, threshold
    }
}

//Border type
//Może size
//Kernel
//Element shape
